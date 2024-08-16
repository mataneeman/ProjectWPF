using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectWpf.Snack
{
    public enum Direction { Up, Down, Left, Right }
    public enum CellState { Empty, Snake, Apple }

    internal class Game
    {
        private Snack? snake; // Nullable declaration
        private Apple? apple; // Nullable declaration
        private int Width;
        private int Height;
        private Random random;
        public bool GameOver { get; private set; }
        public int Score { get; private set; }
        private HighScoresManager highScoresManager;

        public Game(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            highScoresManager = new HighScoresManager();
            random = new Random(); // Initialize Random
            ResetGame();
        }

        public void ResetGame()
        {
            snake = new Snack(Width / 2, Height / 2); // Initialize Snack
            GenerateApple(); // Initialize Apple
            Score = 0;
            GameOver = false;
        }

        public void Update()
        {
            if (GameOver || snake == null || apple == null) return;

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
                highScoresManager.AddScore(Score); // Save the score when game is over
            }
        }

        public void ChangeDirection(Direction direction)
        {
            if (snake != null)
            {
                snake.ChangeDirection(direction);
            }
        }

        public CellState GetCellState(int x, int y)
        {
            if (snake == null) return CellState.Empty;
            if (snake.Body.Any(p => p.X == x && p.Y == y))
                return CellState.Snake;
            if (apple != null && apple.X == x && apple.Y == y)
                return CellState.Apple;
            return CellState.Empty;
        }

        private void GenerateApple()
        {
            if (snake == null) return;

            do
            {
                apple = new Apple(random.Next(Width), random.Next(Height));
            } while (snake.Body.Any(p => p.X == apple.X && p.Y == apple.Y));
        }

        private bool IsGameOver()
        {
            if (snake == null) return true;

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
