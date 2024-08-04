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

namespace ProjectWpf.Snack
{
    /// <summary>
    /// Interaction logic for SnackPage.xaml
    /// </summary>
    public partial class SnackPage : Page
    {
        public SnackPage()
        {
            InitializeComponent();
        }

        private void snack_Click(object sender, RoutedEventArgs e)
        {
            SnackGame snackGameWindow = new SnackGame();
            snackGameWindow.Show();
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
