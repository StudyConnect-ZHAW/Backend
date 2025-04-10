using Microsoft.EntityFrameworkCore;
using StudyConnect.Data.Entities;
using StudyConnect.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudyConnect.Data.Repositories
{
    public class MemberRoleRepository : IMemberRoleRepository
    {
        private readonly StudyConnectDbContext _context;

        public MemberRoleRepository(StudyConnectDbContext context)
        {
            _context = context;
        }

        public async Task<MemberRole?> GetByIdAsync(Guid id)
        {
            return await _context.MemberRoles.FindAsync(id);
        }

        public async Task<IEnumerable<MemberRole>> GetAllAsync()
        {
            return await _context.MemberRoles.ToListAsync();
        }

        public async Task AddAsync(MemberRole entity)
        {
            await _context.MemberRoles.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MemberRole entity)
        {
            _context.MemberRoles.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(MemberRole entity)
        {
            _context.MemberRoles.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}