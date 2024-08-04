using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWpf.Todo_List
{
    class TaskModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsCompleted { get; set; }

        public TaskModel(int id, string description)
        {
            Id = id;
            Description = description;
            IsCompleted = false;
            CreationTime = DateTime.Now;
        }
        public override string ToString()
        {
            return $"{Id}. {Description} - {CreationTime.ToString()} - Is Done: {IsCompleted}";
        }
    }
}
