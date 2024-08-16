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

        public SnackGame()
        {
            InitializeComponent();
            highScoresManager = new HighScoresManager();
            game = new Game(GridSize, GridSize); // Initialize Game here
            gameTimer = new DispatcherTimer // Initialize DispatcherTimer here
            {
                Interval = TimeSpan.FromMilliseconds(200) // Default interval
            };
            gameTimer.Tick += GameTimer_Tick;

            InitializeGame(); // Initialize game settings

            // Set default selection to "Easy"
            foreach (ComboBoxItem item in DifficultyComboBox.Items)
            {
                if (item.Content.ToString() == "Easy")
                {
                    DifficultyComboBox.SelectedItem = item;
                    break;
                }
            }
        }

        private void InitializeGame()
        {
            InitializeGameGrid();
            KeyDown += MainWindow_KeyDown;
            gameTimer.Stop();
        }

        private void InitializeGameGrid()
        {
            // Define the number of rows and columns for UniformGrid
            GameGrid.Rows = GridSize;
            GameGrid.Columns = GridSize;

            // Clear previous content
            GameGrid.Children.Clear();

            // Add cells to the UniformGrid
            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    bool isLightGreen = (row + col) % 2 == 0; // Alternate colors
                    GridCell cell = new GridCell(isLightGreen);

                    // Add cell to the UniformGrid without specifying row and column
                    GameGrid.Children.Add(cell);
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Set default selection to "Easy"
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
                highScoresManager.AddScore(game.Score); // Save the score
                GameOverOverlay.Visibility = Visibility.Visible; // Show the game over overlay
                UpdateHighScoresUI(); // Update high scores display
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

        private void UpdateHighScoresUI()
        {
            List<int> highScores = highScoresManager.HighScores;
            string highScoresText = "High Scores:\n";
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
                game.ResetGame(); // Restart the game if it was over
                GameOverOverlay.Visibility = Visibility.Collapsed; // Hide the game over overlay
                StartButton.Visibility = Visibility.Collapsed; // Hide the start button
                ResetButton.Visibility = Visibility.Visible; // Show the reset button
            }
            else
            {
                StartButton.Visibility = Visibility.Collapsed; // Hide the start button
                ResetButton.Visibility = Visibility.Visible; // Show the reset button
                DifficultyComboBox.IsEnabled = false; // Disable difficulty selection during game
                gameTimer.Start(); // Start the game timer
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            game.ResetGame(); // Restart the game
            GameOverOverlay.Visibility = Visibility.Collapsed; // Hide the game over overlay
            StartButton.Visibility = Visibility.Visible; // Show the start button
            ResetButton.Visibility = Visibility.Collapsed; // Hide the reset button
            DifficultyComboBox.IsEnabled = true; // Enable difficulty selection
        }

       /* private void ShowHighScoresButton_Click(object sender, RoutedEventArgs e)
        {
            string highScoresText = string.Join("\n", highScoresManager.HighScores.Select((score, index) => $"{index + 1}. {score}"));
            MessageBox.Show(highScoresText, "High Scores");
        }*/
    }
}
