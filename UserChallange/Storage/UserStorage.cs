using Microsoft.EntityFrameworkCore;
using UserChallange.Model.Service;
using UserChallange.Model.Users;

namespace UserChallange.Storage
{
    // UserStorageStub - заглушка для IUserRepository
    // хранит всех 'UserStorage' в статическом списке
    public class UserStorage : IUserRepository
    {
        // статическая БД
        //private static List<User> users = new List<User>()
        //{
        //    // тестовый пользователь
        //    // uuid: a88966aa-f26d-4028-8ec6-064455070950
        //    // api-key: 2b8fd0d0fd27dc4935dbe91076736d10
        //    new User()
        //    {
        //        UUID = Guid.Parse("a88966aa-f26d-4028-8ec6-064455070950"),
        //        Login = "test_user",
        //        Email = "test@test.com",
        //        RegisteredAt = DateTime.Parse("2025-01-12T10:39:07.8052232Z").ToUniversalTime(),
        //        IsVIP = false,
        //    }
        // };

        // затаскиваем наш 'dbContext'
        private readonly ApplicationDbContext _db;
        public UserStorage(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByLoginAsync(string login)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Login == login);
        }

        public async Task<User?> GetByUUIDAsync(Guid uuid)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.UUID == uuid);
        }

        public async Task InsertAsync(User user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
        }

        public async Task<List<User>> SelectAllAsync()
        {
            return await _db.Users.ToListAsync();
        }

        public async Task UpdateAsync(User user)
        {
            User? updated = await _db.Users.FirstOrDefaultAsync(u => u.UUID == user.UUID);
            if (updated != null)
            {
                // удалить сначала по значению из списка
                //_db.Users.Remove(updated);             
                // а потом добавить обновленного
                //_db.Users.Add(user);

                _db.Update(user);
                await _db.SaveChangesAsync();
            }
        }
    }
}