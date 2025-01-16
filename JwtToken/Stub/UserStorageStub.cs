using UserChallange.Model.Service;
using UserChallange.Model.Users;

namespace JwtToken.Stub
{
    // UserStorageStub - заглушка для IUserRepository
    public class UserStorageStub : IUserRepository
    {
        private static List<User> users = new List<User>()
        {
            // тестовый пользователь
            // uuid: f2260b19-5805-403e-9798-03b1c6d2be6c
            // api-key: 293fc8a744a4644cb802e2d83090d9dd
            new User()
            {
                UUID = Guid.Parse("f2260b19-5805-403e-9798-03b1c6d2be6c"),
                Login = "test_user",
                Email = "test@test.com",
                RegisteredAt = DateTime.Parse("2024-12-28T16:30:10.3755953Z").ToUniversalTime(),
                IsVIP = false,
            }
        };

        public async Task<User?> GetByEmailAsync(string email)
        {
            return users.FirstOrDefault(u => u.Email == email);
        }

        public async Task<User?> GetByLoginAsync(string login)
        {
            return users.FirstOrDefault(u => u.Login == login);
        }

        public async Task<User?> GetByUUIDAsync(Guid uuid)
        {
            return users.FirstOrDefault(u => u.UUID == uuid);
        }

        public async Task InsertAsync(User user)
        {
            users.Add(user);
        }

        public async Task<List<User>> SelectAllAsync()
        {
            return users;
        }

        public async Task UpdateAsync(User user)
        {
            User? updated = users.FirstOrDefault(u => u.UUID == user.UUID);
            if (updated != null)
            {
                users.Remove(user);
                users.Add(user);
            }
        }
    }
}
