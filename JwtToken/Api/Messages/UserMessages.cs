namespace UserChallange.Api.Messages
{
    // RegistrationMessage - сообщение для регистрации пользователя
    public record RegistrationMessage(string Login, string Email);

    // ApiKeyMessage - сообщение для апи-ключа
    public record ApiKeyMessage(string ApiKey);
    // сообщение для токена
    public record TokenMessage(string Token);
}
