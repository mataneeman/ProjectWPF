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
                Width = 50,
                Height = 100,
                Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Save Ball Game/Images/basket.png")))
            };

            Canvas.SetLeft(Rectangle, (canvasWidth - Rectangle.Width) / 2);
            Canvas.SetTop(Rectangle, canvasHeight - Rectangle.Height);
        }
        public void SetPosition(double x, double canvasWidth)
        {
            if (x < 0) x = 0;
            if (x > canvasWidth - Rectangle.Width) x = canvasWidth - Rectangle.Width;
            Canvas.SetLeft(Rectangle, x);
        }

        public void Move(double x, double canvasWidth)
        {
            double newX = Canvas.GetLeft(Rectangle) + x;

            if (newX < 0) newX = 0;
            if (newX > canvasWidth - Rectangle.Width) newX = canvasWidth - Rectangle.Width;

            Canvas.SetLeft(Rectangle, newX);
        }


    }
}