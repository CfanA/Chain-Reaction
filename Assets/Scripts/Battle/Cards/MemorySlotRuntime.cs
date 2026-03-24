using System;

namespace ChainReaction.Battle.Cards
{
    [Serializable]
    public class MemorySlotRuntime
    {
        private readonly int slotIndex;
        private BattleCardRuntime assignedCard;

        public int SlotIndex => slotIndex;
        public BattleCardRuntime AssignedCard => assignedCard;
        public bool IsOccupied => assignedCard != null;

        public MemorySlotRuntime(int slotIndex)
        {
            this.slotIndex = slotIndex;
        }

        public void AssignCard(BattleCardRuntime cardRuntime)
        {
            assignedCard = cardRuntime;
        }

        public void Clear()
        {
            assignedCard = null;
        }
    }
}
