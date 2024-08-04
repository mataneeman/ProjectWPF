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

namespace ProjectWpf.Brick_Braker
{
    /// <summary>
    /// Interaction logic for BrickBrakerPage.xaml
    /// </summary>
    public partial class BrickBrakerPage : Page
    {
        public BrickBrakerPage()
        {
            InitializeComponent();
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void BrickBrakerGame_Click(object sender, RoutedEventArgs e)
        {
            BrickBrakerGame brickBrakerGameWindow = new BrickBrakerGame();
            brickBrakerGameWindow.Show();
        }
    }
}
