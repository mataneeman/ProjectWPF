using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows;

namespace ProjectWpf.Snack
{
    public class GridCell : Grid
    {
        private Rectangle rectangle;
        private Brush originalFill;

        public GridCell(bool isLightGreen)
        {
            originalFill = isLightGreen ? Brushes.LightGreen : Brushes.LimeGreen;

            rectangle = new Rectangle
            {
                Fill = originalFill,
                Stretch = Stretch.Fill,
                RadiusX = 1,
                RadiusY = 1,
                Margin = new Thickness(1) 
            };
            Children.Add(rectangle);
        }

        public void SetState(CellState state)
        {
            switch (state)
            {
                case CellState.Empty:
                    rectangle.Fill = originalFill;
                    rectangle.Effect = null; 
                    break;
                case CellState.Snake:
                    rectangle.Fill = Brushes.Green;
                    rectangle.Effect = new DropShadowEffect
                    {
                        Color = Colors.DarkGreen,
                        Direction = 315,
                        ShadowDepth = 3,
                        Opacity = 0.6,
                        BlurRadius = 6
                    };
                    AnimateGrowth(); 
                    break;
                case CellState.Apple:
                    rectangle.Fill = Brushes.Red;
                    rectangle.Effect = null;
                    break;
                case CellState.Wall:
                    rectangle.Fill = Brushes.DimGray;
                    rectangle.Effect = null; 
                    break;
            }
        }

        private void AnimateGrowth()
        {
            ScaleTransform scaleTransform = new ScaleTransform();
            rectangle.RenderTransform = scaleTransform;

            DoubleAnimation scaleAnimation = new DoubleAnimation
            {
                From = 1,
                To = 1.2,
                Duration = TimeSpan.FromMilliseconds(200),
                AutoReverse = true
            };

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
        }
    }
}
