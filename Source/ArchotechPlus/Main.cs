using System.Reflection;
using HarmonyLib;
using Verse;

namespace ArchotechPlus
{
    [StaticConstructorOnStartup]
    public static class Main
    {
        static Main()
        {
            var harmony = new Harmony("Mlie.ArchotechPlus");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Log.Message("[ArchotechPlus] Patched Hediff applyer");
        }
    }
}