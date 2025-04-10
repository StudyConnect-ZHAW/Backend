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

public class UserRepositoryTests : IDisposable
{
    private readonly DbContextOptions<StudyConnectDbContext> _options;
    private readonly StudyConnectDbContext _context;
    private readonly UserRepository _repository;
    private readonly IConfiguration _configuration;
    private bool _disposed = false;

    public UserRepositoryTests()
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

        _context = new StudyConnectDbContext(_options, _configuration );
        _context.Database.EnsureCreated();
        _repository = new UserRepository(_context);
    }

    /// <summary>
    /// Disposes the resources used by the <see cref="UserRepositoryTests"/> class.
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
    ~UserRepositoryTests()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    [Fact]
    public async Task AddAsync_AddsUser()
    {
        // Arrange
        var user = new User { UserGuid = Guid.NewGuid(), FirstName = "Test", LastName = "User", Email = "test@example.com", URole_ID = Guid.NewGuid() };

        // Act
        await _repository.AddAsync(user);

        // Assert
        using (var context = new StudyConnectDbContext(_options, _configuration))
        {
            var addedUser = await context.Users.FirstOrDefaultAsync(c => c.FirstName == "Test");
            Assert.NotNull(addedUser);
            Assert.Equal("Test", addedUser.FirstName);
        }
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsUser()
    {
        // Arrange
        var user = new User { UserGuid = Guid.NewGuid(), FirstName = "Test", LastName = "User", Email = "test@example.com", URole_ID = Guid.NewGuid() };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var retrievedUser = await _repository.GetByIdAsync(user.UserGuid);

        // Assert
        Assert.NotNull(retrievedUser);
        Assert.Equal("Test", retrievedUser.FirstName);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllUsers()
    {
        // Arrange
        var user1 = new User { UserGuid = Guid.NewGuid(), FirstName = "Test1", LastName = "User1", Email = "test1@example.com", URole_ID = Guid.NewGuid() };
        var user2 = new User { UserGuid = Guid.NewGuid(), FirstName = "Test2", LastName = "User2", Email = "test2@example.com", URole_ID = Guid.NewGuid() };
        _context.Users.AddRange(user1, user2);
        await _context.SaveChangesAsync();

        // Act
        var users = await _repository.GetAllAsync();

        // Assert
        Assert.NotNull(users);
        Assert.Equal(2, users.Count());
    }

    [Fact]
    public async Task UpdateAsync_UpdatesUser()
    {
        // Arrange
        var user = new User { UserGuid = Guid.NewGuid(), FirstName = "Test", LastName = "User", Email = "test@example.com", URole_ID = Guid.NewGuid() };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        user.FirstName = "Updated Test";
        await _repository.UpdateAsync(user);

        // Assert
        using (var context = new StudyConnectDbContext(_options, _configuration))
        {
            var updatedUser = await context.Users.FirstOrDefaultAsync(c => c.UserGuid == user.UserGuid);
            Assert.NotNull(updatedUser);
            Assert.Equal("Updated Test", updatedUser.FirstName);
        }
    }

    [Fact]
    public async Task DeleteAsync_DeletesUser()
    {
        // Arrange
        var user = new User { UserGuid = Guid.NewGuid(), FirstName = "Test", LastName = "User", Email = "test@example.com", URole_ID = Guid.NewGuid() };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(user.UserGuid);

        // Assert
        using (var context = new StudyConnectDbContext(_options, _configuration))
        {
            var deletedUser = await context.Users.FirstOrDefaultAsync(c => c.UserGuid == user.UserGuid);
            Assert.Null(deletedUser);
        }
    }
}