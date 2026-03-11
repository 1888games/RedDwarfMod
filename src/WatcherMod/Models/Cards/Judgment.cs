using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace WatcherMod.Models.Cards;

public sealed class Judgment() : CardModel(1, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("DamageThreshold", 30)
    ];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null)
            return;

        var target = cardPlay.Target;


        if (target.CurrentHp <= DynamicVars["DamageThreshold"].IntValue)
        {
            target.SetCurrentHpInternal(0);
            await CreatureCmd.Kill(target);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["DamageThreshold"].UpgradeValueBy(10m);
    }
}