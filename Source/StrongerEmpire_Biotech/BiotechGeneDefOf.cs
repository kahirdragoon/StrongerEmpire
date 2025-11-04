using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace StrongerEmpire.Biotech
{
    [DefOf]
    public static class SE_GeneDefOf
    {
        public static GeneDef MinTemp_SmallDecrease;
        public static GeneDef MaxTemp_SmallIncrease;
        public static GeneDef Delicate;

        // minus metabolism genes
        public static GeneDef AcidSpray;
        public static GeneDef AnimalWarcall;
        public static GeneDef FireSpew;
        public static GeneDef FoamSpray;
        public static GeneDef Superclotting;
        public static GeneDef MoveSpeed_VeryQuick;
        public static GeneDef MinTemp_LargeDecrease;
        public static GeneDef MaxTemp_LargeIncrease;
        public static GeneDef ToxResist_Total;
        public static GeneDef FireResistant;
        public static GeneDef MeleeDamage_Strong;
        public static GeneDef Robust;
        public static GeneDef DarkVision;
        public static GeneDef ElongatedFingers;
        public static GeneDef PollutionRush;
        public static GeneDef WoundHealing_SuperFast;
        public static GeneDef Unstoppable;
        public static GeneDef AptitudeRemarkable_Shooting;
        public static GeneDef AptitudeRemarkable_Melee;
        
        // Only one of them should be added
        public static GeneDef PsychicAbility_Deaf;
        public static GeneDef PsychicAbility_Extreme;

        // plus metabolism genes
        public static GeneDef Mood_Depressive;
        public static GeneDef KillThirst;
        public static GeneDef Sterile;
        public static GeneDef Instability_Major;
        public static GeneDef AptitudeTerrible_Construction;
        public static GeneDef AptitudeTerrible_Mining;
        public static GeneDef AptitudeTerrible_Cooking;
        public static GeneDef AptitudeTerrible_Plants;
        public static GeneDef AptitudeTerrible_Animals;
        public static GeneDef AptitudeTerrible_Social;
        public static GeneDef AptitudeTerrible_Artistic;
        public static GeneDef AptitudeTerrible_Crafting;
        public static GeneDef AptitudeTerrible_Medicine;
        public static GeneDef AptitudeTerrible_Intellectual;
        public static GeneDef ChemicalDependency_Psychite;
        public static GeneDef ChemicalDependency_WakeUp;
        public static GeneDef ChemicalDependency_GoJuice;
        public static GeneDef Aggression_HyperAggressive;
        public static GeneDef Pain_Extra;
        public static GeneDef Beauty_VeryUgly;

        static SE_GeneDefOf()
        {
            if (ModsConfig.BiotechActive)
                DefOfHelper.EnsureInitializedInCtor(typeof(GeneDefOf));
        }
    }
}
