using Microsoft.EntityFrameworkCore;
using StudyConnect.Data.Entities;
using StudyConnect.Data.Interfaces;

namespace StudyConnect.Data.Repositories
{
    public class ForumCategoryRepository : IForumCategoryRepository
    {
        private readonly StudyConnectDbContext _context;

        public ForumCategoryRepository(StudyConnectDbContext context)
        {
            _context = context;
        }

        public async Task<ForumCategory?> GetByIdAsync(Guid id)
        {
            return await _context.ForumCategories.FindAsync(id);
        }

        public async Task<IEnumerable<ForumCategory>> GetAllAsync()
        {
            return await _context.ForumCategories.ToListAsync();
        }

        public async Task AddAsync(ForumCategory entity)
        {
            await _context.ForumCategories.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ForumCategory entity)
        {
            _context.ForumCategories.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ForumCategory entity)
        {
            _context.ForumCategories.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}