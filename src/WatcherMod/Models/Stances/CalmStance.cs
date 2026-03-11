using MegaCrit.Sts2.Core.Entities.Creatures;

namespace WatcherMod.Models.Stances;

public class CalmStance : StancePower
{
    protected override string AuraScenePath => "res://scenes/watcher_mod/vfx/calm_aura.tscn";

    public override Task OnExitStance(Creature creature)
    {
        if (creature.IsPlayer) creature.Player!.PlayerCombatState!.GainEnergy(2);
        return base.OnExitStance(creature);
    }
}