﻿using System;
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

namespace ProjectWpf.User_Management
{
    /// <summary>
    /// Interaction logic for UserManagementPage.xaml
    /// </summary>
    public partial class UserManagementPage : Page
    {
        public UserManagementPage()
        {
            InitializeComponent();
        }

        private void UserManagement_Click(object sender, RoutedEventArgs e)
        {
            UserManagement userManagementpage = new UserManagement();
            userManagementpage.Show();
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
