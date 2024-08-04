using System.Collections.Generic;
using System.Linq;

namespace UserManagement
{
    public class Department
    {
        private string _departmentName = null!; 
 

        public int DepartmentId { get; set; }
        public string DepartmentName
        {
            get => _departmentName;
            set => _departmentName = value.ToUpper();
        }

        public List<User> Users { get; set; }

        public Department(int departmentId, string departmentName, List<User> users)
        {
            DepartmentId = departmentId;
            DepartmentName = departmentName;
            Users = users;
        }

        public void AddUser(User user)
        {
            Users.Add(user);
        }

        public void RemoveUser(int id)
        {
            User? userToRemove = Users.FirstOrDefault(u => u.Id == id);
            if (userToRemove != null)
            {
                Users.Remove(userToRemove);
            }
        }

        public List<User> GetUsers()
        {
            return Users;
        }
    }
}
