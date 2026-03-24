using ChainReaction.Battle.Cards;
using TMPro;
using UnityEngine;

namespace ChainReaction.Battle.UI
{
    public class MemorySlotView : MonoBehaviour
    {
        [SerializeField] private TMP_Text slotIndexText;
        [SerializeField] private TMP_Text slotStateText;
        [SerializeField] private TMP_Text cardNameText;
        [SerializeField] private TMP_Text cardTypeText;

        public void Render(MemorySlotRuntime slotRuntime)
        {
            if (slotIndexText != null)
            {
                slotIndexText.text = slotRuntime != null ? $"Slot {slotRuntime.SlotIndex + 1}" : string.Empty;
            }

            if (slotStateText != null)
            {
                slotStateText.text = slotRuntime != null && slotRuntime.IsOccupied ? "Occupied" : "Empty";
            }

            if (cardNameText != null)
            {
                cardNameText.text = slotRuntime != null && slotRuntime.AssignedCard != null
                    ? slotRuntime.AssignedCard.DisplayName
                    : "None";
            }

            if (cardTypeText != null)
            {
                cardTypeText.text = slotRuntime != null && slotRuntime.AssignedCard != null
                    ? slotRuntime.AssignedCard.CardType.ToString()
                    : string.Empty;
            }
        }
    }
}
