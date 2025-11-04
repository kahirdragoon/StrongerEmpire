using HarmonyLib;
using RimWorld;
using System.Linq;
using Verse;

namespace StrongerEmpire
{
    [StaticConstructorOnStartup]
    public class Initialize
    {
        public const string ModName = "StrongerEmpire";

        static Initialize()
        {
            var harmony = new Harmony(ModName);
            harmony.PatchAll();

            PawnKindModificator.MultiplyCombatPower();
        }
    }
}