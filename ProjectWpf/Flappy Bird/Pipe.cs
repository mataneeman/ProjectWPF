using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

public class Pipe
{
    public Image TopPipeImage { get; private set; }
    public Image BottomPipeImage { get; private set; }
    public double XPosition { get; set; }
    public double Gap { get; private set; }
    public double CanvasHeight { get; private set; }

    private const double PipeWidth = 80;
    private const double PipeHeight = 200;
    private const double MinBottomPipeY = 0;
    private const double MaxBottomPipeYOffset = -150;

    private bool isPassedByBird = false; 
    private double pipeSpeed; 

    public Pipe(double xPosition, double canvasHeight, double gap, double speed)
    {
        TopPipeImage = new Image
        {
            Source = new BitmapImage(new Uri("pack://application:,,,/Flappy Bird/Resources/pipe-top.png")),
            Width = PipeWidth,
            Height = PipeHeight
        };

        BottomPipeImage = new Image
        {
            Source = new BitmapImage(new Uri("pack://application:,,,/Flappy Bird/Resources/pipe-bottom.png")),
            Width = PipeWidth,
            Height = PipeHeight
        };

        XPosition = xPosition;
        Gap = gap;
        CanvasHeight = canvasHeight;
        pipeSpeed = speed; 

        InitializePipePositions();
    }

    private void InitializePipePositions()
    {
        Random random = new Random();

        double bottomPipeY = CanvasHeight - PipeHeight - random.Next((int)MaxBottomPipeYOffset, (int)MinBottomPipeY + 1);
        double topPipeY = bottomPipeY - PipeHeight - Gap;

        Canvas.SetLeft(BottomPipeImage, XPosition);
        Canvas.SetTop(BottomPipeImage, bottomPipeY);

        Canvas.SetLeft(TopPipeImage, XPosition);
        Canvas.SetTop(TopPipeImage, topPipeY);
    }

    public void Update()
    {
        XPosition -= pipeSpeed; 
        Canvas.SetLeft(TopPipeImage, XPosition);
        Canvas.SetLeft(BottomPipeImage, XPosition);
    }

    public bool IsOutOfCanvas(Canvas canvas)
    {
        double pipeLeft = Canvas.GetLeft(TopPipeImage);
        double pipeRight = pipeLeft + TopPipeImage.Width;

        double canvasWidth = canvas.ActualWidth;

        return pipeRight < 0;
    }


    public void RemoveFromCanvas(Canvas canvas)
    {
        canvas.Children.Remove(TopPipeImage);
        canvas.Children.Remove(BottomPipeImage);
    }



    public bool IsCollidingWithBird(Image birdImage)
    {
        double birdLeft = Canvas.GetLeft(birdImage);
        double birdTop = Canvas.GetTop(birdImage);
        double birdRight = birdLeft + birdImage.Width;
        double birdBottom = birdTop + birdImage.Height;

        double pipeLeft = Canvas.GetLeft(TopPipeImage);
        double pipeRight = pipeLeft + TopPipeImage.Width;
        double pipeTop = Canvas.GetTop(TopPipeImage);
        double pipeBottom = Canvas.GetTop(BottomPipeImage) + BottomPipeImage.Height;

        bool isCollidingWithTopPipe = birdRight > pipeLeft &&
                                       birdLeft < pipeRight &&
                                       birdTop < pipeTop + TopPipeImage.Height &&
                                       birdBottom > pipeTop;

        bool isCollidingWithBottomPipe = birdRight > pipeLeft &&
                                          birdLeft < pipeRight &&
                                          birdTop < pipeBottom &&
                                          birdBottom > pipeBottom - BottomPipeImage.Height;

        return isCollidingWithTopPipe || isCollidingWithBottomPipe;
    }

    public bool IsPassedByBird(Image birdImage)
    {
        double birdLeft = Canvas.GetLeft(birdImage);
        double birdRight = birdLeft + birdImage.Width;

        double pipeLeft = Canvas.GetLeft(TopPipeImage);
        double pipeRight = pipeLeft + TopPipeImage.Width;

        return !isPassedByBird && birdLeft > pipeRight;
    }

    public void MarkAsPassed()
    {
        isPassedByBird = true;
    }
}
