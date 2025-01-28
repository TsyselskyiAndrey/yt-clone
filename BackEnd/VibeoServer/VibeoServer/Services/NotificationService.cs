using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Threading.Channels;
using VibeoServer.Data;
using VibeoServer.Models;
using VibeoServer.Services.Interfaces;

namespace VibeoServer.Services
{
    public class NotificationService : INotificationService
    {
        private readonly SqlDbContext _sqlDbContext;

        public NotificationService(SqlDbContext sqlDbContext)
        {
            _sqlDbContext = sqlDbContext;
        }


        public async Task<Notification?> CreateAsync(Notification? notification)
        {
            if (notification == null)
            {
                return null;
            }
            await _sqlDbContext.Notifications.AddAsync(notification);
            await _sqlDbContext.SaveChangesAsync();
            return notification;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var notification = await _sqlDbContext.Notifications.FindAsync(id);
            if (notification == null)
            {
                return false;
            }

            _sqlDbContext.Notifications.Remove(notification);
            await _sqlDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteByVideoIdAsync(int videoId)
        {
            var notifications = await _sqlDbContext.Notifications.Where(n => n.VideoId == videoId).ToListAsync();
            foreach (var notification in notifications)
            {
                await DeleteAsync(notification.Id);
            }

            return true;
        }
        public IQueryable<Notification> GetAllQuery()
        {
            return _sqlDbContext.Notifications.AsQueryable();
        }

        public IQueryable<Notification> GetByIdQuery(int id)
        {
            return _sqlDbContext.Notifications.Where(n => n.Id == id).AsQueryable();
        }

        public async Task<Notification?> UpdateAsync(int id, Notification? notification)
        {
            if (notification == null)
            {
                return null;
            }
            try
            {
                _sqlDbContext.Notifications.Update(notification);
                await _sqlDbContext.SaveChangesAsync();
                return notification;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return null;
            }
        }
    }
}
