using HarmonyLib;
using RimWorld;
using Verse;

namespace ArchotechPlus
{
    [HarmonyPatch(typeof(CompUseEffect_InstallImplant), "CanBeUsedBy")]
    public static class CompUseEffect_InstallImplant_CanBeUsedBy
    {
        /// <summary>
        ///     This is the best place for games with the run in background is turned on.
        /// </summary>
        [HarmonyPrefix]
        public static bool Prefix(ref Pawn p, CompUseEffect_InstallImplant __instance, ref bool __result)
        {
            if (__instance.Props.hediffDef.defName != "ArchotechRegenerator")
            {
                return true;
            }

            if ((!p.IsFreeColonist || p.HasExtraHomeFaction()) && !__instance.Props.allowNonColonists)
            {
                return false;
            }

            if (p.RaceProps.body.GetPartsWithDef(__instance.Props.bodyPart).FirstOrFallback() == null)
            {
                return false;
            }

            var existingImplant = __instance.GetExistingImplant(p);
            if (existingImplant != null)
            {
                if (!__instance.Props.canUpgrade)
                {
                    return false;
                }

                var hediff_Level = (Hediff_LevelWithComps) existingImplant;
                if (hediff_Level.level >= hediff_Level.def.maxSeverity)
                {
                    return false;
                }
            }

            __result = true;
            return false;
        }
    }
}