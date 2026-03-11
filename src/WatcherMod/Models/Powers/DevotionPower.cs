using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace WatcherMod.Models.Powers;

public class DevotionPower : PowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;


    public override async Task BeforeHandDrawLate(Player player, PlayerChoiceContext choiceContext,
        CombatState combatState)
    {
        await PowerCmd.Apply<MantraPower>(player.Creature, Amount, player.Creature, null);
        Flash();
    }
}