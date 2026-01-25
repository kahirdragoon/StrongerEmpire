using RimWorld;
using System;
using UnityEngine;
using Verse;
using Verse.Sound;
using static UnityEngine.UI.CanvasScaler;

namespace StrongerEmpire;

public class StrongerEmpireMod : Mod
{
    public static StrongerEmpireSettings settings;
    private string genesAddedperStepBuffer;

    public StrongerEmpireMod(ModContentPack content) : base(content)
    {
        settings = GetSettings<StrongerEmpireSettings>();
    }

    public override void WriteSettings()
    {
        base.WriteSettings();

        PawnKindModificator.MultiplyCombatPower();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        Listing_Standard listing = new Listing_Standard();
        listing.Begin(inRect);

        DocombatPowerMultiplicatorSettings(listing);
        listing.GapLine();

        DoMilitaryTrainingSettings(listing);
        listing.GapLine();

        DoLuciferiumSettings(listing);
        listing.GapLine();

        DoUnwaveringLoyalSettings(listing);
        listing.GapLine();

        if (ModsConfig.BiotechActive)
            DoGeneSettings(listing);

        if (ModsConfig.OdysseyActive)
            DoUniqueWeaponSettings(listing);

        listing.End();
    }

    private void DoGeneSettings(Listing_Standard listing)
    {
        listing.CheckboxLabeled("Enable gene modification", ref settings.enableGeneModification);

        if (!settings.enableGeneModification)
            return;

        listing.Label($"Minimum raid points: {Mathf.RoundToInt(settings.startGeneModdingRaidPointThreshold)}",
            tooltip: "Raid Points at which the empire will start sending in gene modded pawn.");

        // Slider (0 to 10000, step of 10)
        settings.startGeneModdingRaidPointThreshold = (int)(Mathf.Round(
            listing.Slider(settings.startGeneModdingRaidPointThreshold, 0f, 10000f) / 10f) * 10f);

        listing.Label($"Raid Point threshold for adding new genes: {Mathf.RoundToInt(settings.geneModificationRaidPointStep)}",
            tooltip: "Every x (this setting) raid points increase y (next setting) new gnes will be added.");

        // Slider (0 to 10000, step of 10)
        settings.geneModificationRaidPointStep = (int)(Mathf.Round(
            listing.Slider(settings.geneModificationRaidPointStep, 0f, 10000f) / 10f) * 10f);

        listing.Label("Genes added per step:");
        listing.TextFieldNumeric<int>(ref settings.genesAddedperStep, ref genesAddedperStepBuffer, 1);

        listing.Gap();

        listing.CheckboxLabeled("Enable Biotech genes", ref settings.enableBiotechGeneSupport);

        if(EnabledMods.VRE_HussarActive)
            listing.CheckboxLabeled("Enable VRE Hussar genes", ref settings.enableVREHussarGeneSupport);
    }

    private void DoLuciferiumSettings(Listing_Standard listing)
    {
        listing.CheckboxLabeled("Enable luciferium infusion", ref settings.enableLuciferiumInfusion);

        if (!settings.enableLuciferiumInfusion)
            return;

        listing.Label($"Minimum raid points: {Mathf.RoundToInt(settings.luciferiumRaidPointsThreshold)}");
        // Slider (0 to 10000, step of 10)
        settings.luciferiumRaidPointsThreshold = (int)(Mathf.Round(
            listing.Slider(settings.luciferiumRaidPointsThreshold, 0f, 10000f) / 10f) * 10f);

        listing.Label($"Chance: {(int)(settings.luciferiumChance * 100)}%");
        // Slider (0 to 1, step of 0.01)
        settings.luciferiumChance = Mathf.Round(listing.Slider(settings.luciferiumChance, 0f, 1f) / 0.01f) * 0.01f;
    }

    private void DoMilitaryTrainingSettings(Listing_Standard listing)
    {
        listing.CheckboxLabeled("Enable military training", ref settings.enableMilitaryTraining);
    }

    private void DoUnwaveringLoyalSettings(Listing_Standard listing)
    {
        listing.CheckboxLabeled("Make all empire pawns unwaveringly loyal", ref settings.enableUnwaveringlyLoyalSoldiers);
    }

    private void DoUniqueWeaponSettings(Listing_Standard listing)
    {
        listing.Label($"Chance to replace weapon with unique weapon: {Mathf.Round(settings.uniqueWeaponSpawnChance * 10000) / 100}%");
        // Slider (0 to 1, step of 0.001)
        settings.uniqueWeaponSpawnChance = Mathf.Round(listing.Slider(settings.uniqueWeaponSpawnChance, 0f, 1f) / 0.001f) * 0.001f;
    }

    private void DocombatPowerMultiplicatorSettings(Listing_Standard listing)
    {
        listing.Label($"Combat Power Multiplicator: {settings.combatPowerMultiplicator}");
        // Slider (0.1 to 10, step of 0.5)
        settings.combatPowerMultiplicator = Mathf.Round(listing.Slider(settings.combatPowerMultiplicator, 0.1f, 10f) / 0.1f) * 0.1f;
    }

    public override string SettingsCategory()
    {
        return "The Empire strikes back";
    }
}
