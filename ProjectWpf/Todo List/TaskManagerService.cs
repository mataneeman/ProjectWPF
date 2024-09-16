using System.Collections.ObjectModel;
using System.Linq;

namespace ProjectWpf.Todo_List
{
    class TaskManagerService
    {
        public ObservableCollection<TaskModel> ActiveTasks { get; set; }
        public ObservableCollection<TaskModel> CompletedTasks { get; set; }

        public TaskManagerService()
        {
            ObservableCollection<TaskModel> allTasks = TaskDataManager.LoadTasks();
            ActiveTasks = new ObservableCollection<TaskModel>(allTasks.Where(t => !t.IsCompleted));
            CompletedTasks = new ObservableCollection<TaskModel>(allTasks.Where(t => t.IsCompleted));
        }

        public void SaveTasks()
        {
            ObservableCollection<TaskModel> allTasks = new ObservableCollection<TaskModel>(ActiveTasks.Concat(CompletedTasks));
            TaskDataManager.SaveTasks(allTasks);
        }

        public void UpdateTask(int taskId, string newDescription)
        {
            TaskModel task = ActiveTasks.FirstOrDefault(t => t.Id == taskId) ?? CompletedTasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null)
            {
                task.Description = newDescription;
                SaveTasks();
            }
            else
            {
                throw new Exception("The task with this Id wasn't found");
            }
        }

        public void ToggleTaskIsComplete(int taskId)
        {
            // חפש את המשימה ברשימת המשימות הפעילות
            TaskModel task = ActiveTasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null)
            {
                // אם המשימה קיימת, שנה את מצב ההשלמה שלה
                task.IsCompleted = true;
                ActiveTasks.Remove(task);
                CompletedTasks.Add(task);
                SaveTasks();
            }
            else
            {
                // חפש את המשימה ברשימת המשימות המושלמות
                task = CompletedTasks.FirstOrDefault(t => t.Id == taskId);
                if (task != null)
                {
                    // אם המשימה קיימת ברשימת המשימות המושלמות, שנה את מצב ההשלמה שלה
                    task.IsCompleted = false;
                    CompletedTasks.Remove(task);
                    ActiveTasks.Add(task);
                    SaveTasks();
                }
            }
        }

        public void AddNewTask(TaskModel task)
        {
            ActiveTasks.Add(task);
            SaveTasks();
        }

        public void RemoveTask(int taskId)
        {
            TaskModel taskToRemove = ActiveTasks.FirstOrDefault(t => t.Id == taskId);
            if (taskToRemove != null)
            {
                ActiveTasks.Remove(taskToRemove);
                SaveTasks();
                return;
            }
            taskToRemove = CompletedTasks.FirstOrDefault(t => t.Id == taskId);
            if (taskToRemove != null)
            {
                CompletedTasks.Remove(taskToRemove);
                SaveTasks();
            }
        }
    }
}
