using Microsoft.EntityFrameworkCore;
using StudyConnect.Core.Interfaces;
using StudyConnect.Core.Models;
using StudyConnect.Core.Common;

namespace StudyConnect.Data.Repositories;

public class CommentRepository : BaseRepository, ICommentRepository
{
    public CommentRepository(StudyConnectDbContext context) : base(context)
    {
        
    }

    public Task<OperationResult<ForumComment?>> AddAsync (Guid userId, Guid parentId, ForumComment comment)
    {
        throw new NotImplementedException();
    }

    public Task<OperationResult<bool>> DeleteAsync(Guid commentId)
    {
        throw new NotImplementedException();
    }

    public Task<OperationResult<IEnumerable<ForumComment>?>> GetAllofPostAsync(Guid postId)
    {
        throw new NotImplementedException();
    }

    public Task<OperationResult<ForumComment?>> GetByIdAsync(Guid commentId)
    {
        throw new NotImplementedException();
    }

    public Task<OperationResult<ForumComment?>> UpdateAsync(Guid commentId, ForumComment comment)
    {
        throw new NotImplementedException();
    }
}

