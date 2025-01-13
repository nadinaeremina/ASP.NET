namespace DISandbox2
{
    public class TimeService
    {
        public TimeService() { }
        public string GetTime()
        {
            return $"{DateTime.UtcNow:G}";
        }
    }
}
