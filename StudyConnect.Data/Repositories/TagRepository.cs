using StudyConnect.Core.Models;
using Microsoft.EntityFrameworkCore;
using StudyConnect.Core.Interfaces;
using StudyConnect.Core.Common;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace StudyConnect.Data.Repositories
{
    /// <summary>
    /// Provides data access methods for working with tags in the database.
    /// </summary>
    public class TagRepository : BaseRepository, ITagRepository
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="TagRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used for data access.</param>
        public TagRepository(StudyConnectDbContext context) : base(context)
        {

        }

        public async Task<OperationResult<Tag>> AddTagAsync(Tag tag)
        {
            try
            {
                var tagToAdd = new Entities.Tag
                {
                    Name = tag.Name,
                    Description = tag.Description
                };

                _context.Tags.Add(tagToAdd);
                await _context.SaveChangesAsync();

                return OperationResult<Tag>.Success(tag);
            }
            catch (Exception ex)
            {
                return OperationResult<Tag>.Failure($"Failed to add tag: {ex.Message}");
            }
        }


        /// <summary>
        /// Retrieves all tags from the database. <see cref="Tag"/> objects.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, contains a collection of tags.</returns>
        public async Task<OperationResult<IEnumerable<Tag>>> GetAllTagsAsync()
        {

            var tags = await _context.Tags
            .AsNoTracking()
            .Select(t => new Tag
            {
                TagId = t.TagId,
                Name = t.Name,
                Description = t.Description ?? string.Empty
            })
            .ToListAsync();

            return OperationResult<IEnumerable<Tag>>.Success(tags);

        }

        public async Task<OperationResult<Tag>> GetTagByIdAsync(Guid tagId)
        {
            try
            {
                var tag = await _context.Tags
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.TagId == tagId);

                if (tag == null)
                {
                    return OperationResult<Tag>.Failure("Tag not found.");
                }

                var tagToModel = new Tag
                {
                    TagId = tag.TagId,
                    Name = tag.Name,
                    Description = tag.Description
                };                
                return OperationResult<Tag>.Success(tagToModel);
            }
            catch (Exception ex)
            {
                return OperationResult<Tag>.Failure($"Failed to retrieve tag: {ex.Message}");
            }
        }
    }

}

