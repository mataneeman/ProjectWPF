using System.Collections.ObjectModel;
using System.Linq;

namespace ProjectWpf.Todo_List
{
    class TaskManagerService
    {
        public ObservableCollection<TaskModel> Tasks { get; set; }

        public TaskManagerService()
        {
            Tasks = TaskDataManager.LoadTasks();
        }

        public void SaveTasks()
        {
            TaskDataManager.SaveTasks(Tasks);
        }

        public void UpdateTask(int taskId, string newDescription)
        {
            TaskModel? task = Tasks.FirstOrDefault(task => task.Id == taskId);
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
            TaskModel? task = Tasks.FirstOrDefault(task => task.Id == taskId);
            if (task != null)
            {
                task.IsCompleted = !task.IsCompleted;
                SaveTasks();
            }
            else
            {
                throw new Exception("The task with this Id wasn't found");
            }
        }

        public void AddNewTask(TaskModel task)
        {
            Tasks.Add(task);
            SaveTasks();
        }

        public void RemoveTask(int taskId)
        {
            TaskModel? taskToRemove = Tasks.FirstOrDefault(task => task.Id == taskId);
            if (taskToRemove != null)
            {
                Tasks.Remove(taskToRemove);
                SaveTasks();
            }
        }
    }
}
