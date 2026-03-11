using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using WatcherMod.Models.Stances;

namespace WatcherMod.Models.Cards;

public sealed class Halt() : CardModel(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override bool GainsBlock => true;


    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar("Block", 3m, ValueProp.Move), new BlockVar("WrathBlock", 9m, ValueProp.Move)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        var isInWrath = Owner.Creature.Powers.OfType<WrathStance>().Any();
        if (isInWrath) await CreatureCmd.GainBlock(Owner.Creature, (BlockVar)DynamicVars["WrathBlock"], cardPlay);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Block"].UpgradeValueBy(1m);
        DynamicVars["WrathBlock"].UpgradeValueBy(5m);
    }
}