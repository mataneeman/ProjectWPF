using System;
using System.Windows;
using System.Windows.Controls;
using UserManagement;

namespace ProjectWpf.User_Management
{
    public partial class EditUser : Window
    {
        private User _user;

        public EditUser(User user)
        {
            InitializeComponent();
            _user = user;

            // Initialize fields with user data
            NameTextBox.Text = user.Name;
            StartDatePicker.SelectedDate = user.StartDate;
            EmailTextBox.Text = user.Email;

            // Set ComboBox selection
            foreach (ComboBoxItem item in RoleComboBox.Items)
            {
                if (item.Content.ToString() == user.Role)
                {
                    RoleComboBox.SelectedItem = item;
                    break;
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Save changes back to user
            _user.Name = NameTextBox.Text;
            _user.StartDate = StartDatePicker.SelectedDate.GetValueOrDefault();
            _user.Email = EmailTextBox.Text;
            _user.Role = ((ComboBoxItem)RoleComboBox.SelectedItem)?.Content?.ToString() ?? "DefaultRole";

            DialogResult = true;
            Close();
        }
    }
}
