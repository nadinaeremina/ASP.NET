using UserChallange.Model.Users;

namespace UserChallange.Model.Service
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
        // получить пользователя по uuid-у
        Task<User?> GetByUUIDAsync(Guid uuid);

        // добавление пользователя в хранилище
        Task InsertAsync(User user);

        // получение списка всех пользователей
        Task<List<User>> SelectAllAsync();
        // обновить пользователя
        Task UpdateAsync(User user);
    }
}
