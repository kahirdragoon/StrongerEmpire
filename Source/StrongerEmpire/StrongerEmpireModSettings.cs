using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace StrongerEmpire
{
    public class StrongerEmpireSettings : ModSettings
    {
        // Gene modding
        public const int StartGeneModdingRaidPointThresholdDefault = 1000;
        public const int GeneModificationPointThresholdDefault = 400;
        public const int GenesAddedperStepDefault = 2;

        public bool enableGeneModification = true;
        public int geneModificationRaidPointStep = GeneModificationPointThresholdDefault;
        public int genesAddedperStep = GenesAddedperStepDefault;
        public int startGeneModdingRaidPointThreshold = StartGeneModdingRaidPointThresholdDefault;
        public bool enableBiotechGeneSupport = true;
        public bool enableVREHussarGeneSupport = true;

        // Luciferium
        public bool enableLuciferiumInfusion = true;
        public const int luciferiumRaidPointsThresholdDefault = 5000;
        public int luciferiumRaidPointsThreshold = luciferiumRaidPointsThresholdDefault;
        public const float luciferiumChanceDefault = 0.3f;
        public float luciferiumChance = luciferiumChanceDefault;

        // Military Training
        public bool enableMilitaryTraining = true;

        // Unwaveringly loyal
        public bool enableUnwaveringlyLoyalSoldiers = true;

        // unique weapon chance
        public const float uniqueWeaponSpawnChanceDefault = 0.01f;
        public float uniqueWeaponSpawnChance = uniqueWeaponSpawnChanceDefault;

        // Combat Power multiplicator
        public const float combatPowerMultiplicatorDefault = 1f;
        public float combatPowerMultiplicator = combatPowerMultiplicatorDefault;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref enableGeneModification, "enableGeneModification", true);
            Scribe_Values.Look(ref startGeneModdingRaidPointThreshold, "startGeneModdingRaidPointThreshold", StartGeneModdingRaidPointThresholdDefault);
            Scribe_Values.Look(ref geneModificationRaidPointStep, "geneModificationPointThreshold", GeneModificationPointThresholdDefault);
            Scribe_Values.Look(ref genesAddedperStep, "genesAddedperStep", 2);
            Scribe_Values.Look(ref enableBiotechGeneSupport, "enableBiotechGeneSupport", true);
            Scribe_Values.Look(ref enableBiotechGeneSupport, "enableVREHussarGeneSupport", true);

            Scribe_Values.Look(ref enableLuciferiumInfusion, "enableLuciferiumInfusion", true);
            Scribe_Values.Look(ref luciferiumRaidPointsThreshold, "luciferiumRaidPointsThreshold", luciferiumRaidPointsThresholdDefault);
            Scribe_Values.Look(ref luciferiumChance, "luciferiumChance", luciferiumChanceDefault);

            Scribe_Values.Look(ref enableMilitaryTraining, "enableMilitary", true);

            Scribe_Values.Look(ref enableUnwaveringlyLoyalSoldiers, "enableUnvaweringlyLoyalSoldiers", true);

            Scribe_Values.Look(ref uniqueWeaponSpawnChance, "uniqueWeaponSpawnChance", uniqueWeaponSpawnChanceDefault);

            Scribe_Values.Look(ref combatPowerMultiplicator, "combatPowerMultiplicator", combatPowerMultiplicatorDefault);

            base.ExposeData();
        }
    }
}
