using Moq;
using StudyConnect.API.Dtos.Requests.Forum;
using StudyConnect.Core.Interfaces.Services;
using StudyConnect.API.Controllers.Forum;
using Microsoft.AspNetCore.Mvc;
using StudyConnect.Core.Models;
using StudyConnect.API.Dtos.Responses.Forum;
using StudyConnect.Core.Common;
using static StudyConnect.Core.Common.ErrorMessages;

namespace StudyConnect.API.Tests.Mock;

public class CategoryControllerTests
{
    private readonly Mock<ICategoryService> _mockRepo;
    private readonly CategoryController _controller;

    public CategoryControllerTests()
    {
        _mockRepo = new Mock<ICategoryService>();
        _controller = new CategoryController(_mockRepo.Object);
    }

    [Fact]
    public void AddCategory_ReturnNotImplemented()
    {
        var dummyDto = new CategoryCreateDto
        {
            Name = "Test",
            Description = "testing"
        };

        var result = _controller.AddCategory(dummyDto);

        var statusResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(501, statusResult.StatusCode);
    }

    [Fact]
    public async Task GetCategoryById_ReturnsOk_WhenCategoryExists()
    {
        var testCategory = new ForumCategory
        {
            ForumCategoryId = Guid.NewGuid(),
            Name = "Test",
            Description = "testing"
        };

        _mockRepo.Setup(repo => repo.GetCategoryByIdAsync(It.IsAny<Guid>()))
                 .ReturnsAsync(OperationResult<ForumCategory>.Success(testCategory));

        // Act
        var result = await _controller.GetCategoryById(Guid.NewGuid());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<CategoryReadDto>(okResult.Value);
        Assert.Equal(testCategory.Name, returnValue.Name);
    }

    [Fact]
    public async Task GetCategoryById_ReturnsBadRequest_WhenNotFound()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.GetCategoryByIdAsync(It.IsAny<Guid>()))
                 .ReturnsAsync(OperationResult<ForumCategory>.Failure(CategoryNotFound));

        // Act
        var result = await _controller.GetCategoryById(Guid.NewGuid());

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Category not found.", badRequest.Value);
    }

    [Fact]
    public async Task GetAllCategories_ReturnsOk_WhenCategoriesExist()
    {
        // Arrange
        var categories = new List<ForumCategory>
            {
                new ForumCategory { ForumCategoryId = Guid.NewGuid(), Name = "Category1", Description = "Desc1" },
                new ForumCategory { ForumCategoryId = Guid.NewGuid(), Name = "Category2", Description = "Desc2" }
            };

        _mockRepo.Setup(repo => repo.GetAllCategoriesAsync())
             .ReturnsAsync(OperationResult<IEnumerable<ForumCategory?>>.Success(categories));

        // Act
        var result = await _controller.GetAllCategories();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<CategoryReadDto>>(okResult.Value);
        Assert.Collection(returnValue,
            item => Assert.Equal("Category1", item.Name),
            item => Assert.Equal("Category2", item.Name));
    }

    [Fact]
    public void DeleteCategory_ReturnNotImplemented()
    {
        var dummyId = Guid.NewGuid();

        var result = _controller.DeleteCategory(dummyId);

        var statusResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(501, statusResult.StatusCode);
    }

    [Fact]
    public async Task GetAllCategories_ReturnsBadRequest_WhenError()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.GetAllCategoriesAsync())
                 .ReturnsAsync(OperationResult<IEnumerable<ForumCategory?>>.Failure("Something went wrong"));

        // Act
        var result = await _controller.GetAllCategories();

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Something went wrong", badRequest.Value);
    }
}
