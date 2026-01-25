using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace StrongerEmpire;

[DefOf]
public static class VanillaThingDefOf
{
    public static ThingDef Turret_Autocannon;
    public static ThingDef TrapIED_HighExplosive;

    static VanillaThingDefOf() => DefOfHelper.EnsureInitializedInCtor(typeof(VanillaThingDefOf));
}
