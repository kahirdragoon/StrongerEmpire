using RimWorld;
using RimWorld.BaseGen;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using static StrongerEmpire.SymbolResolver_DeathrestPost;

namespace StrongerEmpire;

public class SymbolResolver_Interior_DeathrestChamber : SymbolResolver
{
    public override void Resolve(ResolveParams rp)
    {
        List<Thing> spawnedBuildings = [];
        PawnHolder pawnHolder = new();

        rp.SetCustom("deathrestBuildings", spawnedBuildings, inherit: true);
        rp.SetCustom("deathrestPawn", pawnHolder, inherit: true);

        BaseGen.symbolStack.Push("empire_deathrestpost", rp);

        int spacing = (rp.rect.Width <= 5 && rp.rect.Height <= 6) ? 0 : 1;
        Rot4 rot = Rot4.South;

        CellRect centerRect = CellRect.CenteredOn(
            rp.rect.CenterCell,
            ThingDefOf.DeathrestCasket.Size.x,
            ThingDefOf.DeathrestCasket.Size.z
        );

        PushDeathrestCasket(rp, rot, centerRect, spawnedBuildings);
        PushDeathrestBuildings(rp, spacing, rot, centerRect, spawnedBuildings);
        PushSanguophage(rp, centerRect, pawn => pawnHolder.Pawn = pawn);
    }

    private void PushDeathrestBuildings(ResolveParams rp, int spacing, Rot4 rot, CellRect centerRect, List<Thing> spawnedBuildings)
    {
        // Build a 3x4 ring of top-left anchor positions around center, then remove 2 for total 10.
        // For 1x2 (width=1, height=2, North), the anchor is the south cell.
        int mWidth = 1;
        int mHeight = 2;

        int stepX = mWidth + spacing;
        int stepZ = mHeight + spacing;

        // Recompute ring bounds in *slot space* (3 by 4 slots)
        int ringLeft = centerRect.CenterCell.x - stepX;
        int ringRight = centerRect.CenterCell.x + stepX;
        int ringBottom = centerRect.CenterCell.z - 2 * stepZ;
        int ringTop = centerRect.CenterCell.z + stepZ;

        // Remove two slots (mid left/right)
        IntVec3 removedA = new(ringLeft, 0, centerRect.CenterCell.z);
        IntVec3 removedB = new(ringRight, 0, centerRect.CenterCell.z);

        List<ThingDef> buildingPool =
        [
            EmpireThingDefOf.PsychofluidPump,
            EmpireThingDefOf.PsychofluidPump,
            EmpireThingDefOf.GlucosoidPump,
            EmpireThingDefOf.GlucosoidPump,
            EmpireThingDefOf.HemogenAmplifier,
            EmpireThingDefOf.HemogenAmplifier,
            EmpireThingDefOf.Hemopump,
            EmpireThingDefOf.Hemopump,
            EmpireThingDefOf.DeathrestAccelerator,
            EmpireThingDefOf.DeathrestAccelerator,
        ];
        buildingPool.Shuffle();

        int count = 0;
        for (int z = ringBottom; z <= ringTop; z += stepZ)
        {
            for (int x = ringLeft; x <= ringRight; x += stepX)
            {
                IntVec3 anchor = new(x, 0, z);

                if (anchor == removedA || anchor == removedB)
                    continue;

                CellRect mRect = CellRect.CenteredOn(anchor, mWidth, mHeight);

                if (!mRect.FullyContainedWithin(rp.rect))
                    continue;

                var thingDef = buildingPool.First();
                buildingPool.RemoveAt(0);

                ResolveParams mRp = rp with
                {
                    singleThingDef = thingDef,
                    rect = mRect,
                    thingRot = rot,
                    postThingSpawn = spawnedBuildings.Add
                };
                BaseGen.symbolStack.Push("thing", mRp);
                count++;
            }
        }
    }

    private static void PushDeathrestCasket(ResolveParams rp, Rot4 rot, CellRect centerRect, List<Thing> spawnedBuildings)
    {
        ResolveParams centerRp = rp with
        {
            singleThingDef = ThingDefOf.DeathrestCasket,
            rect = centerRect,
            thingRot = rot,
            postThingSpawn = spawnedBuildings.Add

        };
        BaseGen.symbolStack.Push("thing", centerRp);
    }

    private static void PushSanguophage(ResolveParams rp, CellRect centerRect, Action<Pawn> onSpawn)
    {
        PawnKindDef dukeKind = DefDatabase<PawnKindDef>.GetNamed("Empire_Royal_Duke", true);
        XenotypeDef forcedXeno = DefDatabase<XenotypeDef>.GetNamed("Sanguophage", true);

        PawnGenerationRequest req = new(
            dukeKind,
            rp.faction,
            tile: new PlanetTile?(BaseGen.globalSettings.map.Tile),
            forceGenerateNewPawn: true,
            forcedXenotype: forcedXeno
        );

        ResolveParams pawnRp = rp with
        {
            rect = centerRect,
            spawnAnywhereIfNoGoodCell = true,
            singlePawnGenerationRequest = req,
            postThingSpawn = thing => 
            {
                if (thing is Pawn pawn)
                    onSpawn(pawn);
            }
        };

        BaseGen.symbolStack.Push("pawn", pawnRp);
    }
}