using Microsoft.EntityFrameworkCore;
using VibeoServer.Data;
using VibeoServer.Models;
using VibeoServer.Services.Interfaces;
using static MimeDetective.Definitions.Default.FileTypes;

namespace VibeoServer.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly SqlDbContext _sqlDbContext;

        public SubscriptionService(SqlDbContext sqlDbContext)
        {
            _sqlDbContext = sqlDbContext;
        }

        public async Task<Subscription?> CreateAsync(Subscription? subscription)
        {
            if (subscription == null)
            {
                return null;
            }
            await _sqlDbContext.Subscriptions.AddAsync(subscription);
            await _sqlDbContext.SaveChangesAsync();
            return subscription;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var subscription = await _sqlDbContext.Subscriptions.FindAsync(id);
            if (subscription == null)
            {
                return false;
            }

            _sqlDbContext.Subscriptions.Remove(subscription);
            await _sqlDbContext.SaveChangesAsync();
            return true;
        }

        public IQueryable<Subscription> GetAllQuery()
        {
            return _sqlDbContext.Subscriptions.AsQueryable();
        }

        public IQueryable<Subscription> GetByIdQuery(int id)
        {
            return _sqlDbContext.Subscriptions.Where(s => s.Id == id).AsQueryable();
        }

        public async Task<Subscription?> UpdateAsync(int id, Subscription? subscription)
        {
            if (subscription == null)
            {
                return null;
            }
            try
            {
                _sqlDbContext.Subscriptions.Update(subscription);
                await _sqlDbContext.SaveChangesAsync();
                return subscription;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return null;
            }
        }
    }
}
