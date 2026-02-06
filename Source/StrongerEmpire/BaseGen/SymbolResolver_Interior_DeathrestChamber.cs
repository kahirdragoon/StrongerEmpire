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
    private static List<PawnKindDef> pawnKinds;
    private static List<XenotypeDef> xenotypes;
    private static List<ThingDef> buildingPool;

    private static List<PawnKindDef> GetPawnKinds()
    {
        pawnKinds ??=
            [
                EmpireDefOf.Empire_Royal_Baron,
                EmpireDefOf.Empire_Royal_Praetor,
                EmpireDefOf.Empire_Royal_Knight,
            ];
        return pawnKinds;
    }

    private static List<XenotypeDef> GetXenotypes()
    {
        if(xenotypes == null)
        {
            xenotypes =
            [
                EmpireDefOf.Sanguophage,
            ];
            if(EmpireDefOf.VRE_Bruxa != null)
                xenotypes.Add(EmpireDefOf.VRE_Bruxa);
            if(EmpireDefOf.VRE_Ekkimian != null)
                xenotypes.Add(EmpireDefOf.VRE_Ekkimian);
            if (EmpireDefOf.VRE_Strigoi != null)
                xenotypes.Add(EmpireDefOf.VRE_Strigoi);
        }

        return xenotypes;
    }

    private static List<ThingDef> GetBuildings()
    {
        buildingPool ??=
            [
                EmpireDefOf.PsychofluidPump,
                EmpireDefOf.PsychofluidPump,
                EmpireDefOf.GlucosoidPump,
                EmpireDefOf.GlucosoidPump,
                EmpireDefOf.HemogenAmplifier,
                EmpireDefOf.HemogenAmplifier,
                EmpireDefOf.Hemopump,
                EmpireDefOf.Hemopump,
                EmpireDefOf.DeathrestAccelerator,
                EmpireDefOf.DeathrestAccelerator,
            ];
        return buildingPool;
    }

    public override void Resolve(ResolveParams rp)
    {
        List<Thing> spawnedBuildings = [];
        PawnHolder pawnHolder = new();

        rp.SetCustom("deathrestBuildings", spawnedBuildings, inherit: true);
        rp.SetCustom("deathrestPawn", pawnHolder, inherit: true);

        BaseGen.symbolStack.Push("empire_deathrestpost", rp);

        Rot4 rot = Rot4.South;

        CellRect centerRect = CellRect.CenteredOn(
            rp.rect.CenterCell,
            ThingDefOf.DeathrestCasket.Size.x,
            ThingDefOf.DeathrestCasket.Size.z
        );

        PushDeathrestCasket(rp, rot, centerRect, spawnedBuildings);
        PushDeathrestBuildings(rp, rot, centerRect, spawnedBuildings);
        PushSanguophage(rp, centerRect, pawn => pawnHolder.Pawn = pawn);
    }

    private void PushDeathrestBuildings(ResolveParams rp, Rot4 rot, CellRect centerRect, List<Thing> spawnedBuildings)
    {
        int spacing = (rp.rect.Width <= 6 && rp.rect.Height <= 6) ? 0 : 1;

        int mWidth = 1;
        int mHeight = 2;

        int stepX = mWidth + spacing;
        int stepZ = mHeight + spacing;

        int ringLeft = centerRect.CenterCell.x - stepX;
        int ringRight = centerRect.CenterCell.x + stepX;
        int ringBottom = centerRect.CenterCell.z - 2 * stepZ;
        int ringTop = centerRect.CenterCell.z + stepZ;

        IntVec3 removedA = new(ringLeft, 0, centerRect.CenterCell.z);
        IntVec3 removedB = new(ringRight, 0, centerRect.CenterCell.z);

        bool shortRoom = rp.rect.Height < 6;
        IntVec3 removedTopCenter = new(centerRect.CenterCell.x, 0, ringTop);
        IntVec3 removedBottomCenter = new(centerRect.CenterCell.x, 0, ringBottom);
        
        var buildingPoolTemp = new List<ThingDef>(GetBuildings());
        buildingPoolTemp.Shuffle();
        Log.Warning($"Shuffled building pool: {string.Join(", ", buildingPoolTemp.Select(b => b?.defName))}");

        for (int z = ringBottom; z <= ringTop; z += stepZ)
        {
            for (int x = ringLeft; x <= ringRight; x += stepX)
            {
                IntVec3 anchor = new(x, 0, z);

                if (anchor == removedA || anchor == removedB)
                    continue;

                if (shortRoom && (anchor == removedTopCenter || anchor == removedBottomCenter))
                    continue;

                CellRect mRect = CellRect.CenteredOn(anchor, mWidth, mHeight);

                if (!mRect.FullyContainedWithin(rp.rect))
                    continue;

                var thingDef = buildingPoolTemp.First();
                buildingPoolTemp.RemoveAt(0);

                Rot4 useRot = rot;
                if (shortRoom && (anchor.z == ringTop || anchor.z == ringBottom))
                    useRot = Rot4.West;

                ResolveParams mRp = rp with
                {
                    singleThingDef = thingDef,
                    rect = mRect,
                    thingRot = useRot,
                    postThingSpawn = spawnedBuildings.Add
                };
                BaseGen.symbolStack.Push("thing", mRp);
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

    private void PushSanguophage(ResolveParams rp, CellRect centerRect, Action<Pawn> onSpawn)
    {
        Log.Warning("Attempting to spawn deathrest pawn...");
        Faction empireFaction = rp.faction ?? Find.FactionManager.FirstFactionOfDef(FactionDefOf.Empire);

        Pawn existing = null;

        Log.Warning($"Trying to spawn a deathrest pawn for faction {empireFaction?.Name} with pawn kinds: {string.Join(", ", GetPawnKinds().Select(k => k?.defName))}");
        
        if (empireFaction != null)
        {
            existing = Find.WorldPawns.AllPawnsAlive
                .Where(p => p != null && p.Faction == empireFaction && p.kindDef != null && GetPawnKinds().Contains(p.kindDef))
                .RandomElementWithFallback();
        }
        Log.Warning(existing != null
            ? $"Found existing pawn {existing.Name} of kind {existing.kindDef?.defName} for faction {empireFaction?.Name}"
            : $"No existing pawn found for faction {empireFaction?.Name}");
        if (existing != null)
        {
            ResolveParams pawnRp = rp with
            {
                rect = centerRect,
                spawnAnywhereIfNoGoodCell = true,
                singlePawnToSpawn = existing,
                postThingSpawn = thing =>
                {
                    if (thing is Pawn pawn)
                        onSpawn(pawn);
                }
            };

            BaseGen.symbolStack.Push("pawn", pawnRp);
            return;
        }
        Log.Warning($"No existing pawn found, generating new one for faction {empireFaction?.Name} with xenotypes: {string.Join(", ", GetXenotypes().Select(x => x?.defName))}");
        PawnGenerationRequest req = new(
            GetPawnKinds().RandomElement(),
            empireFaction,
            tile: new PlanetTile?(BaseGen.globalSettings.map.Tile),
            forceGenerateNewPawn: true,
            forcedXenotype: GetXenotypes().RandomElementWithFallback(EmpireDefOf.Sanguophage),
            forcedXenogenes:
            [
                EmpireGeneDefOf.FireResistant,
                EmpireGeneDefOf.AptitudeTerrible_Construction,
                EmpireGeneDefOf.AptitudeTerrible_Mining,
            ]
        );
        Log.Warning($"Generated pawn request: Kind={req.KindDef?.defName}, Faction={req.Faction?.Name}, Xenotype={req.ForcedXenotype?.defName}, Xenogenes={string.Join(", ", req.ForcedXenogenes?.Select(g => g?.defName) ?? [])}");
        ResolveParams pawnRpNew = rp with
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

        BaseGen.symbolStack.Push("pawn", pawnRpNew);
    }

}