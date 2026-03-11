using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using WatcherMod.Models.Cards.CardModels;
using WatcherMod.Models.Stances;

public static class ChangeStanceCmd
{
    /// <summary>
    ///     Event triggered whenever a creature changes stance.
    ///     Powers can subscribe to this for stance-dependent effects.
    /// </summary>
    public static event Func<Creature, PlayerChoiceContext?, Task>? StanceChanged;

    /// <summary>
    ///     Change a creature's stance. Handles exit/enter hooks and notifications.
    /// </summary>
    public static async Task Execute(Creature creature, StancePower? newStance, PlayerChoiceContext? context)
    {
        var currentStance = GetCurrentStance(creature);

        // Early exit if no actual change is happening
        if (!ShouldChangeStance(currentStance, newStance))
            return;

        // Exit current stance
        await ExitCurrentStance(currentStance, creature);

        // Enter new stance
        await EnterNewStance(newStance, creature);

        // Notify all listeners about the stance change (pass context!)
        await NotifyPowers(creature, context);
        await NotifyCards(creature);
    }

    /// <summary>
    ///     Get the creature's current stance, if any.
    /// </summary>
    private static StancePower? GetCurrentStance(Creature creature)
    {
        return creature.Powers.OfType<StancePower>().FirstOrDefault();
    }

    /// <summary>
    ///     Check if a stance change should occur.
    ///     Returns false if switching to the same stance or both are null.
    /// </summary>
    private static bool ShouldChangeStance(StancePower? currentStance, StancePower? newStance)
    {
        // Both null - no change
        if (currentStance == null && newStance == null)
            return false;

        // Switching to the same stance - no change
        if (currentStance != null && newStance != null &&
            currentStance.GetType() == newStance.GetType())
            return false;

        return true;
    }

    /// <summary>
    ///     Exit the current stance, calling OnExitStance and removing it.
    /// </summary>
    private static async Task ExitCurrentStance(StancePower? currentStance, Creature creature)
    {
        if (currentStance == null)
            return;

        await currentStance.OnExitStance(creature);
        currentStance.RemoveInternal();
    }

    /// <summary>
    ///     Enter a new stance, applying it and calling OnEnterStance.
    /// </summary>
    private static async Task EnterNewStance(StancePower? newStance, Creature creature)
    {
        if (newStance == null)
            return;

        var mutableStance = newStance.ToMutable();
        mutableStance.ApplyInternal(creature, 1);
        await ((StancePower)mutableStance).OnEnterStance(creature);
    }

    /// <summary>
    ///     Notify powers that have subscribed to stance changes (e.g., Mental Fortress).
    ///     Pass context so they can use CardPileCmd.Draw etc.
    /// </summary>
    private static async Task NotifyPowers(Creature creature, PlayerChoiceContext? context)
    {
        if (StanceChanged != null) await StanceChanged.Invoke(creature, context);
    }

    /// <summary>
    ///     Notify all Watcher cards across all piles about the stance change.
    /// </summary>
    private static async Task NotifyCards(Creature creature)
    {
        var player = creature.Player;

        // Iterate through ALL piles (Hand, Draw, Discard, Exhaust, etc.)
        var allPiles = player?.PlayerCombatState?.AllPiles;
        if (allPiles != null)
            foreach (var pile in allPiles)
            {
                // Make a copy to avoid modification during iteration
                var watcherCards = pile.Cards.OfType<WatcherCardModel>().ToList();

                foreach (var card in watcherCards) await card.OnStanceChanged(creature);
            }
    }
}