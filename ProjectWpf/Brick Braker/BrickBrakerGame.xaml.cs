using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Threading;
using System.Collections.Generic;

namespace ProjectWpf.Brick_Braker
{
    public partial class BrickBrakerGame : Window
    {
        private Ellipse? _ball; // Allow nullable
        private Paddle? _paddle; // Allow nullable
        private List<Rectangle> _bricks = new List<Rectangle>(); // Initialize in-place
        private DispatcherTimer? _gameTimer; // Allow nullable
        private Vector _ballVelocity;
        private bool _isGameRunning;
        private GameViewModel _viewModel;
        private int _currentLevel;
        private const int TotalLevels = 10;

        public BrickBrakerGame()
        {
            InitializeComponent();
            _viewModel = new GameViewModel();
            DataContext = _viewModel;
            _currentLevel = 1;
            InitializeGame();
        }

        private void InitializeGame()
        {
            GameCanvas.Children.Clear();

            _ball = new Ellipse
            {
                Width = 15,
                Height = 15,
                Fill = Brushes.Red
            };

            _paddle = new Paddle
            {
                Width = 90, 
                Height = 13 
            };

            Canvas.SetLeft(_paddle, 150);
            Canvas.SetTop(_paddle, 360);

            CreateBricks();

            if (_ball != null) GameCanvas.Children.Add(_ball);
            if (_paddle != null) GameCanvas.Children.Add(_paddle);

            _ballVelocity = new Vector(5, -5);

            _gameTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(20)
            };
            if (_gameTimer != null) _gameTimer.Tick += GameTimer_Tick;

            _viewModel.Score = 0;
            UpdateLevelDisplay();

