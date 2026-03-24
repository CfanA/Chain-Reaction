using System;

namespace ChainReaction.Data.Battle
{
    [Serializable]
    public struct BattleActorSetup
    {
        public string actorId;
        public string displayName;
        public int maxHealth;
        public int startingHealth;
        public int startingEnergy;

        public int ResolveStartingHealth()
        {
            if (startingHealth > 0)
            {
                return startingHealth;
            }

            return maxHealth;
        }
    }
}
