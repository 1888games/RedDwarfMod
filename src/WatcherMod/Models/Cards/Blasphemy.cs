using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using WatcherMod.Commands;
using WatcherMod.Models.Powers;
using WatcherMod.Models.Stances;

namespace WatcherMod.Models.Cards;

public sealed class Blasphemy() : CardModel(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<DivinityStance>()
    ];

    public override HashSet<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<BlasphemerPower>(Owner.Creature, 1, Owner.Creature, this);
        await ChangeStanceCmd.Execute(Owner.Creature, ModelDb.Power<DivinityStance>(), choiceContext);
    }


    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
}