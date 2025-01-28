using Microsoft.EntityFrameworkCore;
using VibeoServer.Data;
using VibeoServer.Models;
using VibeoServer.Services.Interfaces;

namespace VibeoServer.Services
{
    public class TagService : ITagService
    {
        private readonly SqlDbContext _sqlDbContext;

        public TagService(SqlDbContext sqlDbContext)
        {
            _sqlDbContext = sqlDbContext;
        }
        public IQueryable<Tag> GetAllQuery()
        {
            return _sqlDbContext.Tags.AsQueryable();
        }
    }
}
