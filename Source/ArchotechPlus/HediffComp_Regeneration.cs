using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse;

namespace ArchotechPlus;

// ReSharper disable once InconsistentNaming
// Disabled for this project to maintain consistency with RimWorld naming conventions.
public class HediffComp_Regeneration : HediffComp
{
    private const int HourTickInterval = 2500;
    private const int AgeMultiplier = 10;
    private static readonly int _maxHealingCharges = ArchotechPlus.Instance.Settings.MaxHealingCharges;
    private static int _resurrectionCharges;
    private static readonly int _maxResurrectionCharges = ArchotechPlus.Instance.Settings.MaxResurrectionCharges;

    private static readonly HediffDef RegenProgress = DefDatabase<HediffDef>.GetNamed("RegenerationProgress");

    private readonly bool _resurrectorEnabled = ArchotechPlus.Instance.Settings.RegeneratorResurrects;

    private BodyPartRecord _bodyPartRegenerationTarget;

    private int _healingCharges;

    private IntRange _healingCooldownRange = ArchotechPlus.Instance.Settings.HealingRange;

    private int _ticks;
    private int _ticksFullCharge;
    private Hediff _woundRegenerationTarget;

    private static long TargetAgeInTicks => (ArchotechPlus.Instance.Settings.TargetAge * 3600000L) + 1800000L;
    private float PercentageCharged => (float)_ticks / _ticksFullCharge;

    public override string CompTipStringExtra => parent.Severity > 1 ? compTipStringBuilder() : null;

    public override void CompPostMake()
    {
        resetChargingTicks();
    }

    public override void CompPostTick(ref float severityAdjustment)
    {
        _ticks++;
        if (_ticks > _ticksFullCharge)
        {
            chargeRegenerator();
            resetChargingTicks();
        }

        if (_ticks % HourTickInterval == 0)
        {
            longTick();
        }
    }

    private void longTick()
    {
        if (isPawnInjured() && usableHealingCharge())
        {
            if (tryRestoreMissingPart() || tryHealRandomPermanentWound())
            {
                _ticks = 0;
                isPawnInjured();
                return;
            }
        }

        if (ArchotechPlus.Instance.Settings.RegeneratorDeAge)
        {
            reduceAge();
        }
    }

    private void chargeRegenerator()
    {
        if (resurrectorCanCharge())
        {
            _resurrectionCharges++;
        }
        else if (healerCanCharge())
        {
            _healingCharges++;
        }
    }

    private bool resurrectorCanCharge()
    {
        return parent.Severity > 2 && _resurrectorEnabled && _resurrectionCharges < _maxResurrectionCharges;
    }

    private bool healerCanCharge()
    {
        return parent.Severity > 1 && _healingCharges < _maxHealingCharges;
    }

    private bool usableHealingCharge()
    {
        if (_healingCharges <= 0)
        {
            return false;
        }

        _healingCharges--;
        return true;
    }

    public override IEnumerable<Gizmo> CompGetGizmos()
    {
        var baseGizmos = base.CompGetGizmos();
        if (baseGizmos?.Any() == true)
        {
            foreach (var gizmo in base.CompGetGizmos())
            {
                yield return gizmo;
            }
        }

        if (!Prefs.DevMode)
        {
            yield break;
        }

        if (healerCanCharge())
        {
            yield return new Command_Action
            {
                defaultLabel = "ArchotechPlusFillRegen".Translate(),
                defaultDesc = "ArchotechPlusFillRegenTT".Translate(),
                icon = ContentFinder<Texture2D>.Get("UI/Overlays/Arrow"),
                action = delegate { _healingCharges = _maxHealingCharges; }
            };
        }

        if (resurrectorCanCharge())
        {
            yield return new Command_Action
            {
                defaultLabel = "ArchotechPlusFillRessurect".Translate(),
                defaultDesc = "ArchotechPlusFillRessurectTT".Translate(),
                icon = ContentFinder<Texture2D>.Get("UI/Overlays/Arrow"),
                action = delegate { _resurrectionCharges = _maxResurrectionCharges; }
            };
        }
    }

    private void resetChargingTicks()
    {
        _ticks = 0;
        _ticksFullCharge = _healingCooldownRange.RandomInRange * HourTickInterval;
    }

    private bool isPawnInjured()
    {
        _bodyPartRegenerationTarget = findBiggestMissingBodyPart();
        _woundRegenerationTarget = findRandomPermanentWound();
        return _bodyPartRegenerationTarget != null || _woundRegenerationTarget != null;
    }

    private BodyPartRecord findBiggestMissingBodyPart(float minCoverage = 0.0f)
    {
        BodyPartRecord bodyPartRecord = null;
        foreach (var partsCommonAncestor in Pawn.health.hediffSet.GetMissingPartsCommonAncestors()
                     .Where(partsCommonAncestor =>
                         partsCommonAncestor.Part.coverageAbsWithChildren >= (double)minCoverage &&
                         !Pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(partsCommonAncestor.Part) &&
                         (bodyPartRecord == null || partsCommonAncestor.Part.coverageAbsWithChildren >
                             (double)bodyPartRecord.coverageAbsWithChildren)))
        {
            bodyPartRecord = partsCommonAncestor.Part;
        }

        return bodyPartRecord;
    }