            GameCanvas.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(ResetBallPosition));
        }

        private void UpdateScore(int newScore)
        {
            _viewModel.Score = newScore;
            if (newScore > _viewModel.HighScore)
            {
                _viewModel.HighScore = newScore;
            }
        }

        private void UpdateLevelDisplay()
        {
            LevelTextBlock.Text = _currentLevel.ToString();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (_isGameRunning)
            {
                if (_ball != null)
                {
                    Canvas.SetLeft(_ball, Canvas.GetLeft(_ball) + _ballVelocity.X);
                    Canvas.SetTop(_ball, Canvas.GetTop(_ball) + _ballVelocity.Y);

                    if (Canvas.GetLeft(_ball) <= 0 || Canvas.GetLeft(_ball) + _ball.Width >= GameCanvas.ActualWidth)
                    {
                        _ballVelocity.X = -_ballVelocity.X;
                    }

                    if (Canvas.GetTop(_ball) <= 0)
                    {
                        _ballVelocity.Y = -_ballVelocity.Y;
                    }

                    if (Canvas.GetTop(_ball) + _ball.Height >= GameCanvas.ActualHeight)
                    {
                        EndGame();
                    }
                }

                if (_paddle != null)
                {
                    double paddleLeft = Canvas.GetLeft(_paddle);
                    double paddleTop = Canvas.GetTop(_paddle);
                    double paddleRight = paddleLeft + _paddle.Width;
                    double paddleBottom = paddleTop + _paddle.Height;

                    if (_ball != null)
                    {
                        double ballLeft = Canvas.GetLeft(_ball);
                        double ballTop = Canvas.GetTop(_ball);
                        double ballRight = ballLeft + _ball.Width;
                        double ballBottom = ballTop + _ball.Height;

                        if (ballBottom >= paddleTop && ballTop < paddleBottom && ballRight > paddleLeft && ballLeft < paddleRight)
                        {
                            _ballVelocity.Y = -_ballVelocity.Y;
                            Canvas.SetTop(_ball, paddleTop - _ball.Height);
                        }
                    }
                }

                for (int i = _bricks.Count - 1; i >= 0; i--)
                {
                    Rectangle brick = _bricks[i];
                    double brickLeft = Canvas.GetLeft(brick);
                    double brickTop = Canvas.GetTop(brick);
                    double brickRight = brickLeft + brick.Width;
                    double brickBottom = brickTop + brick.Height;

                    if (_ball != null)
                    {
                        double ballLeft = Canvas.GetLeft(_ball);
                        double ballTop = Canvas.GetTop(_ball);
                        double ballRight = ballLeft + _ball.Width;
                        double ballBottom = ballTop + _ball.Height;

                        if (ballBottom >= brickTop && ballTop <= brickBottom &&
                            ballRight >= brickLeft && ballLeft <= brickRight)
                        {
                            _ballVelocity.Y = -_ballVelocity.Y;
                            GameCanvas.Children.Remove(brick);
                            _bricks.RemoveAt(i);
                            _viewModel.Score++;

                            if (_bricks.Count == 0)
                            {
                                EndGame(true);
                            }

                            break;
                        }
                    }
                }
            }
        }

        private void CreateBricks()
        {
            const int brickWidth = 39;
            const int brickHeight = 20;
            const int gap = 1;

            Brush[] colors = { Brushes.Purple, Brushes.Goldenrod, Brushes.Blue };
            Random random = new Random();

            foreach (var brick in _bricks.ToList())
            {
                GameCanvas.Children.Remove(brick);
            }
            _bricks.Clear();

            int rows = Math.Min(_currentLevel, 10);
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < 10; col++)
                {
                    Rectangle brick = new Rectangle
                    {
                        Width = brickWidth,
                        Height = brickHeight,
                        Fill = colors[random.Next(colors.Length)]
                    };
                    Canvas.SetLeft(brick, col * (brickWidth + gap));
                    Canvas.SetTop(brick, row * (brickHeight + gap));

                    _bricks.Add(brick);
                }
            }

            foreach (var brick in _bricks)
            {
                GameCanvas.Children.Add(brick);
            }

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                MovePaddle(-10);
            }
            else if (e.Key == Key.Right)
            {
                MovePaddle(10);
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                if (_isGameRunning)
                {
                    _isGameRunning = false;
                    if (_gameTimer != null) _gameTimer.Stop();
                }
                else
                {
                    _isGameRunning = true;
                    if (_gameTimer != null) _gameTimer.Start();
                }
            }
        }

        private void MovePaddle(double offset)
        {
            if (_paddle != null)
            {
                double newLeft = Canvas.GetLeft(_paddle) + offset;
                if (newLeft < 0)
                {
                    newLeft = 0;
                }
                else if (newLeft > GameCanvas.ActualWidth - _paddle.Width)
                {
                    newLeft = GameCanvas.ActualWidth - _paddle.Width;
                }
                Canvas.SetLeft(_paddle, newLeft);
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_isGameRunning)
            {
                _isGameRunning = true;
                if (_gameTimer != null) _gameTimer.Start();
                _ballVelocity = new Vector(5, -5);
                StartButton.Visibility = Visibility.Collapsed;
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            _currentLevel = 1;
            GameCanvas.Children.Clear();
            InitializeGame();
            GameOverOverlay.Visibility = Visibility.Collapsed;
            StartButton.Visibility = Visibility.Visible;
        }

        private void EndGame(bool won = false)
        {
            _isGameRunning = false;
            if (_gameTimer != null) _gameTimer.Stop();

            if (_viewModel.Score > _viewModel.HighScore)
            {
                _viewModel.HighScore = _viewModel.Score;
            }

            if (won)
            {
                if (_currentLevel < TotalLevels)
                {
                    _currentLevel++;
                    CreateBricks();
                    ResetBallPosition(); 
                    UpdateLevelDisplay();
                    StartButton.Visibility = Visibility.Visible;
                    StartButton.Content = "Start Next Level";
                }
                else
                {
                    GameOverText.Text = "You Win!";
                    GameOverOverlay.Visibility = Visibility.Visible;
                    StartButton.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                GameOverText.Text = "Game Over";
                GameOverOverlay.Visibility = Visibility.Visible;
                StartButton.Visibility = Visibility.Collapsed;
            }
        }

        private void ResetBallPosition()
        {
            if (_ball != null)
            {
                GameCanvas.UpdateLayout();

                double canvasWidth = GameCanvas.ActualWidth;
                double canvasHeight = GameCanvas.ActualHeight;

                double ballLeft = (canvasWidth - _ball.Width) / 2;
                double ballTop = (canvasHeight - _ball.Height) / 2;

                Canvas.SetLeft(_ball, ballLeft);
                Canvas.SetTop(_ball, ballTop);
            }
        }
    }
}
