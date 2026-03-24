using System;
using ChainReaction.Data.Battle;
using UnityEngine;

namespace ChainReaction.Battle.Actors
{
    [Serializable]
    public class BattleActorRuntime
    {
        [SerializeField] private string actorId;
        [SerializeField] private string displayName;
        [SerializeField] private int maxHealth;
        [SerializeField] private int baseEnergy;
        [SerializeField] private int currentHealth;
        [SerializeField] private int currentEnergy;

        public string ActorId => actorId;
        public string DisplayName => displayName;
        public int MaxHealth => maxHealth;
        public int BaseEnergy => baseEnergy;
        public int CurrentHealth => currentHealth;
        public int CurrentEnergy => currentEnergy;
        public bool IsAlive => currentHealth > 0;

        public BattleActorRuntime(BattleActorSetup setup)
        {
            actorId = setup.actorId;
            displayName = setup.displayName;
            maxHealth = Mathf.Max(1, setup.maxHealth);
            baseEnergy = Mathf.Max(0, setup.startingEnergy);
            currentHealth = Mathf.Clamp(setup.ResolveStartingHealth(), 0, maxHealth);
            currentEnergy = baseEnergy;
        }

        public void SetEnergy(int value)
        {
            currentEnergy = Mathf.Max(0, value);
        }

        public void RestoreEnergy(int amount)
        {
            currentEnergy = Mathf.Max(0, currentEnergy + amount);
        }

        public void ResetEnergyToBase()
        {
            currentEnergy = baseEnergy;
        }

        public void ReceiveDamage(int amount)
        {
            currentHealth = Mathf.Clamp(currentHealth - Mathf.Max(0, amount), 0, maxHealth);
        }

        public void Heal(int amount)
        {
            currentHealth = Mathf.Clamp(currentHealth + Mathf.Max(0, amount), 0, maxHealth);
        }
    }
}
