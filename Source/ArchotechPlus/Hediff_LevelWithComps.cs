using UnityEngine;
using Verse;

namespace ArchotechPlus
{
    public class Hediff_LevelWithComps : HediffWithComps
    {
        // Token: 0x04000E7B RID: 3707
        public int level = 1;

        public override string Label => def.label + " (" + "Level".Translate() + " " + level + ")";

        // Token: 0x170003EA RID: 1002
        // (get) Token: 0x0600137E RID: 4990 RVA: 0x0006F839 File Offset: 0x0006DA39
        public override bool ShouldRemove => level == 0;

        // Token: 0x06001380 RID: 4992 RVA: 0x0006F86F File Offset: 0x0006DA6F
        public override void Tick()
        {
            base.Tick();
            Severity = level;
        }

        // Token: 0x06001381 RID: 4993 RVA: 0x0006F884 File Offset: 0x0006DA84
        public virtual void ChangeLevel(int levelOffset)
        {
            level = (int) Mathf.Clamp(level + levelOffset, def.minSeverity, def.maxSeverity);
        }

        // Token: 0x06001382 RID: 4994 RVA: 0x0006F8B1 File Offset: 0x0006DAB1
        public virtual void SetLevelTo(int targetLevel)
        {
            if (targetLevel != level)
            {
                ChangeLevel(targetLevel - level);
            }
        }

        // Token: 0x06001383 RID: 4995 RVA: 0x0006F8CC File Offset: 0x0006DACC
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref level, "level");
        }
    }
}