using StudyConnect.Core.Models;
using StudyConnect.API.Dtos.Responses;
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

        public async Task<IEnumerable<TagDto>> GetAllTagsAsync()
        {

            return await _context.Tags
            .AsNoTracking()
            .Select(t => new TagDto
                {
                    Id = t.TagId,
                    Name = t.Name,
                    Description = t.Description
                })
            .ToListAsync();
          
        }
        
    }
}
