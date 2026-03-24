using ChainReaction.Battle.Contexts;
using UnityEngine;

namespace ChainReaction.Battle.Logging
{
    public class UnityDebugBattleLogger : MonoBehaviour, IBattleLogger
    {
        [SerializeField] private string logPrefix = "[Battle]";

        public void Log(string message)
        {
            Debug.Log($"{logPrefix} {message}", this);
        }

        public void LogWarning(string message)
        {
            Debug.LogWarning($"{logPrefix} {message}", this);
        }

        public void LogEvent(BattleEventContext eventContext)
        {
            if (eventContext == null)
            {
                Debug.LogWarning($"{logPrefix} Tried to log a null battle event.", this);
                return;
            }

            Debug.Log($"{logPrefix} Event -> {eventContext.ToSummary()}", this);
        }
    }
}
