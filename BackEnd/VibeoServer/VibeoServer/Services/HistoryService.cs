using Microsoft.EntityFrameworkCore;
using VibeoServer.Data;
using VibeoServer.Models;
using VibeoServer.Services.Interfaces;

namespace VibeoServer.Services
{
    public class HistoryService : IHistoryService
    {
        private readonly SqlDbContext _sqlDbContext;

        public HistoryService(SqlDbContext sqlDbContext)
        {
            _sqlDbContext = sqlDbContext;
        }

        public async Task<History?> CreateAsync(History? history)
        {
            if (history == null)
            {
                return null;
            }
            await _sqlDbContext.Histories.AddAsync(history);
            await _sqlDbContext.SaveChangesAsync();
            return history;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var history = await _sqlDbContext.Histories.FindAsync(id);
            if (history == null)
            {
                return false;
            }

            _sqlDbContext.Histories.Remove(history);
            await _sqlDbContext.SaveChangesAsync();
            return true;
        }

        public IQueryable<History> GetAllQuery()
        {
            return _sqlDbContext.Histories.AsQueryable();
        }

        public IQueryable<History> GetByIdQuery(int id)
        {
            return _sqlDbContext.Histories.Where(vs => vs.Id == id).AsQueryable();
        }

        public async Task<History?> UpdateAsync(int id, History? history)
        {
            if (history == null)
            {
                return null;
            }
            try
            {
                _sqlDbContext.Histories.Update(history);
                await _sqlDbContext.SaveChangesAsync();
                return history;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return null;
            }
        }
    }
}
