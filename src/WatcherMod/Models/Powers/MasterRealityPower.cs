using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace WatcherMod.Models.Powers;

public sealed class MasterRealityPower : PowerModel
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    public override Task AfterCardGeneratedForCombat(CardModel card, bool addedByPlayer)
    {
        if (card is { IsUpgradable: true, IsUpgraded: false }) CardCmd.Upgrade(card);
        return Task.CompletedTask;
    }
}