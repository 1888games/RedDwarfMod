using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using WatcherMod.Commands;
using WatcherMod.Models.Stances;

namespace WatcherMod.Models.Cards;

public sealed class InnerPeace() : CardModel(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(3)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<CalmStance>()
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var isInCalm = Owner.Creature.Powers.OfType<CalmStance>().Any();

        if (isInCalm)
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        else
            // Enter Wrath
            await ChangeStanceCmd.Execute(Owner.Creature, ModelDb.Power<CalmStance>(), choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1m);
    }
}