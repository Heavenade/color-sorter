using UnityEngine;
using ColorSorter.Abstractions;

namespace ColorSorter.Services
{
    public class PlayerPrefsHighScoreService : IHighScoreService
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