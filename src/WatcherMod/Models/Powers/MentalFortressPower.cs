using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using WatcherMod.Commands;

namespace WatcherMod.Models.Powers;

public sealed class MentalFortressPower : PowerModel
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    /// <summary>
    ///     Subscribe to stance changes when this power is applied.
    /// </summary>
    public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        await base.AfterApplied(applier, cardSource);

        // Subscribe to stance changes
        ChangeStanceCmd.StanceChanged += OnStanceChanged;
    }

    /// <summary>
    ///     Unsubscribe from stance changes when this power is removed.
    /// </summary>
    public override async Task AfterRemoved(Creature owner)
    {
        await base.AfterRemoved(owner);

        // Unsubscribe to prevent memory leaks
        ChangeStanceCmd.StanceChanged -= OnStanceChanged;
    }

    /// <summary>
    ///     Called whenever any creature changes stance.
    ///     Gain block if it's the owner who changed stance.
    /// </summary>
    private async Task OnStanceChanged(Creature creature, PlayerChoiceContext? context)
    {
        // Only trigger for the owner of this power
        if (creature != Owner)
            return;

        // Gain Amount block (4 by default, more if upgraded)
        await CreatureCmd.GainBlock(
            Owner,
            Amount,
            ValueProp.Unpowered, // Not from a card
            null // No card play
        );
    }
}