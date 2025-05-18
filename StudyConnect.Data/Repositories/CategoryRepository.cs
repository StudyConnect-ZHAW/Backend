using Microsoft.EntityFrameworkCore;
using StudyConnect.Core.Interfaces.Repositories;
using StudyConnect.Core.Models;
using StudyConnect.Data.Utilities;

namespace StudyConnect.Data.Repositories;

public class CategoryRepository : BaseRepository, ICategoryRepository
{
    public CategoryRepository(StudyConnectDbContext context) : base(context) { }

    public async Task<Guid> AddAsync(ForumCategory category)
    {
        var entity = new Entities.ForumCategory
        {
            ForumCategoryId = Guid.NewGuid(),
            Name = category.Name,
            Description = category.Description
        };

        await _context.ForumCategories.AddAsync(entity);
        await _context.SaveChangesAsync();

        return entity.ForumCategoryId;
    }

    public async Task<ForumCategory?> GetByIdAsync(Guid id)
    {
        var entity = await _context.ForumCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.ForumCategoryId == id);

        return entity?.ToCategoryModel();
    }

    public async Task<ForumCategory?> GetByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return null;

        var entity = await _context.ForumCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Name == name);

        return entity?.ToCategoryModel();
    }

    public async Task<IEnumerable<ForumCategory>> GetAllAsync()
    {
        var entities = await _context.ForumCategories
            .AsNoTracking()
            .ToListAsync();

        return entities
            .Select(c => c.ToCategoryModel());
    }


    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.ForumCategories.FindAsync(id);
        if (entity == null) return;

        _context.ForumCategories.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistAsync(Guid id) =>
        await _context.ForumCategories.AnyAsync(c => c.ForumCategoryId == categoryId);

    public async Task<bool> NameExistsAsync(string name) =>
        await _context.ForumCategories.AnyAsync(c => c.Name == name);
}

