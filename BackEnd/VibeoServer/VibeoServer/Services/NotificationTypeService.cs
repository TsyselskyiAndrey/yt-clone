using VibeoServer.Data;
using VibeoServer.Models;
using VibeoServer.Services.Interfaces;

namespace VibeoServer.Services
{
    public class NotificationTypeService : INotificationTypeService
    {
        private readonly SqlDbContext _sqlDbContext;

        public NotificationTypeService(SqlDbContext sqlDbContext)
        {
            _sqlDbContext = sqlDbContext;
        }
        public IQueryable<NotificationType> GetByNameQuery(string name)
        {
            return _sqlDbContext.NotificationTypes.Where(nt => nt.Name == name).AsQueryable();
        }
    }
}
