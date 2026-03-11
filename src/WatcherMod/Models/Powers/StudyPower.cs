using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using WatcherMod.Models.Cards;

namespace WatcherMod.Models.Powers;

public class StudyPower : PowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (Owner.Player == null || Owner.Player?.Creature.Side != side) return;


        var insightCard = CombatState.CreateCard<Insight>(Owner.Player!);

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
}