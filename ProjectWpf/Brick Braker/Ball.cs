using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ProjectWpf.Brick_Braker
{
    public class Ball : UserControl
    {
        private Ellipse _ellipse;

        public Vector Velocity { get; set; }

        public Ball()
        {
            _ellipse = new Ellipse
            {
                Width = 20,
                Height = 20,

            };

            // Set the Ball as the content of the UserControl
            this.Content = _ellipse;

            // Set default velocity
            Velocity = new Vector(5, -5);
        }
    }
}
