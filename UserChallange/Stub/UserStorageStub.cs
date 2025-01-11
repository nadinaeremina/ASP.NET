using UserController.Model.Service;
using UserController.Model.Users;

namespace UserController.Stub
{
    // UserStorageStub - заглушка для IUserRepository
    public class UserStorageStub : IUserRepository
    {
        private static List<User> users = new List<User>();

        public async Task<User?> GetByEmailAsync(string email)
        {
            return users.FirstOrDefault(u => u.Email == email);
        }

        public async Task<User?> GetByLoginAsync(string login)
        {
            return users.FirstOrDefault(u => u.Login == login);
        }

        public async Task InsertAsync(User user)
        {
            users.Add(user);
        }

        public async Task<List<User>> SelectAllAsync()
        {
            return users;
        }
    }
}
