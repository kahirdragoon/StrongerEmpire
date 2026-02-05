using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace StrongerEmpire;

[DefOf]
public static class EmpireThingDefOf
{
    // Vanilla
    public static ThingDef Turret_Autocannon;
    public static ThingDef TrapIED_HighExplosive;
    [MayRequireBiotech]
    public static ThingDef Hemopump;
    [MayRequireBiotech]
    public static ThingDef HemogenAmplifier;
    [MayRequireBiotech]
    public static ThingDef PsychofluidPump;
    [MayRequireBiotech]
    public static ThingDef GlucosoidPump;
    [MayRequireBiotech]
    public static ThingDef DeathrestAccelerator;

    // VRE Sanguaphage
    [MayRequire("vanillaracesexpanded.sanguophage")]
    public static ThingDef VRE_InvocationMatrix;
    [MayRequire("vanillaracesexpanded.sanguophage")]
    public static ThingDef VRE_HemodynamicAccelerator;
    [MayRequire("vanillaracesexpanded.sanguophage")]
    public static ThingDef VRE_HemogenSolidifier;
    [MayRequire("vanillaracesexpanded.sanguophage")]
    public static ThingDef VRE_SmallHemogenAmplifier;
    [MayRequire("vanillaracesexpanded.sanguophage")]
    public static ThingDef VRE_SmallHemopump;


    static EmpireThingDefOf() => DefOfHelper.EnsureInitializedInCtor(typeof(EmpireThingDefOf));
}
