using VibeoServer.Models;

namespace VibeoServer.Services.Interfaces
{
    public interface ITagService
    {
        IQueryable<Tag> GetAllQuery();
    }
}
