using System.Reflection;
using HarmonyLib;
using Verse;

namespace ArchotechPlus;

[StaticConstructorOnStartup]
public static class Main
{
    static Main()
    {
        new Harmony("Mlie.ArchotechPlus").PatchAll(Assembly.GetExecutingAssembly());
    }
}