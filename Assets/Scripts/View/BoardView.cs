using System.Collections;
using System.Collections.Generic;
using ColorSorter.GameSystem;
using UnityEngine;
using UnityEngine.UI;


namespace ColorSorter.View
{
    public class BoardView : MonoBehaviour
    {
        [SerializeField] private Transform laneRoot;
        [SerializeField] private NoteView notePrefab;

        private List<NoteView> pool = new List<NoteView>();
        private int visibleCount;

        public void Build(int visibleCount)
        {
            // Clear
            foreach (var note in pool)
            {
                if (note)
                    Destroy(note.gameObject);
            }
            pool.Clear();

            // Create
            for (int i = 0; i < visibleCount; i++)
            {
                var note = Instantiate(notePrefab, laneRoot);
                note.gameObject.SetActive(false);
                pool.Add(note);
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
