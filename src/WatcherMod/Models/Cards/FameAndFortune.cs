using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using WatcherMod.src.WatcherMod.Models.Cards.@as;

namespace WatcherMod.Models.Cards;

public sealed class FameAndFortune() : WishModel(-1, CardType.Skill, CardRarity.Token, TargetType.None)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new GoldVar(25)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<StrengthPower>()
    ];

    public override async Task OnWish(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        VfxCmd.PlayOnCreature(Owner.Creature, "vfx/vfx_coin_explosion_regular");
        await PlayerCmd.GainGold(DynamicVars.Gold.IntValue, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Gold.UpgradeValueBy(5);
    }
}