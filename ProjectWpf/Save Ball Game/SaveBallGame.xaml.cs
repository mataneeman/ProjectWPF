using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Threading;

namespace ProjectWpf.Save_Ball_Game
{
    public partial class SaveBallGame : Window
    {
        private GameEngine gameEngine;
        private Rectangle playerRectangle;
        private const double PlayerWidth = 80;
        private const double PlayerHeight = 80;
        private const int MaxLives = 5;
        private int remainingLives;
        private HighScoresHandler highScoresHandler;

        public SaveBallGame()
        {
            InitializeComponent();
            MyCanvas.Loaded += Canvas_Loaded;
            highScoresHandler = new HighScoresHandler("save_ball_game");
        }

        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            ClearCanvas();

            playerRectangle = new Rectangle
            {
                Width = PlayerWidth,
                Height = PlayerHeight,
                Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Save Ball Game/Images/basket.png")))
            };

            Canvas.SetLeft(playerRectangle, (MyCanvas.ActualWidth - PlayerWidth) / 2);
            Canvas.SetTop(playerRectangle, MyCanvas.ActualHeight - PlayerHeight);

            MyCanvas.Children.Add(playerRectangle);

            MyCanvas.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Save Ball Game/Images/background.jpg")));

            gameEngine = new GameEngine(MyCanvas, playerRectangle, UpdateLives);

            remainingLives = MaxLives;
            UpdateLivesDisplay();
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point position = e.GetPosition(MyCanvas);
            double newX = position.X - PlayerWidth / 2;

            if (newX < 0) newX = 0;
            if (newX > MyCanvas.ActualWidth - PlayerWidth) newX = MyCanvas.ActualWidth - PlayerWidth;

            Canvas.SetLeft(playerRectangle, newX);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            gameEngine.StartGame();
            StartButton.Visibility = Visibility.Collapsed;
        }

        private void UpdateLives(int livesLeft)
        {
            remainingLives = livesLeft;
            UpdateLivesDisplay();

            if (remainingLives <= 0)
            {
                gameEngine.StopGame();
                GameOverOverlay.Visibility = Visibility.Visible;
                StartButton.Visibility = Visibility.Collapsed;
                highScoresHandler.SaveHighScore(gameEngine.Score);
                UpdateHighScoresDisplay();
            }
        }

        private void UpdateLivesDisplay()
        {
            for (int i = 1; i <= MaxLives; i++)
            {
                Image heart = (Image)FindName($"Heart{i}");
                if (heart != null)
                {
                    string imagePath = i <= remainingLives
                        ? "pack://application:,,,/Save Ball Game/Images/red_heart.png"
                        : "pack://application:,,,/Save Ball Game/Images/white_heart.png";

                    heart.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
                }
            }
        }

        private void ClearCanvas()
        {
            MyCanvas.Children.Clear();
        }

        private void UpdateHighScoresDisplay()
        {
            List<int> highScores = highScoresHandler.GetTopHighScores(5);
            HighScoresText.Text = "High Scores:\n" + string.Join("\n", highScores);
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            gameEngine.StopGame();
            InitializeGame();
            GameOverOverlay.Visibility = Visibility.Collapsed;
            StartButton.Visibility = Visibility.Visible;
        }
    }
}
