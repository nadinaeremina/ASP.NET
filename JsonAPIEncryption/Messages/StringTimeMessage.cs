namespace JsonAPIEncryption.Messages
{
    public class StringTimeMessage
    {
        public string Message { get; set; } = String.Empty;
        public string StartTime { get; set; } = String.Empty;
        public string NowTime { get; set; } = String.Empty;
        public StringTimeMessage() { }
    }
}
