using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace ProjectWpf.Todo_List
{
    static class TaskDataManager
    {
        private static readonly string FilePath = "tasks.json";

        public static void SaveTasks(ObservableCollection<TaskModel> tasks)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(tasks, options);
            File.WriteAllText(FilePath, json);
        }

        public static ObservableCollection<TaskModel> LoadTasks()
        {
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);
                return JsonSerializer.Deserialize<ObservableCollection<TaskModel>>(json) ?? new ObservableCollection<TaskModel>();
            }
            return new ObservableCollection<TaskModel>();
        }
    }
}
