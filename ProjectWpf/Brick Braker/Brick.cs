using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ProjectWpf.Brick_Braker
{
    public class Brick
    {
        private Rectangle _rectangle;

        public Brick(double x, double y, double width, double height)
        {
            _rectangle = new Rectangle
            {
                Width = width,
                Height = height,
                RadiusX = 5,
                RadiusY = 5,
            };
            

            Canvas.SetLeft(_rectangle, x);
            Canvas.SetTop(_rectangle, y);
        }

        public double Width => _rectangle.Width;
        public double Height => _rectangle.Height;
        public Brush Fill
        {
            get => _rectangle.Fill;
            set => _rectangle.Fill = value;
        }

        public UIElement GetUIElement() => _rectangle;
    }
}
