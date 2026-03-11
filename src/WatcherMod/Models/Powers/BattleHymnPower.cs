using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using WatcherMod.Models.Cards;

namespace WatcherMod.Models.Powers;

public sealed class BattleHymnPower : PowerModel
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, CombatState combatState)
    {
        var insightCards = new List<CardModel>();
        // Add Amount Insight cards to hand
        for (var i = 0; i < Amount; i++) insightCards.Add(CombatState.CreateCard<Smite>(player));
        // Add to hand at top position
        CardCmd.PreviewCardPileAdd(
            await CardPileCmd.AddGeneratedCardsToCombat(
                insightCards,
                PileType.Hand,
                false,
                CardPilePosition.Top
            )
        );
        Flash();
    }
}