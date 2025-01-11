namespace RequestChallange.Model
{
    // интерфейс хранилища для запросов 
    // чтобы отделить зависимость базы от нашей модели
    // отделим бизнес-логику от того, что делает база
    public interface IRequestRepository
    {
        Task<List<RequestInfo>> SelectAllAsync();
        Task InsertAsync(RequestInfo requestInfo);
        Task DeleteAllAsync();
    }
}
