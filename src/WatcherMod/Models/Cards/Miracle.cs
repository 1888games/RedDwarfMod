using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace WatcherMod.Models.Cards;

public sealed class Miracle() : CardModel(0, CardType.Skill, CardRarity.Token, TargetType.Self)
{
    public override CardPoolModel Pool => ModelDb.CardPool<TokenCardPool>();

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new EnergyVar(1)
    ];


    public override HashSet<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Retain,
        CardKeyword.Exhaust
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Energy.UpgradeValueBy(1m);
    }
}