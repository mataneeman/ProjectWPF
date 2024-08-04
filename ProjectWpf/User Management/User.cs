namespace UserManagement
{
    public class User
    {
        private static int _nextId = 1; 

        public int Id { get; private set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public User(string name, DateTime startDate, string email, string role)
        {
            Id = _nextId++;
            Name = name;
            StartDate = startDate;
            Email = email;
            Role = role;
        }

        public double GetExp()
        {
            DateTime currentDate = DateTime.Now;
            double exp = (currentDate - StartDate).TotalDays / 365;
            return exp;
        }
    }
}
