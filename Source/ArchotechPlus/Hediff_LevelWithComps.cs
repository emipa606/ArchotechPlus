using UnityEngine;
using Verse;

namespace ArchotechPlus;

public class Hediff_LevelWithComps : HediffWithComps
{
    public int level = 1;

    public override string Label => $"{$"{def.label} (" + "Level".Translate() + " "}{level})";

    public override bool ShouldRemove => level == 0;

    public override void Tick()
    {
        base.Tick();
        Severity = level;
    }

    public virtual void ChangeLevel(int levelOffset)
    {
        level = (int)Mathf.Clamp(level + levelOffset, def.minSeverity, def.maxSeverity);
    }

    public virtual void SetLevelTo(int targetLevel)
    {
        if (targetLevel != level)
        {
            ChangeLevel(targetLevel - level);
        }
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref level, "level");
    }
}