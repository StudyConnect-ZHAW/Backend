using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using StudyConnect.Data.Entities;
using StudyConnect.Data.Repositories;

namespace StudyConnect.Data.Tests;

public class CategoryRepositoryTests
{


    [Fact]
    public async Task AddAsync_SholdReturnFailure_WhenCategoryIsNull()
    {
        var options = TestUtils.CreateNewContextOptions();
        var configuration = TestUtils.CreateNewConfiguration();

        using (var context = new StudyConnectDbContext(options, configuration))
        {
            var repo = new CategoryRepository(context);

            var result = await repo.AddAsync(null);

            Assert.False(result.IsSuccess);
            Assert.Equal("Category cannot be null.", result.ErrorMessage);
        }
    }

    [Fact]
    public async Task AddAsync_ShouldReturnFailure_WhenCategoryExists()
    {
        var options = TestUtils.CreateNewContextOptions();
        var configuration = TestUtils.CreateNewConfiguration();

        var id = Guid.NewGuid();
        var category = new ForumCategory
        {
            ForumCategoryId = id,
            Name = "Test",
            Description = "Succes"
        };
        var modelCategory = new StudyConnect.Core.Models.ForumCategory
        {
            ForumCategoryId = id,
            Name = "Test",
            Description = "Succes"
        };

        using (var seedContext = new StudyConnectDbContext(options, configuration))
        {
            seedContext.ForumCategories.Add(category);
            seedContext.SaveChanges();
        }

        using (var testContext = new StudyConnectDbContext(options, configuration))
        {
            var repo = new CategoryRepository(testContext);

            var result = await repo.AddAsync(modelCategory);

            Assert.False(result.IsSuccess);
            Assert.Equal("A category already exists.", result.ErrorMessage);
        }
    }

    [Fact]
    public async Task AddAsync_ShluldReturnSucces_WhenCategoryNotFound()
    {
        var options = TestUtils.CreateNewContextOptions();
        var configuration = TestUtils.CreateNewConfiguration();

        var category = new StudyConnect.Core.Models.ForumCategory
        {
            ForumCategoryId = Guid.NewGuid(),
            Name = "Succes",
            Description = "testing gone right"
        };

        using (var context = new StudyConnectDbContext(options, configuration))
        {
            var repo = new CategoryRepository(context);

            var result = await repo.AddAsync(category);

            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
            Assert.Single(context.ForumCategories);
        }
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnFailure_WhenIdIsEmpty()
    {
        // Arrange
        var options = TestUtils.CreateNewContextOptions();
        var configuration = TestUtils.CreateNewConfiguration();

        using (var context = new StudyConnectDbContext(options, configuration))
        {
            var repo = new CategoryRepository(context);

            //Act
            var result = await repo.GetByIdAsync(Guid.Empty);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Invalid category ID.", result.ErrorMessage);
        }
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnSuccess_WhenCategoryExists()
    {
        // Arrange
        var options = TestUtils.CreateNewContextOptions();
        var configuration = TestUtils.CreateNewConfiguration();

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

        using (var testContext = new StudyConnectDbContext(options, configuration))
        {
            var repository = new CategoryRepository(testContext);

            // Act
            var result = await repository.GetByIdAsync(testCategory.ForumCategoryId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("testCategory", result.Data?.Name);
        }
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnFailure_WhenCategoryNotFound()
    {
        // Arrange
        var options = TestUtils.CreateNewContextOptions();
        var configuration = TestUtils.CreateNewConfiguration();

        using (var context = new StudyConnectDbContext(options, configuration))
        {
            var repo = new CategoryRepository(context);

            // Act
            var nonExsitingCategoryId = Guid.NewGuid();
            var result = await repo.GetByIdAsync(nonExsitingCategoryId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Category not found.", result.ErrorMessage);
        }
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnSuccess_WhenCategoriesExists()
    {
        var options = TestUtils.CreateNewContextOptions();
        var configuration = TestUtils.CreateNewConfiguration();

        var testCategories = new List<ForumCategory>
        {
            new ForumCategory { ForumCategoryId = Guid.NewGuid(), Name = "TestOne", Description = "Test One"},
            new ForumCategory { ForumCategoryId = Guid.NewGuid(), Name = "TestTwo", Description = "Test Two"}
        };

        // seed categories into the memory
        using (var context = new StudyConnectDbContext(options, configuration))
        {
            context.ForumCategories.AddRange(testCategories);
            context.SaveChanges();
        }

        using (var context = new StudyConnectDbContext(options, configuration))
        {
            var repo = new CategoryRepository(context);

            var result = await repo.GetAllAsync();

            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data?.Count());
        }

    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnFailure_WhenNoCategoriesExist()
    {
        var options = TestUtils.CreateNewContextOptions();
        var configuration = TestUtils.CreateNewConfiguration();

        using (var context = new StudyConnectDbContext(options, configuration))
        {
            var repo = new CategoryRepository(context);

            // Act
            var result = await repo.GetAllAsync();

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("No categories were found.", result.ErrorMessage);
        }
    }
}
