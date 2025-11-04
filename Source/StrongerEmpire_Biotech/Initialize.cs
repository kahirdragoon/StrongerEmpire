using HarmonyLib;
using RimWorld;
using System.Linq;
using Verse;

namespace StrongerEmpire.Biotech
{
    [StaticConstructorOnStartup]
    public class Initialize
    {
        public const string ModName = "StrongerEmpire_Biotech";

        static Initialize()
        {
            var harmony = new Harmony(ModName);
            harmony.PatchAll();

            TotalGenepool.genepools.Add("biotech", new Genepool { ModId = "biotech", ProCombatGenes = BiotechGenePool.ProCombatGenes, MetaIncreasingGenes = BiotechGenePool.MetaReducingGenes });
        }
    }
}