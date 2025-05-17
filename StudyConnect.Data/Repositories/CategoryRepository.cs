using Microsoft.EntityFrameworkCore;
using StudyConnect.Core.Interfaces;
using StudyConnect.Core.Models;
using StudyConnect.Data.Utilities;

namespace StudyConnect.Data.Repositories;

public class CategoryRepository : BaseRepository, ICategoryRepository
{
    public CategoryRepository(StudyConnectDbContext context) : base(context)
    {

    }

    public async Task<Guid> AddAsync(ForumCategory category)
    {
        // Add the category to the database
        var toAdd = new Entities.ForumCategory
        {
            ForumCategoryId = Guid.NewGuid(),
            Name = category.Name,
            Description = category.Description,
        };

        // Add the category to the database context
        await _context.ForumCategories.AddAsync(toAdd);
        await _context.SaveChangesAsync();

        return toAdd.ForumCategoryId;
    }

    public async Task<ForumCategory?> GetByIdAsync(Guid id)
    {
        var category = await _context.ForumCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.ForumCategoryId == id);

        return category.ToCategoryModel();
    }

    public async Task<ForumCategory?> GetByNameAsync(string name)
    {
        var category = await _context.ForumCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Name == name);

        return category.ToCategoryModel();
    }

    public async Task<IEnumerable<ForumCategory?>> GetAllAsync()
    {
        var categories = await _context.ForumCategories
            .AsNoTracking()
            .ToListAsync();

        var result = categories.Select(c => c.ToCategoryModel());
        return result;
    }

    public async Task DeleteAsync(Guid id)
    {
        var category = await _context.ForumCategories.FindAsync(id);

        _context.ForumCategories.Remove(category!);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistAsync(Guid categoryId) =>
        await _context.ForumCategories.AnyAsync(c => c.ForumCategoryId == categoryId);

    public async Task<bool> NameExistsAsync(string name) =>
        await _context.ForumCategories.AnyAsync(c => c.Name == name);
}
