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

[HarmonyPatch(typeof(PawnGroupKindWorker_Normal))]
[HarmonyPatch("GeneratePawns", MethodType.Normal)]
[HarmonyPatch([
    typeof(PawnGroupMakerParms),
    typeof(PawnGroupMaker),
    typeof(List<Pawn>),
    typeof(bool)
])]
public static class Patch_PawnGroupKindWorker_GeneratePawns
{
    [HarmonyPostfix] 
    public static void Postfix(PawnGroupMakerParms parms,
        PawnGroupMaker groupMaker,
        List<Pawn> outPawns,
        bool errorOnZeroResults)
    {
        if (parms.faction?.def != FactionDefOf.Empire
            || parms.points < StrongerEmpireMod.settings.startGeneModdingRaidPointThreshold)
            return;
        if (outPawns == null)
        {
            Log.ErrorOnce($"[{Initialize.ModName}]: Pawn list unexpectedly null during generation! Aborting patch", 2589631);
            return;
        }
        
        foreach (var pawn in outPawns)
        {
            if (!pawn.kindDef.isFighter)
                continue;

            if (StrongerEmpireMod.settings.enableMilitaryTraining)
                PawnModificator.AddMilitaryTraining(pawn);

            if (StrongerEmpireMod.settings.enableLuciferiumInfusion)
                PawnModificator.AddLuciferium(pawn);

            if (StrongerEmpireMod.settings.enableUnwaveringlyLoyalSoldiers)
                PawnModificator.MakeUnwaveringlyLoyal(pawn);

            if(ModsConfig.OdysseyActive && StrongerEmpireMod.settings.uniqueWeaponSpawnChance > 0f)
                PawnModificator.MaybeGiveUniqueWeapon(pawn);

            PawnModificator.FixServerity(pawn);
        }
    }
}
