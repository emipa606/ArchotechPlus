using Verse;

namespace ArchotechPlus;

public class ArchotechPlusSettings : ModSettings
{
    public IntRange HealingRange = new(120, 240);
    public int MaxHealingCharges = 1;
    public int MaxResurrectionCharges = 1;
    public bool RegeneratorDeAge = true;
    public bool RegeneratorResurrects = true;
    public IntRange ResurrectionRange = new(12, 96);
    public int TargetAge = 30;

    public override void ExposeData()
    {
        Scribe_Values.Look(ref RegeneratorDeAge, "regeneratorDeAge", true);
        Scribe_Values.Look(ref TargetAge, "targetAge", 28);
        Scribe_Values.Look(ref MaxHealingCharges, "maxHealingCharges", 1);
        Scribe_Values.Look(ref RegeneratorResurrects, "regeneratorResurrects", true);
        Scribe_Values.Look(ref MaxResurrectionCharges, "maxResurrectionCharges", 1);
        Scribe_Values.Look(ref HealingRange, "healingRange", new IntRange(120, 240));
        Scribe_Values.Look(ref ResurrectionRange, "resurrectionRange", new IntRange(12, 96));
        base.ExposeData();
    }
}