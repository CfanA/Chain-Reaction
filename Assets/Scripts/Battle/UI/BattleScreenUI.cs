using System.Collections.Generic;
using ChainReaction.Battle.Cards;
using ChainReaction.Battle.Contexts;
using UnityEngine;
using UnityEngine.UI;

namespace ChainReaction.Battle.UI
{
    public class BattleScreenUI : MonoBehaviour
    {
        [Header("Battle References")]
        [SerializeField] private BattleManager battleManager;

        [Header("Status Views")]
        [SerializeField] private EnemyStatusView enemyStatusView;
        [SerializeField] private PlayerStatusView playerStatusView;

        [Header("Panels")]
        [SerializeField] private HandPanelView handPanelView;
        [SerializeField] private BattleLogPanelView battleLogPanelView;

        [Header("Memory Slots")]
        [SerializeField] private Transform memorySlotContainer;
        [SerializeField] private MemorySlotView memorySlotViewPrefab;

        [Header("Actions")]
        [SerializeField] private Button executeButton;

        private readonly List<MemorySlotView> spawnedMemorySlotViews = new List<MemorySlotView>();

        private void OnEnable()
        {
            if (executeButton != null)
            {
                executeButton.onClick.AddListener(HandleExecuteClicked);
            }

            RegisterBattleEvents();
            RefreshAll();
        }

        private void OnDisable()
        {
            if (executeButton != null)
            {
                executeButton.onClick.RemoveListener(HandleExecuteClicked);
            }

            UnregisterBattleEvents();
        }

        private void RegisterBattleEvents()
        {
            if (battleManager == null)
            {
                return;
            }

            battleManager.BattleInitialized += HandleBattleInitialized;
            battleManager.HandChanged += HandleHandChanged;
            battleManager.MemorySlotsChanged += HandleMemorySlotsChanged;
            battleManager.ActorStateChanged += HandleActorStateChanged;
            battleManager.BattleMessageRaised += HandleBattleMessageRaised;
        }

        private void UnregisterBattleEvents()
        {
            if (battleManager == null)
            {
                return;
            }

            battleManager.BattleInitialized -= HandleBattleInitialized;
            battleManager.HandChanged -= HandleHandChanged;
            battleManager.MemorySlotsChanged -= HandleMemorySlotsChanged;
            battleManager.ActorStateChanged -= HandleActorStateChanged;
            battleManager.BattleMessageRaised -= HandleBattleMessageRaised;
        }

        private void HandleBattleInitialized(BattleContext context)
        {
            RefreshAll();
        }

        private void HandleHandChanged()
        {
            RefreshHand();
        }

        private void HandleMemorySlotsChanged()
        {
            RefreshMemorySlots();
        }

        private void HandleActorStateChanged()
        {
            RefreshStatusViews();
            RefreshExecuteButtonState();
            RefreshHand();
        }

        private void HandleBattleMessageRaised(string message)
        {
            if (battleLogPanelView != null && !string.IsNullOrWhiteSpace(message))
            {
                battleLogPanelView.AppendLog(message);
            }
        }

        private void HandleExecuteClicked()
        {
            if (battleManager == null)
            {
                return;
            }

            battleManager.EndPlayerTurn();
        }

        private void HandleCardClicked(BattleCardRuntime cardRuntime)
        {
            if (battleManager == null || cardRuntime == null)
            {
                return;
            }

            battleManager.TryPlayCard(cardRuntime.InstanceId);
        }

        private void RefreshAll()
        {
            RefreshStatusViews();
            RefreshMemorySlots();
            RefreshHand();
            RefreshExecuteButtonState();
        }

        private void RefreshStatusViews()
        {
            if (battleManager == null || battleManager.CurrentContext == null)
            {
                return;
            }

            BattleContext context = battleManager.CurrentContext;

            if (enemyStatusView != null)
            {
                enemyStatusView.Render(context.Enemy, context.CurrentTurnOwnerId == context.Enemy.ActorId);
            }

            if (playerStatusView != null)
            {
                playerStatusView.Render(context.Player, context.IsPlayerTurn);
            }
        }

        private void RefreshHand()
        {
            if (handPanelView == null)
            {
                return;
            }

            if (battleManager == null || battleManager.CurrentContext == null)
            {
                handPanelView.Render(null, HandleCardClicked, false);
                return;
            }

            handPanelView.Render(
                battleManager.CurrentContext.PlayerHand,
                HandleCardClicked,
                battleManager.CurrentContext.IsPlayerTurn);
        }

        private void RefreshMemorySlots()
        {
            if (battleManager == null || battleManager.CurrentContext == null)
            {
                ClearMemorySlotViews();
                return;
            }

            IReadOnlyList<MemorySlotRuntime> slots = battleManager.CurrentContext.MemorySlots;
            RebuildMemorySlotViews(slots);
        }

        private void RebuildMemorySlotViews(IReadOnlyList<MemorySlotRuntime> slots)
        {
            ClearMemorySlotViews();

            if (memorySlotContainer == null || memorySlotViewPrefab == null || slots == null)
            {
                return;
            }

            for (int i = 0; i < slots.Count; i++)
            {
                MemorySlotView slotViewInstance = Instantiate(memorySlotViewPrefab, memorySlotContainer);
                slotViewInstance.Render(slots[i]);
                spawnedMemorySlotViews.Add(slotViewInstance);
            }
        }

        private void ClearMemorySlotViews()
        {
            for (int i = 0; i < spawnedMemorySlotViews.Count; i++)
            {
                if (spawnedMemorySlotViews[i] != null)
                {
                    Destroy(spawnedMemorySlotViews[i].gameObject);
                }
            }

            spawnedMemorySlotViews.Clear();
        }

        private void RefreshExecuteButtonState()
        {
            if (executeButton == null)
            {
                return;
            }

            executeButton.interactable = battleManager != null &&
                                         battleManager.CurrentContext != null &&
                                         battleManager.CurrentContext.IsPlayerTurn;
        }
    }
}
