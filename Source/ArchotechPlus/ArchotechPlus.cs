using Mlie;
using RimWorld;
using UnityEngine;
using Verse;

namespace ArchotechPlus;

public class ArchotechPlus : Mod
{
    private static string currentVersion;
    public static ArchotechPlus Instance;
    public readonly ArchotechPlusSettings Settings;

    public ArchotechPlus(ModContentPack content) : base(content)
    {
        Instance = this;
        Settings = GetSettings<ArchotechPlusSettings>();
        currentVersion =
            VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        var targetAgeBuffer = Settings.TargetAge.ToString();
        var healingChargesBuffer = Settings.MaxHealingCharges.ToString();
        var resurrectorChargesBuffer = Settings.MaxResurrectionCharges.ToString();

        var listingStandard = new Listing_Standard(GameFont.Small) { ColumnWidth = inRect.width / 3f };
        listingStandard.Begin(inRect);
        listingStandard.CheckboxLabeled("ArchotechPlusDeAge".Translate(), ref Settings.RegeneratorDeAge,
            "ArchotechPlusDeAgeTooltip".Translate());

        if (Settings.RegeneratorDeAge)
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
            ref Settings.RegeneratorResurrects,
            "ArchotechPlusRegeneratorTooltip".Translate());
        if (Settings.RegeneratorResurrects)
        {
            listingStandard.Label("ArchotechPlusRessurectCharges".Translate(), -1,
                "ArchotechPlusRessurectChargesTooltip".Translate());
            listingStandard.Gap(Text.LineHeight / 2);
            listingStandard.Label("ArchotechPlusRessurectTime".Translate(), -1,
                "ArchotechPlusRessurectTimeTooltip".Translate());
        }

        listingStandard.NewColumn();
        listingStandard.Gap(Text.LineHeight + 3f);
        if (Settings.RegeneratorDeAge)
        {
            listingStandard.IntEntry(ref Settings.TargetAge, ref targetAgeBuffer);
        }

        listingStandard.IntEntry(ref Settings.MaxHealingCharges, ref healingChargesBuffer);
        listingStandard.IntRange(ref Settings.HealingRange, 1, 360);
        listingStandard.Gap();
        if (Settings.RegeneratorResurrects)
        {
            listingStandard.Gap(Text.LineHeight + 3f);
            listingStandard.IntEntry(ref Settings.MaxResurrectionCharges,
                ref resurrectorChargesBuffer);
            listingStandard.IntRange(ref Settings.ResurrectionRange, 1, 360);
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
            $"{(Settings.HealingRange.min * 2500).ToStringTicksToPeriod()} - {(Settings.HealingRange.max * 2500).ToStringTicksToPeriod()}");

        if (Settings.RegeneratorResurrects)
        {
            listingStandard.Gap(Text.LineHeight * 3.25f);
            listingStandard.Label(
                $"{(Settings.ResurrectionRange.min * 2500).ToStringTicksToPeriod()} - {(Settings.ResurrectionRange.max * 2500).ToStringTicksToPeriod()}");
        }

        listingStandard.End();
        base.DoSettingsWindowContents(inRect);
    }

    public override string SettingsCategory()
    {
        return "Archotech+";
    }
}