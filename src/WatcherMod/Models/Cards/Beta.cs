using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace WatcherMod.Models.Cards;

public sealed class Beta() : CardModel(2, CardType.Skill, CardRarity.Token, TargetType.Self)
{
    public override CardPoolModel Pool => ModelDb.CardPool<TokenCardPool>();

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromCard<Omega>()
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // Create Insight card
        if (CombatState != null)
        {
            var insightCard = CombatState.CreateCard<Omega>(Owner);

            // Shuffle it into draw pile (Random position)
            CardCmd.PreviewCardPileAdd(
                await CardPileCmd.AddGeneratedCardToCombat(
                    insightCard,
                    PileType.Draw,
                    true,
                    CardPilePosition.Random
                )
            );
        }
    }
}