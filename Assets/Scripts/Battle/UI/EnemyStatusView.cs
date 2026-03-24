using ChainReaction.Battle.Actors;
using TMPro;
using UnityEngine;

namespace ChainReaction.Battle.UI
{
    public class EnemyStatusView : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private TMP_Text energyText;
        [SerializeField] private TMP_Text turnStateText;

        public void Render(BattleActorRuntime actorRuntime, bool isCurrentTurn)
        {
            if (nameText != null)
            {
                nameText.text = actorRuntime != null ? actorRuntime.DisplayName : string.Empty;
            }

            if (healthText != null)
            {
                healthText.text = actorRuntime != null
                    ? $"HP {actorRuntime.CurrentHealth}/{actorRuntime.MaxHealth}"
                    : string.Empty;
            }

            if (energyText != null)
            {
                energyText.text = actorRuntime != null
                    ? $"Energy {actorRuntime.CurrentEnergy}/{actorRuntime.BaseEnergy}"
                    : string.Empty;
            }

            if (turnStateText != null)
            {
                turnStateText.text = isCurrentTurn ? "Enemy Turn" : "Idle";
            }
        }
    }
}
