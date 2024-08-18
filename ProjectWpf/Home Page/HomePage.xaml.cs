using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectWpf
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Adjust the window size if the page is inside a window
            var window = Window.GetWindow(this);
            if (window != null)
            {
                window.Width = 900; // Set to desired width
                window.Height = 600; // Set to desired height
            }
        }
        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Image image)
            {
                image.Opacity = 0.7; 
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
    }
}
