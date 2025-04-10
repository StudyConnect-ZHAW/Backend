using Microsoft.EntityFrameworkCore;
using StudyConnect.Data.Entities;
using StudyConnect.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudyConnect.Data.Repositories
{
    public class GroupMembersRepository : IGroupMembersRepository
    {
        private readonly StudyConnectDbContext _context;

        public GroupMembersRepository(StudyConnectDbContext context)
        {
            _context = context;
        }

        public async Task<GroupMembers?> GetByIdAsync(Guid id)
        {
            return await _context.GroupMembers.FindAsync(id);
        }

        public async Task<IEnumerable<GroupMembers>> GetAllAsync()
        {
            return await _context.GroupMembers.ToListAsync();
        }

        public async Task AddAsync(GroupMembers entity)
        {
            await _context.GroupMembers.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(GroupMembers entity)
        {
            _context.GroupMembers.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(GroupMembers entity)
        {
            _context.GroupMembers.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}