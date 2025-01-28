using VibeoServer.Models;

namespace VibeoServer.Services.Interfaces
{
    public interface INotificationTypeService
    {
        IQueryable<NotificationType> GetByNameQuery(string name);
    }
}
