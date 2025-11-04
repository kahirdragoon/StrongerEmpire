using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace StrongerEmpire
{
    internal static class PawnKindModificator
    {
        internal static void MultiplyCombatPower()
        {
            DefDatabase<PawnKindDef>.AllDefsListForReading
                .Where(pk => pk.defaultFactionDef == FactionDefOf.Empire && pk.combatPower > 0)
                .ToList()
                .ForEach(pk => pk.combatPower *= StrongerEmpireMod.settings.combatPowerMultiplicator);
        }
    }
}
