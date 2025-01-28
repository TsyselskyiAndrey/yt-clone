using Microsoft.EntityFrameworkCore;
using VibeoServer.Data;
using VibeoServer.Models;
using VibeoServer.Services.Interfaces;

namespace VibeoServer.Services
{
    public class ChannelService : IChannelService
    {
        private readonly SqlDbContext _sqlDbContext;
        private readonly IVideoService _videoService;
        private readonly ICommentService _commentService;

        public ChannelService(SqlDbContext sqlDbContext, IVideoService videoService, ICommentService commentService)
        {
            _sqlDbContext = sqlDbContext;
            _videoService = videoService;
            _commentService = commentService;
        }

        public async Task<Channel?> CreateAsync(Channel? channel)
        {
            if (channel == null)
            {
                return null;
            }
            await _sqlDbContext.Channels.AddAsync(channel);
            await _sqlDbContext.SaveChangesAsync();
            return channel;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _commentService.DeleteByChannelIdAsync(id);
            await _videoService.DeleteByChannelIdAsync(id);
 
            var channel = await _sqlDbContext.Channels.FindAsync(id);
            if (channel == null)
            {
                return false;
            }

            _sqlDbContext.Channels.Remove(channel);
            await _sqlDbContext.SaveChangesAsync();
            return true;
        }

        public IQueryable<Channel> GetAllQuery()
        {
            return _sqlDbContext.Channels.AsQueryable();
        }

        public IQueryable<Channel> GetByIdQuery(int id)
        {
            return _sqlDbContext.Channels.Where(ch => ch.Id == id).AsQueryable();
        }

        public async Task<bool> IsHandleTakenAsync(string handle)
        {
            var res = await _sqlDbContext.Channels.FirstOrDefaultAsync(x => x.Handle == handle);
            return res != null;
        }

        public async Task<Channel?> UpdateAsync(int id, Channel? channel)
        {
            if (channel == null)
            {
                return null;
            }
            try
            {
                _sqlDbContext.Channels.Update(channel);
                await _sqlDbContext.SaveChangesAsync();
                return channel;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return null;
            }
        }
    }
}
