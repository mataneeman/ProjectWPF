using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using UserManagement;

namespace ProjectWpf.User_Management
{
    public partial class AddUser : Window
    {
        private List<User> _users;

        public AddUser(List<User> users)
        {
            InitializeComponent();
            _users = users;
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text.Trim();
            DateTime startDate = dpStartDate.SelectedDate ?? DateTime.Now;
            string email = txtEmail.Text.Trim();
            string role = ((ComboBoxItem)cmbRole.SelectedItem)?.Content?.ToString() ?? "DefaultRole";

            
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Name is required.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Email is required.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            

           
            User newUser = new User(name, startDate, email, role);
            _users.Add(newUser);

            MessageBox.Show("User added successfully with ID: " + newUser.Id);
            DialogResult = true;
        }
    }
}
