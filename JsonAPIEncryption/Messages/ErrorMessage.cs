namespace JsonAPIEncryption.Messages
{
    // класс для описания ошибки
    public class ErrorMessage
    {
        public string Type { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public ErrorMessage(string type, string message)
        {
            Type = type;
            Message = message;
        }
        public ErrorMessage() { }
    }
}
