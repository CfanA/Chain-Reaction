using System;
using System.Collections.Generic;
using ChainReaction.Battle.Actors;
using ChainReaction.Battle.Cards;
using ChainReaction.Core.Enums;
using ChainReaction.Data.Battle;
using ChainReaction.Data.Cards;

namespace ChainReaction.Battle.Contexts
{
    [Serializable]
    public class BattleContext
    {
        private BattleActorRuntime player;
        private BattleActorRuntime enemy;
        private int turnIndex;
        private bool nextTurnBelongsToPlayer;
        private string currentTurnOwnerId;
        private bool isInitialized;
        private readonly List<BattleCardRuntime> playerHand = new List<BattleCardRuntime>();
        private readonly List<MemorySlotRuntime> memorySlots = new List<MemorySlotRuntime>();

        public BattleActorRuntime Player => player;
        public BattleActorRuntime Enemy => enemy;
        public int TurnIndex => turnIndex;
        public string CurrentTurnOwnerId => currentTurnOwnerId;
        public bool IsInitialized => isInitialized;
        public bool IsPlayerTurn => isInitialized && player != null && currentTurnOwnerId == player.ActorId;
        public IReadOnlyList<BattleCardRuntime> PlayerHand => playerHand;
        public IReadOnlyList<MemorySlotRuntime> MemorySlots => memorySlots;

        public void Initialize(BattleSetupDefinition setupDefinition)
        {
            player = new BattleActorRuntime(setupDefinition.PlayerSetup);
            enemy = new BattleActorRuntime(setupDefinition.EnemySetup);
            turnIndex = Math.Max(0, setupDefinition.StartingTurn - 1);
            nextTurnBelongsToPlayer = setupDefinition.PlayerActsFirst;
            currentTurnOwnerId = string.Empty;
            InitializePlayerHand(setupDefinition.StartingHandCards);
            InitializeMemorySlots(setupDefinition.MemorySlotCount);
            isInitialized = true;
        }

        public BattleActorRuntime BeginNextTurn()
        {
            if (!isInitialized)
            {
                return null;
            }

            turnIndex++;

            BattleActorRuntime activeActor = nextTurnBelongsToPlayer ? player : enemy;
            currentTurnOwnerId = activeActor.ActorId;
            activeActor.ResetEnergyToBase();
            nextTurnBelongsToPlayer = !nextTurnBelongsToPlayer;
            return activeActor;
        }

        public BattleActorRuntime FindActor(string actorId)
        {
            if (player != null && player.ActorId == actorId)
            {
                return player;
            }

            if (enemy != null && enemy.ActorId == actorId)
            {
                return enemy;
            }

            return null;
        }

        public bool TryPlayCard(
            string cardInstanceId,
            out BattleCardRuntime playedCard,
            out MemorySlotRuntime assignedSlot,
            out string failureReason)
        {
            playedCard = null;
            assignedSlot = null;
            failureReason = string.Empty;

            if (!IsPlayerTurn)
            {
                failureReason = "It is not the player's turn.";
                return false;
            }

            BattleCardRuntime handCard = FindHandCard(cardInstanceId);
            if (handCard == null)
            {
                failureReason = "Card was not found in hand.";
                return false;
            }

            if (player.CurrentEnergy < handCard.EnergyCost)
            {
                failureReason = "Not enough energy to play this card.";
                return false;
            }

            player.SetEnergy(player.CurrentEnergy - handCard.EnergyCost);
            playerHand.Remove(handCard);
            playedCard = handCard;

            if (handCard.CardType == CardType.Condition)
            {
                assignedSlot = FindFirstEmptyMemorySlot();
                if (assignedSlot == null)
                {
                    playerHand.Add(handCard);
                    player.SetEnergy(player.CurrentEnergy + handCard.EnergyCost);
                    playedCard = null;
                    failureReason = "No empty memory slot is available.";
                    return false;
                }

                assignedSlot.AssignCard(handCard);
            }

            return true;
        }

        private void InitializePlayerHand(CardDefinition[] startingCards)
        {
            playerHand.Clear();
            if (startingCards == null)
            {
                return;
            }

            for (int i = 0; i < startingCards.Length; i++)
            {
                CardDefinition cardDefinition = startingCards[i];
                if (cardDefinition == null)
                {
                    continue;
                }

                playerHand.Add(new BattleCardRuntime(cardDefinition));
            }
        }

        private void InitializeMemorySlots(int slotCount)
        {
            memorySlots.Clear();
            int validSlotCount = Math.Max(1, slotCount);

            for (int i = 0; i < validSlotCount; i++)
            {
                memorySlots.Add(new MemorySlotRuntime(i));
            }
        }

        private BattleCardRuntime FindHandCard(string cardInstanceId)
        {
            for (int i = 0; i < playerHand.Count; i++)
            {
                if (playerHand[i].InstanceId == cardInstanceId)
                {
                    return playerHand[i];
                }
            }

            return null;
        }

        private MemorySlotRuntime FindFirstEmptyMemorySlot()
        {
            for (int i = 0; i < memorySlots.Count; i++)
            {
                if (!memorySlots[i].IsOccupied)
                {
                    return memorySlots[i];
                }
            }

            return null;
        }
    }
}
