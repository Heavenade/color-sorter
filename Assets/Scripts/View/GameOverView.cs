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
                throw new System.ArgumentNullException(nameof(gameOverPanel));
            if (finalScoreText == null)
                throw new System.ArgumentNullException(nameof(finalScoreText));
            if (bestScoreText == null)
                throw new System.ArgumentNullException(nameof(bestScoreText));
            if (restartButton == null)
                throw new System.ArgumentNullException(nameof(restartButton));

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

