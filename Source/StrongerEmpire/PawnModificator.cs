using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Burst.Intrinsics;
using Verse;

namespace StrongerEmpire
{
    public static class PawnModificator
    {
        private static HediffDef militaryTrainingHediffDef;
        private static ThingDef luciferiumDef;

        internal static void AddMilitaryTraining(Pawn pawn)
        {
            pawn.health.AddHediff(GetMilitaryTrainingHediffDef());
        }

        private static HediffDef GetMilitaryTrainingHediffDef()
        {
            if (militaryTrainingHediffDef == null)
                militaryTrainingHediffDef = HediffDef.Named("MilitaryTraining");
            return militaryTrainingHediffDef;
        }

        internal static void AddLuciferium(Pawn pawn)
        {
            if (!StrongerEmpireMod.settings.enableLuciferiumInfusion
                || pawn?.RaceProps?.Humanlike == false
                || !Rand.Chance(StrongerEmpireMod.settings.luciferiumChance))
                return;

            ThingWithComps luci = (ThingWithComps)ThingMaker.MakeThing(GetLuciferiumDef());

            luci.stackCount = 1;
            luci.Ingested(pawn, 0f);
        }

        private static ThingDef GetLuciferiumDef()
        {
            if (luciferiumDef == null)
                luciferiumDef = ThingDef.Named("Luciferium");
            return luciferiumDef;
        }

        internal static void MakeUnwaveringlyLoyal(Pawn pawn)
        {
            if (pawn.guest != null)
                pawn.guest.Recruitable = false;
        }

        // Mainly for IntegratedImplants extra arms having ~ 0.1 Serverity when added
        internal static void FixServerity(Pawn pawn)
        {
            List<Hediff> hediffs = new List<Hediff>();
            pawn.health.hediffSet.GetHediffs(ref hediffs, h => h.Severity <= 0.2f && h.def.defName.Contains("Extra"));
            foreach (var hediff in hediffs)
                hediff.Severity = 1f;
        }

        internal static void MaybeGiveUniqueWeapon(Pawn pawn)
        {
            if (!Rand.Chance(StrongerEmpireMod.settings.uniqueWeaponSpawnChance))
                return;

            var pawnWeapon = pawn.equipment.Primary;
            if (pawnWeapon == null)
                return;

            var uniqueWeaponDef = DefDatabase<ThingDef>.GetNamedSilentFail(pawnWeapon.def.defName + "_Unique");
            if (uniqueWeaponDef == null) 
                return;

            var uniqueWeapon = (ThingWithComps)ThingMaker.MakeThing(uniqueWeaponDef);

            pawn.equipment.DestroyEquipment(pawnWeapon);
            pawn.equipment.AddEquipment(uniqueWeapon);
        }
    }
}
