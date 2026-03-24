using ChainReaction.Core.Enums;
using UnityEngine;

namespace ChainReaction.Data.Cards
{
    [CreateAssetMenu(
        fileName = "CardDefinition",
        menuName = "Chain Reaction/Cards/Card Definition")]
    public class CardDefinition : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private string cardId;
        [SerializeField] private string displayName;
        [TextArea]
        [SerializeField] private string description;

        [Header("Gameplay")]
        [SerializeField] private CardType cardType = CardType.Active;
        [SerializeField] private CardRarity rarity = CardRarity.Common;
        [Min(0)]
        [SerializeField] private int energyCost = 1;

        public string CardId => cardId;
        public string DisplayName => displayName;
        public string Description => description;
        public CardType CardType => cardType;
        public CardRarity Rarity => rarity;
        public int EnergyCost => energyCost;
    }
}
