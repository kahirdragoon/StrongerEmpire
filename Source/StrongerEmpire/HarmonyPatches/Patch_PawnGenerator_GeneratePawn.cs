using HarmonyLib;
using RimWorld;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Burst.Intrinsics;
using Verse;

namespace StrongerEmpire;

[HarmonyPatch(typeof(PawnGenerator), nameof(PawnGenerator.GeneratePawn), [typeof(PawnGenerationRequest)])]
public static class Patch_PawnGenerator_GeneratePawn
{
    [HarmonyPostfix] 
    public static void Postfix(Pawn __result, PawnGenerationRequest request)
    {
        if (__result is null || request.Faction?.def != FactionDefOf.Empire || __result.kindDef is null || !__result.kindDef.isFighter)
            return;
        
        if (StrongerEmpireMod.settings.enableMilitaryTraining)
            PawnModificator.AddMilitaryTraining(__result);

        if (StrongerEmpireMod.settings.enableLuciferiumInfusion)
            PawnModificator.AddLuciferium(__result);

        if (StrongerEmpireMod.settings.enableUnwaveringlyLoyalSoldiers)
            PawnModificator.MakeUnwaveringlyLoyal(__result);

        if(ModsConfig.OdysseyActive && StrongerEmpireMod.settings.uniqueWeaponSpawnChance > 0f)
            PawnModificator.MaybeGiveUniqueWeapon(__result);

        PawnModificator.FixServerity(__result);
    }
}
