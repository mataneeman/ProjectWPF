using ProjectWpf.Memory_Game_Merage;

public class GameResultsManager
{
    private readonly Player _player1;
    private readonly Player _player2;

    public GameResultsManager(Player player1, Player player2)
    {
        _player1 = player1;
        _player2 = player2;
    }

    public string GetGameOverMessage()
    {
        if (_player2 == null)
        {
            return _player1.Score > 0 ? "Game Over!" : "Game Over!";
        }

        if (_player1.Score == _player2.Score)
        {
            return "It's a Draw!";
        }

        string? winner = _player1.Score > _player2.Score ? _player2.Name : _player1.Name;
        return $"{winner} Wins!";
    }
}
