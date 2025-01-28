using VibeoServer.Models;

namespace VibeoServer.Services.Interfaces
{
    public interface IVideoService
    {
        Task<Video?> CreateAsync(Video? video);
        IQueryable<Video> GetAllQuery();
        IQueryable<Video> GetByIdQuery(int id);
        Task<Video?> UpdateAsync(int id, Video? video);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteByChannelIdAsync(int channelId);
    }
}
