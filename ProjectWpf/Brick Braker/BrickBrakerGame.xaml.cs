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
        private Ellipse? _ball;
        private Paddle? _paddle;
        private List<Rectangle> _bricks = new List<Rectangle>();
        private Vector _ballVelocity;
        private bool _isGameRunning;
        private GameViewModel _viewModel;
        private int _currentLevel;
        private const int TotalLevels = 10;
        private const double PaddleSpeed = 20;
        private List<PowerUp> _powerUps = new List<PowerUp>();
        private DispatcherTimer? _powerUpTimer, _paddleTimer, _gameTimer, _ballSpeedTimer, _shieldTimer;
        private double _originalBallSpeed;
        private Dictionary<PowerUp, DateTime> _powerUpExpiration = new Dictionary<PowerUp, DateTime>();
        private const double PowerUpDisplayDuration = 12;
        private int _remainingShieldTime;



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
                Fill = Brushes.Red,
            };

            _paddle = new Paddle
            {
                Width = 90,
                Height = 18
            };

            Canvas.SetLeft(_paddle, 150);
            Canvas.SetTop(_paddle, 380);

            CreateBricks();

            if (_ball != null) GameCanvas.Children.Add(_ball);
            if (_paddle != null) GameCanvas.Children.Add(_paddle);

            _ballVelocity = new Vector(3, -3);
            _originalBallSpeed = 3; // שים לב שהמהירות הנוכחית של הכדור מוגדרת כ-3

            _gameTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(5)
            };

            if (_gameTimer != null) _gameTimer.Tick += GameTimer_Tick;

            _viewModel.Score = 0;
            UpdateLevelDisplay();

            GameCanvas.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(ResetBallPosition));

            _powerUpTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(12) // הזמן בו תופיע תוספת חדשה
            };
            _powerUpTimer.Tick += PowerUpTimer_Tick;
            _powerUpTimer.Start();
        }

        private void PowerUpTimer_Tick(object? sender, EventArgs e)
        {
            CreatePowerUp();
        }

        private void CreatePowerUp()
        {
            PowerUpType type = (PowerUpType)new Random().Next(Enum.GetNames(typeof(PowerUpType)).Length);
            PowerUp powerUp = new PowerUp(type);

            double powerUpWidth = powerUp.Width;
            double powerUpHeight = powerUp.Height;

            bool positionFound = false;
            double x = 0, y = 0;

            while (!positionFound)
            {
                x = new Random().NextDouble() * (GameCanvas.ActualWidth - powerUpWidth);
                y = new Random().NextDouble() * (GameCanvas.ActualHeight - powerUpHeight);

                // קבע מיקום בתנאים שלא יכסה את הפדל או את הבלוקים
                Rect powerUpRect = new Rect(x, y, powerUpWidth, powerUpHeight);
                bool overlapsWithBricks = _bricks.Any(brick =>
                {
                    double brickLeft = Canvas.GetLeft(brick);
                    double brickTop = Canvas.GetTop(brick);
                    double brickRight = brickLeft + brick.Width;
                    double brickBottom = brickTop + brick.Height;
                    Rect brickRect = new Rect(brickLeft, brickTop, brick.Width, brick.Height);
                    return powerUpRect.IntersectsWith(brickRect);
                });

                bool overlapsWithPaddle = _paddle != null &&
                                          new Rect(Canvas.GetLeft(_paddle), Canvas.GetTop(_paddle), _paddle.Width, _paddle.Height)
                                          .IntersectsWith(powerUpRect);

                if (!overlapsWithBricks && !overlapsWithPaddle)
                {
                    positionFound = true;
                }
            }

            Canvas.SetLeft(powerUp, x);
            Canvas.SetTop(powerUp, y);

            _powerUps.Add(powerUp);
            GameCanvas.Children.Add(powerUp);

            // הוספת זמן תפוג לתוסף
            _powerUpExpiration[powerUp] = DateTime.Now.AddSeconds(PowerUpDisplayDuration);
        }

        public void RemovePowerUp(PowerUp powerUp)
        {

            if (_powerUps.Contains(powerUp))
            {
                _powerUps.Remove(powerUp);
                GameCanvas.Children.Remove(powerUp);

            }
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

        private void GameTimer_Tick(object? sender, EventArgs e)
        {
            if (_isGameRunning)
            {
                if (_ball != null)
                {
                    double ballLeft = Canvas.GetLeft(_ball);
                    double ballTop = Canvas.GetTop(_ball);
                    double ballRight = ballLeft + _ball.Width;
                    double ballBottom = ballTop + _ball.Height;

                    double canvasLeft = 0;
                    double canvasTop = 0;
                    double canvasRight = GameCanvas.ActualWidth;
                    double canvasBottom = GameCanvas.ActualHeight;

                    if (ballLeft <= canvasLeft || ballRight >= canvasRight)
                    {
                        _ballVelocity.X = -_ballVelocity.X;
                        if (ballLeft <= canvasLeft) Canvas.SetLeft(_ball, canvasLeft);
                        if (ballRight >= canvasRight) Canvas.SetLeft(_ball, canvasRight - _ball.Width);
                    }

                    if (ballTop <= canvasTop)
                    {
                        _ballVelocity.Y = -_ballVelocity.Y;
                        Canvas.SetTop(_ball, canvasTop);
                    }
                    else if (ballBottom >= canvasBottom)
                    {
                        EndGame();
                        return;
                    }
                    Canvas.SetLeft(_ball, ballLeft + _ballVelocity.X);
                    Canvas.SetTop(_ball, ballTop + _ballVelocity.Y);
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

                List<PowerUp> expiredPowerUps = new List<PowerUp>();
                foreach (PowerUp powerUp in _powerUps)
                {
                    if (_powerUpExpiration[powerUp] <= DateTime.Now)
                    {
                        expiredPowerUps.Add(powerUp);
                    }
                }

                foreach (PowerUp powerUp in expiredPowerUps)
                {
                    RemovePowerUp(powerUp);
                }
                for (int i = _powerUps.Count - 1; i >= 0; i--)
                {
                    PowerUp powerUp = _powerUps[i];
                    double powerUpLeft = Canvas.GetLeft(powerUp);
                    double powerUpTop = Canvas.GetTop(powerUp);
                    double powerUpRight = powerUpLeft + powerUp.Width;
                    double powerUpBottom = powerUpTop + powerUp.Height;

                    if (_ball != null)
                    {
                        double ballLeft = Canvas.GetLeft(_ball);
                        double ballTop = Canvas.GetTop(_ball);
                        double ballRight = ballLeft + _ball.Width;
                        double ballBottom = ballTop + _ball.Height;

                        if (ballBottom >= powerUpTop && ballTop <= powerUpBottom &&
                            ballRight >= powerUpLeft && ballLeft <= powerUpRight)
                        {
                            ApplyPowerUp(powerUp);
                            GameCanvas.Children.Remove(powerUp);
                            _powerUps.RemoveAt(i);
                        }
                    }
                }
            }
        }

        private void ApplyPowerUp(PowerUp powerUp)
        {
            switch (powerUp.Type)
            {
                case PowerUpType.Shield:
                    ActivateShield();
                    break;

                case PowerUpType.SpeedReduction:
                    _ballVelocity *= 1.5;
                    if (_ballSpeedTimer != null)
                    {
                        _ballSpeedTimer.Stop();
                    }
                    _ballSpeedTimer = new DispatcherTimer
                    {
                        Interval = TimeSpan.FromSeconds(8) // הגדרת זמן סיום לתוספת מהירות
                    };
                    _ballSpeedTimer.Tick += BallSpeedTimer_Tick;
                    _ballSpeedTimer.Start();
                    break;

                case PowerUpType.PaddleIncrease:
                    if (_paddle != null)
                    {
                        _paddle.SetWidth(_paddle.Width + 30);
                        _paddle.SetFill(Brushes.Blue);

                        if (_paddleTimer != null)
                        {
                            _paddleTimer.Stop();
                        }

                        _paddleTimer = new DispatcherTimer
                        {
                            Interval = TimeSpan.FromSeconds(10)
                        };

                        _paddleTimer.Tick += PaddleTimer_Tick;
                        _paddleTimer.Start();
                    }
                    break;

                case PowerUpType.ExtraBlocks:
                    AddExtraRowOfBricks();
                    break;

                    // Remove case for Explosion
            }
        }

        private void ActivateShield()
        {
            if (_paddle != null)
            {
                double canvasWidth = GameCanvas.ActualWidth;
                _paddle.SetWidth(canvasWidth);
                _paddle.IsShieldActive = true;
                _paddle.UpdateAppearance();

                if (_shieldTimer != null)
                {
                    _shieldTimer.Stop();
                }

                _shieldTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(1) // Update every second
                };

                _shieldTimer.Tick += ShieldTimer_Tick;
                _shieldTimer.Start();

                _remainingShieldTime = 15; // Shield duration in seconds
            }
        }

        private void ShieldTimer_Tick(object? sender, EventArgs e)
        {
            if (_paddle != null)
            {
                _remainingShieldTime -= 1; // Decrement remaining time
                _paddle.UpdateCountdown((int)_remainingShieldTime); // Update countdown display with integer value

                if (_remainingShieldTime <= 0)
                {
                    _paddle.IsShieldActive = false;
                    _paddle.ResetToOriginal();
                    _paddle.UpdateAppearance();

                    _shieldTimer.Stop();
                }
            }
        }

        public void BallSpeedTimer_Tick(object? sender, EventArgs e)
        {
            double speedFactor = Math.Sqrt(_ballVelocity.X * _ballVelocity.X + _ballVelocity.Y * _ballVelocity.Y);
            if (speedFactor > 0)
            {
                _ballVelocity = new Vector(
                    _originalBallSpeed * (_ballVelocity.X / speedFactor),
                    _originalBallSpeed * (_ballVelocity.Y / speedFactor)
                );
            }

            if (_ballSpeedTimer != null)
            {
                _ballSpeedTimer.Stop();
            }
            _ballVelocity = new Vector(3, -3);

        }

        private void PaddleTimer_Tick(object? sender, EventArgs e)
        {
            if (_paddle != null)
            {
                _paddle.ResetToOriginal();

                if (_paddleTimer != null)
                {
                    _paddleTimer.Stop();
                }
            }
        }

        public void AddExtraRowOfBricks()
        {
            const int brickWidth = 37;
            const int brickHeight = 20;
            const int gap = 3;

            Brush[] colors = { Brushes.Cyan, Brushes.Salmon };
            Random random = new Random();

            int newRow = (_bricks.Count / 10) + 1;
            for (int col = 0; col < 10; col++)
            {
                Rectangle brick = new Rectangle
                {
                    Width = brickWidth,
                    Height = brickHeight,
                    Fill = colors[random.Next(colors.Length)],
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };
                Canvas.SetLeft(brick, col * (brickWidth + gap));
                Canvas.SetTop(brick, newRow * (brickHeight + gap) + 2);

                _bricks.Add(brick);
                GameCanvas.Children.Add(brick);
            }
        }

        private void CreateBricks()
        {
            const int brickWidth = 37;
            const int brickHeight = 20;
            const int gap = 3;
            const int topMargin = 2;

            Brush[] colors = { Brushes.Cyan, Brushes.Salmon };
            Random random = new Random();

            int rows = Math.Min(_currentLevel, 10);
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < 10; col++)
                {
                    Rectangle brick = new Rectangle
                    {
                        Width = brickWidth,
                        Height = brickHeight,
                        Fill = colors[random.Next(colors.Length)],
                        Stroke = Brushes.Black,
                        StrokeThickness = 1
                    };
                    Canvas.SetLeft(brick, col * (brickWidth + gap));
                    Canvas.SetTop(brick, row * (brickHeight + gap) + topMargin);

                    _bricks.Add(brick);
                    GameCanvas.Children.Add(brick);
                }
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                MovePaddle(-PaddleSpeed);
            }
            else if (e.Key == Key.Right)
            {
                MovePaddle(PaddleSpeed);
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
                _ballVelocity = new Vector(3, -3);
                StartButton.Visibility = Visibility.Collapsed;
            }
        }

        private void EndGame(bool won = false)
        {
            _isGameRunning = false;
            if (_gameTimer != null) _gameTimer.Stop();

            HighScoreManager highScoreManager = new HighScoreManager();

            if (_viewModel.Score > _viewModel.HighScore)
            {
                _viewModel.HighScore = _viewModel.Score;
                highScoreManager.SaveHighScore(_viewModel.Score);
            }

            // ניקוי כל הכוחניות לפני המעבר לשלב הבא או סיום המשחק
            foreach (PowerUp powerUp in _powerUps.ToList())
            {
                GameCanvas.Children.Remove(powerUp);
            }
            _powerUps.Clear();

            List<int> highScores = highScoreManager.LoadHighScores();

            // הצגת תוצאות השיא רק כאשר המשחק נגמר
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
                    ShowHighScores(highScores); // הצגת תוצאות השיא
                    GameOverOverlay.Visibility = Visibility.Visible;
                    StartButton.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                GameOverText.Text = "Game Over";
                ShowHighScores(highScores); // הצגת תוצאות השיא
                GameOverOverlay.Visibility = Visibility.Visible;
                StartButton.Visibility = Visibility.Collapsed;
                StartButton.Content = "Start";
            }
        }

        private void ShowHighScores(List<int> highScores)
        {
            HighScoresText.Visibility = Visibility.Visible; // לוודא שהתצוגה מוצגת רק כאן
            string highScoresText = "High Scores:\n";
            for (int i = 0; i < highScores.Count; i++)
            {
                highScoresText += $"{i + 1}. {highScores[i]}\n";
            }
            HighScoresText.Text = highScoresText;
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            // הסרת הבלוקים מהקנבס
            foreach (Rectangle brick in _bricks.ToList())
            {
                GameCanvas.Children.Remove(brick);
            }
            _bricks.Clear();

            // הסרת התוספות מהקנבס
            foreach (PowerUp powerUp in _powerUps.ToList())
            {
                GameCanvas.Children.Remove(powerUp);
            }
            _powerUps.Clear();

            CreateBricks();
            ResetBallPosition();
            _viewModel.Score = 0;
            UpdateScore(0);

            // הסתרת תוצאות השיא בעת משחק מחדש
            HighScoresText.Visibility = Visibility.Collapsed;

            if (!_isGameRunning)
            {
                GameOverOverlay.Visibility = Visibility.Collapsed;
                StartButton.Visibility = Visibility.Visible;
            }
        }


        private void ResetBallPosition()
        {
            if (_ball != null && _paddle != null)
            {
                GameCanvas.UpdateLayout();

                // מקם את הכדור מעל הפדל
                double paddleLeft = Canvas.GetLeft(_paddle);
                double paddleTop = Canvas.GetTop(_paddle);

                double ballLeft = paddleLeft + (_paddle.Width - _ball.Width) / 2;
                double ballTop = paddleTop - _ball.Height;

                Canvas.SetLeft(_ball, ballLeft);
                Canvas.SetTop(_ball, ballTop);
            }
        }
    }
}
