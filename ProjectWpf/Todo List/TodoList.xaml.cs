using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
            taskPanel.ItemsSource = _todoList.Tasks; 
        }

        private void OnTaskToggled(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.DataContext is TaskModel task)
            {
                _todoList.ToggleTaskIsComplete(task.Id);
            }
        }

        private void OnTextBlockMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (sender is TextBlock textBlock && textBlock.DataContext is TaskModel task)
                {
                    FrameworkElement parent = (FrameworkElement)textBlock.Parent;
                    TextBox editTextBox = (TextBox)parent.FindName("editTaskDescription");
                    Button btnSave = (Button)parent.FindName("btnSave");

                    textBlock.Visibility = Visibility.Collapsed;
                    editTextBox.Visibility = Visibility.Visible;
                    btnSave.Visibility = Visibility.Visible;

                    editTextBox.Text = textBlock.Text;
                }
            }
        }

        private void OnEditTask(object sender, RoutedEventArgs e)
        {
            if (sender is Button editButton)
            {
                FrameworkElement parent = (FrameworkElement)editButton.Parent;
                TextBlock textBlock = (TextBlock)parent.FindName("txtTaskDescription");
                TextBox editTextBox = (TextBox)parent.FindName("editTaskDescription");
                Button btnSave = (Button)parent.FindName("btnSave");

                if (textBlock != null && editTextBox != null && btnSave != null)
                {
                    textBlock.Visibility = Visibility.Collapsed;
                    editTextBox.Visibility = Visibility.Visible;
                    btnSave.Visibility = Visibility.Visible;

                    editTextBox.Text = textBlock.Text;
                }
            }
        }

        private void OnSaveEdit(object sender, RoutedEventArgs e)
        {
            if (sender is Button btnSave && btnSave.DataContext is TaskModel task)
            {
                FrameworkElement parent = (FrameworkElement)btnSave.Parent;
                TextBox editTextBox = (TextBox)parent.FindName("editTaskDescription");
                TextBlock textBlock = (TextBlock)parent.FindName("txtTaskDescription");

                editTextBox.Visibility = Visibility.Collapsed;
                btnSave.Visibility = Visibility.Collapsed;
                textBlock.Visibility = Visibility.Visible;

                textBlock.Text = editTextBox.Text;
                _todoList.UpdateTask(task.Id, editTextBox.Text);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtNewTask.Text))
            {
                TaskModel newTask = new TaskModel(_todoList.Tasks.Count + 1, txtNewTask.Text);
                _todoList.AddNewTask(newTask);
                txtNewTask.Clear();
            }
        }

        private void OnDeleteTask(object sender, RoutedEventArgs e)
        {
            if (sender is Button deleteButton && deleteButton.DataContext is TaskModel task)
            {
                _todoList.RemoveTask(task.Id);
            }
        }
    }
}
