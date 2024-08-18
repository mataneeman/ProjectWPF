public class GameBoard
{
    private const int Empty = 0;
    private const int Player1 = 1;
    private const int Player2 = 2;

    private int[,] board = new int[3, 3];

    public int this[int row, int col]
    {
        get { return board[row, col]; }
        set { board[row, col] = value; }
    }

    public void UpdateBoardState(int row, int col, bool isPlayer1Turn)
    {
        board[row, col] = isPlayer1Turn ? Player1 : Player2;
    }
    public void ReverseBoardState(int row, int col)
    {
        board[row, col] = 0;
    }
    public bool CheckForWin(int player)
    {
        return CheckRows(player) || CheckColumns(player) || CheckDiagonals(player);
    }

    private bool CheckRows(int player)
    {
        for (int i = 0; i < 3; i++)
        {
            if ((board[i, 0] == player && board[i, 1] == player && board[i, 2] == player))
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckColumns(int player)
    {
        for (int i = 0; i < 3; i++)
        {
            if ((board[0, i] == player && board[1, i] == player && board[2, i] == player))
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckDiagonals(int player)
    {
        return (board[0, 0] == player && board[1, 1] == player && board[2, 2] == player) ||
               (board[0, 2] == player && board[1, 1] == player && board[2, 0] == player);
    }

    public void ResetBoard()
    {
        board = new int[3, 3];
    }

    public bool IsBoardFull()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board[i, j] == Empty)
                {
                    return false;
                }
            }
        }
        return true;
    }
}
