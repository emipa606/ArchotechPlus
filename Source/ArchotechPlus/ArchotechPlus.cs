using Mlie;
using RimWorld;
using UnityEngine;
using Verse;

namespace ArchotechPlus;

public class ArchotechPlus : Mod
{
    private static string currentVersion;
    private readonly ArchotechPlusSettings _settings;

    public ArchotechPlus(ModContentPack content) : base(content)
    {
        _settings = GetSettings<ArchotechPlusSettings>();
        currentVersion =
            VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        var targetAgeBuffer = ArchotechPlusSettings.TargetAge.ToString();
        var healingChargesBuffer = ArchotechPlusSettings.MaxHealingCharges.ToString();
        var resurrectorChargesBuffer = ArchotechPlusSettings.MaxResurrectionCharges.ToString();

        var listingStandard = new Listing_Standard(GameFont.Small) { ColumnWidth = inRect.width / 3f };
        listingStandard.Begin(inRect);
        listingStandard.CheckboxLabeled("ArchotechPlusDeAge".Translate(), ref ArchotechPlusSettings.RegeneratorDeAge,
            "ArchotechPlusDeAgeTooltip".Translate());

        if (ArchotechPlusSettings.RegeneratorDeAge)
        {
            listingStandard.Label("ArchotechPlusAge".Translate(), -1,
                "ArchotechPlusAgeTooltip".Translate());
        }

        listingStandard.Label("ArchotechPlusMaxCharge".Translate(), -1,
            "ArchotechPlusMaxChargeTooltip".Translate());
        listingStandard.Gap(Text.LineHeight / 2);
        listingStandard.Label("ArchotechPlusHealingTime".Translate(), -1,
            "ArchotechPlusHealingTimeTooltip".Translate());
        listingStandard.Gap();
        listingStandard.CheckboxLabeled("ArchotechPlusRegenerator".Translate(),
            ref ArchotechPlusSettings.RegeneratorResurrects,
            "ArchotechPlusRegeneratorTooltip".Translate());
        if (ArchotechPlusSettings.RegeneratorResurrects)
        {
            listingStandard.Label("ArchotechPlusRessurectCharges".Translate(), -1,
                "ArchotechPlusRessurectChargesTooltip".Translate());
            listingStandard.Gap(Text.LineHeight / 2);
            listingStandard.Label("ArchotechPlusRessurectTime".Translate(), -1,
                "ArchotechPlusRessurectTimeTooltip".Translate());
        }

        listingStandard.NewColumn();
        listingStandard.Gap(Text.LineHeight + 3f);
        if (ArchotechPlusSettings.RegeneratorDeAge)
        {
            listingStandard.IntEntry(ref ArchotechPlusSettings.TargetAge, ref targetAgeBuffer);
        }

        listingStandard.IntEntry(ref ArchotechPlusSettings.MaxHealingCharges, ref healingChargesBuffer);
        listingStandard.IntRange(ref ArchotechPlusSettings.HealingRange, 1, 360);
        listingStandard.Gap();
        if (ArchotechPlusSettings.RegeneratorResurrects)
        {
            listingStandard.Gap(Text.LineHeight + 3f);
            listingStandard.IntEntry(ref ArchotechPlusSettings.MaxResurrectionCharges,
                ref resurrectorChargesBuffer);
            listingStandard.IntRange(ref ArchotechPlusSettings.ResurrectionRange, 1, 360);
        }
        else
        {
            listingStandard.Gap(Text.LineHeight);
        }

        if (currentVersion != null)
        {
            listingStandard.Gap();
            GUI.contentColor = Color.gray;
            listingStandard.Label("ArchotechPlusModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listingStandard.NewColumn();
        listingStandard.Gap(Text.LineHeight * 2.75f);
        listingStandard.Label(
            $"{(ArchotechPlusSettings.HealingRange.min * 2500).ToStringTicksToPeriod()} - {(ArchotechPlusSettings.HealingRange.max * 2500).ToStringTicksToPeriod()}");

        if (ArchotechPlusSettings.RegeneratorResurrects)
        {
            listingStandard.Gap(Text.LineHeight * 3.25f);
            listingStandard.Label(
                $"{(ArchotechPlusSettings.ResurrectionRange.min * 2500).ToStringTicksToPeriod()} - {(ArchotechPlusSettings.ResurrectionRange.max * 2500).ToStringTicksToPeriod()}");
        }

        listingStandard.End();
        base.DoSettingsWindowContents(inRect);
    }

    public override void WriteSettings()
    {
        Log.Message($"[Archotech+] Regenerator Target Age: {ArchotechPlusSettings.TargetAge}");
        Log.Message($"[Archotech+] Maximum Healing Charges: {ArchotechPlusSettings.MaxHealingCharges} charges");
        Log.Message(
            $"[Archotech+] Regenerator Resurrection is enabled with {ArchotechPlusSettings.MaxResurrectionCharges} maximum charges");
        base.WriteSettings();
    }

    public override string SettingsCategory()
    {
        return "Archotech+";
    }
}