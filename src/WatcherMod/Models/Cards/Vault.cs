using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace WatcherMod.Models.Cards;

public sealed class Vault() : CardModel(3, CardType.Skill, CardRarity.Rare, TargetType.None)
{
    private bool _hasExtraTurn;

    public override bool ShouldTakeExtraTurn(Player player)
    {
        return _hasExtraTurn && player == Owner;
    }

    protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        _hasExtraTurn = true;
        // End your turn
        PlayerCmd.EndTurn(Owner, false);
        return Task.CompletedTask;
    }

    public override Task AfterTakingExtraTurn(Player player)
    {
        if (player != Owner) return Task.CompletedTask;
        _hasExtraTurn = false;
        return Task.CompletedTask;
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}