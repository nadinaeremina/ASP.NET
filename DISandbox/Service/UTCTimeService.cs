namespace DISandbox.Service
{
    // некоторый сервис для получения времени (UTC)
    public class UTCTimeService: ITimeService
    {
        //// трансиентный сервис - каждый раз будет создаваться заново
        //public TimeService()
        //{
        // каждый раз будет создаваться новый хэшкод
        //    Console.WriteLine($"{GetHashCode()}: created"); 
        //}
        //~TimeService()
        //{
        //    Console.WriteLine($"{GetHashCode()}: finalized");
        //}
        // финализатор - метод сработает, когда в куче удалитяс обьект
        public string GetTime()
        {
            return $"{DateTime.UtcNow:HH.mm.ss}";
        }
    }
}
