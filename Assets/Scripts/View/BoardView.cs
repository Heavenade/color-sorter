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

            for (int i = 0; i < pool.Count; i++)
            {
                if (i < visibleQueue.Count)
                {
                    pool[i].SetColor(visibleQueue[i]);
                    pool[i].SetActive(true);
                    pool[i].SetHighlighted(highlightFront && i == 0);
                }
                else
                {
                    pool[i].SetActive(false);
                }
            }
        }
    }
}
