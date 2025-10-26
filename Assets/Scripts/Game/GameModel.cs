namespace ColorSorter.GameSystem
{
    public sealed class GameModel
    {
        public GameState State { get; private set; }
        public float RemainingTimeSec { get; private set; }
        public int Score { get; private set; }
        public int MissCount { get; private set; }
        public int MaxMissAllowed { get; private set; }

        private readonly float durationSec;

        public GameModel(int maxMissAllowed, float durationSec)
        {
            this.MaxMissAllowed = maxMissAllowed;
            RemainingTimeSec = durationSec;

            this.durationSec = durationSec;
        }

        public void StartGame()
        {
            State = GameState.Playing;
            Score = 0;
            MissCount = 0;
            RemainingTimeSec = durationSec; // 게임 시작 시 시간 초기화
        }

        public void UpdateTime(float deltaSec)
        {
            if (State != GameState.Playing || deltaSec <= 0f)
                return;

            RemainingTimeSec -= deltaSec;
            if (RemainingTimeSec <= 0f)
            {
                RemainingTimeSec = 0f;
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

            MissCount++;
            if (MissCount >= MaxMissAllowed)
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
