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
        private int fallingBallCount = 0;
        private double ballSpeed = 10;
        private List<Ball> fallingBalls = new List<Ball>();
        private DispatcherTimer gameTimer = new DispatcherTimer();

        public GameEngine(Canvas canvas, Rectangle player)
        {
            this.canvas = canvas;
            this.player = player;
            this.gameTimer.Tick += GameEngine_Tick;
            this.gameTimer.Interval = TimeSpan.FromMilliseconds(20);
        }

        public void StartGame()
        {
            gameTimer.Start();
        }

        public void StopGame()
        {
            gameTimer.Stop();
        }

        private void GameEngine_Tick(object sender, EventArgs e)
        {
            // Update score and missed count
            (canvas.FindName("scoreText") as TextBlock).Text = "" + score;
            (canvas.FindName("missedText") as TextBlock).Text = "" + missed;

            // Continuously add new balls
            if (fallingBalls.Count == 0 || !fallingBalls.Any(b => !b.IsOutOfBounds(canvas)))
            {
                AddNewBall();
            }

            // Move and check each ball
            foreach (var ball in fallingBalls.ToList()) // Use a copy of the list to safely remove items
            {
                ball.MoveDown();

                if (ball.IsOutOfBounds(canvas))
                {
                    fallingBalls.Remove(ball);
                    canvas.Children.Remove(ball.Rectangle);
                    missed++;
                    fallingBallCount++;
                    CheckSpeedIncrease();
                }
                else if (IsCollidingWithPlayer(ball.Rectangle))
                {
                    fallingBalls.Remove(ball);
                    canvas.Children.Remove(ball.Rectangle);
                    score++;
                    fallingBallCount++;
                    CheckSpeedIncrease();
                }
            }
        }

        private void AddNewBall()
        {
            Ball newBall = new Ball(rand, canvas.ActualWidth, ballSpeed);
            newBall.AddToCanvas(canvas);
            fallingBalls.Add(newBall);
        }

        private void CheckSpeedIncrease()
        {
            // Increase speed after every 5 balls
            if (fallingBallCount % 5 == 0)
            {
                ballSpeed += 2; // Increase the speed by 2 (adjust as needed)
            }
        }

        private bool IsCollidingWithPlayer(Rectangle ball)
        {
            Rect playerRect = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);
            Rect ballRect = new Rect(Canvas.GetLeft(ball), Canvas.GetTop(ball), ball.Width, ball.Height);
            return playerRect.IntersectsWith(ballRect);
        }
    }
}
