using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace StrongerEmpire.Biotech
{
    internal static class BiotechGenePool
    {
        public static List<GeneDef> ProCombatGenes => new List<GeneDef>()
        {
            SE_GeneDefOf.AcidSpray,
            SE_GeneDefOf.AnimalWarcall,
            SE_GeneDefOf.FireSpew,
            SE_GeneDefOf.FoamSpray,
            SE_GeneDefOf.Superclotting,
            SE_GeneDefOf.MoveSpeed_VeryQuick,
            SE_GeneDefOf.MinTemp_LargeDecrease,
            SE_GeneDefOf.MaxTemp_LargeIncrease,
            SE_GeneDefOf.ToxResist_Total,
            SE_GeneDefOf.FireResistant,
            SE_GeneDefOf.MeleeDamage_Strong,
            SE_GeneDefOf.Robust,
            SE_GeneDefOf.DarkVision,
            SE_GeneDefOf.ElongatedFingers,
            SE_GeneDefOf.PollutionRush,
            SE_GeneDefOf.WoundHealing_SuperFast,
            SE_GeneDefOf.Unstoppable,
            SE_GeneDefOf.AptitudeRemarkable_Shooting,
            SE_GeneDefOf.AptitudeRemarkable_Melee,
        };

        public static List<GeneDef> MetaReducingGenes => new List<GeneDef>()
        {
            SE_GeneDefOf.Mood_Depressive,
            SE_GeneDefOf.KillThirst,
            SE_GeneDefOf.Sterile,
            SE_GeneDefOf.Instability_Major,
            SE_GeneDefOf.AptitudeTerrible_Construction,
            SE_GeneDefOf.AptitudeTerrible_Mining,
            SE_GeneDefOf.AptitudeTerrible_Cooking,
            SE_GeneDefOf.AptitudeTerrible_Plants,
            SE_GeneDefOf.AptitudeTerrible_Animals,
            SE_GeneDefOf.AptitudeTerrible_Social,
            SE_GeneDefOf.AptitudeTerrible_Artistic,
            SE_GeneDefOf.AptitudeTerrible_Crafting,
            SE_GeneDefOf.AptitudeTerrible_Medicine,
            SE_GeneDefOf.AptitudeTerrible_Intellectual,
            SE_GeneDefOf.ChemicalDependency_Psychite,
            SE_GeneDefOf.ChemicalDependency_WakeUp,
            SE_GeneDefOf.ChemicalDependency_GoJuice,
            SE_GeneDefOf.Aggression_HyperAggressive,
            SE_GeneDefOf.Beauty_VeryUgly
        };
    }
}
