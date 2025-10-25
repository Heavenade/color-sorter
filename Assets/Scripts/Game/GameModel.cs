namespace ColorSorter.GameSystem
{
    public sealed class GameModel
    {
        public GameState State { get; private set; }
        public float remainingTimeSec { get; private set; }
        public int Score { get; private set; }
        public int missCount { get; private set; }
        public int maxMissAllowed { get; private set; }

        private readonly float durationSec;

        public GameModel(int maxMissAllowed, float durationSec)
        {
            this.maxMissAllowed = maxMissAllowed;
            remainingTimeSec = durationSec;

            this.durationSec = durationSec;
        }

        public void StartGame()
        {
            State = GameState.Playing;
            Score = 0;
            missCount = 0;
            remainingTimeSec = durationSec; // 게임 시작 시 시간 초기화
        }

        public void UpdateTime(float deltaSec)
        {
            if (State != GameState.Playing || deltaSec <= 0f)
                return;

            remainingTimeSec -= deltaSec;
            if (remainingTimeSec <= 0f)
            {
                remainingTimeSec = 0f;
                EndGame();
            }
        }

        public void AddScore(int scorePerHit)
        {
            if (State != GameState.Playing || scorePerHit <= 0)
                return;

            Score += scorePerHit;
        }

        public void AddMiss()
        {
            if (State != GameState.Playing)
                return;

            missCount++;
            if (missCount >= maxMissAllowed)
            {
                EndGame();
            }
        }

        private void EndGame()
        {
            State = GameState.GameOver;
        }

        public bool IsGameOver()
        {
            return State == GameState.GameOver;
        }
        public bool IsPlaying()
        {
            return State == GameState.Playing;
        }
    }
}
