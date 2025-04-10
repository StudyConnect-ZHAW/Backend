using Microsoft.EntityFrameworkCore;
using StudyConnect.Data.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudyConnect.Data.Repositories;

namespace StudyConnect.Data.Tests.Unit;

public class ForumCategoryRepositoryTests : IDisposable
{
    private readonly DbContextOptions<StudyConnectDbContext> _options;
    private readonly StudyConnectDbContext _context;
    private readonly ForumCategoryRepository _repository;
    private readonly IConfiguration _configuration;
    private bool _disposed = false;

    public ForumCategoryRepositoryTests()
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
        _repository = new ForumCategoryRepository(_context);
    }

    /// <summary>
    /// Disposes the resources used by the <see cref="ForumCategoryRepositoryTests"/> class.
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
    ~ForumCategoryRepositoryTests()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    [Fact]
    public async Task AddAsync_AddsForumCategory()
    {
        // Arrange
        var forumCategory = new ForumCategory { ForumCategoryId = Guid.NewGuid(), Name = "Test Category", Description = "Test Description"};

        // Act
        await _repository.AddAsync(forumCategory);

        // Assert
        using (var context = new StudyConnectDbContext(_options, _configuration))
        {
            var addedForumCategory = await context.ForumCategories.FirstOrDefaultAsync(c => c.Name == "Test Category");
            Assert.NotNull(addedForumCategory);
            Assert.Equal("Test Category", addedForumCategory.Name);
        }
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsForumCategory()
    {
        // Arrange
        var forumCategory = new ForumCategory { ForumCategoryId = Guid.NewGuid(), Name = "Test Category", Description = "Test Description"};
        _context.ForumCategories.Add(forumCategory);
        await _context.SaveChangesAsync();

        // Act
        var retrievedForumCategory = await _repository.GetByIdAsync(forumCategory.ForumCategoryId);

        // Assert
        Assert.NotNull(retrievedForumCategory);
        Assert.Equal("Test Category", retrievedForumCategory.Name);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllForumCategories()
    {
        // Arrange
        var forumCategory1 = new ForumCategory { ForumCategoryId = Guid.NewGuid(), Name = "Test Category 1", Description = "Test Description 1"};
        var forumCategory2 = new ForumCategory { ForumCategoryId = Guid.NewGuid(), Name = "Test Category 2", Description = "Test Description 2"};
        _context.ForumCategories.AddRange(forumCategory1, forumCategory2);
        await _context.SaveChangesAsync();

        // Act
        var forumCategories = await _repository.GetAllAsync();

        // Assert
        Assert.NotNull(forumCategories);
        Assert.Equal(2, forumCategories.Count());
    }

    [Fact]
    public async Task UpdateAsync_UpdatesForumCategory()
    {
        // Arrange
        var forumCategory = new ForumCategory { ForumCategoryId = Guid.NewGuid(), Name = "Test Category", Description = "Test Description"};
        _context.ForumCategories.Add(forumCategory);
        await _context.SaveChangesAsync();

        // Act
        forumCategory.Name = "Updated Test Category";
        await _repository.UpdateAsync(forumCategory);

        // Assert
        using (var context = new StudyConnectDbContext(_options, _configuration))
        {
            var updatedForumCategory = await context.ForumCategories.FirstOrDefaultAsync(c => c.ForumCategoryId == forumCategory.ForumCategoryId);
            Assert.NotNull(updatedForumCategory);
            Assert.Equal("Updated Test Category", updatedForumCategory.Name);
        }
    }

    [Fact]
    public async Task DeleteAsync_DeletesForumCategory()
    {
        // Arrange
        var forumCategory = new ForumCategory { ForumCategoryId = Guid.NewGuid(), Name = "Test Category", Description = "Test Description"};
        _context.ForumCategories.Add(forumCategory);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(forumCategory);

        // Assert
        using (var context = new StudyConnectDbContext(_options, _configuration))
        {
            var deletedForumCategory = await context.ForumCategories.FirstOrDefaultAsync(c => c.ForumCategoryId == forumCategory.ForumCategoryId);
            Assert.Null(deletedForumCategory);
        }
    }
}