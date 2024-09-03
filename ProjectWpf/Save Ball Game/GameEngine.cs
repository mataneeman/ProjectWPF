using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Media;
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
        private double ballSpeed = 10;
        private double currentBallSpeed;
        private List<Ball> fallingBalls = new List<Ball>();
        private DispatcherTimer gameTimer = new DispatcherTimer();
        private Action<int> updateLives;
        private DateTime lastBallCreationTime = DateTime.Now;
        private const double BallSpeedIncrement = 0.4;
        private const double BallCreationIntervalSeconds = 1.5;
        private const int MaxBallsOnScreen = 10;
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
            this.gameTimer.Interval = TimeSpan.FromMilliseconds(33); // 30 FPS
            this.currentBallSpeed = ballSpeed;
            UpdateLivesDisplay();
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
                ball.MoveDown(currentBallSpeed);
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
            if (updateLives != null)
            {
                updateLives(lives);
            }
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
            // Weighted probabilities
            int chance = rand.Next(0, 100);
            if (chance < 80) // 80% chance for regular balls
            {
                return Ball.BallType.Regular;
            }
            else if (chance < 95) // 10% chance for bombs
            {
                return Ball.BallType.Bomb;
            }
            else if (chance < 95) // 5% chance for viruses
            {
                return Ball.BallType.Virus;
            }
            else if (chance < 95) // 5% chance for coins
            {
                return Ball.BallType.Coin;
            }
            else // 0% chance for hearts
            {
                return Ball.BallType.live;
            }
        }


        private bool RectsOverlap(Rect rect1, Rect rect2)
        {
            return rect1.IntersectsWith(rect2);
        }

        private void CheckSpeedIncrease()
        {
            if (score % 5 == 0)
            {
                currentBallSpeed += BallSpeedIncrement;
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
            double playerMargin = 10;

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

            player.Width *= shrinkFactor;
            player.Height *= shrinkFactor;

            // Calculate new position
            double newLeft = leftRatio * (canvas.ActualWidth - player.Width);

            // Ensure new position is within canvas bounds
            double adjustedLeft = Math.Max(0, Math.Min(newLeft, canvas.ActualWidth - player.Width));
            Canvas.SetLeft(player, adjustedLeft);
            Canvas.SetTop(player, canvas.ActualHeight - player.Height);

            restoreTimer.Interval = TimeSpan.FromSeconds(15);
            restoreTimer.Tick += RestoreBasketSize;
            restoreTimer.Start();
        }

        private void RestoreBasketSize(object sender, EventArgs e)
        {
            // Restore original size
            player.Width = originalWidth;
            player.Height = originalHeight;

            // Recalculate and update position
            UpdatePlayerPosition();

            restoreTimer.Stop();
            restoreTimer.Tick -= RestoreBasketSize;
        }

        private void UpdatePlayerPosition()
        {
            double leftRatio = Canvas.GetLeft(player) / (canvas.ActualWidth - player.Width);
            double newLeft = leftRatio * (canvas.ActualWidth - player.Width);

            // Ensure new position is within canvas bounds
            double adjustedLeft = Math.Max(0, Math.Min(newLeft, canvas.ActualWidth - player.Width));
            Canvas.SetLeft(player, adjustedLeft);
            Canvas.SetTop(player, canvas.ActualHeight - player.Height);
        }
    }
}