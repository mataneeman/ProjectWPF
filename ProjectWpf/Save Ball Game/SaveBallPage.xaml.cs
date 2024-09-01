using ProjectWpf.Memory_Game_Merage;
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

namespace ProjectWpf.Save_Ball_Game
{
    /// <summary>
    /// Interaction logic for SaveBallPage.xaml
    /// </summary>
    public partial class SaveBallPage : Page
    {
        public SaveBallPage()
        {
            InitializeComponent();
        }

        private void SaveBallGamePlay_click(object sender, RoutedEventArgs e)
        {
            SaveBallGame SaveBallGameWindow = new SaveBallGame();
            SaveBallGameWindow.Show();
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
