using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace ProjectWpf.Snack
{
    public partial class SnackGame : Window
    {
        private Game game;
        private DispatcherTimer gameTimer;
        private const int GridSize = 20;
        private HighScoresManager highScoresManager;
        private DispatcherTimer wallMoveTimer;


        public SnackGame()
        {
            InitializeComponent();
            highScoresManager = new HighScoresManager();
            game = new Game(GridSize, GridSize); 
            gameTimer = new DispatcherTimer 
            {
                Interval = TimeSpan.FromMilliseconds(200) 
            };
            gameTimer.Tick += GameTimer_Tick;

            wallMoveTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(10) 
            };
            wallMoveTimer.Tick += WallMoveTimer_Tick;

            InitializeGame();

            foreach (ComboBoxItem item in DifficultyComboBox.Items)
            {
                if (item.Content.ToString() == "Easy")
                {
                    DifficultyComboBox.SelectedItem = item;
                    break;
                }
            }
        }
        private void WallMoveTimer_Tick(object? sender, EventArgs e)
        {
            game.GenerateWalls(); 
            UpdateUI(); 
        }



        private void InitializeGame()
        {
            InitializeGameGrid();
            KeyDown += MainWindow_KeyDown;
            gameTimer.Stop();
        }

        private void InitializeGameGrid()
        {
            GameGrid.Rows = GridSize;
            GameGrid.Columns = GridSize;

            GameGrid.Children.Clear();

            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    bool isLightGreen = (row + col) % 2 == 0; 
                    GridCell cell = new GridCell(isLightGreen);

                    GameGrid.Children.Add(cell);
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (ComboBoxItem item in DifficultyComboBox.Items)
            {
                if (item.Content.ToString() == "Easy")
                {
                    DifficultyComboBox.SelectedItem = item;
                    break;
                }
            }
        }

        private void GameTimer_Tick(object? sender, EventArgs e)
        {
            if (game.GameOver)
            {
                gameTimer.Stop();
                wallMoveTimer.Stop();
                highScoresManager.AddScore(game.Score, GetSelectedDifficulty()); // שמירת התוצאה לפי רמת הקושי
                GameOverOverlay.Visibility = Visibility.Visible;
                UpdateHighScoresUI();
                return;
            }

            game.Update();
            UpdateUI();
        }


        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    game.ChangeDirection(Direction.Up);
                    break;
                case Key.Down:
                    game.ChangeDirection(Direction.Down);
                    break;
                case Key.Left:
                    game.ChangeDirection(Direction.Left);
                    break;
                case Key.Right:
                    game.ChangeDirection(Direction.Right);
                    break;
            }
        }

        private void UpdateUI()
        {
            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    int index = row * GridSize + col;
                    if (index < GameGrid.Children.Count)
                    {
                        GridCell cell = (GridCell)GameGrid.Children[index];
                        CellState state = game.GetCellState(col, row);
                        cell.SetState(state);
                    }
                }
            }

            ScoreText.Text = game.Score.ToString();
        }

        private string GetSelectedDifficulty()
        {
            if (DifficultyComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                return (string)selectedItem.Content;
            }
            return "Easy"; // ברירת מחדל במקרה שאין רמה נבחרת
        }
        private void UpdateHighScoresUI()
        {
            string difficulty = GetSelectedDifficulty();
            List<int> highScores = highScoresManager.GetHighScores(difficulty);
            string highScoresText = $"High Scores ({difficulty}):\n";
            for (int i = 0; i < highScores.Count; i++)
            {
                highScoresText += $"{i + 1}. {highScores[i]}\n";
            }
            HighScoresText.Text = highScoresText;
        }

        private void DifficultyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DifficultyComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedDifficulty = (string)selectedItem.Content;
                game.SetDifficulty(selectedDifficulty);

                switch (selectedDifficulty)
                {
                    case "Easy":
                        gameTimer.Interval = TimeSpan.FromMilliseconds(300);
                        break;
                    case "Medium":
                        gameTimer.Interval = TimeSpan.FromMilliseconds(200);
                        break;
                    case "Hard":
                        gameTimer.Interval = TimeSpan.FromMilliseconds(100);
                        break;
                }
            }
        }


        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (game.GameOver)
            {
                game.ResetGame(); 
                GameOverOverlay.Visibility = Visibility.Collapsed; 
                StartButton.Visibility = Visibility.Collapsed; 
                ResetButton.Visibility = Visibility.Visible; 
            }
            else
            {
                StartButton.Visibility = Visibility.Collapsed; 
                ResetButton.Visibility = Visibility.Visible; 
                DifficultyComboBox.IsEnabled = false; 
                gameTimer.Start(); 
                wallMoveTimer.Start(); 
            }
        }


        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            game.ResetGame(); 
            GameOverOverlay.Visibility = Visibility.Collapsed; 
            StartButton.Visibility = Visibility.Visible; 
            ResetButton.Visibility = Visibility.Collapsed;
            DifficultyComboBox.IsEnabled = true; 
        }

    }
}
