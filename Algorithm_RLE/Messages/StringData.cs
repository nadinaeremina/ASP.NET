using Algorithm_RLE.Messages;

namespace JsonAPI.Messages
{
    // обьект со строковыми данными
    public class StringData: IMessage
    {
        public string Data { get; set; } = string.Empty;
        public StringData() {}
    }
}
