using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using WatcherMod.Models.Cards;

namespace WatcherMod.Patches;

[HarmonyPatch]
public class TokenCardPoolPatch
{
    private static MethodBase TargetMethod()
    {
        return AccessTools.Method(typeof(TokenCardPool), "GenerateAllCards");
    }

    [HarmonyPostfix]
    public static void Postfix(ref CardModel[] __result)
    {
        // Add new cards to the array
        var adds = new CardModel[]
        {
            ModelDb.Card<Insight>(),
            ModelDb.Card<Smite>(),
            ModelDb.Card<Safety>(),
            ModelDb.Card<Miracle>(),
            ModelDb.Card<ThroughViolence>(),
            ModelDb.Card<Beta>(),
            ModelDb.Card<Omega>(),
            ModelDb.Card<Expunger>(),
            ModelDb.Card<BecomeAlmighty>(),
            ModelDb.Card<LiveForever>(),
            ModelDb.Card<FameAndFortune>()
        };

        __result = __result.Concat(adds).ToArray();
    }
}