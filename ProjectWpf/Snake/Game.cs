using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWpf.Snack
{
   
    public enum Direction { Up, Down, Left, Right }
    public enum CellState { Empty, Snake, Apple }

    internal class Game
    {

        private Snack snake;
        private Apple apple;
        private int Width;
        private int Height;
        private Random random;
        public bool GameOver { get; private set; }

        public int Score { get; private set; }

        public Game(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            ResetGame();
        }
        public void ResetGame()
        {
            snake = new Snack(Width / 2, Height / 2);
            random = new Random();
            GenerateApple();
            Score = 0;
            GameOver = false;
        }

        public void Update()
        {
            if (GameOver) return;

            snake.Move();

            if (snake.Head.X == apple.X && snake.Head.Y == apple.Y)
            {
                snake.Grow();
                GenerateApple();
                Score++;
            }

            if (IsGameOver())
            {
                GameOver = true;
            }
        }

        public void ChangeDirection(Direction direction)
        {
            snake.ChangeDirection(direction);
        }

        public CellState GetCellState(int x, int y)
        {
            if (snake.Body.Any(p => p.X == x && p.Y == y))
                return CellState.Snake;
            if (apple.X == x && apple.Y == y)
                return CellState.Apple;
            return CellState.Empty;
        }



        private void GenerateApple()
        {
            do
            {
                apple = new Apple(random.Next(Width), random.Next(Height));
            } while (snake.Body.Any(p => p.X == apple.X && p.Y == apple.Y));
        }




        private bool IsGameOver()
        {
            bool outOfBounds = snake.Head.X < 0 || snake.Head.X >= Width || snake.Head.Y < 0 || snake.Head.Y >= Height;

            bool collidesWithBody = false;
            for (int i = 0; i < snake.Body.Count - 2; i++)
            {
                if (snake.Body[i].X == snake.Head.X && snake.Body[i].Y == snake.Head.Y)
                {
                    collidesWithBody = true;
                    break;
                }
            }

            return outOfBounds || collidesWithBody;
        }


    }
}
