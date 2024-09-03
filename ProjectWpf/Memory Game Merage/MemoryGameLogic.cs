using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ProjectWpf.Memory_Game_Merage
{
    public class MemoryGameLogic
    {
        private List<int> _numbers;
        private Button? _firstChoice;
        private Button? _secondChoice;
        private int _pairsFound;
        private readonly int _totalPairs;
        private readonly Random _rand = new Random();
        private Dictionary<int, Shape>? _shapeDictionary; 
        private readonly int _totalCards;
        private bool _isTwoPlayersMode;
        private Player _currentPlayer;
        private Player _player1;
        private Player? _player2;
        private bool _isPlayer1Turn;
        private bool _isDraw;

        public Player Player1 => _player1;
        public Player? Player2 => _player2;
        public bool IsGameOver { get; private set; }
        public bool IsPlayer1Turn => _isPlayer1Turn;
        public event Action<int>? ScoreChanged;
        public event Action? GameOver;
        public event Action? PlayerTurnChanged;

        public MemoryGameLogic(string difficultyLevel, bool isTwoPlayersMode)
        {
            _isTwoPlayersMode = isTwoPlayersMode;
            _player1 = new Player { Name = "Player 1" };
            _player2 = isTwoPlayersMode ? new Player { Name = "Player 2" } : null;
            _currentPlayer = _player1;
            _isPlayer1Turn = true;

            _totalCards = difficultyLevel switch
            {
                "Medium" => 30,
                "Hard" => 36,
                _ => 16
            };

            _numbers = Enumerable.Range(1, _totalCards / 2).SelectMany(x => new[] { x, x }).ToList();
            _totalPairs = _totalCards / 2;

            // Initialize _shapeDictionary to an empty dictionary
            _shapeDictionary = new Dictionary<int, Shape>();
        }



        public void LoadShapes(List<Button> buttons, Dictionary<int, Shape> shapeDictionary)
        {
            _shapeDictionary = shapeDictionary ?? throw new ArgumentNullException(nameof(shapeDictionary));
            _numbers = _numbers.OrderBy(x => _rand.Next()).ToList();
            for (int i = 0; i < buttons.Count; i++)
            {
                Button button = buttons[i];
                button.Tag = _numbers[i];
                button.Content = GetBackShape();
                button.IsEnabled = true;
            }
        }


        public void ButtonClicked(Button button)
        {
            if (this.IsGameOver || _shapeDictionary == null)
            {
                return;
            }

            if (this._firstChoice == null)
            {
                this._firstChoice = button;
                if (_shapeDictionary.TryGetValue((int)this._firstChoice.Tag, out var shape))
                {
                    this._firstChoice.Content = shape;
                }
            }
            else if (this._secondChoice == null && button != this._firstChoice)
            {
                this._secondChoice = button;
                if (_shapeDictionary.TryGetValue((int)this._secondChoice.Tag, out var shape))
                {
                    this._secondChoice.Content = shape;
                }
                CheckMatch();
            }
        }




        private async void CheckMatch()
        {
            if ((int)_firstChoice.Tag == (int)_secondChoice.Tag)
            {
                _currentPlayer.Score++;
                ScoreChanged?.Invoke(_currentPlayer.Score);
                _pairsFound++;

                await Task.Delay(500);
                _firstChoice.Visibility = Visibility.Collapsed;
                _secondChoice.Visibility = Visibility.Collapsed;
                _firstChoice = null;
                _secondChoice = null;

                if (_pairsFound == _totalPairs)
                {
                    IsGameOver = true;
                    _isDraw = _player1.Score == (_player2?.Score ?? 0);
                    GameOver?.Invoke();
                }
                else if (_isTwoPlayersMode)
                {
                    SwitchPlayer();
                }
            }
            else
            {
                await Task.Delay(1000);
                _firstChoice.Content = GetBackShape();
                _secondChoice.Content = GetBackShape();
                _firstChoice = null;
                _secondChoice = null;

                if (_isTwoPlayersMode)
                {
                    SwitchPlayer();
                }
            }
        }

        private void SwitchPlayer()
        {
            if (_isTwoPlayersMode)
            {
                _isPlayer1Turn = !_isPlayer1Turn;
                _currentPlayer = _isPlayer1Turn ? _player1 : _player2;
                PlayerTurnChanged?.Invoke();
            }
        }


        private Shape GetBackShape()
        {
            Rectangle backShape = new Rectangle
            {
                Width = 100,
                Height = 100,
                Fill = Brushes.DarkBlue 
            };

            return backShape;
        }




    }
}
