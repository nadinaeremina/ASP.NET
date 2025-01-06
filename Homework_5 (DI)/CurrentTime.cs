namespace Homework_5__DI_
{
    public class CurrentTime: ITimeInformator
    {
        public CurrentTime() {}
        public string GetTimeString()
        {
            return $"{DateTime.Now:HH:mm:ss}";
        }
        public StringMessage GetTimeStringMessage()
        {
            return new StringMessage(GetTimeString());
        }
    }
}
