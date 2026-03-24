using System;
using ChainReaction.Battle.Actors;
using ChainReaction.Battle.Cards;
using ChainReaction.Battle.Contexts;
using ChainReaction.Battle.Logging;
using ChainReaction.Core.Enums;
using ChainReaction.Data.Battle;
using UnityEngine;

namespace ChainReaction.Battle
{
    public class BattleManager : MonoBehaviour
    {
        [Header("Services")]
        [SerializeField] private MonoBehaviour battleLoggerSource;

        [Header("Flow")]
        [SerializeField] private bool autoStartFirstTurnAfterInitialize = true;

        private IBattleLogger battleLogger;

        public BattleContext CurrentContext { get; private set; }
        public bool IsPlayerTurn => CurrentContext != null && CurrentContext.IsPlayerTurn;

        public event Action<BattleContext> BattleInitialized;
        public event Action<BattleActorRuntime> TurnStarted;
        public event Action<BattleEventContext> BattleEventRaised;
        public event Action<string> BattleMessageRaised;
        public event Action HandChanged;
        public event Action MemorySlotsChanged;
        public event Action ActorStateChanged;

        private void Awake()
        {
            ResolveLogger();
        }

        public void Initialize(BattleSetupDefinition setupDefinition)
        {
            if (setupDefinition == null)
            {
                battleLogger?.LogWarning("Battle setup definition is missing.");
                return;
            }

            ResolveLogger();

            CurrentContext = new BattleContext();
            CurrentContext.Initialize(setupDefinition);

            string initializeMessage =
                $"Battle initialized. Player={CurrentContext.Player.DisplayName}, Enemy={CurrentContext.Enemy.DisplayName}";
            battleLogger?.Log(initializeMessage);
            RaiseMessage(initializeMessage);

            BattleInitialized?.Invoke(CurrentContext);
            NotifyStateChanged();

            if (CurrentContext.PlayerHand.Count == 0)
            {
                RaiseWarning("Starting hand is empty. Check BattleSetupDefinition.startingHandCards.");
            }

            PublishEvent(new BattleEventContext(TriggerType.OnBattleStart)
            {
                sourceActorId = CurrentContext.Player.ActorId,
                targetActorId = CurrentContext.Enemy.ActorId,
                description = "Battle start"
            });

            if (autoStartFirstTurnAfterInitialize)
            {
                StartNextTurn();
            }
        }

        [ContextMenu("Start Next Turn")]
        public void StartNextTurn()
        {
            if (CurrentContext == null || !CurrentContext.IsInitialized)
            {
                battleLogger?.LogWarning("StartNextTurn called before battle initialization.");
                return;
            }

            BattleActorRuntime activeActor = CurrentContext.BeginNextTurn();
            if (activeActor == null)
            {
                battleLogger?.LogWarning("No active actor was returned when starting a new turn.");
                return;
            }

            string turnStartMessage = $"Turn {CurrentContext.TurnIndex} started. Active actor: {activeActor.DisplayName}";
            battleLogger?.Log(turnStartMessage);
            RaiseMessage(turnStartMessage);

            BattleEventContext turnStartEvent = new BattleEventContext(TriggerType.OnTurnStart)
            {
                sourceActorId = activeActor.ActorId,
                targetActorId = activeActor.ActorId,
                turnIndex = CurrentContext.TurnIndex,
                description = "Turn start"
            };

            BattleEventRaised?.Invoke(turnStartEvent);
            battleLogger?.LogEvent(turnStartEvent);
            TurnStarted?.Invoke(activeActor);
            NotifyStateChanged();
        }

        public bool TryPlayCard(string cardInstanceId)
        {
            if (CurrentContext == null || !CurrentContext.IsInitialized)
            {
                RaiseWarning("TryPlayCard called before battle initialization.");
                return false;
            }

            bool playedSuccessfully = CurrentContext.TryPlayCard(
                cardInstanceId,
                out BattleCardRuntime playedCard,
                out MemorySlotRuntime assignedSlot,
                out string failureReason);

            if (!playedSuccessfully)
            {
                RaiseWarning(failureReason);
                return false;
            }

            string playMessage = assignedSlot != null
                ? $"{CurrentContext.Player.DisplayName} deployed {playedCard.DisplayName} into memory slot {assignedSlot.SlotIndex + 1}."
                : $"{CurrentContext.Player.DisplayName} played {playedCard.DisplayName}.";

            battleLogger?.Log(playMessage);
            RaiseMessage(playMessage);

            PublishEvent(new BattleEventContext(TriggerType.OnCardPlayed)
            {
                sourceActorId = CurrentContext.Player.ActorId,
                targetActorId = assignedSlot != null ? CurrentContext.Player.ActorId : CurrentContext.Enemy.ActorId,
                sourceCardId = playedCard.CardId,
                sourceCardType = playedCard.CardType,
                value = playedCard.EnergyCost,
                description = playMessage
            });

            NotifyStateChanged();
            return true;
        }

        public void EndPlayerTurn()
        {
            if (CurrentContext == null || !CurrentContext.IsInitialized)
            {
                RaiseWarning("EndPlayerTurn called before battle initialization.");
                return;
            }

            if (!CurrentContext.IsPlayerTurn)
            {
                RaiseWarning("EndPlayerTurn can only be used during the player's turn.");
                return;
            }

            string executeMessage = $"{CurrentContext.Player.DisplayName} pressed Execute.";
            battleLogger?.Log(executeMessage);
            RaiseMessage(executeMessage);

            PublishEvent(new BattleEventContext(TriggerType.OnExecutePressed)
            {
                sourceActorId = CurrentContext.Player.ActorId,
                targetActorId = CurrentContext.Enemy.ActorId,
                description = executeMessage
            });

            PublishEvent(new BattleEventContext(TriggerType.OnTurnEnd)
            {
                sourceActorId = CurrentContext.Player.ActorId,
                targetActorId = CurrentContext.Player.ActorId,
                description = "Player turn end"
            });

            StartNextTurn();
        }

        public void PublishEvent(BattleEventContext eventContext)
        {
            if (eventContext == null)
            {
                battleLogger?.LogWarning("PublishEvent received a null event context.");
                return;
            }

            if (CurrentContext != null)
            {
                eventContext.turnIndex = CurrentContext.TurnIndex;
            }

            BattleEventRaised?.Invoke(eventContext);
            battleLogger?.LogEvent(eventContext);
        }

        private void NotifyStateChanged()
        {
            HandChanged?.Invoke();
            MemorySlotsChanged?.Invoke();
            ActorStateChanged?.Invoke();
        }

        private void RaiseMessage(string message)
        {
            BattleMessageRaised?.Invoke(message);
        }

        private void RaiseWarning(string message)
        {
            battleLogger?.LogWarning(message);
            BattleMessageRaised?.Invoke(message);
        }

        private void ResolveLogger()
        {
            if (battleLoggerSource == null)
            {
                battleLogger = null;
                return;
            }

            battleLogger = battleLoggerSource as IBattleLogger;

            if (battleLogger == null)
            {
                Debug.LogWarning("Battle logger source does not implement IBattleLogger.", this);
            }
        }
    }
}
