using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Screens.Shops;

[HarmonyPatch(typeof(NMerchantCharacter))]
public static class NMerchantCharacterPatch
{
    // Patch the PlayAnimation method
    [HarmonyPrefix]
    [HarmonyPatch(nameof(NMerchantCharacter.PlayAnimation))]
    public static bool PlayAnimation_Prefix(NMerchantCharacter __instance, string anim, bool loop)
    {
        if (__instance.GetChildCount() == 0)
            return false; // skip original method

        var child = __instance.GetChild(0) as Node2D;
        return child != null && child.GetClass() == "SpineSprite";
    }
}