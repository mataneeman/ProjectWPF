﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ProjectWpf.Brick_Braker
{
    public class Paddle : UserControl
    {
        private Rectangle _rectangle;

        public Paddle()
        {
            _rectangle = new Rectangle
            {
                Width = 90,
                Height = 13,
                RadiusX = 5,
                RadiusY = 5,
                Fill = Brushes.Black, 
                StrokeThickness = 1 
            };

            this.Content = _rectangle;

            this.Width = _rectangle.Width;
            this.Height = _rectangle.Height;
        }

        public void Move(double offset)
        {
            double newLeft = Canvas.GetLeft(this) + offset;
            if (newLeft < 0) newLeft = 0;
            if (newLeft + _rectangle.Width > ((Canvas)this.Parent).ActualWidth)
                newLeft = ((Canvas)this.Parent).ActualWidth - _rectangle.Width;

            Canvas.SetLeft(this, newLeft);
        }
    }
}
