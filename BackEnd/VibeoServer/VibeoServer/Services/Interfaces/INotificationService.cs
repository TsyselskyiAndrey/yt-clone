using VibeoServer.Models;

namespace VibeoServer.Services.Interfaces
{
    public interface INotificationService
    {
        Task<Notification?> CreateAsync(Notification? notification);
        IQueryable<Notification> GetAllQuery();
        IQueryable<Notification> GetByIdQuery(int id);
        Task<Notification?> UpdateAsync(int id, Notification? notification);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteByVideoIdAsync(int videoId);
    }
}
