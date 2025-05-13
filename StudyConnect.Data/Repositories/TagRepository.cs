using StudyConnect.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace StudyConnect.Data.Repositories
{
    public class TagRepository
    {
        private readonly StudyConnectDbContext _context;

        public TagRepository(StudyConnectDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tag>> GetAllTagsAsync()
        {

            return await _context.Tags
            .AsNoTracking()
            .ToListAsync();
          
        }
        
    }
}
