using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace WatcherMod.Models.Cards.CardModels;

public abstract class WatcherCardModel(
    int canonicalEnergyCost,
    CardType type,
    CardRarity rarity,
    TargetType targetType,
    bool shouldShowInCardLibrary = true)
    : CardModel(canonicalEnergyCost, type, rarity, targetType, shouldShowInCardLibrary)
{
    public virtual Task OnStanceChanged(Creature creature)
    {
        return Task.CompletedTask;
    }

    public virtual Task OnScryed(Player player, int amount)
    {
        return Task.CompletedTask;
    }
}