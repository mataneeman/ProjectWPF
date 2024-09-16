using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;

public class Bird
{
    public Image BirdImage { get; private set; }
    public double YPosition { get; set; }
    public double Velocity { get; set; }

    public Bird()
    {
        BirdImage = new Image
        {
            Source = new BitmapImage(new Uri("pack://application:,,,/Flappy Bird/Resources/bird.png")),
            Width = 30,
            Height = 30
        };
        YPosition = 200; 
        Velocity = 0;
    }

    public void Update()
    {
        YPosition += Velocity;
        Velocity += 0.5; 
        Canvas.SetLeft(BirdImage, 100);
        Canvas.SetTop(BirdImage, YPosition);

        if (YPosition < 0)
            YPosition = 0;
        if (YPosition > ((Canvas)BirdImage.Parent).ActualHeight - BirdImage.Height)
            YPosition = ((Canvas)BirdImage.Parent).ActualHeight - BirdImage.Height;
    }

    public void Jump()
    {
        Velocity = -7; 
    }
}
