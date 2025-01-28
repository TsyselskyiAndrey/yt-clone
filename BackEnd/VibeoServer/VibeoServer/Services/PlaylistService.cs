using Microsoft.EntityFrameworkCore;
using VibeoServer.Data;
using VibeoServer.Models;
using VibeoServer.Services.Interfaces;
using static MimeDetective.Definitions.Default.FileTypes;

namespace VibeoServer.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly SqlDbContext _sqlDbContext;

        public PlaylistService(SqlDbContext sqlDbContext)
        {
            _sqlDbContext = sqlDbContext;
        }


        public async Task<Playlist?> CreateAsync(Playlist? playlist)
        {
            if (playlist == null)
            {
                return null;
            }
            await _sqlDbContext.Playlists.AddAsync(playlist);
            await _sqlDbContext.SaveChangesAsync();
            return playlist;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var playlist = await _sqlDbContext.Playlists.FindAsync(id);
            if (playlist == null)
            {
                return false;
            }

            _sqlDbContext.Playlists.Remove(playlist);
            await _sqlDbContext.SaveChangesAsync();
            return true;
        }

        public IQueryable<Playlist> GetAllQuery()
        {
            return _sqlDbContext.Playlists.AsQueryable();
        }

        public IQueryable<Playlist> GetByIdQuery(int id)
        {
            return _sqlDbContext.Playlists.Where(p => p.Id == id).AsQueryable();
        }

        public async Task<Playlist?> UpdateAsync(int id, Playlist? playlist)
        {
            if (playlist == null)
            {
                return null;
            }
            try
            {
                _sqlDbContext.Playlists.Update(playlist);
                await _sqlDbContext.SaveChangesAsync();
                return playlist;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return null;
            }
        }
    }
}
