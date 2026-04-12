using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace StrongerEmpire.HarmonyPatches;

// [HarmonyPatch(typeof(MedicalRecipesUtility), nameof(MedicalRecipesUtility.SpawnThingsFromHediffs))]
public static class Patch_MedicalRecipesUtility_SpawnThingsFromHediffs
{
    public static bool Prefix(Pawn pawn, BodyPartRecord part, IntVec3 pos, Map map)
    {
        if (!pawn.health.hediffSet.GetNotMissingParts().Contains(part))
            return false;

        foreach (Hediff hediff in pawn.health.hediffSet.hediffs.Where(x => x.Part == part))
        {
            if (hediff.def.spawnThingOnRemoved == null) continue;

            Thing spawned = GenSpawn.Spawn(hediff.def.spawnThingOnRemoved, pos, map);
            CompBiocodable comp = spawned.TryGetComp<CompBiocodable>();
            if (comp != null && !comp.Biocoded)
                comp.CodeFor(pawn);
        }

        for (int i = 0; i < part.parts.Count; i++)
        {
            MedicalRecipesUtility.SpawnThingsFromHediffs(pawn, part.parts[i], pos, map);
        }

        return false;
    }
}
