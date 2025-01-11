using UserController.Model.Users;

namespace UserController.Model.Service
{
    // storage
    // IUserRepository - интерфейс хранилища пользователей
    public interface IUserRepository
    {
        // SQL-подобные запросы

        // получить пользователя по логину
        Task<User?> GetByLoginAsync(string login);

        // получить пользователя по email-у
        Task<User?> GetByEmailAsync(string email);

        // добавление пользователя в хранилище
        Task InsertAsync(User user);

        // получение списка всех пользователей
        Task<List<User>> SelectAllAsync();
    }
}
