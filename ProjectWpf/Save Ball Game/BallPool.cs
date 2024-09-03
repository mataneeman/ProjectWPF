using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace ProjectWpf.Save_Ball_Game
{
    public class BallPool
    {
        private Canvas canvas;
        private Queue<Ball> availableBalls = new Queue<Ball>();
        private Random rand = new Random();

        public BallPool(Canvas canvas)
        {
            this.canvas = canvas;
        }

        public Ball GetBall()
        {
            if (availableBalls.Count > 0)
            {
                Ball ball = availableBalls.Dequeue();
                ball.Show(); // Ensure the ball is visible
                return ball;
            }
            return CreateNewBall();
        }

        public void ReturnBall(Ball ball)
        {
            ball.Hide();
            availableBalls.Enqueue(ball);
        }

        private Ball CreateNewBall()
        {
            Ball.BallType type = (Ball.BallType)rand.Next(0, 4); // 0: Regular, 1: Bomb, 2: Virus, 3: Coin
            Ball newBall = new Ball(canvas.ActualWidth, type);
            return newBall;
        }
    }
}
