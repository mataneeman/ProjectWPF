using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;

namespace ProjectWpf.Memory_Game_Merage
{
    public static class ShapeFactory
    {
        public static Shape CreateCircle(double size)
        {
            return new Ellipse
            {
                Width = size,
                Height = size,
                Fill = Brushes.Gray,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
        }

        public static Shape CreateRectangle(double size)
        {
            return new Rectangle
            {
                Width = size,
                Height = size,
                Fill = Brushes.Gray,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
        }

        public static Shape CreateTriangle(double size)
        {
            Polygon triangle = new Polygon
            {
                Points = new PointCollection
                {
                    new Point(size / 2, 0),
                    new Point(size, size),
                    new Point(0, size)
                },
                Fill = Brushes.Gray,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            return triangle;
        }

        public static Shape CreateHexagon(double size)
        {
            Polygon hexagon = new Polygon
            {
                Points = new PointCollection
                {
                    new Point(size * 0.5, 0),
                    new Point(size, size * 0.25),
                    new Point(size, size * 0.75),
                    new Point(size * 0.5, size),
                    new Point(0, size * 0.75),
                    new Point(0, size * 0.25)
                },
                Fill = Brushes.Gray,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            return hexagon;
        }

        public static Shape CreateRoundedRectangle(double size)
        {
            return new Rectangle
            {
                Width = size,
                Height = size,
                RadiusX = size * 0.25,
                RadiusY = size * 0.25,
                Fill = Brushes.Gray,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
        }

        public static Shape CreateStar(double size)
        {
            Polygon star = new Polygon
            {
                Points = new PointCollection
                {
                    new Point(size * 0.5, 0),
                    new Point(size * 0.6, size * 0.35),
                    new Point(size, size * 0.35),
                    new Point(size * 0.68, size * 0.57),
                    new Point(size * 0.8, size),
                    new Point(size * 0.5, size * 0.7),
                    new Point(size * 0.2, size),
                    new Point(size * 0.32, size * 0.57),
                    new Point(0, size * 0.35),
                    new Point(size * 0.4, size * 0.35)
                },
                Fill = Brushes.Gray,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            return star;
        }

        public static Shape CreateDiamond(double size)
        {
            Polygon diamond = new Polygon
            {
                Points = new PointCollection
                {
                    new Point(size * 0.5, 0),
                    new Point(size, size * 0.5),
                    new Point(size * 0.5, size),
                    new Point(0, size * 0.5)
                },
                Fill = Brushes.Gray,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            return diamond;
        }

        public static Shape CreatePentagon(double size)
        {
            Polygon pentagon = new Polygon
            {
                Points = new PointCollection
                {
                    new Point(size * 0.5, 0),
                    new Point(size, size * 0.4),
                    new Point(size * 0.8, size),
                    new Point(size * 0.2, size),
                    new Point(0, size * 0.4)
                },
                Fill = Brushes.Gray,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            return pentagon;
        }

        public static Shape CreateOctagon(double size)
        {
            Polygon octagon = new Polygon
            {
                Points = new PointCollection
                {
                    new Point(size * 0.3, 0),
                    new Point(size * 0.7, 0),
                    new Point(size, size * 0.3),
                    new Point(size, size * 0.7),
                    new Point(size * 0.7, size),
                    new Point(size * 0.3, size),
                    new Point(0, size * 0.7),
                    new Point(0, size * 0.3)
                },
                Fill = Brushes.Gray,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            return octagon;
        }

        public static Shape CreateTrapezium(double size)
        {
            Polygon trapezium = new Polygon
            {
                Points = new PointCollection
                {
                    new Point(size * 0.2, 0),
                    new Point(size * 0.8, 0),
                    new Point(size, size),
                    new Point(0, size)
                },
                Fill = Brushes.Gray,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            return trapezium;
        }

        public static Shape CreateTrapezoid(double size)
        {
            Polygon trapezoid = new Polygon
            {
                Points = new PointCollection
                {
                    new Point(size * 0.2, 0),
                    new Point(size * 0.8, 0),
                    new Point(size * 0.6, size),
                    new Point(size * 0.4, size)
                },
                Fill = Brushes.Gray,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            return trapezoid;
        }

        public static Shape CreateEllipseShape(double size)
        {
            return new Ellipse
            {
                Width = size * 1.5,
                Height = size,
                Fill = Brushes.Gray,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
        }

        public static Shape CreateHalfCircle(double size)
        {
            EllipseGeometry halfCircle = new EllipseGeometry
            {
                Center = new Point(size * 0.5, size * 0.5),
                RadiusX = size * 0.5,
                RadiusY = size * 0.5
            };
            Path path = new Path
            {
                Data = halfCircle,
                Fill = Brushes.Gray,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            return path;
        }

        public static Shape CreateInvertedTrapezium(double size)
        {
            Polygon invertedTrapezium = new Polygon
            {
                Points = new PointCollection
                {
                    new Point(size * 0.2, size),
                    new Point(size * 0.8, size),
                    new Point(size, 0),
                    new Point(0, 0)
                },
                Fill = Brushes.Gray,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            return invertedTrapezium;
        }

        public static Shape CreateRoundedSquare(double size)
        {
            return new Rectangle
            {
                Width = size,
                Height = size,
                RadiusX = size * 0.15,
                RadiusY = size * 0.15,
                Fill = Brushes.Gray,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
        }

        public static Shape CreateEllipseaShape(double size)
        {
            return new Ellipse
            {
                Width = size * 1.2,
                Height = size,
                Fill = Brushes.Gray,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
        }
    }
}
