using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Saves.Managers;
using WatcherMod.Models.Characters;

namespace WatcherMod.Patches;

[HarmonyPatch]
public static class ProgressSaveManagerCustomCharPatch
{
    private static bool SkipEpochChecksForCustomCharacter(Player localPlayer)
    {
        // skip original method if the player is a custom character
        return !(localPlayer.Character is Watcher);
    }

    public static void Apply(Harmony harmony)
    {
        string[] privateMethods = new[]
        {
            "CheckFifteenElitesDefeatedEpoch",
            "CheckFifteenBossesDefeatedEpoch",
            "PostRunUnlockCharacterEpochCheck",
            "CheckAscensionOneCompleted"
        };

        foreach (var methodName in privateMethods)
        {
            var method = AccessTools.Method(typeof(ProgressSaveManager), methodName, new[] { typeof(Player) });
            if (method != null)
                harmony.Patch(method,
                    new HarmonyMethod(typeof(ProgressSaveManagerCustomCharPatch),
                        nameof(SkipEpochChecksForCustomCharacter)));
        }
    }
}