using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading; 

namespace ProjectWpf
{
    public partial class HomePage : Page
    {
        private DispatcherTimer? _clockTimer;

        public HomePage()
        {
            InitializeComponent();
            InitializeClock();
        }

        private void InitializeClock()
        {
            _clockTimer = new DispatcherTimer();
            _clockTimer.Interval = TimeSpan.FromSeconds(1);
            _clockTimer.Tick += UpdateDateTime;
            _clockTimer.Start();
        }

        private void UpdateDateTime(object? sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            DateTimeTextBlock.Text = now.ToString("dd.MM.yyyy    HH:mm"); 
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            if (window != null)
            {
                window.Width = 900;
                window.Height = 780;
            }
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Image image)
            {
                image.Opacity = 0.3;
            }
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Image image)
            {
                image.Opacity = 1.0;
            }
        }

        private void TicTacToe_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Tic_Tac_Toe.TicTacToePage());
        }

        private void TodoList_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Todo_List.TodoListPage());
        }

        private void UserManagement_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new User_Management.UserManagementPage());
        }

        private void Snack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Snack.SnackPage());
        }

        private void BrickBrakerPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Brick_Braker.BrickBrakerPage());
        }

        private void Countries_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Countries.CountriesPage());
        }

        private void MemoryGame_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Memory_Game_Merage.MemoryGamePage());
        }

        private void SaveBallGame_Click(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Save_Ball_Game.SaveBallPage());
        }

        private void FlappyBird_click(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Flappy_Bird.FlappyBirdPage());
        }
    }
}
