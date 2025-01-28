using Microsoft.EntityFrameworkCore;
using VibeoServer.Data;
using VibeoServer.Models;
using VibeoServer.Services.Interfaces;

namespace VibeoServer.Services
{
    public class CommentStatusService : ICommentStatusService
    {
        private readonly SqlDbContext _sqlDbContext;

        public CommentStatusService(SqlDbContext sqlDbContext)
        {
            _sqlDbContext = sqlDbContext;
        }


        public async Task<CommentStatus?> CreateAsync(CommentStatus? commentStatus)
        {
            if (commentStatus == null)
            {
                return null;
            }
            await _sqlDbContext.CommentStatuses.AddAsync(commentStatus);
            await _sqlDbContext.SaveChangesAsync();
            return commentStatus;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var commentStatus = await _sqlDbContext.CommentStatuses.FindAsync(id);
            if (commentStatus == null)
            {
                return false;
            }

            _sqlDbContext.CommentStatuses.Remove(commentStatus);
            await _sqlDbContext.SaveChangesAsync();
            return true;
        }

        public IQueryable<CommentStatus> GetAllQuery()
        {
            return _sqlDbContext.CommentStatuses.AsQueryable();
        }

        public IQueryable<CommentStatus> GetByIdQuery(int id)
        {
            return _sqlDbContext.CommentStatuses.Where(cs => cs.Id == id).AsQueryable();
        }

        public async Task<CommentStatus?> UpdateAsync(int id, CommentStatus? commentStatus)
        {
            if (commentStatus == null)
            {
                return null;
            }
            try
            {
                _sqlDbContext.CommentStatuses.Update(commentStatus);
                await _sqlDbContext.SaveChangesAsync();
                return commentStatus;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return null;
            }
        }
    }
}
