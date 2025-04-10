using Microsoft.EntityFrameworkCore;
using StudyConnect.Data.Entities;
using StudyConnect.Data.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace StudyConnect.Data.Tests.Unit;

public class GroupRepositoryTests : IDisposable
{
    private readonly DbContextOptions<StudyConnectDbContext> _options;
    private readonly StudyConnectDbContext _context;
    private readonly GroupRepository _repository;
    private readonly IConfiguration _configuration;
    private bool _disposed = false;

    public GroupRepositoryTests()
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
        _repository = new GroupRepository(_context);
    }

    /// <summary>
    /// Disposes the resources used by the <see cref="GroupRepositoryTests"/> class.
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
    ~GroupRepositoryTests()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    [Fact]
    public async Task AddAsync_AddsGroup()
    {
        // Arrange
        var group = new Group { GroupId = Guid.NewGuid(), Name = "Test Group", Description = "Test Description", CreatedAt = DateTime.UtcNow };

        // Act
        await _repository.AddAsync(group);

        // Assert
        using (var context = new StudyConnectDbContext(_options, _configuration))
        {
            var addedGroup = await context.Groups.FirstOrDefaultAsync(c => c.Name == "Test Group");
            Assert.NotNull(addedGroup);
            Assert.Equal("Test Group", addedGroup.Name);
        }
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsGroup()
    {
        // Arrange
        var group = new Group { GroupId = Guid.NewGuid(), Name = "Test Group", Description = "Test Description", CreatedAt = DateTime.UtcNow };
        _context.Groups.Add(group);
        await _context.SaveChangesAsync();

        // Act
        var retrievedGroup = await _repository.GetByIdAsync(group.GroupId);

        // Assert
        Assert.NotNull(retrievedGroup);
        Assert.Equal("Test Group", retrievedGroup.Name);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllGroups()
    {
        // Arrange
        var group1 = new Group { GroupId = Guid.NewGuid(), Name = "Test Group 1", Description = "Test Description 1", CreatedAt = DateTime.UtcNow };
        var group2 = new Group { GroupId = Guid.NewGuid(), Name = "Test Group 2", Description = "Test Description 2", CreatedAt = DateTime.UtcNow };
        _context.Groups.AddRange(group1, group2);
        await _context.SaveChangesAsync();

        // Act
        var groups = await _repository.GetAllAsync();

        // Assert
        Assert.NotNull(groups);
        Assert.Equal(2, groups.Count());
    }

    [Fact]
    public async Task UpdateAsync_UpdatesGroup()
    {
        // Arrange
        var group = new Group { GroupId = Guid.NewGuid(), Name = "Test Group", Description = "Test Description", CreatedAt = DateTime.UtcNow };
        _context.Groups.Add(group);
        await _context.SaveChangesAsync();

        // Act
        group.Name = "Updated Test Group";
        await _repository.UpdateAsync(group);

        // Assert
        using (var context = new StudyConnectDbContext(_options, _configuration))
        {
            var updatedGroup = await context.Groups.FirstOrDefaultAsync(c => c.GroupId == group.GroupId);
            Assert.NotNull(updatedGroup);
            Assert.Equal("Updated Test Group", updatedGroup.Name);
        }
    }

    [Fact]
    public async Task DeleteAsync_DeletesGroup()
    {
        // Arrange
        var group = new Group { GroupId = Guid.NewGuid(), Name = "Test Group", Description = "Test Description", CreatedAt = DateTime.UtcNow };
        _context.Groups.Add(group);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(group);

        // Assert
        using (var context = new StudyConnectDbContext(_options, _configuration))
        {
            var deletedGroup = await context.Groups.FirstOrDefaultAsync(c => c.GroupId == group.GroupId);
            Assert.Null(deletedGroup);
        }
    }
}