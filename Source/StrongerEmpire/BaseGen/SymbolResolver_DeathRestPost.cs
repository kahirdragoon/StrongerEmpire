using RimWorld;
using RimWorld.BaseGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace StrongerEmpire;

public class SymbolResolver_DeathrestPost : SymbolResolver
{
    public sealed class PawnHolder
    {
        public Pawn Pawn;
    }

    public override void Resolve(ResolveParams rp)
    {
        Pawn pawn = rp.GetCustom<PawnHolder>("deathrestPawn")?.Pawn;
        List<Thing> buildings = rp.GetCustom<List<Thing>>("deathrestBuildings");
        var deathrestGene = pawn?.genes?.GetFirstGeneOfType<Gene_Deathrest>();

        if (deathrestGene == null || pawn == null || pawn.Dead || buildings == null)
            return;

        if (buildings.Count > 1)
            deathrestGene?.OffsetCapacity(buildings.Count - 2);

        foreach (Thing thing in buildings)
        {
            var compDeahtrestBindable = thing.TryGetComp<CompDeathrestBindable>();
            if(compDeahtrestBindable == null)
                continue;

            compDeahtrestBindable.BindTo(pawn);
            deathrestGene.BindTo(compDeahtrestBindable);

            switch (thing.def.defName)
            {
                case "Hemopump":
                    Gene_Hemogen gene = pawn.genes?.GetFirstGeneOfType<Gene_Hemogen>();
                    gene?.SetMax(gene.Max + 0.25f);
                    break;
                case "HemogenAmplifier":
                    pawn.health.AddHediff(DefDatabase<HediffDef>.GetNamed("HemogenAmplified"));
                    break;
                case "GlucosoidPump":
                    pawn.health.AddHediff(DefDatabase<HediffDef>.GetNamed("GlucosoidRush"));
                    break;
                case "PsychofluidPump":
                    pawn.health.AddHediff(DefDatabase<HediffDef>.GetNamed("PsychofluidRush"));
                    break;
            }
        }
    }
}

