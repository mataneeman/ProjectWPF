using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using UserManagement;

namespace ProjectWpf.User_Management
{
    public partial class UserManagement : Window
    {
        private List<User> _users = new List<User>();

        public UserManagement()
        {
            InitializeComponent();
            InitializeData();
            DataContext = this;
        }

        private void InitializeData()
        {
            _users.Add(new User("John Doe", new DateTime(2023, 10, 10), "john.doe@email.com", "Admin"));
            _users.Add(new User("Jane Smith", new DateTime(2023, 10, 12), "jane.smith@email.com", "User"));
            _users.Add(new User("Michael Johnson", new DateTime(2023, 10, 15), "michael.johnson@email.com", "Manager"));

            lvUsers.ItemsSource = _users;
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            if (_users != null) 
            {
                AddUser addUserWindow = new AddUser(_users);
                addUserWindow.ShowDialog();
                if (addUserWindow.DialogResult == true)
                {
                    lvUsers.Items.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Users list is null or empty.");
            }
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Button button && button.DataContext is User userToDelete)
            {
                _users?.Remove(userToDelete);
                lvUsers.Items.Refresh(); 
                MessageBox.Show("User deleted.");
            }
        }

    }
}
