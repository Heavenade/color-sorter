using UnityEngine;
using System;
using System.Collections.Generic;
using ColorSorter.Game;
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


        // Input
        [SerializeField] private GameInputHandler inputHandler;


        // Services
        private IRandom rng;
        private IHighScoreService highScoreService;


        // Game System
        private ColorSpawner colorSpawner;
        private GameModel gameModel;
        private QueueModel queueModel;


        // Runtime State
        private int bestScore;
        private bool bestSavedThisRound;
        private bool gameOverShown;
        private List<ColorType> visibleQueue;


        private void Awake()
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

        private void Start()
        {
            // 보드를 먼저 생성
            if (boardView)
            {
                boardView.Build(gameConfig.visibleCount);
            }

            StartNewGame();
        }

        private void Update()
        {
            if (!gameModel.IsPlaying())
                return;

            gameModel.UpdateTime(Time.deltaTime);
            CheckGameOver();

            RenderUI(false);
        }


        // Input Control

        public void HandleInput(ColorType input)
        {
            if (!gameModel.IsPlaying())
                return;

            var front = queueModel.PeekFront();
            var judge = Judge.JudgeHitOrMiss(input, front);

            bool queueChanged = false;

            if (judge == JudgeType.Hit)
            {
                if (queueModel.DequeueFront(out _))
                {
                    queueModel.EnqueueBack(colorSpawner.SpawnColor());
                    gameModel.AddScore(gameConfig.scorePerHit);

                    RefreshVisibleQueue();
                    queueChanged = true;
                }
            }
            else
            {
                gameModel.AddMiss();
            }

            CheckGameOver();
            RenderUI(queueChanged);
        }


        // Game Control

        public void StartNewGame()
        {
            gameModel.StartGame();

            bestSavedThisRound = false;
            gameOverShown = false;

            if (inputHandler)
                inputHandler.SetEnabled(true);

            if (gameOverView)
                gameOverView.Hide();

            var initialCount = gameConfig.initialQueueSize;
            var initialColors = new List<ColorType>(initialCount);

            for (int i = 0; i < initialCount; i++)
            {
                initialColors.Add(colorSpawner.SpawnColor());
            }

            queueModel.Init(initialColors);
            RefreshVisibleQueue();

            RenderUI();
        }

        public void RestartGame()
        {
            StartNewGame();
        }

        private void CheckGameOver()
        {
            if (!gameModel.IsGameOver())
                return;

            // 게임오버 전환 시 한 번만 최고 점수를 저장한다.
            if (!bestSavedThisRound)
            {
                SaveBestScore();
                bestSavedThisRound = true;
            }

            // 게임오버 UI 역시 라운드마다 한 번만 표시한다.
            if (gameOverShown)
                return;

            gameOverShown = true;

            if (inputHandler)
                inputHandler.SetEnabled(false);

            if (gameOverView)
                gameOverView.Show(
                    gameModel.Score,
                    bestScore,
                    RestartGame);
        }

        private void SaveBestScore()
        {
            var score = gameModel.Score;

            if (score <= bestScore)
                return;

            bestScore = score;
            highScoreService.SetHighScore(bestScore);
        }


        // UI

        private GameUIState BuildUIState()
        {
            return new GameUIState
            {
                State = gameModel.State,
                TimeRemainingSec = gameModel.RemainingTimeSec,
                Score = gameModel.Score,
                MissCount = gameModel.MissCount,
                BestScore = bestScore,
                VisibleQueue = visibleQueue,
                HighlightFront = queueModel.Count > 0
            };
        }

        private void RenderUI(bool renderBoard = true)
        {
            var snapshot = BuildUIState();

            if (renderBoard && boardView)
            {
                boardView.Render(
                    snapshot.VisibleQueue,
                    snapshot.HighlightFront);
            }

            if (hudView)
            {
                hudView.Render(snapshot);
            }
        }

        private void RefreshVisibleQueue()
        {
            visibleQueue = queueModel.GetVisibles();
        }


        // Validation

        private static void ValidateConfigOrThrow(
            GameConfig cfg,
            SpawnTable table)
        {
            if (cfg.durationSec <= 0f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(cfg.durationSec),
                    "durationSec must be greater than zero.");
            }

            if (cfg.maxMissAllowed <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(cfg.maxMissAllowed),
                    "maxMissAllowed must be greater than zero.");
            }

            if (cfg.visibleCount < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(cfg.visibleCount),
                    "visibleCount must be zero or greater.");
            }

            if (cfg.initialQueueSize < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(cfg.initialQueueSize),
                    "initialQueueSize must be zero or greater.");
            }

            // 가중치 검사
            if (float.IsNaN(table.blueWeight) ||
                float.IsInfinity(table.blueWeight) ||
                table.blueWeight < 0f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(table.blueWeight),
                    "blueWeight must be a non-negative finite value.");
            }

            if (float.IsNaN(table.redWeight) ||
                float.IsInfinity(table.redWeight) ||
                table.redWeight < 0f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(table.redWeight),
                    "redWeight must be a non-negative finite value.");
            }

            var sum = table.blueWeight + table.redWeight;

            if (float.IsNaN(sum) ||
                float.IsInfinity(sum) ||
                sum <= 0f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(sum),
                    "The total spawn weight must be a positive finite value.");
            }
        }
    }
}