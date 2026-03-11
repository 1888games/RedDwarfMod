using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace WatcherMod.Models.Cards;

public sealed class FollowUp() : CardModel(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override bool ShouldGlowGoldInternal => WasLastCardPlayedAttack;

    private bool WasLastCardPlayedAttack
    {
        get
        {
            var lastCardEntry = CombatManager.Instance.History.CardPlaysStarted
                .LastOrDefault(e =>
                    e.CardPlay.Card.Owner == Owner &&
                    e.HappenedThisTurn(CombatState) &&
                    e.CardPlay.Card != this);

            if (lastCardEntry == null) return false;

            return lastCardEntry.CardPlay.Card.Type == CardType.Attack;
        }
    }


    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(7m, ValueProp.Move)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        if (WasLastCardPlayedAttack) Owner.PlayerCombatState!.GainEnergy(1);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4m);
    }
}