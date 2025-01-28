using VibeoServer.Models;

namespace VibeoServer.Services.Interfaces
{
    public interface ISubscriptionService
    {
        Task<Subscription?> CreateAsync(Subscription? subscription);
        IQueryable<Subscription> GetAllQuery();
        IQueryable<Subscription> GetByIdQuery(int id);
        Task<Subscription?> UpdateAsync(int id, Subscription? subscription);
        Task<bool> DeleteAsync(int id);
    }
}
