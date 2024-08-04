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

namespace ProjectWpf.Tic_Tac_Toe
{
    /// <summary>
    /// Interaction logic for TicTacToePage.xaml
    /// </summary>
    public partial class TicTacToePage : Page
    {
        public TicTacToePage()
        {
            InitializeComponent();
        }

        private void tictactoeGame_Click(object sender, RoutedEventArgs e)
        {
            TicTacToe ticTacToeWindow = new TicTacToe(); 
            ticTacToeWindow.Show();
            
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
