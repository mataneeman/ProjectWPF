using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Threading;

namespace ProjectWpf.Save_Ball_Game
{
    public class GameEngine
    {
        private Canvas canvas;
        private Rectangle player;
        private Random rand = new Random();
        private int score = 0;
        private int missed = 0;
        private double ballSpeed = 12;
        private List<Ball> fallingBalls = new List<Ball>();
        private DispatcherTimer gameTimer = new DispatcherTimer();
        private Action<int> updateLives;
        private DateTime lastBallCreationTime = DateTime.Now;
        private const double BallSpeedIncrement = 0.4;
        private const double BallCreationIntervalSeconds = 1.3;
        private const int MaxBallsOnScreen = 15;
        private DispatcherTimer restoreTimer = new DispatcherTimer();
        private double originalWidth;
        private double originalHeight;
        private int lives = 5;

        public int Score => score;

        public GameEngine(Canvas canvas, Rectangle player, Action<int> updateLives)
        {
            this.canvas = canvas;
            this.player = player;
            this.updateLives = updateLives;
            this.gameTimer.Tick += GameEngine_Tick;
            this.gameTimer.Interval = TimeSpan.FromMilliseconds(50); // 30 FPS
        }

        public void StartGame()
        {
            gameTimer.Start();
        }

        public void StopGame()
        {
            gameTimer.Stop();
            ClearBalls();
        }

        private void GameEngine_Tick(object sender, EventArgs e)
        {
            UpdateScoreDisplay();

            if ((DateTime.Now - lastBallCreationTime).TotalSeconds >= BallCreationIntervalSeconds)
            {
                AddNewBall();
                lastBallCreationTime = DateTime.Now;
            }

            foreach (Ball ball in fallingBalls.ToList())
            {
                ball.MoveDown(ballSpeed);
                if (ball.IsOutOfBounds(canvas))
                {
                    if (ball.GetBallType() == Ball.BallType.Regular)
                    {
                        RemoveBall(ball);
                        missed++;
                        lives--;
                        UpdateLivesDisplay();
                    }
                    else
                    {
                        RemoveBall(ball);
                    }
                }
                else if (IsCollidingWithPlayer(ball.Rectangle))
                {
                    HandleCollisionWithPlayer(ball);
                    RemoveBall(ball);
                }
            }
        }

        private void HandleCollisionWithPlayer(Ball ball)
        {
            switch (ball.GetBallType())
            {
                case Ball.BallType.Regular:
                    score++;
                    CheckSpeedIncrease(); // Check speed increase
                    break;
                case Ball.BallType.Bomb:
                    lives--;
                    UpdateLivesDisplay();
                    break;
                case Ball.BallType.Virus:
                    ShrinkBasket();
                    break;
                case Ball.BallType.Coin:
                    score += 5;
                    CheckSpeedIncrease(); // Check speed increase
                    break;
                case Ball.BallType.live:
                    if (lives < 5) // Only add life if less than 5
                    {
                        lives++;
                        UpdateLivesDisplay();
                    }
                    break;
            }
        }

        private void RemoveBall(Ball ball)
        {
            canvas.Children.Remove(ball.Rectangle);
            fallingBalls.Remove(ball);
        }

        private void UpdateLivesDisplay()
        {
            updateLives?.Invoke(lives);
        }

        private void UpdateScoreDisplay()
        {
            TextBlock scoreText = canvas.FindName("scoreText") as TextBlock;
            TextBlock missedText = canvas.FindName("missedText") as TextBlock;

            if (scoreText != null) scoreText.Text = score.ToString();
            if (missedText != null) missedText.Text = missed.ToString();
        }

        public void AddNewBall()
        {
            if (fallingBalls.Count >= MaxBallsOnScreen)
                return;

            Ball.BallType type = GetRandomBallType();
            Ball newBall = new Ball(canvas.ActualWidth, type);
            newBall.AddToCanvas(canvas);
            fallingBalls.Add(newBall);
        }

        private Ball.BallType GetRandomBallType()
        {
            int chance = rand.Next(0, 100);
            if (chance < 60) // 60% chance for regular balls
            {
                return Ball.BallType.Regular;
            }
            else if (chance < 75) // 15% chance for bombs
            {
                return Ball.BallType.Bomb;
            }
            else if (chance < 85) // 10% chance for viruses
            {
                return Ball.BallType.Virus;
            }
            else if (chance < 95) // 10% chance for coins
            {
                return Ball.BallType.Coin;
            }
            else // 5% chance for hearts
            {
                return Ball.BallType.live;
            }
        }


        private void CheckSpeedIncrease()
        {
            if (score % 2 == 0)
            {
                ballSpeed += BallSpeedIncrement;
            }
        }

        public void ClearBalls()
        {
            foreach (Ball ball in fallingBalls.ToList())
            {
                canvas.Children.Remove(ball.Rectangle);
            }
            fallingBalls.Clear();
        }

        private bool IsCollidingWithPlayer(Rectangle ball)
        {
            double playerMargin = 2;

            Rect playerRect = new Rect(
                Canvas.GetLeft(player) + playerMargin,
                Canvas.GetTop(player) + playerMargin,
                player.Width - 2 * playerMargin,
                player.Height - 2 * playerMargin
            );

            Rect ballRect = new Rect(
                Canvas.GetLeft(ball),
                Canvas.GetTop(ball),
                ball.Width,
                ball.Height
            );

            return playerRect.IntersectsWith(ballRect);
        }

        private void ShrinkBasket()
        {
            float shrinkFactor = 0.8f;

            originalWidth = player.Width;
            originalHeight = player.Height;

            // Store original position ratio
            double leftRatio = Canvas.GetLeft(player) / (canvas.ActualWidth - originalWidth);
            double topRatio = Canvas.GetTop(player) / (canvas.ActualHeight - originalHeight);

            player.Width *= shrinkFactor;
            player.Height *= shrinkFactor;

            // Restore position
            double newLeft = leftRatio * (canvas.ActualWidth - player.Width);
            double newTop = topRatio * (canvas.ActualHeight - player.Height);

            // Ensure the new position is within bounds
            if (newLeft < 0) newLeft = 0;
            if (newLeft > canvas.ActualWidth - player.Width) newLeft = canvas.ActualWidth - player.Width;
            if (newTop < 0) newTop = 0;
            if (newTop > canvas.ActualHeight - player.Height) newTop = canvas.ActualHeight - player.Height;

            Canvas.SetLeft(player, newLeft);
            Canvas.SetTop(player, newTop);

            restoreTimer.Tick += RestoreBasketSize;
            restoreTimer.Interval = TimeSpan.FromSeconds(10);
            restoreTimer.Start();
        }


        private void RestoreBasketSize(object sender, EventArgs e)
        {
            restoreTimer.Stop();

            // Restore original size
            player.Width = originalWidth;
            player.Height = originalHeight;

            // Calculate new position ensuring it stays within the canvas bounds
            double newLeft = Canvas.GetLeft(player) * (originalWidth / player.Width);
            double newTop = Canvas.GetTop(player) * (originalHeight / player.Height);

            // Ensure the new position is within bounds
            if (newLeft < 0) newLeft = 0;
            if (newLeft > canvas.ActualWidth - player.Width) newLeft = canvas.ActualWidth - player.Width;
            if (newTop < 0) newTop = 0;
            if (newTop > canvas.ActualHeight - player.Height) newTop = canvas.ActualHeight - player.Height;

            Canvas.SetLeft(player, newLeft);
            Canvas.SetTop(player, newTop);
        }


    }
}
