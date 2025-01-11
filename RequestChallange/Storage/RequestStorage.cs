using Microsoft.EntityFrameworkCore;
using RequestChallange.Model;

namespace RequestChallange.Storage
{
    // переопределяем 'RequestStorage'
    public class RequestStorage : IRequestRepository
    {
        // затаскиваем наш 'dbContext'
        private readonly ApplicationDbContext _db;
        public RequestStorage(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task DeleteAllAsync()
        {
            _db.Requests.RemoveRange(_db.Requests);
            await _db.SaveChangesAsync();
        }

        public async Task InsertAsync(RequestInfo requestInfo)
        {
            RequestData data = new RequestData()
            {
                Method = requestInfo.Method,
                Path = requestInfo.Path,
                ReceivedAt = requestInfo.ReceivedAt,
                StatusCode = requestInfo.StatusCode
            };
            await _db.Requests.AddAsync(data);
            await _db.SaveChangesAsync();
        }

        public async Task<List<RequestInfo>> SelectAllAsync()
        {
            return await _db.Requests.Select(r => new RequestInfo()
            {
                Method = r.Method,
                Path = r.Path,
                ReceivedAt = r.ReceivedAt,
                StatusCode = r.StatusCode
            }).ToListAsync();
        }
    }
}
