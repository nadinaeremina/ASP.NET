using Algorithm_RLE.Messages;

namespace JsonAPI.Messages
{
    // вывод сообщения
    public class StringMessage: IMessage
    {
        public string Message { get; set; } = String.Empty;
        public StringMessage() {}
    }
}
