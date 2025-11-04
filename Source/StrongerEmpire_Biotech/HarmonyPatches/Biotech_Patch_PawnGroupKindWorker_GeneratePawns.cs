using HarmonyLib;
using RimWorld;
using StrongerEmpire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Burst.Intrinsics;
using Verse;

namespace StrongerEmpire.Biotech
{
    [HarmonyPatch(typeof(PawnGroupKindWorker_Normal))]
    [HarmonyPatch("GeneratePawns", MethodType.Normal)]
    [HarmonyPatch(new[] {
        typeof(PawnGroupMakerParms),
        typeof(PawnGroupMaker),
        typeof(List<Pawn>),
        typeof(bool)
    })]
    public static class Biotech_Patch_PawnGroupKindWorker_GeneratePawns
    {
        [HarmonyPostfix]
        public static void Postfix(PawnGroupMakerParms parms,
            PawnGroupMaker groupMaker,
            List<Pawn> outPawns,
            bool errorOnZeroResults)
        {
            var isSettlementFight = groupMaker.kindDef.defName == "Settlement";
            if (!StrongerEmpireMod.settings.enableGeneModification
                || parms.faction?.def != FactionDefOf.Empire
                || (parms.points < StrongerEmpireMod.settings.startGeneModdingRaidPointThreshold && !isSettlementFight)
                )
                return;

            if (outPawns == null)
            {
                Log.ErrorOnce($"[{Initialize.ModName}]: Pawn list unexpectedly null during generation! Aborting patch", 2589631);
                return;
            }

            var steps = Math.Max(0, (int)((parms.points - StrongerEmpireMod.settings.startGeneModdingRaidPointThreshold)
                               / StrongerEmpireMod.settings.geneModificationRaidPointStep));
            var numberOfGenesToAdd = isSettlementFight ? 99 : steps * StrongerEmpireMod.settings.genesAddedperStep;

            var genepool = new Genepool();

            if (StrongerEmpireMod.settings.enableBiotechGeneSupport && TotalGenepool.genepools.ContainsKey("biotech"))
            {
                genepool.ProCombatGenes.AddRangeFast<GeneDef>(TotalGenepool.genepools["biotech"].ProCombatGenes);
                genepool.MetaIncreasingGenes.AddRangeFast<GeneDef>(TotalGenepool.genepools["biotech"].MetaIncreasingGenes);
            }

            if (StrongerEmpireMod.settings.enableVREHussarGeneSupport && TotalGenepool.genepools.ContainsKey("vanillaracesexpanded.hussar"))
            {
                genepool.ProCombatGenes.AddRangeFast<GeneDef>(TotalGenepool.genepools["vanillaracesexpanded.hussar"].ProCombatGenes);
                genepool.MetaIncreasingGenes.AddRangeFast<GeneDef>(TotalGenepool.genepools["vanillaracesexpanded.hussar"].MetaIncreasingGenes);
            }

            bool inSpace = parms.tile.LayerDef.isSpace;

            foreach (var pawn in outPawns)
            {
                if (pawn.genes?.Xenotype is null)
                    continue;

                AddGenes(pawn, numberOfGenesToAdd, genepool, inSpace);
            }
        }

        private static void AddGenes(Pawn pawn, int numberOfGenesToAdd, Genepool genepool, bool inSpace)
        {
            var plusGenePool = genepool.ProCombatGenes
                .Where(g => !pawn.genes.HasXenogene(g))
                .ToList();
            if (pawn.HasPsylink && !pawn.genes.HasXenogene(SE_GeneDefOf.PsychicAbility_Extreme))
                plusGenePool.Add(SE_GeneDefOf.PsychicAbility_Extreme);
            else if(!pawn.genes.HasXenogene(SE_GeneDefOf.PsychicAbility_Deaf))
                plusGenePool.Add(SE_GeneDefOf.PsychicAbility_Deaf);

            var minusGenePool = genepool.MetaIncreasingGenes
                .Where(g => !pawn.genes.HasXenogene(g))
                .ToList();
            
            if (ModsConfig.OdysseyActive && inSpace)
            {
                var vaccuumGeneDef = DefDatabase<GeneDef>.GetNamed("VacuumResistance_Partial");
                if(vaccuumGeneDef != null && !pawn.genes.HasActiveGene(vaccuumGeneDef))
                    pawn.genes.AddGene(vaccuumGeneDef, true);
                else if (plusGenePool.Count <= 0)
                    return;
            }
            else if (plusGenePool.Count <= 0)
                return;

            pawn.genes.xenotypeName = "Modified " + pawn.genes.Xenotype.label;


            if (numberOfGenesToAdd >= plusGenePool.Count)
            {
                foreach (var gene in plusGenePool)
                    pawn.genes.AddGene(gene, true);
            }
            else
            {
                for (int i = 0; i < numberOfGenesToAdd && plusGenePool.Count > 0; i++)
                {
                    var gene = plusGenePool.RandomElement();
                    plusGenePool.Remove(gene);
                    pawn.genes.AddGene(gene, true);
                }
            }

            // Remove weaker versions of some xenogenes that otherweise would overwrite the stronger ones
            if (pawn.genes.HasXenogene(SE_GeneDefOf.MaxTemp_LargeIncrease) && pawn.genes.HasXenogene(SE_GeneDefOf.MaxTemp_SmallIncrease))
                pawn.genes.RemoveGene(pawn.genes.GetGene(SE_GeneDefOf.MaxTemp_SmallIncrease));
            if (pawn.genes.HasXenogene(SE_GeneDefOf.MinTemp_LargeDecrease) && pawn.genes.HasXenogene(SE_GeneDefOf.MinTemp_SmallDecrease))
                pawn.genes.RemoveGene(pawn.genes.GetGene(SE_GeneDefOf.MinTemp_SmallDecrease));
            if (pawn.genes.HasXenogene(SE_GeneDefOf.Robust) && pawn.genes.HasXenogene(SE_GeneDefOf.Delicate))
                pawn.genes.RemoveGene(pawn.genes.GetGene(SE_GeneDefOf.Delicate));

            while (minusGenePool.Count > 0 && pawn.genes.Xenogenes.Sum(g => g.def.biostatMet) < 3)
            {
                var geneDef = minusGenePool.RandomElement();
                pawn.genes.AddGene(geneDef, true);
                minusGenePool.Remove(geneDef);
                if (plusGenePool.Count <= 0)
                    break;
            }
        }
    }
}
