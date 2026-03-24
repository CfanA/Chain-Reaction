using TMPro;
using UnityEngine;

namespace ChainReaction.Battle.UI
{
    public class BattleLogEntryView : MonoBehaviour
    {
        [SerializeField] private TMP_Text messageText;

        public void Render(string message)
        {
            if (messageText != null)
            {
                messageText.text = message;
            }
        }
    }
}
