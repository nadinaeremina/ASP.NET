namespace UserController.Model.Users
{
    // описание сущности пользователя
    public class User
    {
        // уникальный идентификатор пользователя, выдается системой
        public Guid UUID { get; set; }
        // логин пользователя
        public required string Login { get; set; }
        // электронная почта
        public required string Email { get; set; }
        // время регистрации
        public DateTime RegisteredAt { get; set; }
        public User() { }
    }
}
