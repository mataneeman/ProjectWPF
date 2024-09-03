using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using UserManagement;

namespace ProjectWpf.Brick_Braker
{
    public enum PowerUpType
    {
        SpeedReduction,
        PaddleIncrease,
        ExtraBlocks,
        Shield,
        Coin // הוסף סוג חדש של תוסף עבור מטבע
    }

    public class PowerUp : UserControl
    {
        private Polygon _diamond;
        private Image _coinImage;
        public PowerUpType Type { get; }

        private DispatcherTimer _disappearTimer;

        public PowerUp(PowerUpType type)
        {
            Type = type;

            if (type == PowerUpType.Coin)
            {
                _coinImage = new Image
                {
                    Width = 25,
                    Height = 25,
                    Source = new BitmapImage(new Uri("pack://application:,,,/Brick Braker/Resources/coin.png", UriKind.RelativeOrAbsolute))
                };
                this.Content = _coinImage;
            }
            else
            {
                _diamond = new Polygon
                {
                    Points = new PointCollection
                    {
                        new Point(10, 0), // Top vertex
                        new Point(20, 10), // Right vertex
                        new Point(10, 20), // Bottom vertex
                        new Point(0, 10)  // Left vertex
                    },
                    Fill = GetFillBrushForType(type),
                    Stroke = Brushes.Transparent
                };
                this.Content = _diamond;
            }

            this.Width = 25;
            this.Height = 25;

            // אתחול של טיימר עבור היעלמות התוספת
            _disappearTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(15) // זמן להיעלם, התאם אם צריך
            };
            _disappearTimer.Tick += DisappearTimer_Tick;
            _disappearTimer.Start();
        }

        private Brush GetFillBrushForType(PowerUpType type)
        {
            switch (type)
            {
                case PowerUpType.SpeedReduction:
                    return Brushes.Green;
                case PowerUpType.PaddleIncrease:
                    return Brushes.Blue;
                case PowerUpType.ExtraBlocks:
                    return Brushes.Orange;
                case PowerUpType.Shield:
                    return Brushes.Red;  // צבע עבור התוסף Shield
                case PowerUpType.Coin:
                    return Brushes.Gold; // צבע עבור המטבע (למרות שלא בשימוש כי יש תמונה)
                default:
                    return Brushes.Transparent; // אם סוג לא מוגדר, השתמש בצבע שקוף
            }
        }

        private void DisappearTimer_Tick(object sender, EventArgs e)
        {
            _disappearTimer.Stop();

            BrickBrakerGame? game = Application.Current.MainWindow as BrickBrakerGame;
            if (game != null)
            {
                game.RemovePowerUp(this);
            }
        }
    }
}
