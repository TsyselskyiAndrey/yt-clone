using Microsoft.EntityFrameworkCore;
using System.Threading.Channels;
using VibeoServer.Data;
using VibeoServer.Models;
using VibeoServer.Services.Interfaces;

namespace VibeoServer.Services
{
    public class VideoService : IVideoService
    {
        private readonly SqlDbContext _sqlDbContext;
        private readonly ICommentService _commentService;
        private readonly INotificationService _notificationService;

        public VideoService(SqlDbContext sqlDbContext, ICommentService commentService, INotificationService notificationService)
        {
            _sqlDbContext = sqlDbContext;
            _commentService = commentService;
            _notificationService = notificationService;
        }

        public async Task<Video?> CreateAsync(Video? video)
        {
            if(video == null)
            {
                return null;
            }
            await _sqlDbContext.Videos.AddAsync(video);
            await _sqlDbContext.SaveChangesAsync();
            return video;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _notificationService.DeleteByVideoIdAsync(id);
            await _commentService.DeleteByVideoIdAsync(id);
            var video = await _sqlDbContext.Videos.FindAsync(id);
            if (video == null)
            {
                return false;
            }

            _sqlDbContext.Videos.Remove(video);
            await _sqlDbContext.SaveChangesAsync();
            return true;
        }


        public async Task<bool> DeleteByChannelIdAsync(int channelId)
        {
            var videos = await _sqlDbContext.Videos.Where(v => v.ChannelId == channelId).ToListAsync();
            foreach (var video in videos)
            {
                await DeleteAsync(video.Id);
            }

            return true;
        }


        public IQueryable<Video> GetAllQuery()
        {
            return _sqlDbContext.Videos.AsQueryable();
        }

        public IQueryable<Video> GetByIdQuery(int id)
        {
            return _sqlDbContext.Videos.Where(v => v.Id == id).AsQueryable();
        }

        public async Task<Video?> UpdateAsync(int id, Video? video)
        {
            if(video == null)
            {
                return null;
            }
            try
            {
                _sqlDbContext.Videos.Update(video);
                await _sqlDbContext.SaveChangesAsync();
                return video;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return null;
            }
        }
    }
}
