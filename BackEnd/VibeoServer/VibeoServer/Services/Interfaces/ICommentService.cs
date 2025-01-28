using VibeoServer.Models;

namespace VibeoServer.Services.Interfaces
{
    public interface ICommentService
    {
        Task<Comment?> CreateAsync(Comment? comment);
        IQueryable<Comment> GetAllQuery();
        IQueryable<Comment> GetByIdQuery(int id);
        Task<Comment?> UpdateAsync(int id, Comment? comment);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteByVideoIdAsync(int videoId);
        Task<bool> DeleteByChannelIdAsync(int channelId);
    }
}
