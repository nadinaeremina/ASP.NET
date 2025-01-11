using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using UserController.Model.Exceptions;
using UserController.Model.Service;
using ValidationException = UserController.Model.Exceptions.ValidationException;

namespace UserController.Model.Users
{
    // то, что приложение умеет делать с пользователями
    public class UserScenarios
    {
        // регулярки валидации
        string loginPattern = @"^[a-zA-Z0-9_]{3,16}$";
        string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        // используемые сервисы
        private readonly IUserRepository _users;
        // нужен будет для генерации ключей
        private readonly IEncoder _apiKeyGenerator;

        public UserScenarios(IUserRepository users, IEncoder apiKeyGenerator)
        {
            _users = users;
            _apiKeyGenerator = apiKeyGenerator;
        }

        // 'RegisterAsync' - регистрация пользователя
        // вход: строка логина и электронной почты пользователя
        // выход: api-ключ для данного пользователя
        // исключения: 'ValidationException' (невалидный логин или емэйл),
        // 'DuplicationException' (не должно быть пользователя с таким же логином или емэйлом)

        public async Task<string> RegisterAsync(string login, string email)
        {
            // валидация строк
            if (!Regex.IsMatch(login, loginPattern))
            {
                throw new ValidationException("login", "login is invalid", login);
            }
            if (!Regex.IsMatch(email, emailPattern))
            {
                throw new ValidationException("email", "email is invalid", email);
            }

            // проверка на дупликацию
            bool isLoginDuplicated = await _users.GetByLoginAsync(login) != null;
            if (isLoginDuplicated)
            {
                throw new DuplicationException("login", login);
            }
            bool isEmailDuplicated = await _users.GetByEmailAsync(email) != null;
            if (isEmailDuplicated)
            {
                throw new DuplicationException("email", email);
            }

            // выполним регистрацию
            User user = new User()
            {
                UUID = Guid.NewGuid(), // генерация UUID для пользователя
                Login = login,
                Email = email,
                RegisteredAt = DateTime.UtcNow,
            };
            string apiKey = generateApiKey(user);
            await _users.InsertAsync(user);
            return apiKey;
        }
        // 'GetUserAsync' - получение данных о пользователе по ключу
        // вход: api-ключ пользователя
        // выход: обьект с информацией о пользователе
        // исключения: UserNotFoundException
        public async Task<User> GetUserAsync(string apiKey)
        {
            foreach (User user in await _users.SelectAllAsync())
            {
                if (generateApiKey(user) == apiKey)
                {
                    return user;
                }
            }
            throw new UserNotFoundException();
        }
        // генерация api-ключа для пользователя
        private string generateApiKey(User user)
        {
            return _apiKeyGenerator.Encode($"{user.UUID} - {user.Login} - {user.Email} - {user.RegisteredAt}");
        }
    }
}
