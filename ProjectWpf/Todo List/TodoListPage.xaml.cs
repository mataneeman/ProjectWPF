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

namespace ProjectWpf.Todo_List
{
    /// <summary>
    /// Interaction logic for TodoListPage.xaml
    /// </summary>
    public partial class TodoListPage : Page
    {
        public TodoListPage()
        {
            InitializeComponent();
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void goToList_Click(object sender, RoutedEventArgs e)
        {
            TodoList todoListwindow = new TodoList();
            todoListwindow.Show();
        }
    }
}