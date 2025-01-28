using VibeoServer.Models;

namespace VibeoServer.Services.Interfaces
{
    public interface ICommentStatusService
    {
        Task<CommentStatus?> CreateAsync(CommentStatus? commentStatus);
        IQueryable<CommentStatus> GetAllQuery();
        IQueryable<CommentStatus> GetByIdQuery(int id);
        Task<CommentStatus?> UpdateAsync(int id, CommentStatus? commentStatus);
        Task<bool> DeleteAsync(int id);
    }
}
