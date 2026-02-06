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
            EmpireGeneDefOf.AcidSpray,
            EmpireGeneDefOf.AnimalWarcall,
            EmpireGeneDefOf.FireSpew,
            EmpireGeneDefOf.FoamSpray,
            EmpireGeneDefOf.Superclotting,
            EmpireGeneDefOf.MoveSpeed_VeryQuick,
            EmpireGeneDefOf.MinTemp_LargeDecrease,
            EmpireGeneDefOf.MaxTemp_LargeIncrease,
            EmpireGeneDefOf.ToxResist_Total,
            EmpireGeneDefOf.FireResistant,
            EmpireGeneDefOf.MeleeDamage_Strong,
            EmpireGeneDefOf.Robust,
            EmpireGeneDefOf.DarkVision,
            EmpireGeneDefOf.ElongatedFingers,
            EmpireGeneDefOf.PollutionRush,
            EmpireGeneDefOf.WoundHealing_SuperFast,
            EmpireGeneDefOf.Unstoppable,
            EmpireGeneDefOf.AptitudeRemarkable_Shooting,
            EmpireGeneDefOf.AptitudeRemarkable_Melee,
        };

        public static List<GeneDef> MetaReducingGenes => new List<GeneDef>()
        {
            EmpireGeneDefOf.Mood_Depressive,
            EmpireGeneDefOf.KillThirst,
            EmpireGeneDefOf.Sterile,
            EmpireGeneDefOf.Instability_Major,
            EmpireGeneDefOf.AptitudeTerrible_Construction,
            EmpireGeneDefOf.AptitudeTerrible_Mining,
            EmpireGeneDefOf.AptitudeTerrible_Cooking,
            EmpireGeneDefOf.AptitudeTerrible_Plants,
            EmpireGeneDefOf.AptitudeTerrible_Animals,
            EmpireGeneDefOf.AptitudeTerrible_Social,
            EmpireGeneDefOf.AptitudeTerrible_Artistic,
            EmpireGeneDefOf.AptitudeTerrible_Crafting,
            EmpireGeneDefOf.AptitudeTerrible_Medicine,
            EmpireGeneDefOf.AptitudeTerrible_Intellectual,
            EmpireGeneDefOf.ChemicalDependency_Psychite,
            EmpireGeneDefOf.ChemicalDependency_WakeUp,
            EmpireGeneDefOf.ChemicalDependency_GoJuice,
            EmpireGeneDefOf.Aggression_HyperAggressive,
            EmpireGeneDefOf.Beauty_VeryUgly
        };
    }
}
