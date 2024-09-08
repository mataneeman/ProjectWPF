using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace ProjectWpf.Brick_Braker
{
    public class Paddle : UserControl
    {
        private Rectangle _rectangle;
        private TextBlock _countdownTextBlock;
        private double _originalWidth;
        private Brush _originalFill;

        public bool IsShieldActive { get; set; }

        public Paddle()
        {
            _rectangle = new Rectangle
            {
                Width = 90,
                Height = 18,
                RadiusX = 8,
                RadiusY = 8,
                Fill = Brushes.Black,
            };

            _countdownTextBlock = new TextBlock
            {
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 12,
                FontWeight = FontWeights.Bold,
                Visibility = Visibility.Collapsed 
            };

            var grid = new Grid();
            grid.Children.Add(_rectangle);
            grid.Children.Add(_countdownTextBlock);

            this.Content = grid;
            this.Width = _rectangle.Width;
            this.Height = _rectangle.Height;

            _originalWidth = _rectangle.Width;
            _originalFill = _rectangle.Fill;
        }

        public void Move(double offset)
        {
            double newLeft = Canvas.GetLeft(this) + offset;
            if (newLeft < 0) newLeft = 0;
            if (newLeft + _rectangle.Width > ((Canvas)this.Parent).ActualWidth)
                newLeft = ((Canvas)this.Parent).ActualWidth - _rectangle.Width;

            Canvas.SetLeft(this, newLeft);
        }

        public void SetWidth(double newWidth)
        {
            DoubleAnimation widthAnimation = new DoubleAnimation
            {
                From = _rectangle.Width,
                To = newWidth,
                Duration = TimeSpan.FromMilliseconds(300)
            };
            _rectangle.BeginAnimation(Rectangle.WidthProperty, widthAnimation);
            this.Width = newWidth;
        }

        public void SetFill(Brush newFill)
        {
            _rectangle.Fill = newFill;
        }

        public void ResetToOriginal()
        {
            DoubleAnimation widthAnimation = new DoubleAnimation
            {
                From = _rectangle.Width,
                To = _originalWidth,
                Duration = TimeSpan.FromMilliseconds(300)
            };
            _rectangle.BeginAnimation(Rectangle.WidthProperty, widthAnimation);
            this.Width = _originalWidth;

            _rectangle.Fill = _originalFill;
        }

        public void UpdateAppearance()
        {
            if (IsShieldActive)
            {
                _rectangle.Fill = Brushes.Firebrick; 
                _countdownTextBlock.Visibility = Visibility.Visible; 
            }
            else
            {
                _rectangle.Fill = _originalFill;
                _countdownTextBlock.Visibility = Visibility.Collapsed; 
            }
        }

        public void UpdateCountdown(int secondsRemaining)
        {
            _countdownTextBlock.Text = secondsRemaining.ToString(); 
        }
    }
}
