using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace StrongerEmpire;

[DefOf]
public static class EmpireDefOf
{
    public static HediffDef Empire_MilitaryTraining;

    // Vanilla
    public static ThingDef Turret_Autocannon;
    public static ThingDef TrapIED_HighExplosive;

    public static HediffDef LuciferiumHigh;
    public static HediffDef LuciferiumAddiction;

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
    
    [MayRequireBiotech]
    public static XenotypeDef Sanguophage;

    [MayRequireRoyalty]
    public static PawnKindDef Empire_Royal_Baron;
    [MayRequireRoyalty]
    public static PawnKindDef Empire_Royal_Praetor;
    [MayRequireRoyalty]
    public static PawnKindDef Empire_Royal_Knight;



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
    [MayRequire("vanillaracesexpanded.sanguophage")]
    public static XenotypeDef VRE_Strigoi;
    [MayRequire("vanillaracesexpanded.sanguophage")]
    public static XenotypeDef VRE_Ekkimian;
    [MayRequire("vanillaracesexpanded.sanguophage")]
    public static XenotypeDef VRE_Bruxa;

    static EmpireDefOf() => DefOfHelper.EnsureInitializedInCtor(typeof(EmpireDefOf));
}
