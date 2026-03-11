using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using WatcherMod.Commands;

namespace WatcherMod.Models.Powers;

public sealed class NirvanaPower : PowerModel
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    /// <summary>
    ///     Subscribe to scry events when this power is applied.
    /// </summary>
    public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        await base.AfterApplied(applier, cardSource);

        // Subscribe to scry events
        ScryCmd.Scryed += OnScryed;
    }

    /// <summary>
    ///     Unsubscribe from scry events when this power is removed.
    /// </summary>
    public override async Task AfterRemoved(Creature owner)
    {
        await base.AfterRemoved(owner);

        // Unsubscribe to prevent memory leaks
        ScryCmd.Scryed -= OnScryed;
    }

    /// <summary>
    ///     Called whenever any player scrys.
    ///     Gain block if it's the owner who scryed.
    /// </summary>
    private async Task OnScryed(Player player, int amount)
    {
        // Only trigger for the owner of this power
        if (player != Owner.Player)
            return;

        // Gain Amount block (3 by default, 4 if upgraded)
        await CreatureCmd.GainBlock(
            Owner,
            Amount,
            ValueProp.Unpowered,
            null
        );
    }
}