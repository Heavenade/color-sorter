using UnityEngine;
using System;
using System.Collections.Generic;
using ColorSorter.GameSystem;
using ColorSorter.Abstractions;
using ColorSorter.Services;

namespace ColorSorter.Controller
{
    public class GameController : MonoBehaviour
    {
        // Data
        [SerializeField] private GameConfig gameConfig;
        [SerializeField] private SpawnTable spawnTable;

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
            StartNewGame();
        }

        void Update()
        {
            if (!gameModel.IsPlaying())
                return;

            gameModel.UpdateTime(Time.deltaTime);

            CheckGameOverAndPersist();
        }

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

            CheckGameOverAndPersist();
        }

        public void StartNewGame()
        {
            gameModel.StartGame();
            bestSavedThisRound = false;

            var initialCount = gameConfig.initialQueueSize;
            var initialColors = new List<ColorType>(initialCount);
            for (int i = 0; i < initialCount; i++)
            {
                initialColors.Add(colorSpawner.SpawnColor());
            }
            queueModel.Init(initialColors);

            // TODO: GameStart UI 활성화
        }

        public void RestartGame()
        {
            if (gameModel.IsGameOver())
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
        private void CheckGameOverAndPersist()
        {
            if (!gameModel.IsGameOver())
                return;

            if (!bestSavedThisRound)
            {
                SaveBestScore();
                bestSavedThisRound = true;
                // TODO: GameOver UI 활성화
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
                throw new ArgumentOutOfRangeException(nameof(sum), "Sum of weights must be > 0 and finite");
        }
    }
}
