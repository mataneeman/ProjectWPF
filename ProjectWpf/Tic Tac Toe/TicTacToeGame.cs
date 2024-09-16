using ProjectWpf.Tic_Tac_Toe;
using System.Windows.Controls;

namespace ProjectWpf
{
    public class TicTacToeGame
    {
        private bool player1Turn = true;
        private GameBoard gameBoard = new GameBoard();
        public bool IsHardDifficulty { get; private set; }

        public bool Player1Turn { get { return player1Turn; } }

        public void SetDifficultyHard()
        {
            IsHardDifficulty = true;
        }

        public void SetDifficultyEasy()
        {
            IsHardDifficulty = false;
        }

        public void MarkButton(Button btn, int row, int col)
        {
            if (btn.IsEnabled)
            {
                btn.Content = player1Turn ? "X" : "O";
                btn.IsEnabled = false;
                gameBoard.UpdateBoardState(row, col, player1Turn);
            }
        }

        public void ReverseMarkButton(Button btn, int row, int col)
        {
            btn.Content = "";
            btn.IsEnabled = true;
            gameBoard.ReverseBoardState(row, col);
        }

        public GameStatus CheckGameStatus()
        {
            int currentPlayer = player1Turn ? 1 : 2;

            if (gameBoard.CheckForWin(currentPlayer))
            {
                return GameStatus.Win;
            }

            bool fullBoard = true;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (gameBoard[i, j] == 0)
                        fullBoard = false;
                }
            }

            if (fullBoard)
            {
                return GameStatus.Draw;
            }

            return GameStatus.InProgress;
        }


        public void ResetGame()
        {
            gameBoard.ResetBoard();
            player1Turn = true;
        }

        public void SwitchTurn()
        {
            player1Turn = !player1Turn;
        }
    }
}
