using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using UserManagement;

namespace ProjectWpf.User_Management
{
    public partial class UserManagement : Window
    {
        private List<User> _users = new List<User>();
        private ListCollectionView _usersCollectionView;
        private string _lastSortProperty = "";
        private ListSortDirection _lastSortDirection = ListSortDirection.Ascending;

        public UserManagement()
        {
            InitializeComponent();
            InitializeData();
            DataContext = this;

            // Ensure lvUsers.ItemsSource is not null before casting
            var collectionView = CollectionViewSource.GetDefaultView(lvUsers.ItemsSource) as ListCollectionView;
            if (collectionView != null)
            {
                _usersCollectionView = collectionView;
            }
            else
            {
                MessageBox.Show("Failed to obtain ListCollectionView from lvUsers.ItemsSource.");
            }
        }

        private void InitializeData()
        {
            _users.Add(new User("Ron Doe", new DateTime(2023, 10, 10), "john.doe@email.com", "Admin"));
            _users.Add(new User("Lane Smith", new DateTime(2021, 10, 12), "jane.smith@email.com", "User"));
            _users.Add(new User("Michael Johnson", new DateTime(2013, 10, 15), "michael.johnson@email.com", "Manager"));

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

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Button button && button.DataContext is User userToEdit)
            {
                EditUser editUserWindow = new EditUser(userToEdit);
                bool? result = editUserWindow.ShowDialog();
                if (result == true)
                {
                    lvUsers.Items.Refresh();
                }
            }
        }

        private void SortByColumn_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is GridViewColumnHeader header)
            {
                string? sortBy = header.Tag?.ToString(); // Use nullable string

                if (sortBy != null)
                {
                    if (_lastSortProperty == sortBy)
                    {
                        _lastSortDirection = _lastSortDirection == ListSortDirection.Ascending
                            ? ListSortDirection.Descending
                            : ListSortDirection.Ascending;
                    }
                    else
                    {
                        _lastSortDirection = ListSortDirection.Ascending;
                    }

                    _lastSortProperty = sortBy;

                    _usersCollectionView.SortDescriptions.Clear();
                    _usersCollectionView.CustomSort = null; // Remove any previous custom sort

                    switch (sortBy)
                    {
                        case "Id":
                            _usersCollectionView.SortDescriptions.Add(new SortDescription("Id", _lastSortDirection));
                            break;
                        case "Name":
                            _usersCollectionView.SortDescriptions.Add(new SortDescription("Name", _lastSortDirection));
                            break;
                        case "StartDate":
                            _usersCollectionView.SortDescriptions.Add(new SortDescription("StartDate", _lastSortDirection));
                            break;
                        case "Email":
                            _usersCollectionView.SortDescriptions.Add(new SortDescription("Email", _lastSortDirection));
                            break;
                        case "Role":
                            _usersCollectionView.SortDescriptions.Add(new SortDescription("Role",_lastSortDirection));
                            break;
                    }
                }
            }
        }
    }
}
