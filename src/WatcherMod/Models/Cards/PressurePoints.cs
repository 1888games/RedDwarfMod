using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using WatcherMod.Models.Powers;

namespace WatcherMod.Models.Cards;

public sealed class PressurePoints() : CardModel(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<MarkPower>(8m)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<MarkPower>()
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        // Apply Mark power
        await PowerCmd.Apply<MarkPower>(
            cardPlay.Target,
            DynamicVars["MarkPower"].IntValue,
            Owner.Creature,
            this
        );
        var combatState = cardPlay.Target.CombatState;
        foreach (var enemy in combatState!.Enemies)
        {
            var markPower = enemy.GetPower<MarkPower>();
            if (markPower != null && markPower.Amount > 0)
                await DamageCmd.Attack(markPower.Amount)
                    .FromCard(this)
                    .Targeting(enemy)
                    .Execute(choiceContext);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["MarkPower"].UpgradeValueBy(3m);
    }
}