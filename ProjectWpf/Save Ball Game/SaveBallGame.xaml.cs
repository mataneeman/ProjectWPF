using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjectWpf.Save_Ball_Game
{
    public partial class SaveBallGame : Window
    {
        private GameEngine gameEngine;
        private Rectangle playerRectangle;
        private const double PlayerWidth = 80;
        private const double PlayerHeight = 80;

        public SaveBallGame()
        {
            InitializeComponent();
            MyCanvas.Loaded += Canvas_Loaded;
        }

        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            // Initialize the player (basket)
            playerRectangle = new Rectangle
            {
                Width = PlayerWidth,
                Height = PlayerHeight,
                Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Save Ball Game/Images/basket.png")))
            };

            // Position the player at the bottom of the canvas
            Canvas.SetLeft(playerRectangle, (MyCanvas.ActualWidth - PlayerWidth) / 2);
            Canvas.SetTop(playerRectangle, MyCanvas.ActualHeight - PlayerHeight);

            // Add player to the canvas
            MyCanvas.Children.Add(playerRectangle);

            // Set the background image for the canvas
            MyCanvas.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Save Ball Game/Images/background.jpg")));

            // Initialize the game engine
            gameEngine = new GameEngine(MyCanvas, playerRectangle);
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point position = e.GetPosition(MyCanvas);
            double newX = position.X - PlayerWidth / 2;

            // Ensure the player stays within the canvas boundaries
            if (newX < 0) newX = 0;
            if (newX > MyCanvas.ActualWidth - PlayerWidth) newX = MyCanvas.ActualWidth - PlayerWidth;

            Canvas.SetLeft(playerRectangle, newX);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            gameEngine.StartGame();
            StartButton.Visibility = Visibility.Collapsed;
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            gameEngine.StopGame();
            gameEngine = new GameEngine(MyCanvas, playerRectangle); // Reset the game engine
            gameEngine.StartGame();
            GameOverOverlay.Visibility = Visibility.Collapsed;
            StartButton.Visibility = Visibility.Visible;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // Handle key down events if needed
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            // Handle key up events if needed
        }
    }
}
