using System;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ProjectWpf.Save_Ball_Game
{
    public class Ball
    {
        public Rectangle Rectangle { get; private set; }
        private const double InitialSpeed = 10;
        private double speed;
        private Random random;

        public Ball(Random random, double canvasWidth, double speed)
        {
            this.random = random;
            this.speed = speed;
            Rectangle = new Rectangle
            {
                Width = 30,
                Height = 30,
                Fill = new ImageBrush(new BitmapImage(new Uri($"pack://application:,,,/Save Ball Game/Images/ball{random.Next(1, 8)}.png")))
            };
            // Position the ball at a random x position within the canvas width
            double xPosition = random.Next(0, (int)(canvasWidth - Rectangle.Width));
            Canvas.SetLeft(Rectangle, xPosition);
            // Position the ball just above the canvas
            Canvas.SetTop(Rectangle, -Rectangle.Height); // Start just above the canvas
        }

        public void MoveDown()
        {
            Canvas.SetTop(Rectangle, Canvas.GetTop(Rectangle) + speed);
        }

        public bool IsOutOfBounds(Canvas canvas)
        {
            return Canvas.GetTop(Rectangle) > canvas.ActualHeight;
        }

        public void AddToCanvas(Canvas canvas)
        {
            canvas.Children.Add(Rectangle);
        }

        public Rect GetRectangleBounds()
        {
            return new Rect(Canvas.GetLeft(Rectangle), Canvas.GetTop(Rectangle), Rectangle.Width, Rectangle.Height);
        }
    }
}
