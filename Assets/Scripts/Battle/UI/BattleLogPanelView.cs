using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ChainReaction.Battle.UI
{
    public class BattleLogPanelView : MonoBehaviour
    {
        [SerializeField] private Transform logEntryContainer;
        [SerializeField] private BattleLogEntryView logEntryPrefab;
        [SerializeField] private ScrollRect scrollRect;

        private readonly List<BattleLogEntryView> spawnedEntries = new List<BattleLogEntryView>();

        public void Clear()
        {
            for (int i = 0; i < spawnedEntries.Count; i++)
            {
                if (spawnedEntries[i] != null)
                {
                    Destroy(spawnedEntries[i].gameObject);
                }
            }

            spawnedEntries.Clear();
        }

        public void AppendLog(string message)
        {
            if (logEntryContainer == null || logEntryPrefab == null)
            {
                return;
            }

            BattleLogEntryView entryInstance = Instantiate(logEntryPrefab, logEntryContainer);
            entryInstance.Render(message);
            spawnedEntries.Add(entryInstance);

            if (scrollRect != null)
            {
                Canvas.ForceUpdateCanvases();
                scrollRect.verticalNormalizedPosition = 0f;
            }
        }
    }
}
