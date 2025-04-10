using Microsoft.EntityFrameworkCore;
using StudyConnect.Data.Entities;
using StudyConnect.Data.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace StudyConnect.Data.Tests.Unit;

public class ForumPostRepositoryTests : IDisposable
{
    private readonly DbContextOptions<StudyConnectDbContext> _options;
    private readonly StudyConnectDbContext _context;
    private readonly ForumPostRepository _repository;
    private readonly IConfiguration _configuration;
    private bool _disposed = false;

    public ForumPostRepositoryTests()
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
        _repository = new ForumPostRepository(_context);
    }

    /// <summary>
    /// Disposes the resources used by the <see cref="ForumPostRepositoryTests"/> class.
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
    ~ForumPostRepositoryTests()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    [Fact]
    public async Task AddAsync_AddsForumPost()
    {
        // Arrange
        var forumPost = new ForumPost { ForumPostId = Guid.NewGuid(), Title = "Test Title", Content = "Test Content", ForumCategoryId = Guid.NewGuid(), UserGuid = Guid.NewGuid() };

        // Act
        await _repository.AddAsync(forumPost);

        // Assert
        using (var context = new StudyConnectDbContext(_options, _configuration))
        {
            var addedForumPost = await context.ForumPosts.FirstOrDefaultAsync(c => c.Title == "Test Title");
            Assert.NotNull(addedForumPost);
            Assert.Equal("Test Title", addedForumPost.Title);
        }
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsForumPost()
    {
        // Arrange
        var forumPost = new ForumPost { ForumPostId = Guid.NewGuid(), Title = "Test Title", Content = "Test Content", ForumCategoryId = Guid.NewGuid(), UserGuid = Guid.NewGuid() };
        _context.ForumPosts.Add(forumPost);
        await _context.SaveChangesAsync();

        // Act
        var retrievedForumPost = await _repository.GetByIdAsync(forumPost.ForumPostId);

        // Assert
        Assert.NotNull(retrievedForumPost);
        Assert.Equal("Test Title", retrievedForumPost.Title);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllForumPosts()
    {
        // Arrange
        var forumPost1 = new ForumPost { ForumPostId = Guid.NewGuid(), Title = "Test Title 1", Content = "Test Content 1", ForumCategoryId = Guid.NewGuid(), UserGuid = Guid.NewGuid() };
        var forumPost2 = new ForumPost { ForumPostId = Guid.NewGuid(), Title = "Test Title 2", Content = "Test Content 2", ForumCategoryId = Guid.NewGuid(), UserGuid = Guid.NewGuid() };
        _context.ForumPosts.AddRange(forumPost1, forumPost2);
        await _context.SaveChangesAsync();

        // Act
        var forumPosts = await _repository.GetAllAsync();

        // Assert
        Assert.NotNull(forumPosts);
        Assert.Equal(2, forumPosts.Count());
    }

    [Fact]
    public async Task UpdateAsync_UpdatesForumPost()
    {
        // Arrange
        var forumPost = new ForumPost { ForumPostId = Guid.NewGuid(), Title = "Test Title", Content = "Test Content", ForumCategoryId = Guid.NewGuid(), UserGuid = Guid.NewGuid() };
        _context.ForumPosts.Add(forumPost);
        await _context.SaveChangesAsync();

        // Act
        forumPost.Title = "Updated Test Title";
        await _repository.UpdateAsync(forumPost);

        // Assert
        using (var context = new StudyConnectDbContext(_options, _configuration))
        {
            var updatedForumPost = await context.ForumPosts.FirstOrDefaultAsync(c => c.ForumPostId == forumPost.ForumPostId);
            Assert.NotNull(updatedForumPost);
            Assert.Equal("Updated Test Title", updatedForumPost.Title);
        }
    }

    [Fact]
    public async Task DeleteAsync_DeletesForumPost()
    {
        // Arrange
        var forumPost = new ForumPost { ForumPostId = Guid.NewGuid(), Title = "Test Title", Content = "Test Content", ForumCategoryId = Guid.NewGuid(), UserGuid = Guid.NewGuid() };
        _context.ForumPosts.Add(forumPost);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(forumPost);

        // Assert
        using (var context = new StudyConnectDbContext(_options, _configuration))
        {
            var deletedForumPost = await context.ForumPosts.FirstOrDefaultAsync(c => c.ForumPostId == forumPost.ForumPostId);
            Assert.Null(deletedForumPost);
        }
    }
}