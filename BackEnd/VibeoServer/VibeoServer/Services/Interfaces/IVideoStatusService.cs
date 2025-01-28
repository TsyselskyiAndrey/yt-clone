using VibeoServer.Models;

namespace VibeoServer.Services.Interfaces
{
    public interface IVideoStatusService
    {
        Task<VideoStatus?> CreateAsync(VideoStatus? videoStatus);
        IQueryable<VideoStatus> GetAllQuery();
        IQueryable<VideoStatus> GetByIdQuery(int id);
        Task<VideoStatus?> UpdateAsync(int id, VideoStatus? videoStatus);
        Task<bool> DeleteAsync(int id);
    }
}
