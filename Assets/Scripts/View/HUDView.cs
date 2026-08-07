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

        void Awake()
        {
            if (scoreText == null)
                Debug.LogError($"{nameof(HUDView)}: {nameof(scoreText)} is not assigned.", this);

            if (bestText == null)
                Debug.LogError($"{nameof(HUDView)}: {nameof(bestText)} is not assigned.", this);

            if (missText == null)
                Debug.LogError($"{nameof(HUDView)}: {nameof(missText)} is not assigned.", this);

            if (timerText == null)
                Debug.LogError($"{nameof(HUDView)}: {nameof(timerText)} is not assigned.", this);
        }

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
