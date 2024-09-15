using ProjectWpf.Tic_Tac_Toe;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;  

namespace ProjectWpf
{
    public partial class TicTacToe : Window
    {
        private TicTacToeGame game = new TicTacToeGame();
        private bool isPlayingWithComputer;
        private int player1Score;
        private int player2Score;
        private Random random = new Random();

        public TicTacToe()
        {
            InitializeComponent();
            InitializeButtons();
            UpdateScoreboard();

        }

        private void InitializeButtons()
        {
            button1.Tag = new Tuple<int, int>(0, 0);
            button2.Tag = new Tuple<int, int>(0, 1);
            button3.Tag = new Tuple<int, int>(0, 2);
            button4.Tag = new Tuple<int, int>(1, 0);
            button5.Tag = new Tuple<int, int>(1, 1);
            button6.Tag = new Tuple<int, int>(1, 2);
            button7.Tag = new Tuple<int, int>(2, 0);
            button8.Tag = new Tuple<int, int>(2, 1);
            button9.Tag = new Tuple<int, int>(2, 2);

            button1.Click += Button_Click;
            button2.Click += Button_Click;
            button3.Click += Button_Click;
            button4.Click += Button_Click;
            button5.Click += Button_Click;
            button6.Click += Button_Click;
            button7.Click += Button_Click;
            button8.Click += Button_Click;
            button9.Click += Button_Click;
            GoBackButton.Click += BackButton_Click;
        }

        private void PlayWithComputer_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Collapsed;
            DifficultyMenu.Visibility = Visibility.Visible;
        }

        private void PlayWithFriend_Click(object sender, RoutedEventArgs e)
        {
            isPlayingWithComputer = false;
            StartGame();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            GameOverOverlay.Visibility = Visibility.Collapsed;
            MainMenu.Visibility = Visibility.Visible;
            GameGrid.Visibility = Visibility.Collapsed;
            Scoreboard.Visibility = Visibility.Collapsed;
            DifficultyMenu.Visibility = Visibility.Collapsed; 

            ResetScores();
        }

        private void DifficultyButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string difficulty = (string)button.Tag;

            if (difficulty == "Easy")
            {
                isPlayingWithComputer = true;
                game.SetDifficultyEasy();
                StartGame();
            }
            else if (difficulty == "Hard")
            {
                isPlayingWithComputer = true;
                game.SetDifficultyHard();
                StartGame();
            }

