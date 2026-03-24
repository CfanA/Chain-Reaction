using System;
using ChainReaction.Core.Enums;
using ChainReaction.Data.Cards;

namespace ChainReaction.Battle.Cards
{
    [Serializable]
    public class BattleCardRuntime
    {
        private readonly string instanceId;
        private readonly CardDefinition definition;

        public string InstanceId => instanceId;
        public CardDefinition Definition => definition;
        public string CardId => definition != null ? definition.CardId : string.Empty;
        public string DisplayName => definition != null ? definition.DisplayName : string.Empty;
        public string Description => definition != null ? definition.Description : string.Empty;
        public CardType CardType => definition != null ? definition.CardType : CardType.Active;
        public CardRarity Rarity => definition != null ? definition.Rarity : CardRarity.Common;
        public int EnergyCost => definition != null ? definition.EnergyCost : 0;

        public BattleCardRuntime(CardDefinition definition)
        {
            this.definition = definition;
            instanceId = Guid.NewGuid().ToString("N");
        }
    }
}
