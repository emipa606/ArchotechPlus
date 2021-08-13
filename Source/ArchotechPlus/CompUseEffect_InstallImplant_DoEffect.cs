using HarmonyLib;
using RimWorld;
using Verse;

namespace ArchotechPlus
{
    [HarmonyPatch(typeof(CompUseEffect_InstallImplant), "DoEffect")]
    public static class CompUseEffect_InstallImplant_DoEffect
    {
        /// <summary>
        ///     This is the best place for games with the run in background is turned on.
        /// </summary>
        [HarmonyPrefix]
        public static bool Prefix(ref Pawn user, CompUseEffect_InstallImplant __instance)
        {
            if (__instance.Props.hediffDef.defName != "ArchotechRegenerator")
            {
                return true;
            }

            var bodyPartRecord = user.RaceProps.body.GetPartsWithDef(__instance.Props.bodyPart).FirstOrFallback();
            if (bodyPartRecord == null)
            {
                return false;
            }

            var firstHediffOfDef = user.health.hediffSet.GetFirstHediffOfDef(__instance.Props.hediffDef);
            if (firstHediffOfDef == null)
            {
                user.health.AddHediff(__instance.Props.hediffDef, bodyPartRecord);
                return false;
            }

            if (__instance.Props.canUpgrade)
            {
                ((Hediff_LevelWithComps) firstHediffOfDef).ChangeLevel(1);
            }

            return false;
        }
    }
}