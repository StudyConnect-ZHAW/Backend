using System;
using Microsoft.EntityFrameworkCore;
using StudyConnect.Core.Common;
using StudyConnect.Core.Interfaces;
using StudyConnect.Core.Models;

namespace StudyConnect.Data.Repositories;

public class CategoryRepository : BaseRepository, ICategoryRepository
{
    public CategoryRepository(StudyConnectDbContext context) : base(context)
    {

    }

    public async Task<OperationResult<bool>> AddAsync(ForumCategory? category)
    {
        if (category == null)
        {
            return OperationResult<bool>.Failure("Category cannot be null.");
        }

        var providedCategoryId = await _context.ForumCategories.FirstOrDefaultAsync(c => c.ForumCategoryId == category.ForumCategoryId);
        var providedCategoryName = await _context.ForumCategories.FirstOrDefaultAsync(c => c.Name == category.Name);
        if (providedCategoryId != null || providedCategoryName != null)
        {
            return OperationResult<bool>.Failure("A category already exists.");
        }

        // Add the category to the database
        try
        {

            var categoryToAdd = new Entities.ForumCategory
            {
                ForumCategoryId = Guid.NewGuid(),
                Name = category.Name,
                Description = category.Description,
            };

            // Add the category to the database context
            await _context.ForumCategories.AddAsync(categoryToAdd);
            await _context.SaveChangesAsync();

            return OperationResult<bool>.Success(true);
        }
        catch (InvalidOperationException ex)
        {
            return OperationResult<bool>.Failure($"Failed to add category: {ex.Message}");
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.Failure($"An error occurred while adding the category: {ex.Message}");
        }

    }

    public async Task<OperationResult<ForumCategory?>> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            return OperationResult<ForumCategory?>.Failure("Invalid category ID.");

        var category = await _context.ForumCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.ForumCategoryId == id);

        if (category == null)
            return OperationResult<ForumCategory?>.Failure("Category not found.");

        var result = new ForumCategory
        {
            ForumCategoryId = category.ForumCategoryId,
            Name = category.Name,
            Description = category.Description
        };

        return OperationResult<ForumCategory?>.Success(result);
    }

    public async Task<OperationResult<ForumCategory?>> GetByNameAsync(string name)
    {
        if (name == String.Empty)
            return OperationResult<ForumCategory?>.Failure("Name should be not empty.");

        var category = await _context.ForumCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Name == name);

        if (category == null)
            return OperationResult<ForumCategory?>.Failure("Category not found.");

        var result = new ForumCategory
        {
            ForumCategoryId = category.ForumCategoryId,
            Name = category.Name,
            Description = category.Description
        };

        return OperationResult<ForumCategory?>.Success(result);
    }

    public async Task<OperationResult<IEnumerable<ForumCategory>>> GetAllAsync()
    {
        var categories = await _context.ForumCategories
            .AsNoTracking()
            .ToListAsync();

        if (categories.Count == 0)
            return OperationResult<IEnumerable<ForumCategory>>.Failure("No categories were found.");

        var result = categories.Select(c => new ForumCategory
        {
            ForumCategoryId = c.ForumCategoryId,
            Name = c.Name,
            Description = c.Description
        });

        return OperationResult<IEnumerable<ForumCategory>>.Success(result);
    }



    public async Task<OperationResult<bool>> DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
            return OperationResult<bool>.Failure("Invalid category ID.");

        var category = await _context.ForumCategories.FindAsync(id);
        if (category is null)
            return OperationResult<bool>.Failure("Category not found.");

        _context.ForumCategories.Remove(category);
        await _context.SaveChangesAsync();

        return OperationResult<bool>.Success(true);
    }

}
