namespace RequestChallange.Model
{
    // через этот сервис отделим бизнес-логику приложения от того, что делает бд
    // сервис для работы  запросами 'RequestInfo'
    public class RequestService
    {
        private readonly IRequestRepository _repo;

        public RequestService(IRequestRepository repo)
        {
            _repo = repo;
        }

        // AddAsync - добавление данных об очередном запросе в систему
        // вход: объекты полученного и обработанного запроса и ответа, а так же время получения запроса
        // выход: -
        public async Task AddAsync(HttpRequest request, HttpResponse response, DateTime time)
        {
            // приложение преобразовывает входные данные в то, во что нужно
            RequestInfo requestInfo = new RequestInfo()
            {
                Method = request.Method,
                Path = request.Path,
                ReceivedAt = time,
                StatusCode = response.StatusCode
            };
            await _repo.InsertAsync(requestInfo);
        }

        // ListAllAsync - получение списка с информацией обо всех запросах, хранящихся в системе
        // вход: -
        // выход: список объектов с информацией о запросах
        public async Task<List<RequestInfo>> ListAllAsync()
        {
            return await _repo.SelectAllAsync();
        }

        // DeleteAllDataAsync - очистка данных о всех запросах в системе
        // вход: -
        // выход: -
        public async Task DeleteAllDataAsync()
        {
            await _repo.DeleteAllAsync();
        }
    }
}
