using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using WatcherMod.Models.Stances;

namespace WatcherMod.Models.Powers;

public class LikeWaterPower : PowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        // Only trigger at the end of the owner's turn
        if (Owner.Player?.Creature.Side != side) return;

        var isInCalm = Owner.Powers.OfType<CalmStance>().Any();

        if (isInCalm)
        {
            await CreatureCmd.GainBlock(Owner, Amount, ValueProp.Unpowered, null);
            Flash();
        }
    }
}