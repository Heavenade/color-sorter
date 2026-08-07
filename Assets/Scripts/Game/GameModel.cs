using System;

namespace ColorSorter.Game
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
            if (maxMissAllowed <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(maxMissAllowed),
                    "maxMissAllowed must be greater than zero.");
            }

            if (durationSec <= 0f ||
                float.IsNaN(durationSec) ||
                float.IsInfinity(durationSec))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(durationSec),
                    "durationSec must be a positive finite value.");
            }

            MaxMissAllowed = maxMissAllowed;
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

        public bool IsGameOver()
        {
            return State == GameState.GameOver;
        }
        public bool IsPlaying()
        {
            return State == GameState.Playing;
        }

        private void EndGame()
        {
            State = GameState.GameOver;
        }
    }
}
