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

        // нужно уметь делать пользователя випами
        public async Task SetVIPAsync(Guid uuid)
        {
            User user = await GetByUUIDAsync(uuid);
            user.IsVIP = true;
            await _users.UpdateAsync(user);
        }

        // забирать випку
        public async Task UnsetVIPAsync(Guid uuid)
        {
            User user = await GetByUUIDAsync(uuid);
            user.IsVIP = false;
            await _users.UpdateAsync(user);
        }

        // посмотреть всех юзеров
        public async Task<List<User>> ListAllAsync()
        {
            return await _users.SelectAllAsync();
        }

        // посмотреть одного юзера по uuid-у
        public async Task<User> GetByUUIDAsync(Guid uuid)
        {
            User? user = await _users.GetByUUIDAsync(uuid);
            if(user == null)
            {
                throw new UserNotFoundException();
            }
            return user;
        }

        // посмотреть одного юзера по email
        public async Task<User> GetByEmailAsync(string email)
        {
            User? user = await _users.GetByEmailAsync(email);
            if (user == null)
            {
                throw new UserNotFoundException();
            }
            return user;
        }

        // посмотреть одного юзера по логину
        public async Task<User> GetByLoginAsync(string login)
        {
            User? user = await _users.GetByLoginAsync(login);
            if (user == null)
            {
                throw new UserNotFoundException();
            }
            return user;
        }

        // добавить юзера
        public async Task InsertAsync(User user)
        {
            await _users.InsertAsync(user);
        }
    }
}
