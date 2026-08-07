namespace ColorSorter.Abstractions
{
    // 최고 점수를 저장/조회하기 위한 인터페이스
    public interface IHighScoreService
    {
        // 저장된 최고 점수를 반환
        int GetHighScore();

        // 최고 점수를 저장
        void SetHighScore(int score);
    }
}
