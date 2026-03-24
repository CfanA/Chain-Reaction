using ChainReaction.Data.Battle;
using UnityEngine;

namespace ChainReaction.Battle
{
    public class BattleBootstrap : MonoBehaviour
    {
        [SerializeField] private BattleManager battleManager;
        [SerializeField] private BattleSetupDefinition battleSetupDefinition;
        [SerializeField] private bool initializeOnStart = true;

        private void Start()
        {
            if (initializeOnStart)
            {
                InitializeBattle();
            }
        }

        [ContextMenu("Initialize Battle")]
        public void InitializeBattle()
        {
            if (battleManager == null)
            {
                Debug.LogWarning("BattleBootstrap is missing a BattleManager reference.", this);
                return;
            }

            if (battleSetupDefinition == null)
            {
                Debug.LogWarning("BattleBootstrap is missing a BattleSetupDefinition reference.", this);
                return;
            }

            battleManager.Initialize(battleSetupDefinition);
        }
    }
}
