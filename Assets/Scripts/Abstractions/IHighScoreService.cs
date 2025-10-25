
namespace ColorSorter.Abstractions
{
    public interface IHighScoreService
    {
        int GetHighScore();
        void SetHighScore(int score);
    }
}
