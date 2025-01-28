using Microsoft.EntityFrameworkCore;
using VibeoServer.Data;
using VibeoServer.Models;
using VibeoServer.Services.Interfaces;

namespace VibeoServer.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly SqlDbContext _sqlDbContext;

        public CategoryService(SqlDbContext sqlDbContext)
        {
            _sqlDbContext = sqlDbContext;
        }

        public IQueryable<Category> GetAllQuery()
        {
            return _sqlDbContext.Categories.AsQueryable();
        }

    }
}
