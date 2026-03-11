using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using WatcherMod.Models.Powers;

namespace WatcherMod.Models.Cards;

public sealed class LessonLearned() : CardModel(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    // Base damage
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(10m, ValueProp.Move)
    ];

    public override HashSet<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<LikeWaterPower>()
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        var target = cardPlay.Target;
        var damageAmount = DynamicVars.Damage.BaseValue;

        // Apply normal damage
        var damageResults = await DamageCmd.Attack(damageAmount)
            .FromCard(this)
            .Targeting(target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        // Check if the target died from this attack
        if (target.IsDead) UpgradeRandomCard(choiceContext);
        // Trigger the Fatal effect: upgrade a random card in your deck
    }

    private void UpgradeRandomCard(PlayerChoiceContext choiceContext)
    {
        var deckCards = Owner.Deck.Cards.ToList();
        if (deckCards.Count == 0)
            return;

        var randomCard = Owner.RunState.Rng.CombatTargets.NextItem(deckCards);
        if (randomCard != null) CardCmd.Upgrade(randomCard);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m); // 10 -> 13
    }
}