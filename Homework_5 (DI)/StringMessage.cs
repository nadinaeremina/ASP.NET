namespace Homework_5__DI_
{
    public class StringMessage
    {
        public string Message { get; set; } = String.Empty;
        public StringMessage() { }
        public StringMessage(string message) 
        {
            Message = message;
        }
    }
}
