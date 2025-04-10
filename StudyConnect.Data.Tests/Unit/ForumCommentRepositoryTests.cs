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

public class ForumCommentRepositoryTests : IDisposable
{
    private readonly DbContextOptions<StudyConnectDbContext> _options;
    private readonly StudyConnectDbContext _context;
    private readonly ForumCommentRepository _repository;
    private readonly IConfiguration _configuration;
    private bool _disposed = false;

    public ForumCommentRepositoryTests()
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
        _repository = new ForumCommentRepository(_context);
    }

    /// <summary>
    /// Disposes the resources used by the <see cref="ForumCommentRepositoryTests"/> class.
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
    ~ForumCommentRepositoryTests()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    [Fact]
    public async Task AddAsync_AddsForumComment()
    {
        // Arrange
        var forumComment = new ForumComment { ForumCommentId = Guid.NewGuid(), Content = "Test Content", ForumPostId = Guid.NewGuid(), UserGuid = Guid.NewGuid() };

        // Act
        await _repository.AddAsync(forumComment);

        // Assert
        using (var context = new StudyConnectDbContext(_options, _configuration))
        {
            var addedForumComment = await context.ForumComments.FirstOrDefaultAsync(c => c.Content == "Test Content");
            Assert.NotNull(addedForumComment);
            Assert.Equal("Test Content", addedForumComment.Content);
        }
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsForumComment()
    {
        // Arrange
        var forumComment = new ForumComment { ForumCommentId = Guid.NewGuid(), Content = "Test Content", ForumPostId = Guid.NewGuid(), UserGuid = Guid.NewGuid() };
        _context.ForumComments.Add(forumComment);
        await _context.SaveChangesAsync();

        // Act
        var retrievedForumComment = await _repository.GetByIdAsync(forumComment.ForumCommentId);

        // Assert
        Assert.NotNull(retrievedForumComment);
        Assert.Equal("Test Content", retrievedForumComment.Content);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllForumComments()
    {
        // Arrange
        var forumComment1 = new ForumComment { ForumCommentId = Guid.NewGuid(), Content = "Test Content 1", ForumPostId = Guid.NewGuid(), UserGuid = Guid.NewGuid() };
        var forumComment2 = new ForumComment { ForumCommentId = Guid.NewGuid(), Content = "Test Content 2", ForumPostId = Guid.NewGuid(), UserGuid = Guid.NewGuid() };
        _context.ForumComments.AddRange(forumComment1, forumComment2);
        await _context.SaveChangesAsync();

        // Act
        var forumComments = await _repository.GetAllAsync();

        // Assert
        Assert.NotNull(forumComments);
        Assert.Equal(2, forumComments.Count());
    }

    [Fact]
    public async Task UpdateAsync_UpdatesForumComment()
    {
        // Arrange
        var forumComment = new ForumComment { ForumCommentId = Guid.NewGuid(), Content = "Test Content", ForumPostId = Guid.NewGuid(), UserGuid = Guid.NewGuid() };
        _context.ForumComments.Add(forumComment);
        await _context.SaveChangesAsync();

        // Act
        forumComment.Content = "Updated Test Content";
        await _repository.UpdateAsync(forumComment);

        // Assert
        using (var context = new StudyConnectDbContext(_options, _configuration))
        {
            var updatedForumComment = await context.ForumComments.FirstOrDefaultAsync(c => c.ForumCommentId == forumComment.ForumCommentId);
            Assert.NotNull(updatedForumComment);
            Assert.Equal("Updated Test Content", updatedForumComment.Content);
        }
    }

    [Fact]
    public async Task DeleteAsync_DeletesForumComment()
    {
        // Arrange
        var forumComment = new ForumComment { ForumCommentId = Guid.NewGuid(), Content = "Test Content", ForumPostId = Guid.NewGuid(), UserGuid = Guid.NewGuid() };
        _context.ForumComments.Add(forumComment);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(forumComment);

        // Assert
        using (var context = new StudyConnectDbContext(_options, _configuration))
        {
            var deletedForumComment = await context.ForumComments.FirstOrDefaultAsync(c => c.ForumCommentId == forumComment.ForumCommentId);
            Assert.Null(deletedForumComment);
        }
    }
}