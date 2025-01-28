using Microsoft.EntityFrameworkCore;
using VibeoServer.Data;
using VibeoServer.Models;
using VibeoServer.Services.Interfaces;

namespace VibeoServer.Services
{
    public class VideoStatusService : IVideoStatusService
    {
        private readonly SqlDbContext _sqlDbContext;

        public VideoStatusService(SqlDbContext sqlDbContext)
        {
            _sqlDbContext = sqlDbContext;
        }

        public async Task<VideoStatus?> CreateAsync(VideoStatus? videoStatus)
        {
            if (videoStatus == null)
            {
                return null;
            }
            await _sqlDbContext.VideoStatuses.AddAsync(videoStatus);
            await _sqlDbContext.SaveChangesAsync();
            return videoStatus;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var videoStatus = await _sqlDbContext.VideoStatuses.FindAsync(id);
            if (videoStatus == null)
            {
                return false;
            }

            _sqlDbContext.VideoStatuses.Remove(videoStatus);
            await _sqlDbContext.SaveChangesAsync();
            return true;
        }

        public IQueryable<VideoStatus> GetAllQuery()
        {
            return _sqlDbContext.VideoStatuses.AsQueryable();
        }

        public IQueryable<VideoStatus> GetByIdQuery(int id)
        {
            return _sqlDbContext.VideoStatuses.Where(vs => vs.Id == id).AsQueryable();
        }

        public async Task<VideoStatus?> UpdateAsync(int id, VideoStatus? videoStatus)
        {
            if (videoStatus == null)
            {
                return null;
            }
            try
            {
                _sqlDbContext.VideoStatuses.Update(videoStatus);
                await _sqlDbContext.SaveChangesAsync();
                return videoStatus;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return null;
            }
        }
    }
}
