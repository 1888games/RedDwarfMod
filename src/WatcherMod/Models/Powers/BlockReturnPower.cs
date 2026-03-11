using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace WatcherMod.Models.Powers;

public sealed class BlockReturnPower : PowerModel
{
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterAttack(AttackCommand command)
    {
        // Check if this creature was attacked
        if (command.Results != null)
            foreach (var result in command.Results)
                if (result.Receiver == Owner) // Owner is the creature with this power
                {
                    var attacker = command.Attacker;
                    if (attacker != null && !attacker.IsDead)
                    {
                        decimal blockAmount = Amount;
                        if (blockAmount > 0)
                        {
                            await CreatureCmd.GainBlock(attacker, blockAmount, ValueProp.Move, null);
                            Flash();
                        }
                    }
                }
    }
}