using UnityEngine;
using ColorSorter.Abstractions;

namespace ColorSorter.Services
{
    // PlayerPrefs를 이용해 최고 점수를 저장/조회하는 서비스
    public sealed class PlayerPrefsHighScoreService : IHighScoreService
    {
        private const string HighScoreKey = "HS_COLOR_SORTER";

        public int GetHighScore()
        {
            return PlayerPrefs.GetInt(HighScoreKey, 0);
        }

        public void SetHighScore(int score)
        {
            PlayerPrefs.SetInt(HighScoreKey, score);
            PlayerPrefs.Save();
        }
    }
}