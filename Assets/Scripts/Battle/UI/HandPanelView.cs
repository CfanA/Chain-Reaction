using System;
using System.Collections.Generic;
using ChainReaction.Battle.Cards;
using UnityEngine;

namespace ChainReaction.Battle.UI
{
    public class HandPanelView : MonoBehaviour
    {
        [SerializeField] private Transform cardContainer;
        [SerializeField] private CardView cardViewPrefab;

        private readonly List<CardView> spawnedCardViews = new List<CardView>();

        public void Render(IReadOnlyList<BattleCardRuntime> handCards, Action<BattleCardRuntime> onCardClicked, bool interactable)
        {
            ClearSpawnedViews();

            if (cardContainer == null)
            {
                Debug.LogWarning("HandPanelView is missing cardContainer.", this);
                return;
            }

            if (cardViewPrefab == null)
            {
                Debug.LogWarning("HandPanelView is missing cardViewPrefab.", this);
                return;
            }

            if (handCards == null)
            {
                return;
            }

            for (int i = 0; i < handCards.Count; i++)
            {
                CardView cardViewInstance = Instantiate(cardViewPrefab, cardContainer);
                cardViewInstance.Bind(handCards[i], onCardClicked, interactable);
                spawnedCardViews.Add(cardViewInstance);
            }
        }

        private void ClearSpawnedViews()
        {
            for (int i = 0; i < spawnedCardViews.Count; i++)
            {
                if (spawnedCardViews[i] != null)
                {
                    Destroy(spawnedCardViews[i].gameObject);
                }
            }

            spawnedCardViews.Clear();
        }
    }
}
