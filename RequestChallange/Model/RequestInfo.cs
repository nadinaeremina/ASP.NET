namespace RequestChallange.Model
{
    // модель (приложения) не привязана к 'storage', она ни от чего не зависит, только от нее все зависит 
    // класс, описывающий информацию о http-запросе
    public class RequestInfo
    {
        // строка с методом
        public string Method { get; set; } = string.Empty;
        // строка пути
        public string Path { get; set; } = string.Empty;
        // время получения
        public DateTime ReceivedAt { get; set; }
        // статус-код результата
        public int StatusCode { get; set; }

        public RequestInfo() { }
    }
}
