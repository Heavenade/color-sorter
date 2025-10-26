using UnityEngine;
using System;
using System.Collections.Generic;
using ColorSorter.GameSystem;
using ColorSorter.Abstractions;
using ColorSorter.Services;
using ColorSorter.View;
using ColorSorter.Controller.ViewData;

namespace ColorSorter.Controller
{
    public class GameController : MonoBehaviour
    {
        // Data
        [SerializeField] private GameConfig gameConfig;
        [SerializeField] private SpawnTable spawnTable;

        // Views
        [SerializeField] private BoardView boardView;
        [SerializeField] private HUDView hudView;
        [SerializeField] private GameOverView gameOverView;

        [SerializeField] private GameInputHandler inputHandler;

        // Services
        private IRandom rng;
        private IHighScoreService highScoreService;

        // Game System
        private ColorSpawner colorSpawner;
        private GameModel gameModel;
        private QueueModel queueModel;

        // variables
        private int bestScore = 0;
        private bool bestSavedThisRound = false;
        private bool gameOverShown = false;

        void Awake()
        {
            if (gameConfig == null)
                throw new ArgumentNullException(nameof(gameConfig));
            if (spawnTable == null)
                throw new ArgumentNullException(nameof(spawnTable));

            ValidateConfigOrThrow(gameConfig, spawnTable);

            // 서비스 초기화
            rng = new UnityRandomProvider();
            highScoreService = new PlayerPrefsHighScoreService();

            // 게임 시스템 초기화
            colorSpawner = new ColorSpawner(
                spawnTable.blueWeight,
                spawnTable.redWeight,
                rng);

            queueModel = new QueueModel(gameConfig.visibleCount);

            gameModel = new GameModel(
                gameConfig.maxMissAllowed,
                gameConfig.durationSec);

            bestScore = highScoreService.GetHighScore();
        }

        void Start()
        {
            // 보드를 먼저 생성
            if (boardView)
            {
                boardView.Build(gameConfig.visibleCount);
            }

            StartNewGame();
        }

        void Update()
        {
            if (!gameModel.IsPlaying())
                return;

            gameModel.UpdateTime(Time.deltaTime);

            CheckGameOver();

            RenderUI();
        }

        /// <summary>
        /// Input Control
        /// </summary>
        public void HandleInput(ColorType input)
        {
            if (!gameModel.IsPlaying())
                return;

            var front = queueModel.PeekFront();
            var judge = Judge.JudgeHitOrMiss(input, front);

            if (judge == JudgeType.Hit)
            {
                if (queueModel.DequeueFront(out _))
                {
                    queueModel.EnqueueBack(colorSpawner.SpawnColor());
                    gameModel.AddScore(gameConfig.scorePerHit);
                }
            }
            else
            {
                gameModel.AddMiss();
            }

            CheckGameOver();
            RenderUI();
        }

        public void StartNewGame()
        {
            gameModel.StartGame();
            bestSavedThisRound = false;
            gameOverShown = false;

            if (gameOverView)
                gameOverView.Hide();

            var initialCount = gameConfig.initialQueueSize;
            var initialColors = new List<ColorType>(initialCount);
            for (int i = 0; i < initialCount; i++)
            {
                initialColors.Add(colorSpawner.SpawnColor());
            }
            queueModel.Init(initialColors);

            RenderUI();
        }

        public void RestartGame()
        {
            if (gameOverView)
                gameOverView.Hide();
            gameOverShown = false;

            if (inputHandler)
                inputHandler.SetEnabled(true);

            StartNewGame();
        }

        private void SaveBestScore()
        {
            var score = gameModel.Score;
            if (score > bestScore)
            {
                bestScore = score;
                highScoreService.SetHighScore(bestScore);
            }
        }

        /// <summary>
        /// GameOver 전환 시점에 단 한 번 저장
        /// </summary>
        private void CheckGameOver()
        {
            if (!gameModel.IsGameOver())
                return;

            if (!bestSavedThisRound)
            {
                SaveBestScore();
                bestSavedThisRound = true;
            }

            if (!gameOverShown)
            {
                gameOverShown = true;

                if (inputHandler)
                    inputHandler.SetEnabled(false);

                if (gameOverView)
                    gameOverView.Show(gameModel.Score, bestScore, RestartGame);
            }
        }

        /// <summary>
        /// 설정값 검사
        /// </summary>
        private static void ValidateConfigOrThrow(GameConfig cfg, SpawnTable table)
        {
            if (cfg.durationSec <= 0f)
                throw new ArgumentOutOfRangeException(nameof(cfg.durationSec), "durationSec must be > 0");
            if (cfg.maxMissAllowed <= 0)
                throw new ArgumentOutOfRangeException(nameof(cfg.maxMissAllowed), "maxMissAllowed must be > 0");
            if (cfg.visibleCount < 0)
                throw new ArgumentOutOfRangeException(nameof(cfg.visibleCount), "visibleCount must be >= 0");
            if (cfg.initialQueueSize < 0)
                throw new ArgumentOutOfRangeException(nameof(cfg.initialQueueSize), "initialQueueSize must be >= 0");

            // 가중치 검사
            if (float.IsNaN(table.blueWeight) || float.IsInfinity(table.blueWeight) || table.blueWeight < 0f)
                throw new ArgumentOutOfRangeException(nameof(table.blueWeight), "blueWeight must be ≥ 0 and finite");
            if (float.IsNaN(table.redWeight) || float.IsInfinity(table.redWeight) || table.redWeight < 0f)
                throw new ArgumentOutOfRangeException(nameof(table.redWeight), "redWeight must be ≥ 0 and finite");

            var sum = table.blueWeight + table.redWeight;
            if (float.IsNaN(sum) || float.IsInfinity(sum) || sum <= 0f)
                throw new ArgumentOutOfRangeException(nameof(sum), "Weight Sum must be > 0 and finite");
        }


        /// <summary>
        /// Create UI State
        /// </summary>
        private GameUIState BuildUIState()
        {
            return new GameUIState
            {
                State = gameModel.State,
                TimeRemainingSec = gameModel.RemainingTimeSec,
                Score = gameModel.Score,
                MissCount = gameModel.MissCount,
                BestScore = bestScore,
                VisibleQueue = queueModel.GetVisibles(),
                HighlightFront = queueModel.Count > 0
            };
        }
        private void RenderUI()
        {
            var snapShot = BuildUIState();
            if (boardView)
                boardView.Render(snapShot.VisibleQueue, snapShot.HighlightFront);
            if (hudView)
                hudView.Render(snapShot);
        }
    }
}
