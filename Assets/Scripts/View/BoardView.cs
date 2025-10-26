using System.Collections.Generic;
using ColorSorter.Game;
using UnityEngine;

namespace ColorSorter.View
{
    public class BoardView : MonoBehaviour
    {
        [SerializeField] private Transform laneRoot;
        [SerializeField] private ColorItem itemPrefab;

        private List<ColorItem> pool = new List<ColorItem>();

        public void Build(int visibleCount)
        {
            // Clear
            foreach (var item in pool)
            {
                if (item)
                    Destroy(item.gameObject);
            }
            pool.Clear();

            // Create
            for (int i = 0; i < visibleCount; i++)
            {
                var item = Instantiate(itemPrefab, laneRoot);
                item.gameObject.SetActive(false);
                pool.Add(item);
            }
        }

        public void Render(List<ColorType> visibleQueue, bool highlightFront)
        {
            if (pool.Count == 0 || visibleQueue == null)
                return;

            int count = visibleQueue.Count;

            for (int i = 0; i < pool.Count; i++)
            {
                // 위쪽부터 채우기 위해 역순 매핑
                int poolIndex = pool.Count - 1 - i;
                var note = pool[poolIndex];

                if (i < count)
                {
                    note.SetColor(visibleQueue[i]);
                    note.SetActive(true);
                    note.SetHighlighted(highlightFront && i == 0);
                }
                else
                {
                    note.SetActive(false);
                }
            }
        }
    }
}
