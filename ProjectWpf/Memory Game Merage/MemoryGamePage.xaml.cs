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

namespace ProjectWpf.Memory_Game_Merage
{
    /// <summary>
    /// Interaction logic for MemoryGamePage.xaml
    /// </summary>
    public partial class MemoryGamePage : Page
    {
        public MemoryGamePage()
        {
            InitializeComponent();
        }

        private void MemoryGame_Click(object sender, RoutedEventArgs e)
        {
            MemoryGame memoryGameWindow = new MemoryGame();
            memoryGameWindow.Show();
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
