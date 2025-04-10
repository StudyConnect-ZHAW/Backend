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

public class GroupMembersRepositoryTests : IDisposable
{
    private readonly DbContextOptions<StudyConnectDbContext> _options;
    private readonly StudyConnectDbContext _context;
    private readonly GroupMembersRepository _repository;
    private readonly IConfiguration _configuration;
    private bool _disposed = false;

    public GroupMembersRepositoryTests()
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
        _repository = new GroupMembersRepository(_context);
    }

    /// <summary>
    /// Disposes the resources used by the <see cref="GroupMembersRepositoryTests"/> class.
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
    ~GroupMembersRepositoryTests()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    [Fact]
    public async Task AddAsync_AddsGroupMembers()
    {
        // Arrange
        var groupMembers = new GroupMembers { GroupMemberId = Guid.NewGuid(), GroupId = Guid.NewGuid(), UserGuid = Guid.NewGuid(), MemberRoleId = Guid.NewGuid() };

        // Act
        await _repository.AddAsync(groupMembers);

        // Assert
        using (var context = new StudyConnectDbContext(_options, _configuration))
        {
            var addedGroupMembers = await context.GroupMembers.FirstOrDefaultAsync(c => c.GroupMemberId == groupMembers.GroupMemberId);
            Assert.NotNull(addedGroupMembers);
            Assert.Equal(groupMembers.GroupMemberId, addedGroupMembers.GroupMemberId);
        }
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsGroupMembers()
    {
        // Arrange
        var groupMembers = new GroupMembers { GroupMemberId = Guid.NewGuid(), GroupId = Guid.NewGuid(), UserGuid = Guid.NewGuid(), MemberRoleId = Guid.NewGuid()};
        _context.GroupMembers.Add(groupMembers);
        await _context.SaveChangesAsync();

        // Act
        var retrievedGroupMembers = await _repository.GetByIdAsync(groupMembers.GroupMemberId);

        // Assert
        Assert.NotNull(retrievedGroupMembers);
        Assert.Equal(groupMembers.GroupMemberId, retrievedGroupMembers.GroupMemberId);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllGroupMembers()
    {
        // Arrange
        var groupMembers1 = new GroupMembers { GroupMemberId = Guid.NewGuid(), GroupId = Guid.NewGuid(), UserGuid = Guid.NewGuid(), MemberRoleId = Guid.NewGuid() };
        var groupMembers2 = new GroupMembers { GroupMemberId = Guid.NewGuid(), GroupId = Guid.NewGuid(), UserGuid = Guid.NewGuid(), MemberRoleId = Guid.NewGuid() };
        _context.GroupMembers.AddRange(groupMembers1, groupMembers2);
        await _context.SaveChangesAsync();

        // Act
        var groupMembers = await _repository.GetAllAsync();

        // Assert
        Assert.NotNull(groupMembers);
        Assert.Equal(2, groupMembers.Count());
    }

    [Fact]
    public async Task UpdateAsync_UpdatesGroupMembers()
    {
        // Arrange
        var groupMembers = new GroupMembers { GroupMemberId = Guid.NewGuid(), GroupId = Guid.NewGuid(), UserGuid = Guid.NewGuid(), MemberRoleId = Guid.NewGuid()};
        _context.GroupMembers.Add(groupMembers);
        await _context.SaveChangesAsync();

        // Act
        var newGroupId = Guid.NewGuid();
        groupMembers.GroupId = newGroupId;
        await _repository.UpdateAsync(groupMembers);

        // Assert
        using (var context = new StudyConnectDbContext(_options, _configuration))
        {
            var updatedGroupMembers = await context.GroupMembers.FirstOrDefaultAsync(c => c.GroupId == newGroupId);
            Assert.NotNull(updatedGroupMembers);
            Assert.Equal(newGroupId, updatedGroupMembers.GroupId);
        }
    }

    [Fact]
    public async Task DeleteAsync_DeletesGroupMembers()
    {
        // Arrange
        var groupMembers = new GroupMembers { GroupMemberId = Guid.NewGuid(), GroupId = Guid.NewGuid(), UserGuid = Guid.NewGuid(), MemberRoleId = Guid.NewGuid()};
        _context.GroupMembers.Add(groupMembers);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(groupMembers);

        // Assert
        using (var context = new StudyConnectDbContext(_options, _configuration))
        {
            var deletedGroupMembers = await context.GroupMembers.FirstOrDefaultAsync(c => c.GroupMemberId == groupMembers.GroupMemberId);
            Assert.Null(deletedGroupMembers);
        }
    }
}