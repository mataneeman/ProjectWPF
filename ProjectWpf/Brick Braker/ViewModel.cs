using System.ComponentModel;

namespace ProjectWpf.Brick_Braker
{
    public class GameViewModel : INotifyPropertyChanged
    {
        private int _score;
        private int _highScore;

        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                OnPropertyChanged(nameof(Score));
            }
        }

        public int HighScore
        {
            get => _highScore;
            set
            {
                _highScore = value;
                OnPropertyChanged(nameof(HighScore));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
