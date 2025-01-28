
using VibeoServer.Models;

namespace VibeoServer.Services.Interfaces
{
    public interface ICategoryService
    {
        IQueryable<Category> GetAllQuery();
    }
}
