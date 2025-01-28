using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VibeoServer.Data;
using VibeoServer.Models;
using VibeoServer.Services.Interfaces;

namespace VibeoServer.Services
{
    public class CommentService : ICommentService
    {
        private readonly SqlDbContext _sqlDbContext;

        public CommentService(SqlDbContext sqlDbContext)
        {
            _sqlDbContext = sqlDbContext;
        }

        public async Task<Comment?> CreateAsync(Comment? comment)
        {
            if (comment == null)
            {
                return null;
            }
            await _sqlDbContext.Comments.AddAsync(comment);
            await _sqlDbContext.SaveChangesAsync();
            return comment;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await DeleteByParentIdAsync(id);
            var comment = await _sqlDbContext.Comments.FindAsync(id);
            if (comment == null)
            {
                return false;
            }

            _sqlDbContext.Comments.Remove(comment);
            await _sqlDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteByParentIdAsync(int parentId)
        {
            var comments = await _sqlDbContext.Comments.Where(c => c.ParentCommentId == parentId).ToListAsync();
            foreach (var comment in comments)
            {
                await DeleteAsync(comment.Id);
            }

            return true;
        }

        public async Task<bool> DeleteByVideoIdAsync(int videoId)
        {
            var comments = await _sqlDbContext.Comments.Where(c => c.VideoId == videoId).ToListAsync();
            foreach (var comment in comments)
            {
                await DeleteAsync(comment.Id);
            }

            return true;
        }

        public async Task<bool> DeleteByChannelIdAsync(int channelId)
        {
            var comments = await _sqlDbContext.Comments.Where(c => c.ChannelId == channelId).ToListAsync();
            foreach (var comment in comments)
            {
                await DeleteAsync(comment.Id);
            }

            return true;
        }

        public IQueryable<Comment> GetAllQuery()
        {
            return _sqlDbContext.Comments.AsQueryable();
        }

        public IQueryable<Comment> GetByIdQuery(int id)
        {
            return _sqlDbContext.Comments.Where(c => c.Id == id).AsQueryable();
        }

        public async Task<Comment?> UpdateAsync(int id, Comment? comment)
        {
            if (comment == null)
            {
                return null;
            }
            try
            {
                _sqlDbContext.Comments.Update(comment);
                await _sqlDbContext.SaveChangesAsync();
                return comment;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return null;
            }
        }
    }
}
