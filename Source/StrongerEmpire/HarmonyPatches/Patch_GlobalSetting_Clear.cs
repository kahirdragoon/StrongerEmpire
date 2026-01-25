using HarmonyLib;
using RimWorld.BaseGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongerEmpire.HarmonyPatches;

[HarmonyPatch(typeof(GlobalSettings), nameof(GlobalSettings.Clear))]
public static class Patch_GlobalSetting_Clear
{
    public static void Postfix()
    {
        BaseGen_GlobalSettingsEmpire.Clear();
    }
}