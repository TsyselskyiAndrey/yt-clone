using VibeoServer.Models;

namespace VibeoServer.Services.Interfaces
{
    public interface IPlaylistService
    {
        Task<Playlist?> CreateAsync(Playlist? playlist);
        IQueryable<Playlist> GetAllQuery();
        IQueryable<Playlist> GetByIdQuery(int id);
        Task<Playlist?> UpdateAsync(int id, Playlist? playlist);
        Task<bool> DeleteAsync(int id);
    }
}
