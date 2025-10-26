using UnityEngine;
using TMPro;
using ColorSorter.Controller.ViewData;

namespace ColorSorter.View
{
    public class HUDView : MonoBehaviour
    {
        [SerializeField] TMP_Text scoreText;
        [SerializeField] TMP_Text bestText;
        [SerializeField] TMP_Text missText;
        [SerializeField] TMP_Text timerText;


        public void Render(GameUIState snapshot)
        {
            if (snapshot == null)
                return;

            if (scoreText != null)
                scoreText.text = $"Score: {snapshot.Score}";
            if (bestText != null)
                bestText.text = $"Best: {snapshot.BestScore}";
            if (missText != null)
                missText.text = $"Miss: {snapshot.MissCount}";
            if (timerText != null)
                timerText.text = snapshot.TimeRemainingSec.ToString("0,0");

        }


        void ShowGameOver()
        {
            // TODO: GameOverUI
        }
    }
}
