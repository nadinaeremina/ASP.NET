namespace UserChallange.Model.Users
{
    // описание сущности пользователя
    public class User
    {
        public int Id { get; set; }
        // уникальный идентификатор пользователя, выдается системой
        // уникальный во всей системе
        public Guid UUID { get; set; }
        // логин пользователя
        public required string Login { get; set; }
        // электронная почта
        public required string Email { get; set; }
        // время регистрации
        public DateTime RegisteredAt { get; set; }
        // является ли VIP-пользователем (для авторизации)
        public bool IsVIP { get; set; }
        public User() { }
    }
}
