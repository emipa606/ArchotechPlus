using HarmonyLib;
using RimWorld;
using Verse;

namespace ArchotechPlus;

[HarmonyPatch(typeof(CompUseEffect_InstallImplant), nameof(CompUseEffect_InstallImplant.CanBeUsedBy))]
public static class CompUseEffect_InstallImplant_CanBeUsedBy
{
    /// <summary>
    ///     This is the best place for games with the run in background is turned on.
    /// </summary>
    [HarmonyPrefix]
    public static bool Prefix(ref Pawn p, CompUseEffect_InstallImplant __instance, ref AcceptanceReport __result)
    {
        if (__instance.Props.hediffDef.defName != "ArchotechRegenerator")
        {
            return true;
        }

        if ((!p.IsFreeColonist || p.HasExtraHomeFaction()) && !__instance.Props.allowNonColonists)
        {
            __result = false;
            return false;
        }

        if (p.RaceProps.body.GetPartsWithDef(__instance.Props.bodyPart).FirstOrFallback() == null)
        {
            __result = false;
            return false;
        }

        var existingImplant = __instance.GetExistingImplant(p);
        if (existingImplant == null)
        {
            __result = true;
            return false;
        }

        if (!__instance.Props.canUpgrade)
        {
            __result = false;
            return false;
        }

        // Upgrade
        switch (existingImplant)
        {
            // Old hack for previous versions (This is not needed in 1.5+)
            case Hediff_LevelWithComps compatibility:
                __result = compatibility.def.maxSeverity > compatibility.level;
                return false;
            // New level version
            case Hediff_Level hediffLevel:
                __result = hediffLevel.def.maxSeverity > hediffLevel.level;
                return false;
            // No existing implant
            default:
                __result = true;
                return false;
        }
    }
}