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


        public void Render(GameUIState state)
        {
            if (state == null)
                return;

            scoreText.text = $"Score: {state.Score}";
            bestText.text = $"Best: {state.BestScore}";
            missText.text = $"Miss: {state.MissCount}";
            timerText.text = state.TimeRemainingSec.ToString("0,0");

        }
    }
}
