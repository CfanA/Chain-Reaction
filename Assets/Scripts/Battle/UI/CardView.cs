using System;
using ChainReaction.Battle.Cards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChainReaction.Battle.UI
{
    public class CardView : MonoBehaviour
    {
        [SerializeField] private Button clickButton;
        [SerializeField] private TMP_Text cardNameText;
        [SerializeField] private TMP_Text cardTypeText;
        [SerializeField] private TMP_Text rarityText;
        [SerializeField] private TMP_Text costText;
        [SerializeField] private TMP_Text descriptionText;

        private BattleCardRuntime boundCard;
        private Action<BattleCardRuntime> onClick;

        public void Bind(BattleCardRuntime cardRuntime, Action<BattleCardRuntime> clickHandler, bool interactable)
        {
            boundCard = cardRuntime;
            onClick = clickHandler;

            if (cardNameText != null)
            {
                cardNameText.text = cardRuntime != null ? cardRuntime.DisplayName : string.Empty;
            }

            if (cardTypeText != null)
            {
                cardTypeText.text = cardRuntime != null ? cardRuntime.CardType.ToString() : string.Empty;
            }

            if (rarityText != null)
            {
                rarityText.text = cardRuntime != null ? cardRuntime.Rarity.ToString() : string.Empty;
            }

            if (costText != null)
            {
                costText.text = cardRuntime != null ? cardRuntime.EnergyCost.ToString() : "0";
            }

            if (descriptionText != null)
            {
                descriptionText.text = cardRuntime != null ? cardRuntime.Description : string.Empty;
            }

            if (clickButton != null)
            {
                clickButton.onClick.RemoveAllListeners();
                clickButton.onClick.AddListener(HandleClick);
                clickButton.interactable = interactable && cardRuntime != null;
            }
        }

        private void HandleClick()
        {
            if (boundCard == null)
            {
                return;
            }

            onClick?.Invoke(boundCard);
        }
    }
}
