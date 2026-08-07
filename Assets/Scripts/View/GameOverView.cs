using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ColorSorter.View
{
    public class GameOverView : MonoBehaviour
    {

        [Header("UI")]
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private TMP_Text finalScoreText;
        [SerializeField] private TMP_Text bestScoreText;
        [SerializeField] private Button restartButton;

        void Awake()
        {
            if (gameOverPanel == null)
                Debug.LogError($"{nameof(GameOverView)}: {nameof(gameOverPanel)} is not assigned.", this);
            if (finalScoreText == null)
                Debug.LogError($"{nameof(GameOverView)}: {nameof(finalScoreText)} is not assigned.", this);
            if (bestScoreText == null)
                Debug.LogError($"{nameof(GameOverView)}: {nameof(bestScoreText)} is not assigned.", this);
            if (restartButton == null)
                Debug.LogError($"{nameof(GameOverView)}: {nameof(restartButton)} is not assigned.", this);

            if (gameOverPanel)
                gameOverPanel.SetActive(false);
        }

        public void Show(int finalScore, int bestScore, Action onRestart)
        {
            finalScoreText.text = $"Final Score: {finalScore}";
            bestScoreText.text = $"Best Score: {bestScore}";


            if (restartButton)
            {
                restartButton.onClick.RemoveAllListeners();
                restartButton.onClick.AddListener(() => onRestart?.Invoke());
            }

            gameOverPanel.SetActive(true);
        }

        public void Hide()
        {
            gameOverPanel.SetActive(false);
        }
    }
}

