using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace WatcherMod.Models.Cards;

public sealed class ScrawlWatcher() : CardModel(1, CardType.Skill, CardRarity.Rare, TargetType.None)
{
    public override HashSet<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var hand = PileType.Hand.GetPile(Owner);
        while (hand.Cards.Count < CardPile.maxCardsInHand)
        {
            await CardPileCmd.Draw(choiceContext, 1, Owner);


            if (PileType.Draw.GetPile(Owner).IsEmpty && PileType.Discard.GetPile(Owner).IsEmpty)
                break;
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}