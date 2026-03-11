using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace WatcherMod.Models.Powers;

public sealed class EstablishmentPower : PowerModel
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardRetained(CardModel card)
    {
        card.EnergyCost.AddThisCombat(-Amount);
        await Task.CompletedTask;
    }
}