    private Hediff findRandomPermanentWound()
    {
        return !Pawn.health.hediffSet.hediffs.Where(hd =>
                hd.def == HediffDefOf.ResurrectionPsychosis || hd.IsPermanent() || hd.def.chronic)
            .TryRandomElement(out var result)
            ? null
            : result;
    }

    private bool tryRestoreMissingPart()
    {
        if (_bodyPartRegenerationTarget == null)
        {
            return false;
        }

        Pawn.health.RestorePart(_bodyPartRegenerationTarget);
        Pawn.health.AddHediff(RegenProgress, _bodyPartRegenerationTarget);
        if (!PawnUtility.ShouldSendNotificationAbout(Pawn))
        {
            return true;
        }

        Messages.Message(
            "ArchotechPlusPartRegenerated".Translate((NamedArgument)parent.LabelCap,
                (NamedArgument)Pawn.LabelShort, (NamedArgument)_bodyPartRegenerationTarget.Label,
                Pawn.Named("PAWN")), Pawn,
            MessageTypeDefOf.PositiveEvent);
        return true;
    }

    private bool tryHealRandomPermanentWound()
    {
        if (_woundRegenerationTarget == null)
        {
            return false;
        }

        _woundRegenerationTarget.Severity = 0.0f;
        if (!PawnUtility.ShouldSendNotificationAbout(Pawn))
        {
            return true;
        }

        Messages.Message(
            "ArchotechPlusMessagePermanentWoundHealed".Translate((NamedArgument)parent.LabelCap,
                (NamedArgument)Pawn.LabelShort, (NamedArgument)_woundRegenerationTarget.Label,
                Pawn.Named("PAWN")), Pawn, MessageTypeDefOf.PositiveEvent);
        return true;
    }

    private void reduceAge()
    {
        if (Pawn.ageTracker.AgeBiologicalTicks < TargetAgeInTicks)
        {
            return;
        }

        Pawn.ageTracker.AgeBiologicalTicks -= HourTickInterval * AgeMultiplier;
    }

    public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
    {
        if (_resurrectionCharges > 0)
        {
            spendResurrectorCharge();
            createResurrector();
        }
        else
        {
            if (PawnUtility.ShouldSendNotificationAbout(Pawn))
            {
                Messages.Message(
                    "ArchotechPlusNoResurrectorCharges".Translate(
                        (NamedArgument)parent.LabelCap,
                        (NamedArgument)Pawn.LabelShort,
                        Pawn.Named("PAWN")), Pawn, MessageTypeDefOf.NegativeEvent);
            }
        }

        base.Notify_PawnDied(dinfo, culprit);
    }

    private void spendResurrectorCharge()
    {
        --_resurrectionCharges;
        _ticks = 0;
        if (!PawnUtility.ShouldSendNotificationAbout(Pawn))
        {
            return;
        }

        Messages.Message(
            "ArchotechPlusResurrectionChargeSpent".Translate(
                (NamedArgument)parent.LabelCap,
                (NamedArgument)Pawn.LabelShort,
                Pawn.Named("PAWN")), Pawn, MessageTypeDefOf.PositiveEvent);
    }

    private void createResurrector()
    {
        var resurrectionTracker = (ThingWithComps)GenSpawn.Spawn(ThingDef.Named("ResurrectorTracker"),
            Pawn.Corpse.Position, Pawn.Corpse.Map);
        resurrectionTracker.GetComp<CompResurrector>().Corpse = Pawn.Corpse;
        resurrectionTracker.GetComp<CompFollowsTarget>().Target = Pawn.Corpse;
    }

    public override void CompExposeData()
    {
        Scribe_Values.Look(ref _ticks, "ticksToHeal");
        Scribe_Values.Look(ref _ticksFullCharge, "ticksFullCharge");
        Scribe_Values.Look(ref _healingCharges, "healingCharges");
        Scribe_Values.Look(ref _resurrectionCharges, "resurrectionCharges");
    }

    public override string CompDebugString()
    {
        return
            $"Ticks: {_ticks}\nTicksToFullCharge{_ticksFullCharge}\nTargetAge: {ArchotechPlus.Instance.Settings.TargetAge}";
    }

    private string compTipStringBuilder()
    {
        var tipBuilder = new StringBuilder();
        if (parent.Severity > 2 && _resurrectorEnabled)
        {
            tipBuilder.AppendLine(
                $"Resurrector {(_resurrectionCharges > 0 ? $"charged({_resurrectionCharges}x)" : "not charged")}");
        }

        tipBuilder.AppendLine(
            $"Healer {(_healingCharges > 0 ? $"charged({_healingCharges}x)" : "not charged")}");
        if (_bodyPartRegenerationTarget != null)
        {
            tipBuilder.AppendLine($"Injury targeted: {_bodyPartRegenerationTarget.LabelCap}");
        }
        else if (_woundRegenerationTarget != null)
        {
            tipBuilder.AppendLine(
                $"Injury targeted: {_woundRegenerationTarget.LabelCap}({_woundRegenerationTarget.Part.LabelCap})");
        }

        if (resurrectorCanCharge() || healerCanCharge())
        {
            tipBuilder.Append($"{PercentageCharged.ToStringPercent()} charged");
        }

        return tipBuilder.ToString();
    }
}