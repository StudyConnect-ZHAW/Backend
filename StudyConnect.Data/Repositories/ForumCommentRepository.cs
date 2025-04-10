using Microsoft.EntityFrameworkCore;
using StudyConnect.Data.Entities;
using StudyConnect.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudyConnect.Data.Repositories
{
    public class ForumCommentRepository : IForumCommentRepository
    {
        private readonly StudyConnectDbContext _context;

        public ForumCommentRepository(StudyConnectDbContext context)
        {
            _context = context;
        }

        public async Task<ForumComment?> GetByIdAsync(Guid id)
        {
            return await _context.ForumComments.FindAsync(id);
        }

        public async Task<IEnumerable<ForumComment>> GetAllAsync()
        {
            return await _context.ForumComments.ToListAsync();
        }

        public async Task AddAsync(ForumComment entity)
        {
            await _context.ForumComments.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ForumComment entity)
        {
            _context.ForumComments.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ForumComment entity)
        {
            _context.ForumComments.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}