using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace StrongerEmpire.VRE_Hussar
{
    [DefOf]
    public static class SE_VRE_Hussar_GeneDefOf
    {
        // minus metabolism genes
        public static GeneDef VREH_BulletproofSkin;
        public static GeneDef VREH_Dutiful;
        public static GeneDef VREH_Giant;
        public static GeneDef VREH_Toughness;
        public static GeneDef VREH_Unconstrained;
        public static GeneDef VREH_Glidewings;

        // plus metabolism genes
        public static GeneDef VREH_Arrogant;

        static SE_VRE_Hussar_GeneDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(SE_VRE_Hussar_GeneDefOf));
        }
    }
}