            DifficultyMenu.Visibility = Visibility.Collapsed;
        }

        private void StartGame()
        {
            MainMenu.Visibility = Visibility.Collapsed;
            Scoreboard.Visibility = Visibility.Visible;
            GameGrid.Visibility = Visibility.Visible;
            GameOverOverlay.Visibility = Visibility.Collapsed;
            game.ResetGame();
            ResetButtonContent();
        }

        private void ResetButtonContent()
        {
            button1.Content = "";
            button2.Content = "";
            button3.Content = "";
            button4.Content = "";
            button5.Content = "";
            button6.Content = "";
            button7.Content = "";
            button8.Content = "";
            button9.Content = "";

            button1.IsEnabled = true;
            button2.IsEnabled = true;
            button3.IsEnabled = true;
            button4.IsEnabled = true;
            button5.IsEnabled = true;
            button6.IsEnabled = true;
            button7.IsEnabled = true;
            button8.IsEnabled = true;
            button9.IsEnabled = true;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Tuple<int,int> position = (Tuple<int, int>)button.Tag;
            int row = position.Item1;
            int col = position.Item2;

            if (!button.IsEnabled) return;

            game.MarkButton(button, row, col);
            GameStatus status = game.CheckGameStatus();

            if (status != GameStatus.InProgress)
            {
                EndGame(status);
            }
            else
            {
                game.SwitchTurn();
                if (isPlayingWithComputer && !game.Player1Turn)
                {
                    await Task.Delay(500); 
                    ComputerPlay();
                }
            }
        }


        private Button? MakeEasyComputerMove()
        {
            Button? button;
            int row;
            int col;
            do
            {
                row = random.Next(3);
                col = random.Next(3);
                button = GetButtonAt(row, col);
            } while (button == null || !button.IsEnabled);

            if (button != null)
            {
                game.MarkButton(button, row, col);
                GameStatus status = game.CheckGameStatus();
                if (status != GameStatus.InProgress)
                {
                    EndGame(status);
                }
                else
                {
                    game.SwitchTurn();
                }
            }
            return button;
        }


        private void MakeHardComputerMove()
        {
            Button? bestMove = FindBestMove(isBlocking: false);

            if (bestMove == null)
            {
                bestMove = FindBestMove(isBlocking: true);
            }

            if (bestMove == null)
            {
                bestMove = MakeEasyComputerMove();
            }

            if (bestMove != null)
            {
                MakeMove(bestMove);
            }
        }




        private void MakeMove(Button button)
        {
            Tuple<int, int>? position = button.Tag as Tuple<int, int>;
            if (position != null)
            {
                game.MarkButton(button, position.Item1, position.Item2);
                GameStatus status = game.CheckGameStatus();
                if (status != GameStatus.InProgress)
                {
                    EndGame(status);
                }
                else
                {
                    game.SwitchTurn();
                }
            }
        }
        private int Minimax(int depth, bool isMaximizing)
        {
            GameStatus status = game.CheckGameStatus();

            if (status == GameStatus.Win)
                return isMaximizing ? -10 : 10;
            if (status == GameStatus.Draw)
                return 0;

            int bestScore = isMaximizing ? int.MinValue : int.MaxValue;

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    Button? button = GetButtonAt(row, col);
                    if (button != null && button.IsEnabled)
                    {
                        game.MarkButton(button, row, col);
                        int score = Minimax(depth + 1, !isMaximizing); 
                        game.ReverseMarkButton(button, row, col); 

                        if (isMaximizing)
                            bestScore = Math.Max(score, bestScore);
                        else
                            bestScore = Math.Min(score, bestScore);
                    }
                }
            }
            return bestScore;
        }

        private Button? FindBestMove(bool isBlocking)
        {
            int bestScore = isBlocking ? int.MaxValue : int.MinValue;
            Button? bestMove = null;

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    Button? button = GetButtonAt(row, col);
                    if (button != null && button.IsEnabled)
                    {
                        game.MarkButton(button, row, col);
                        int score = Minimax(0, !isBlocking); 
                        game.ReverseMarkButton(button, row, col); 

                        if (isBlocking)
                        {
                            if (score < bestScore)
                            {
                                bestScore = score;
                                bestMove = button;
                            }
                        }
                        else
                        {
                            if (score > bestScore)
                            {
                                bestScore = score;
                                bestMove = button;
                            }
                        }
                    }
                }
            }
            return bestMove;
        }



        private async void ComputerPlay()
        {
            if (game.IsHardDifficulty)
            {
                Button? bestMove = FindBestMove(isBlocking: false);

                if (bestMove == null)
                {
                    bestMove = FindBestMove(isBlocking: true);
                }

                if (bestMove == null)
                {
                    bestMove = MakeEasyComputerMove();
                }

                await Task.Delay(1000);

                if (bestMove != null)
                {
                    MakeMove(bestMove);
                }
            }
            else
            {
                Button? easyMove = MakeEasyComputerMove();

                await Task.Delay(1000);

                if (easyMove != null)
                {
                    MakeMove(easyMove);
                }
            }
        }




        private Button? FindWinningMove()
        {
            return FindBestMove(isBlocking: false);
        }

        private Button? FindBlockingMove()
        {
            return FindBestMove(isBlocking: true);
        }


        private Button? GetButtonAt(int row, int col)
        {
            return GameGrid.Children.OfType<Button>()
                .FirstOrDefault(x => Grid.GetRow(x) == row && Grid.GetColumn(x) == col);
        }


        private void UpdateScoreboard()
        {
            Player1Score.Text = $"Player 1 (X): {player1Score}";
            Player2Score.Text = $"Player 2 (O): {player2Score}";
        }

        private void EndGame(GameStatus status)
        {
            string winnerMessage = "";

            switch (status)
            {
                case GameStatus.Win:
                    winnerMessage = $"Player {(game.Player1Turn ? 1 : 2)} ({(game.Player1Turn ? "X" : "O")}) wins!";
                    if (game.Player1Turn)
                    {
                        player1Score++;
                    }
                    else
                    {
                        player2Score++;
                    }
                    break;
                case GameStatus.Draw:
                    winnerMessage = "It's a draw!";
                    break;
            }

            UpdateScoreboard();
            GameOverMessage.Text = winnerMessage;  
            GameOverOverlay.Visibility = Visibility.Visible;
        }

        private void PlayAgainButton_Click(object sender, RoutedEventArgs e)
        {
            StartGame();
        }

        private void ResetScores()
        {
            player1Score = 0;
            player2Score = 0;
              UpdateScoreboard();
        }
    }
}
