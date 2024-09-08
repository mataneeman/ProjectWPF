using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace ProjectWpf.Memory_Game_Merage
{
    public partial class MemoryGame : Window
    {
        private MemoryGameLogic? _gameLogic;
        private List<Button> _buttonControls = new List<Button>();
        private Dictionary<int, Shape> _shapeDictionary = new Dictionary<int, Shape>();
        private string _difficultyLevel = "Easy";
        private bool _isTwoPlayersMode;
        private bool _isPlayer1Turn;
        private int _player1SelectedCards;
        private int _player2SelectedCards;
        private GameResultsManager? _gameResultsManager;
        private DispatcherTimer? _gameTimer;
        private TimeSpan _timeLeft;

        public MemoryGame()
        {
            InitializeComponent();
            InitializeShapes();
            ShowEntranceScreen();
            CurrentPlayerTextBlock.Visibility = Visibility.Collapsed;
        }

        private void ShowEntranceScreen()
        {
            EntranceScreen.Visibility = Visibility.Visible;
            GameScreen.Visibility = Visibility.Collapsed;
        }

        private void ShowGameScreen()
        {
            EntranceScreen.Visibility = Visibility.Collapsed;
            GameScreen.Visibility = Visibility.Visible;
        }

        private void InitializeShapes()
        {
            double shapeSize = _difficultyLevel switch
            {
                "Easy" => 50,
                "Medium" => 40,
                "Hard" => 30,
                _ => 55
            };

            for (int i = 1; i <= 18; i++)
            {
                _shapeDictionary[i] = CreateShape(i, shapeSize);
            }
        }

        private void InitializeGame()
        {
            _gameLogic = new MemoryGameLogic(_difficultyLevel, _isTwoPlayersMode);
            _buttonControls = new List<Button>();

            GameCanvas.Children.Clear();

            int rows, columns;
            double margin = 5;
            double borderMargin = 5;

            (rows, columns) = _difficultyLevel switch
            {
                "Easy" => (4, 4),
                "Medium" => (6, 5),
                "Hard" => (6, 6),
                _ => (4, 4)
            };

            double canvasWidth = GameCanvas.ActualWidth - 2 * borderMargin;
            double canvasHeight = GameCanvas.ActualHeight - 2 * borderMargin;

            double buttonSize;
            if (canvasWidth / columns < canvasHeight / rows)
            {
                buttonSize = (canvasWidth - (columns - 1) * margin) / columns;
            }
            else
            {
                buttonSize = (canvasHeight - (rows - 1) * margin) / rows;
            }

            double leftOffset = borderMargin + (canvasWidth - (columns * buttonSize + (columns - 1) * margin)) / 2;
            double topOffset = borderMargin + (canvasHeight - (rows * buttonSize + (rows - 1) * margin)) / 2;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Button button = new Button
                    {
                        Width = buttonSize,
                        Height = buttonSize,
                        Tag = i * columns + j,
                        Cursor = Cursors.Hand
                    };
                    button.Click += CardButton_Click;

                    Canvas.SetLeft(button, leftOffset + j * (buttonSize + margin));
                    Canvas.SetTop(button, topOffset + i * (buttonSize + margin));

                    GameCanvas.Children.Add(button);
                    _buttonControls.Add(button);

                    Shape shape = CreateShape((i * columns + j) % _shapeDictionary.Count + 1, buttonSize);

                    Grid container = new Grid();
                    container.Children.Add(shape);
                    button.Content = container;
                }
            }

            _gameLogic.LoadShapes(_buttonControls, _shapeDictionary);

            _gameLogic.ScoreChanged += OnScoreChanged;
            _gameLogic.GameOver += OnGameOver;

            _gameResultsManager = new GameResultsManager(_gameLogic.Player1, _gameLogic.Player2 ?? new Player { Name = "Default Player" });

            _timeLeft = _difficultyLevel switch
            {
                "Easy" => TimeSpan.FromMinutes(1),
                "Medium" => TimeSpan.FromMinutes(2),
                "Hard" => TimeSpan.FromMinutes(3),
                _ => TimeSpan.FromMinutes(1)
            };

            _gameTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _gameTimer.Tick += GameTimer_Tick;
            _gameTimer.Start();
            UpdateTimerDisplay();
        }

        private void GameTimer_Tick(object? sender, EventArgs e)
        {
            if (_timeLeft > TimeSpan.Zero)
            {
                _timeLeft = _timeLeft.Subtract(TimeSpan.FromSeconds(1));
                UpdateTimerDisplay();
            }
            else
            {
                _gameTimer?.Stop();
                OnGameOver();
            }
        }

        private void UpdateTimerDisplay()
        {
            if (TimerTextBlock != null)
            {
                TimerTextBlock.Text = _timeLeft.ToString(@"mm\:ss");

                if (_timeLeft <= TimeSpan.FromSeconds(10))
                {
                    TimerTextBlock.Foreground = Brushes.Red;
                }
                else
                {
                    TimerTextBlock.Foreground = Brushes.White;
                }
            }
        }

        private void UpdateCurrentPlayerDisplay()
        {
            if (_isTwoPlayersMode)
            {
                if (CurrentPlayerTextBlock != null)
                {
                    CurrentPlayerTextBlock.Text = _isPlayer1Turn ? "Player 1's Turn" : "Player 2's Turn";
                    CurrentPlayerTextBlock.Foreground = _isPlayer1Turn ? Brushes.Gold : Brushes.Silver;
                    CurrentPlayerTextBlock.Visibility = Visibility.Visible;
                }
            }
            else
            {
                if (CurrentPlayerTextBlock != null)
                {
                    CurrentPlayerTextBlock.Visibility = Visibility.Collapsed;
                }
            }
        }

        private Shape CreateShape(int shapeId, double size)
        {
            Shape shape;
            switch (shapeId)
            {
                case 1:
                    shape = ShapeFactory.CreateCircle(size);
                    shape.Fill = Brushes.Red;
                    break;
                case 2:
                    shape = ShapeFactory.CreateRectangle(size);
                    shape.Fill = Brushes.Blue;
                    break;
                case 3:
                    shape = ShapeFactory.CreateTriangle(size);
                    shape.Fill = Brushes.Green;
                    break;
                case 4:
                    shape = ShapeFactory.CreateHexagon(size);
                    shape.Fill = Brushes.Yellow;
                    break;
                case 5:
                    shape = ShapeFactory.CreateCircle(size);
                    shape.Fill = Brushes.Purple;
                    break;
                case 6:
                    shape = ShapeFactory.CreateRectangle(size);
                    shape.Fill = Brushes.Orange;
                    break;
                case 7:
                    shape = ShapeFactory.CreateRoundedRectangle(size);
                    shape.Fill = Brushes.Cyan;
                    break;
                case 8:
                    shape = ShapeFactory.CreateStar(size);
                    shape.Fill = Brushes.Magenta;
                    break;
                case 9:
                    shape = ShapeFactory.CreateDiamond(size);
                    shape.Fill = Brushes.Brown;
                    break;
                case 10:
                    shape = ShapeFactory.CreatePentagon(size);
                    shape.Fill = Brushes.Teal;
                    break;
                case 11:
                    shape = ShapeFactory.CreateOctagon(size);
                    shape.Fill = Brushes.Lime;
                    break;
                case 12:
                    shape = ShapeFactory.CreateTrapezium(size);
                    shape.Fill = Brushes.Maroon;
                    break;
                case 13:
                    shape = ShapeFactory.CreateTrapezoid(size);
                    shape.Fill = Brushes.SkyBlue;
                    break;
                case 14:
                    shape = ShapeFactory.CreateEllipseShape(size);
                    shape.Fill = Brushes.OrangeRed;
                    break;
                case 15:
                    shape = ShapeFactory.CreateHalfCircle(size);
                    shape.Fill = Brushes.Coral;
                    break;
                case 16:
                    shape = ShapeFactory.CreateInvertedTrapezium(size);
                    shape.Fill = Brushes.Pink;
                    break;
                case 17:
                    shape = ShapeFactory.CreateRoundedSquare(size);
                    shape.Fill = Brushes.LightGreen;
                    break;
                case 18:
                    shape = ShapeFactory.CreateEllipseShape(size);
                    shape.Fill = Brushes.Salmon;
                    break;
                default:
                    shape = ShapeFactory.CreateRectangle(size);
                    shape.Fill = Brushes.Transparent;
                    break;
            }
            return shape;
        }

        private async Task HandleTurnEndAsync()
        {
            await Task.Delay(1000);
            if (_isTwoPlayersMode)
            {
                _isPlayer1Turn = !_isPlayer1Turn;
                UpdateCurrentPlayerDisplay();
            }
        }

        private async void CardButton_Click(object sender, RoutedEventArgs e)
        {
            if (_gameLogic == null) return;

            Button? clickedButton = sender as Button;
            if (clickedButton != null && !_gameLogic.IsGameOver)
            {
                _gameLogic.ButtonClicked(clickedButton);

                if (_isTwoPlayersMode)
                {
                    if (_gameLogic.IsPlayer1Turn != _isPlayer1Turn)
                    {
                        if (_isPlayer1Turn)
                        {
                            _player1SelectedCards++;
                            if (_player1SelectedCards == 2)
                            {
                                _player1SelectedCards = 0;
                                await HandleTurnEndAsync();
                            }
                        }
                        else
                        {
                            _player2SelectedCards++;
                            if (_player2SelectedCards == 2)
                            {
                                _player2SelectedCards = 0;
                                await HandleTurnEndAsync();
                            }
                        }
                    }
                }
            }
        }

        private void OnScoreChanged(int score)
        {
            Dispatcher.Invoke(() =>
            {
                if (_isTwoPlayersMode)
                {
                    if (_isPlayer1Turn)
                    {
                        Player1ScoreTextBlock.Text = $"Player 1: {score}";
                    }
                    else
                    {
                        Player2ScoreTextBlock.Text = $"Player 2: {score}";
                    }
                }
                else
                {
                    if (Player1ScoreTextBlock != null)
                    {
                        Player1ScoreTextBlock.Text = $"Player 1: {score}";
                    }
                }
            });
        }

        private void OnGameOver()
        {
            Dispatcher.Invoke(() =>
            {
                if (_gameLogic == null) return;

                if (_gameLogic.Player2 == null)
                {
                    if (_gameLogic.Player1 != null)
                    {
                        GameOverText.Text = _gameLogic.Player1.Score > 0 ? $"{_gameLogic.Player1.Name} Wins!" : "Game Over!";
                    }
                    else
                    {
                        GameOverText.Text = "Game Over!";
                    }
                }
                else
                {
                    GameOverText.Text = _gameResultsManager?.GetGameOverMessage() ?? "Game Over!";
                }

                _gameTimer?.Stop();
                if (CurrentPlayerTextBlock != null)
                {
                    CurrentPlayerTextBlock.Visibility = Visibility.Collapsed;
                }
                if (GameOverOverlay != null)
                {
                    GameOverOverlay.Visibility = Visibility.Visible;
                }
                if (StartButton != null)
                {
                    StartButton.Visibility = Visibility.Collapsed;
                }
            });
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            InitializeGame();
            UpdateTimerDisplay();
            _gameTimer?.Start();
            if (GameOverOverlay != null)
            {
                GameOverOverlay.Visibility = Visibility.Collapsed;
            }
            if (StartButton != null)
            {
                StartButton.Visibility = Visibility.Collapsed;
            }
            if (DifficultyComboBox != null)
            {
                DifficultyComboBox.IsEnabled = false;
            }
            UpdateCurrentPlayerDisplay();
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            InitializeGame();
            ResetGame();

            _timeLeft = _difficultyLevel switch
            {
                "Easy" => TimeSpan.FromMinutes(1),
                "Medium" => TimeSpan.FromMinutes(2),
                "Hard" => TimeSpan.FromMinutes(3),
                _ => TimeSpan.FromMinutes(1)
            };
            UpdateTimerDisplay();
            _gameTimer?.Stop();
        }

        private void DifficultyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem? selectedItem = DifficultyComboBox.SelectedItem as ComboBoxItem;
            if (selectedItem != null && selectedItem.Content is string selectedContent)
            {
                _difficultyLevel = selectedContent;
            }
            else
            {
                _difficultyLevel = "Easy";
            }

            InitializeShapes();
        }

        private void SinglePlayerButton_Click(object sender, RoutedEventArgs e)
        {
            _isTwoPlayersMode = false;
            ShowGameScreen();
            if (Scoreboard != null)
            {
                Scoreboard.Visibility = Visibility.Visible;
            }
            if (Player2ScoreTextBlock != null)
            {
                Player2ScoreTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void TwoPlayersButton_Click(object sender, RoutedEventArgs e)
        {
            _isTwoPlayersMode = true;
            ShowGameScreen();
            if (Scoreboard != null)
            {
                Scoreboard.Visibility = Visibility.Visible;
            }
            if (Player2ScoreTextBlock != null)
            {
                Player2ScoreTextBlock.Visibility = Visibility.Visible;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ResetGame();
            if (CurrentPlayerTextBlock != null)
            {
                CurrentPlayerTextBlock.Visibility = Visibility.Collapsed;
            }
            ShowEntranceScreen();
            _timeLeft = _difficultyLevel switch
            {
                "Easy" => TimeSpan.FromMinutes(1),
                "Medium" => TimeSpan.FromMinutes(2),
                "Hard" => TimeSpan.FromMinutes(3),
                _ => TimeSpan.FromMinutes(1)
            };
            UpdateTimerDisplay();
            _gameTimer?.Stop();
        }

        private void ResetGame()
        {
            _gameLogic = null;
            _buttonControls.Clear();
            GameCanvas.Children.Clear();
            if (CurrentPlayerTextBlock != null)
            {
                CurrentPlayerTextBlock.Text = "";
            }
            if (Player1ScoreTextBlock != null)
            {
                Player1ScoreTextBlock.Text = "Player 1: 0";
            }
            if (Player2ScoreTextBlock != null)
            {
                Player2ScoreTextBlock.Text = "Player 2: 0";
            }
            if (GameOverOverlay != null)
            {
                GameOverOverlay.Visibility = Visibility.Collapsed;
            }
            if (StartButton != null)
            {
                StartButton.Visibility = Visibility.Visible;
            }
            if (DifficultyComboBox != null)
            {
                DifficultyComboBox.IsEnabled = true;
            }
        }
    }
}
