using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;

namespace ProjectWpf.Snack
{
    public enum Direction { Up, Down, Left, Right }
    public enum CellState { Empty, Snake, Apple, Wall }

    internal class Game
    {
        private Snack? snake; 
        private Apple? apple; 
        private List<Point> walls; 
        private int Width;
        private int Height;
        private Random random;
        private DispatcherTimer wallChangeTimer; 
        private int wallCount;
        public bool GameOver { get; private set; }
        public int Score { get; private set; }

        public Game(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            random = new Random(); 
            walls = new List<Point>();
            wallChangeTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(10) 
            };
            wallChangeTimer.Tick += (sender, e) => GenerateWalls();
            ResetGame();
        }

        public void ResetGame()
        {
            snake = new Snack(Width / 2, Height / 2); 
            GenerateApple(); 
            GenerateWalls(); 
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
                wallChangeTimer.Stop();
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
            if (walls.Any(w => w.X == x && w.Y == y))
                return CellState.Wall;
            return CellState.Empty;
        }

        private void GenerateApple()
        {
            if (snake == null) return;

            do
            {
                apple = new Apple(random.Next(Width), random.Next(Height));
            } while (snake.Body.Any(p => p.X == apple.X && p.Y == apple.Y) || walls.Any(w => w.X == apple.X && w.Y == apple.Y));
        }

        public void GenerateWalls()
        {
            walls.Clear();

            for (int i = 0; i < wallCount; i++)
            {
                Point wallPoint;
                do
                {
                    wallPoint = new Point(random.Next(Width), random.Next(Height));
                } while (walls.Contains(wallPoint) || (snake != null && snake.Body.Contains(wallPoint)));

                walls.Add(wallPoint);
            }
        }
        public void SetDifficulty(string difficulty)
        {
            switch (difficulty)
            {
                case "Easy":
                    wallCount = 3;
                    break;
                case "Medium":
                    wallCount = 5;
                    break;
                case "Hard":
                    wallCount = 8;
                    break;
            }
            GenerateWalls();
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

            bool collidesWithWalls = walls.Any(wall => wall.X == snake.Head.X && wall.Y == snake.Head.Y);


            return outOfBounds || collidesWithBody || collidesWithWalls;
        }

    }
}
