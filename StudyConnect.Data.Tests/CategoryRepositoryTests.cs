using Microsoft.EntityFrameworkCore;
using StudyConnect.Data.Entities;
using StudyConnect.Data.Repositories;
using Microsoft.Extensions.Configuration;

namespace StudyConnect.Data.Tests;

public class CategoryRepositoryTests
{
    [Fact]
    public async Task GetByIdAsync_ShouldReturnFailure_WhenIdIsEmpty()
    {
        // Arrange
        var options = CreateNewContextOptions();
        var configuration = CreateNewConfiguration();

        using var context = new StudyConnectDbContext(options, configuration);
        var repo = new CategoryRepository(context);

        //Act
        var result = await repo.GetByIdAsync(Guid.Empty);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Invalid category ID.", result.ErrorMessage);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnSuccess_WhenCategoryExists()
    {
        // Arrange
        var options = CreateNewContextOptions();
        var configuration = CreateNewConfiguration();

        // create category
        var testCategory = new ForumCategory
        {
            ForumCategoryId = Guid.NewGuid(),
            Name = "testCategory",
            Description = "A test For Category"
        };

        // seed category into the memory
        using (var seedContext = new StudyConnectDbContext(options, configuration))
        {
            seedContext.ForumCategories.Add(testCategory);
            seedContext.SaveChanges();
        }

        // Test
        using (var testContext = new StudyConnectDbContext(options, configuration))
        {
            var repository = new CategoryRepository(testContext);
            var result = await repository.GetByIdAsync(testCategory.ForumCategoryId);

            Assert.True(result.IsSuccess);
            Assert.Equal("testCategory", result.Data?.Name);
        }
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnFailure_WhenCategoryNotFound()
    {
        // Arrange
        var options = CreateNewContextOptions();
        var configuration = CreateNewConfiguration();

        var context = new StudyConnectDbContext(options, configuration);
        var repo = new CategoryRepository(context);

        // Act
        var nonExsitingCategoryId = Guid.NewGuid();
        var result = await repo.GetByIdAsync(nonExsitingCategoryId);

        Assert.False(result.IsSuccess);
        Assert.Equal("Category not found.", result.ErrorMessage);


    }

    private DbContextOptions<StudyConnectDbContext> CreateNewContextOptions()
    {
        return new DbContextOptionsBuilder<StudyConnectDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    private IConfiguration CreateNewConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
    }
}
