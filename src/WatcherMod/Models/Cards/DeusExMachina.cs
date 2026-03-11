using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace WatcherMod.Models.Cards;

public sealed class DeusExMachina() : CardModel(-1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override HashSet<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable];


    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(2)
    ];


    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromCard<Miracle>()
    ];

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel drawnCard, bool fromHandDraw)
    {
        if (drawnCard != this)
            return;
        var miracleCount = DynamicVars.Cards.IntValue;

        // Create that many Miracle cards
        var miracles = new List<CardModel>();
        for (var i = 0; i < miracleCount; i++)
        {
            var miracle = CombatState?.CreateCard<Miracle>(Owner);
            if (miracle != null)
                miracles.Add(miracle);
        }

        if (miracles.Count > 0) await CardPileCmd.AddGeneratedCardsToCombat(miracles, PileType.Hand, true);

        // Exhaust this card from wherever it is
        await CardPileCmd.Add(this, PileType.Exhaust, CardPilePosition.Top);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1m);
    }
}