using HarmonyLib;
using RimWorld;
using StrongerEmpire;
using StrongerEmpire.Biotech;
using System.Linq;
using Verse;

namespace StrongerEmpire.VRE_Hussar
{
    [StaticConstructorOnStartup]
    public class Initialize
    {
        public const string ModName = "StrongerEmpire_VRE_Hussar";

        static Initialize()
        {
            EnabledMods.VRE_HussarActive = true;

            TotalGenepool.genepools.Add("vanillaracesexpanded.hussar", new Genepool 
            { 
                ModId = "vanillaracesexpanded.hussar", 
                ProCombatGenes = VREHussarGenePool.ProCombatGenes, 
                MetaIncreasingGenes = VREHussarGenePool.MetaReducingGenes 
            });
        }
    }
}