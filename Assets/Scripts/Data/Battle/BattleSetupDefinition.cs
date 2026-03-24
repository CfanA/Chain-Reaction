using ChainReaction.Data.Cards;
using UnityEngine;

namespace ChainReaction.Data.Battle
{
    [CreateAssetMenu(
        fileName = "BattleSetupDefinition",
        menuName = "Chain Reaction/Battle/Battle Setup Definition")]
    public class BattleSetupDefinition : ScriptableObject
    {
        [Header("Player")]
        [SerializeField] private BattleActorSetup playerSetup;

        [Header("Enemy")]
        [SerializeField] private BattleActorSetup enemySetup;

        [Header("Flow")]
        [Min(1)]
        [SerializeField] private int startingTurn = 1;
        [SerializeField] private bool playerActsFirst = true;

        [Header("Prototype Player Setup")]
        [Min(1)]
        [SerializeField] private int memorySlotCount = 3;
        [SerializeField] private CardDefinition[] startingHandCards;

        public BattleActorSetup PlayerSetup => playerSetup;
        public BattleActorSetup EnemySetup => enemySetup;
        public int StartingTurn => startingTurn;
        public bool PlayerActsFirst => playerActsFirst;
        public int MemorySlotCount => memorySlotCount;
        public CardDefinition[] StartingHandCards => startingHandCards;
    }
}
