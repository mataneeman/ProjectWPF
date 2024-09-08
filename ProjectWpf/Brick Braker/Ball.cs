﻿using System.Windows;
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
                Width = 15,
                Height = 15,
               

            };

            this.Content = _ellipse;

            Velocity = new Vector(3, -3);
        }
    }
}
