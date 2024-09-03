using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
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
        private const string FileName = "users.json";

        public UserManagement()
        {
            InitializeComponent();
            InitializeData();
            DataContext = this;

            // Ensure lvUsers.ItemsSource is not null before casting
            ListCollectionView collectionView = CollectionViewSource.GetDefaultView(lvUsers.ItemsSource) as ListCollectionView;
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
            if (File.Exists(FileName))
            {
                string json = File.ReadAllText(FileName);
                _users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
            }
            else
            {
                // Add default users if file does not exist
                _users.Add(new User("Ron Doe", new DateTime(2023, 10, 10), "john.doe@email.com", "Admin"));
                _users.Add(new User("Lane Smith", new DateTime(2021, 10, 12), "jane.smith@email.com", "User"));
                _users.Add(new User("Michael Johnson", new DateTime(2013, 10, 15), "michael.johnson@email.com", "Manager"));
            }

            lvUsers.ItemsSource = _users;
        }

        private void SaveData()
        {
            string json = JsonSerializer.Serialize(_users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FileName, json);
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            if (_users != null)
            {
                AddUser addUserWindow = new AddUser(_users);
                bool? result = addUserWindow.ShowDialog();
                if (result == true)
                {
                    lvUsers.Items.Refresh();
                    SaveData(); // Save data after adding a user
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
                if (_users != null)
                {
                    _users.Remove(userToDelete);
                    lvUsers.Items.Refresh();
                    SaveData(); // Save data after deleting a user
                    MessageBox.Show("User deleted.");
                }
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
                    SaveData(); // Save data after editing a user
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
                            _usersCollectionView.SortDescriptions.Add(new SortDescription("Role", _lastSortDirection));
                            break;
                    }
                }
            }
        }
    }
}
