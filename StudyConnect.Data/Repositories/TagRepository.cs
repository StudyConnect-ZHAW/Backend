using StudyConnect.Core.Models;
using StudyConnect.API.Dtos.Responses;
using Microsoft.EntityFrameworkCore;

namespace StudyConnect.Data.Repositories
{
    /// <summary>
    /// Provides data access methods for working with tags in the database.
    /// </summary>
    public class TagRepository
    {
        private readonly StudyConnectDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="TagRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used for data access.</param>
        public TagRepository(StudyConnectDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all tags from the database. <see cref="TagDto"/> objects.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, contains a collection of tags.</returns>
        public async Task<IEnumerable<TagDto>> GetAllTagsAsync()
        {

            return await _context.Tags
            .AsNoTracking()
            .Select(t => new TagDto
            {
                Id = t.TagId,
                Name = t.Name,
                Description = t.Description ?? string.Empty
            })
            .ToListAsync();

        }

    }
}
