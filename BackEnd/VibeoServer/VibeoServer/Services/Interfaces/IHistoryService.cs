using VibeoServer.Models;

namespace VibeoServer.Services.Interfaces
{
    public interface IHistoryService
    {
        Task<History?> CreateAsync(History? history);
        IQueryable<History> GetAllQuery();
        IQueryable<History> GetByIdQuery(int id);
        Task<History?> UpdateAsync(int id, History? history);
        Task<bool> DeleteAsync(int id);
    }
}
