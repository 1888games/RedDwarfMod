using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Saves.Managers;
using WatcherMod.Models.Characters;

namespace WatcherMod.Patches;

[HarmonyPatch]
public static class ProgressSaveManagerCustomCharPatch
{
    [HarmonyPrefix]
    private static bool SkipEpochChecksForCustomCharacter(Player localPlayer)
    {
        return localPlayer.Character is not Watcher;
    }

    public static void Apply(Harmony harmony)
    {
        var privateMethods = new[]
        {
            "CheckFifteenElitesDefeatedEpoch",
            "CheckFifteenBossesDefeatedEpoch",
            "PostRunUnlockCharacterEpochCheck",
            "CheckAscensionOneCompleted"
        };

        foreach (var methodName in privateMethods)
        {
            var method = AccessTools.Method(typeof(ProgressSaveManager), methodName, [typeof(Player)]);
            if (method != null)
                harmony.Patch(method,
                    new HarmonyMethod(typeof(ProgressSaveManagerCustomCharPatch),
                        nameof(SkipEpochChecksForCustomCharacter)));
        }
    }
}