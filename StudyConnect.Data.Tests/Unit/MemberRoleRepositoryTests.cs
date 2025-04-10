using Microsoft.EntityFrameworkCore;
using Xunit;
using StudyConnect.Data;
using StudyConnect.Data.Entities;
using StudyConnect.Data.Interfaces;
using StudyConnect.Data.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace StudyConnect.Data.Tests.Unit;

public class MemberRoleRepositoryTests : IDisposable
{
    private readonly DbContextOptions<StudyConnectDbContext> _options;
    private readonly StudyConnectDbContext _context;
    private readonly MemberRoleRepository _repository;
    private readonly IConfiguration _configuration;
    private bool _disposed = false;

    public MemberRoleRepositoryTests()
    {
        // Build configuration
        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .Build());

        var serviceProvider = services.BuildServiceProvider();
        _configuration = serviceProvider.GetService<IConfiguration>() ?? throw new InvalidOperationException("Unable to resolve IConfiguration");

        // Use a unique in-memory database for each test
        _options = new DbContextOptionsBuilder<StudyConnectDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new StudyConnectDbContext(_options, _configuration);
        _context.Database.EnsureCreated();
        _repository = new MemberRoleRepository(_context);
    }

    /// <summary>
    /// Disposes the resources used by the <see cref="MemberRoleRepositoryTests"/> class.
    /// </summary>
    /// <param name="disposing">True if called from <see cref="Dispose()"/> method, false if called from finalizer.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Dispose managed resources.
                _context.Database.EnsureDeleted();
                _context.Dispose();
            }

            // Dispose unmanaged resources (if any).

            _disposed = true;
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    ~MemberRoleRepositoryTests()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    [Fact]
    public async Task AddAsync_AddsMemberRole()
    {
        // Arrange
        var memberRole = new MemberRole { MemberRoleId = Guid.NewGuid(), Name = "Test MemberRole", Description = "Test Description"};

        // Act
        await _repository.AddAsync(memberRole);

        // Assert
        using (var context = new StudyConnectDbContext(_options, _configuration))
        {
            var addedMemberRole = await context.MemberRoles.FirstOrDefaultAsync(c => c.Name == "Test MemberRole");
            Assert.NotNull(addedMemberRole);
            Assert.Equal("Test MemberRole", addedMemberRole.Name);
        }
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsMemberRole()
    {
        // Arrange
        var memberRole = new MemberRole { MemberRoleId = Guid.NewGuid(), Name = "Test MemberRole", Description = "Test Description"};
        _context.MemberRoles.Add(memberRole);
        await _context.SaveChangesAsync();

        // Act
        var retrievedMemberRole = await _repository.GetByIdAsync(memberRole.MemberRoleId);

        // Assert
        Assert.NotNull(retrievedMemberRole);
        Assert.Equal("Test MemberRole", retrievedMemberRole.Name);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllMemberRoles()
    {
        // Arrange
        var memberRole1 = new MemberRole { MemberRoleId = Guid.NewGuid(), Name = "Test MemberRole 1", Description = "Test Description 1" };
        var memberRole2 = new MemberRole { MemberRoleId = Guid.NewGuid(), Name = "Test MemberRole 2", Description = "Test Description 2" };
        _context.MemberRoles.AddRange(memberRole1, memberRole2);
        await _context.SaveChangesAsync();

        // Act
        var memberRoles = await _repository.GetAllAsync();

        // Assert
        Assert.NotNull(memberRoles);
        Assert.Equal(2, memberRoles.Count());
    }

    [Fact]
    public async Task UpdateAsync_UpdatesMemberRole()
    {
        // Arrange
        var memberRole = new MemberRole { MemberRoleId = Guid.NewGuid(), Name = "Test MemberRole", Description = "Test Description" };
        _context.MemberRoles.Add(memberRole);
        await _context.SaveChangesAsync();

        // Act
        memberRole.Name = "Updated Test MemberRole";
        await _repository.UpdateAsync(memberRole);

        // Assert
        using (var context = new StudyConnectDbContext(_options, _configuration))
        {
            var updatedMemberRole = await context.MemberRoles.FirstOrDefaultAsync(c => c.MemberRoleId == memberRole.MemberRoleId);
            Assert.NotNull(updatedMemberRole);
            Assert.Equal("Updated Test MemberRole", updatedMemberRole.Name);
        }
    }

    [Fact]
    public async Task DeleteAsync_DeletesMemberRole()
    {
        // Arrange
        var memberRole = new MemberRole { MemberRoleId = Guid.NewGuid(), Name = "Test MemberRole", Description = "Test Description" };
        _context.MemberRoles.Add(memberRole);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(memberRole);

        // Assert
        using (var context = new StudyConnectDbContext(_options, _configuration))
        {
            var deletedMemberRole = await context.MemberRoles.FirstOrDefaultAsync(c => c.MemberRoleId == memberRole.MemberRoleId);
            Assert.Null(deletedMemberRole);
        }
    }
}