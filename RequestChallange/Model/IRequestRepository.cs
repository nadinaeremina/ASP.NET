namespace RequestChallange.Model
{
    // интерфейс хранилища для запросов
    public interface IRequestRepository
    {
        Task InsertAsync(RequestInfo requestInfo);
        Task<List<RequestInfo>> SelectAllAsync();
        Task DeleteAllAsync();
    }
}
