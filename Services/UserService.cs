using UserManagementAPI.Models;

namespace UserManagementAPI.Services
{
    public class UserService
    {
        private readonly List<User> _users = new();

        public List<User> GetAll() => _users;
        public User? GetById(int id) => _users.FirstOrDefault(u => u.Id == id);
        public void Add(User user) => _users.Add(user);
        public bool Update(User user)
        {
            var existing = GetById(user.Id);
            if (existing == null) return false;
            existing.FullName = user.FullName;
            existing.Email = user.Email;
            existing.Department = user.Department;
            return true;
        }
        public bool Delete(int id) => _users.RemoveAll(u => u.Id == id) > 0;
    }
}