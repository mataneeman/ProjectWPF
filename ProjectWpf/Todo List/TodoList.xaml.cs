using System.Windows;
using System.Windows.Controls;

namespace ProjectWpf.Todo_List
{
    public partial class TodoList : Window
    {
        private TaskManagerService _todoList = new TaskManagerService();

        public TodoList()
        {
            InitializeComponent();
            InitializeTasks();
        }

        private void InitializeTasks()
        {
            _todoList = new TaskManagerService();
            taskPanel.ItemsSource = _todoList.ActiveTasks;
            completedTaskPanel.ItemsSource = _todoList.CompletedTasks;
        }

        private void OnTaskToggled(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                TaskModel task = checkBox.DataContext as TaskModel;
                if (task != null)
                {
                    _todoList.ToggleTaskIsComplete(task.Id);
                }
            }
        }

        private void OnEditTask(object sender, RoutedEventArgs e)
        {
            Button editButton = sender as Button;
            if (editButton != null)
            {
                FrameworkElement parent = (FrameworkElement)editButton.Parent;
                TextBlock textBlock = (TextBlock)parent.FindName("txtTaskDescription");
                TextBox editTextBox = (TextBox)parent.FindName("editTaskDescription");
                Button btnSave = (Button)parent.FindName("btnSave");

                if (textBlock != null && editTextBox != null && btnSave != null)
                {
                    editTextBox.Text = textBlock.Text;
                    textBlock.Visibility = Visibility.Collapsed;
                    editTextBox.Visibility = Visibility.Visible;
                    btnSave.Visibility = Visibility.Visible;
                }
            }
        }

        private void OnSaveEdit(object sender, RoutedEventArgs e)
        {
            Button saveButton = sender as Button;
            if (saveButton != null)
            {
                FrameworkElement parent = (FrameworkElement)saveButton.Parent;
                TextBlock textBlock = (TextBlock)parent.FindName("txtTaskDescription");
                TextBox editTextBox = (TextBox)parent.FindName("editTaskDescription");

                if (textBlock != null && editTextBox != null)
                {
                    TaskModel task = textBlock.DataContext as TaskModel;
                    if (task != null)
                    {
                        _todoList.UpdateTask(task.Id, editTextBox.Text);
                        textBlock.Text = editTextBox.Text;
                        textBlock.Visibility = Visibility.Visible;
                        editTextBox.Visibility = Visibility.Collapsed;
                        saveButton.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string newTaskText = txtNewTask.Text;
            if (!string.IsNullOrWhiteSpace(newTaskText))
            {
                int id = _todoList.ActiveTasks.Count + _todoList.CompletedTasks.Count + 1;
                TaskModel newTask = new TaskModel(id, newTaskText);
                _todoList.AddNewTask(newTask);
                txtNewTask.Clear();
            }
        }

        private void OnDeleteTask(object sender, RoutedEventArgs e)
        {
            Button deleteButton = sender as Button;
            if (deleteButton != null)
            {
                FrameworkElement parent = (FrameworkElement)deleteButton.Parent;
                TextBlock textBlock = (TextBlock)parent.FindName("txtTaskDescription");

                if (textBlock != null)
                {
                    TaskModel task = textBlock.DataContext as TaskModel;
                    if (task != null)
                    {
                        _todoList.RemoveTask(task.Id);
                    }
                }
            }
        }
    }
}
