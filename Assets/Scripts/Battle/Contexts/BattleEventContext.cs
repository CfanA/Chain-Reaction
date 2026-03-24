using System;
using ChainReaction.Core.Enums;

namespace ChainReaction.Battle.Contexts
{
    [Serializable]
    public class BattleEventContext
    {
        public TriggerType triggerType;
        public string sourceActorId;
        public string targetActorId;
        public string sourceCardId;
        public CardType sourceCardType;
        public int value;
        public int turnIndex;
        public string description;

        public BattleEventContext(TriggerType triggerType)
        {
            this.triggerType = triggerType;
        }

        public string ToSummary()
        {
            return $"Trigger={triggerType}, Source={sourceActorId}, Target={targetActorId}, Value={value}, Note={description}";
        }
    }
}
