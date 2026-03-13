using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Ancients;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using WatcherMod.Models.Characters;

[HarmonyPatch(typeof(TheArchitect), "DefineDialogues")]
public static class ArchitectDialoguePatch
{
    private static void Postfix(ref AncientDialogueSet __result)
    {
        var dict = __result.CharacterDialogues;
        var watcherKey = ModelDb.Character<Watcher>().Id.Entry;

        if (dict.ContainsKey(watcherKey))
            return;
        dict[watcherKey] = new List<AncientDialogue>
        {
            new("", "") { VisitIndex = 0, EndAttackers = ArchitectAttackers.Both },
            new("", "", "", "") { VisitIndex = 1, EndAttackers = ArchitectAttackers.Both, IsRepeating = true }
        };
    }
}