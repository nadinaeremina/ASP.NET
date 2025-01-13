namespace DISandbox2
{
    public class GreetingService
    {
        public readonly TimeService _time;
        public readonly string _locale;
        public GreetingService(TimeService time, string locale)
        {
            _time = time;
            _locale = locale;
        }
        public string GetGreeting()
        {
            switch(_locale)
            {
                case "ru":
                    return $"{_time.GetTime()}: Привет ";
                case "en":
                    return $"{_time.GetTime()}: Hello ";
                case "sp":
                    return $"{_time.GetTime()}: Hola ";
                default:
                    return $"{_time.GetTime()}: Unknown locale '${_locale}'";
            }
        }
    }
}
