using UserChallange.Model.Exceptions;
using UserChallange.Model.Service;
using UserChallange.Model.Users;

namespace UserChallange.Model.Users
{
    // сценарии для администрирования пользователей
    public class UserAdministrationScenarios
    {
        private readonly IUserRepository _users;
        public UserAdministrationScenarios(IUserRepository users)
        {
            _users = users;
        }

        // Методы для управления пользователями (администрирование)

        public async Task SetVIPAsync(Guid uuid)
        {
            User user = await GetUserAsync(uuid);
            user.IsVIP = true;
            await _users.UpdateAsync(user);
        }

        public async Task UnsetVIPAsync(Guid uuid)
        {
            User user = await GetUserAsync(uuid);
            user.IsVIP = false;
            await _users.UpdateAsync(user);
        }

        public async Task<List<User>> ListAllAsync()
        {
            return await _users.SelectAllAsync();
        }

        public async Task<User> GetUserAsync(Guid uuid)
        {
            User? user = await _users.GetByUUIDAsync(uuid);
            if (user == null)
            {
                throw new UserNotFoundException();
            }
            return user;
        }
    }
}
