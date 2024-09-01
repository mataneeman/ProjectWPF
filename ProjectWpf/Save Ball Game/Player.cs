using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;

namespace ProjectWpf.Save_Ball_Game
{
    public class Player
    {
        public Rectangle Rectangle { get; private set; }

        public Player(double canvasWidth, double canvasHeight)
        {
            Rectangle = new Rectangle
            {
                Width = 50,  // Width of the player
                Height = 100,  // Height of the player
                Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Save Ball Game/Images/basket.png")))
            };

            // Position the player at the bottom of the canvas
            Canvas.SetLeft(Rectangle, (canvasWidth - Rectangle.Width) / 2);
            Canvas.SetTop(Rectangle,( canvasHeight - Rectangle.Height));
        }
        
        public void SetPosition(double x, double canvasWidth)
        {
            // Ensure the player stays within the canvas boundaries
            if (x < 0) x = 0;
            if (x > canvasWidth - Rectangle.Width) x = canvasWidth - Rectangle.Width;
            Canvas.SetLeft(Rectangle, x);
        }

        public bool IsCollidingWith(Ball ball)
        {
            Rect playerRect = new Rect(Canvas.GetLeft(Rectangle), Canvas.GetTop(Rectangle), Rectangle.Width, Rectangle.Height);
            Rect ballRect = ball.GetRectangleBounds();
            return playerRect.IntersectsWith(ballRect);
        }
    }
}
