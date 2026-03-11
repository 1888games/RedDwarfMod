using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using WatcherMod.src.WatcherMod.Models.Cards.@as;

namespace WatcherMod.Models.Cards;

public sealed class WishWatcher() : CardModel(3, CardType.Skill, CardRarity.Rare, TargetType.None)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new GoldVar(25), new PowerVar<StrengthPower>(3), new PowerVar<PlatingPower>(6)];


    public override HashSet<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cardsToChoose = new List<WishModel>
        {
            (ModelDb.Card<BecomeAlmighty>().MutableClone() as WishModel)!,
            (ModelDb.Card<FameAndFortune>().MutableClone() as WishModel)!,
            (ModelDb.Card<LiveForever>().MutableClone() as WishModel)!
        };

        foreach (var c in cardsToChoose)
        {
            c.Owner = Owner;

            if (IsUpgraded)
                CardCmd.Upgrade(c);
        }

        var card = await CardSelectCmd.FromChooseACardScreen(
            choiceContext,
            cardsToChoose,
            Owner
        );

        if (card == null)
            return;

        var wish = card as WishModel;

        if (wish != null)
            await wish.OnWish(choiceContext, cardPlay);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Gold.UpgradeValueBy(5);
        DynamicVars["PlatingPower"].UpgradeValueBy(2);
        DynamicVars.Strength.UpgradeValueBy(1);
    }
}