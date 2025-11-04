using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace StrongerEmpire.VRE_Hussar
{
    internal static class VREHussarGenePool
    {
        public static List<GeneDef> ProCombatGenes => new List<GeneDef>()
        {
            SE_VRE_Hussar_GeneDefOf.VREH_BulletproofSkin,
            SE_VRE_Hussar_GeneDefOf.VREH_Dutiful,
            SE_VRE_Hussar_GeneDefOf.VREH_Giant,
            SE_VRE_Hussar_GeneDefOf.VREH_Toughness,
            SE_VRE_Hussar_GeneDefOf.VREH_Unconstrained,
            SE_VRE_Hussar_GeneDefOf.VREH_Glidewings,
        };

        public static List<GeneDef> MetaReducingGenes => new List<GeneDef>()
        {
            SE_VRE_Hussar_GeneDefOf.VREH_Arrogant
        };
    }
}
