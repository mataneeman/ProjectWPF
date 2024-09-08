using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ProjectWpf.Flappy_Bird
{
    public class GameManager
    {
        private readonly Window mainWindow;
        private readonly Canvas gameCanvas;
        private Image? birdImage; 
        private List<Pipe> pipes;
        private DispatcherTimer gameTimer;
        private DispatcherTimer pipeTimer;
        private double birdYPosition;
        private double birdVelocity;
        private const double gravity = 0.5;
        private const double jumpStrength = -5;
        private double currentPipeSpeed;
        private const double initialPipeSpeed = 5;
        private const double speedIncreaseInterval = 10;
        private double pipeSpawnInterval = 1700;
        private Random random;
        private bool isGameRunning;
        private bool gameOver;
        private int score;
        private HighScoreManager highScoreManager;
        private List<Pipe> passedPipes = new List<Pipe>();

        public GameManager(Window window, Canvas canvas)
        {
            mainWindow = window;
            gameCanvas = canvas;
            pipes = new List<Pipe>();
            random = new Random();
            gameTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(20)
            };
            gameTimer.Tick += GameTimer_Tick;
            pipeTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(pipeSpawnInterval)
            };
            pipeTimer.Tick += PipeTimer_Tick;
            highScoreManager = new HighScoreManager("FlappyBirdGame");

            InitializeGame();
        }

        private void InitializeGame()
        {
            if (birdImage != null)
            {
                gameCanvas.Children.Remove(birdImage);
            }

            birdImage = new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/Flappy Bird/Resources/bird.png")),
                Width = 30,
                Height = 30
            };
            birdYPosition = 200;
            birdVelocity = 0;

            gameCanvas.Children.Add(birdImage);
            Canvas.SetLeft(birdImage, 100);
            Canvas.SetTop(birdImage, birdYPosition);

            currentPipeSpeed = initialPipeSpeed;
            isGameRunning = false;
            gameOver = false;
            score = 0;

            UpdateUIForGameState();
        }

        private void PipeTimer_Tick(object? sender, EventArgs e)
        {
            if (sender == null || !isGameRunning) return;

            double pipeX = gameCanvas.ActualWidth;
            double canvasHeight = gameCanvas.ActualHeight;
            double pipeGap = 150;

            Pipe newPipe = new Pipe(pipeX, canvasHeight, pipeGap, currentPipeSpeed);
            gameCanvas.Children.Add(newPipe.TopPipeImage);
            gameCanvas.Children.Add(newPipe.BottomPipeImage);
            pipes.Add(newPipe);
        }

        private void GameTimer_Tick(object? sender, EventArgs e)
        {
            if (sender == null || !isGameRunning) return;
            birdVelocity += gravity;
            birdYPosition += birdVelocity;

            if (birdImage == null || birdYPosition < 0 || birdYPosition > gameCanvas.ActualHeight - birdImage.Height)
            {
                EndGame();
                return;
            }

            Canvas.SetTop(birdImage, birdYPosition);

            foreach (Pipe pipe in pipes.ToList())
            {
                pipe.Update();

                if (pipe.IsPassedByBird(birdImage))
                {
                    if (!passedPipes.Contains(pipe))
                    {
                        passedPipes.Add(pipe);
                        score++;
                        UpdateScore();
                    }
                    pipe.MarkAsPassed();
                }

                if (pipe.IsPassedByBird(birdImage) || pipe.IsOutOfCanvas(gameCanvas))
                {
                    pipe.RemoveFromCanvas(gameCanvas);
                    pipes.Remove(pipe);
                }
            }

            CheckForCollisions();
        }

        private void CheckForCollisions()
        {
            if (birdImage == null) return;

            foreach (Pipe pipe in pipes)
            {
                if (pipe.IsCollidingWithBird(birdImage))
                {
                    EndGame();
                    break;
                }
            }
        }


        private void EndGame()
        {
            if (gameOver) return;

            gameTimer.Stop();
            pipeTimer.Stop();
            isGameRunning = false;
            gameOver = true;

            highScoreManager.AddScore(score);
            List<int> topScores = highScoreManager.GetTopScores();
            StringBuilder scoresBuilder = new StringBuilder();
            for (int i = 0; i < topScores.Count; i++)
            {
                scoresBuilder.AppendLine($"{i + 1}. {topScores[i]}");
            }

            if (mainWindow is FlappyBirdGame game)
            {
                game.HighScoresText.Text = scoresBuilder.ToString();
                game.GameOverOverlay.Visibility = Visibility.Visible;
                game.ScoreTextBlock.Visibility = Visibility.Collapsed;
                game.StartButton.Visibility = Visibility.Collapsed;
            }
        }


        private void UpdateUIForGameState()
        {
            if (mainWindow is FlappyBirdGame game)
            {
                if (isGameRunning)
                {
                    game.StartButton.Visibility = Visibility.Collapsed;
                    game.GameOverOverlay.Visibility = Visibility.Collapsed;
                }
                else if (gameOver)
                {
                    game.StartButton.Visibility = Visibility.Collapsed;
                    game.ScoreTextBlock.Visibility = Visibility.Collapsed;
                    game.GameOverOverlay.Visibility = Visibility.Visible;
                }
                else
                {
                    game.StartButton.Visibility = Visibility.Visible;
                    game.GameOverOverlay.Visibility = Visibility.Collapsed;
                }
            }
        }

        public void StartGame()
        {
            if (!isGameRunning && !gameOver)
            {
                InitializeGame();
                isGameRunning = true;
                gameOver = false;
                gameTimer.Start();
                pipeTimer.Start();
                UpdateUIForGameState();
            }
        }

        public void ResetGame()
        {
            gameCanvas.Children.Clear();
            pipes.Clear();

            isGameRunning = true;
            gameTimer.Start();
            pipeTimer.Start();
            UpdateUIForGameState();
        }

        public void BirdJump()
        {
            if (isGameRunning)
            {
                birdVelocity = jumpStrength;
            }
        }

        private void UpdateScore()
        {
            if (mainWindow is FlappyBirdGame game)
            {
                game.ScoreTextBlock.Text = score.ToString();
            }

            if (score % speedIncreaseInterval == 0 && score > 0)
            {
                currentPipeSpeed += 1;
            }
        }
    }
}
