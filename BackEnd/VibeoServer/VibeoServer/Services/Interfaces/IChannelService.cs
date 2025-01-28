using VibeoServer.Models;

namespace VibeoServer.Services.Interfaces
{
    public interface IChannelService
    {
        Task<Channel?> CreateAsync(Channel? channel);
        IQueryable<Channel> GetAllQuery();
        IQueryable<Channel> GetByIdQuery(int id);
        Task<bool> IsHandleTakenAsync(string handle);
        Task<Channel?> UpdateAsync(int id, Channel? channel);
        Task<bool> DeleteAsync(int id);
    }
}
