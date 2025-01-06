namespace DISandbox.Service
{
    // класс, который позволяет получать время с часовым поясом каким-то
    public class LocalTimeService: ITimeService
    {
        private readonly int _timezone;
        public LocalTimeService(): this(TimeZoneInfo.Local.BaseUtcOffset.Hours){}
        public LocalTimeService(int timezone) 
        {
            if (timezone < -12 || timezone > 14)
            {
                throw new ArgumentOutOfRangeException("$timezone must be in range: [-12; 14]");
            }
            _timezone = timezone;
        }
        public string GetTime()
        {
            return $"{DateTime.Now.AddHours(_timezone):HH.mm.ss}";
        }
    }
}
