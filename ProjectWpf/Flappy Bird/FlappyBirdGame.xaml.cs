using System.Windows.Input;
using System.Windows;

namespace ProjectWpf.Flappy_Bird
{
    public partial class FlappyBirdGame : Window
    {
        private GameManager gameManager;

        public FlappyBirdGame()
        {
            InitializeComponent();
            gameManager = new GameManager(this, GameCanvas);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            gameManager.StartGame();
            flappybirdpic.Visibility = Visibility.Collapsed;
            ScoreTextBlock.Visibility = Visibility.Visible;
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            gameManager.ResetGame();
            flappybirdpic.Visibility = Visibility.Visible;
            StartButton.Visibility = Visibility.Visible;
            // Do not start the game here; only reset the game state
        }



        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                gameManager.BirdJump();
            }
        }
    }
}
