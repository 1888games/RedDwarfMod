using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;

namespace WatcherMod.Models.Powers;

public sealed class RushdownPower : PowerModel
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        await base.AfterApplied(applier, cardSource);
        ChangeStanceCmd.StanceChanged += OnStanceChanged;
    }

    public override async Task AfterRemoved(Creature owner)
    {
        await base.AfterRemoved(owner);
        ChangeStanceCmd.StanceChanged -= OnStanceChanged;
    }

    public override async Task AfterCombatEnd(CombatRoom room)
    {
        await base.AfterCombatEnd(room);
        ChangeStanceCmd.StanceChanged -= OnStanceChanged;
    }

    private async Task OnStanceChanged(Creature creature, PlayerChoiceContext? context)
    {
        if (creature != Owner)
            return;

        // Check if we entered Wrath
        var isInWrath = Owner.Powers.OfType<WrathStance>().Any();
        if (!isInWrath)
            return;


        var player = Owner.Player;
        if (player == null || context == null)
            return;


        // Draw Amount cards using proper context!
        await CardPileCmd.Draw(context, Amount, player);
        Flash();
    }
}