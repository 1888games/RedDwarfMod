using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using WatcherMod.Models.Powers;

namespace WatcherMod.Models.Cards;

public sealed class Pray() : CardModel(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<MantraPower>(),
        HoverTipFactory.FromCard<Insight>()
    ];


    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<MantraPower>(3m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<MantraPower>(
            Owner.Creature,
            DynamicVars["MantraPower"].IntValue,
            Owner.Creature,
            this
        );

        // Create Insight card
        if (CombatState == null) return;
        var insightCard = CombatState.CreateCard<Insight>(Owner);

        // Shuffle it into draw pile (Random position)
        CardCmd.PreviewCardPileAdd(
            await CardPileCmd.AddGeneratedCardToCombat(
                insightCard,
                PileType.Draw,
                true,
                CardPilePosition.Random
            )
        );
    }

    protected override void OnUpgrade()
    {
        DynamicVars["MantraPower"].UpgradeValueBy(1m);
    }
}