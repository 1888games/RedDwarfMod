using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using WatcherMod.Commands;
using WatcherMod.Models.Stances;

namespace WatcherMod.Models.Cards;

public sealed class Crescendo() : CardModel(1, CardType.Skill, CardRarity.Common, TargetType.None)
{
    public override HashSet<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Retain,
        CardKeyword.Exhaust
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await ChangeStanceCmd.Execute(Owner.Creature, ModelDb.Power<WrathStance>(), choiceContext);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}