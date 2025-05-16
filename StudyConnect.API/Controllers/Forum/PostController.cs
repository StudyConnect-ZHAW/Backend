using Microsoft.AspNetCore.Mvc;
using StudyConnect.Core.Interfaces.Services;
using StudyConnect.API.Dtos.Requests.Forum;
using StudyConnect.API.Dtos.Responses.Forum;
using StudyConnect.API.Dtos.Responses.User;
using StudyConnect.Core.Models;
using StudyConnect.API.Dtos;
using static StudyConnect.Core.Common.ErrorMessages;

namespace StudyConnect.API.Controllers.Forum;

/// <summary>
/// Controller for managing the posts
/// Provides endpoints to create, retrieve, update, and delete posts.
/// </summary>
[ApiController]
[Route("api/v1/posts")]
public class PostController : BaseController
{

    protected readonly IPostService _postService;


    public PostController(IPostService postService)
    {
        _postService = postService;
    }


    /// <summary>
    /// Creates a new post.
    /// </summary>
    /// <param name="createDto">A Date Transfer Object containing information for post creating.</param>
    /// <returns>On success a HTTP 200 status code, on failure a HTTP 400 status code.</returns>
    [HttpPost]
    public async Task<IActionResult> AddPost([FromBody] PostCreateDto createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var post = new ForumPost
        {
            Title = createDto.Title,
            Content = createDto.Content != null
                ? createDto.Content
                : ""
        };

        var userId = createDto.UserId;
        var categoryId = createDto.ForumCategoryId;

        var newPost = await _postService.AddPostAsync(userId, categoryId, post);
        if (!newPost.IsSuccess || newPost.Data == null)
            return newPost.ErrorMessage!.Contains(GeneralNotFound)
                ? NotFound(new ApiResponse<string>(newPost.ErrorMessage))
                : BadRequest(new ApiResponse<string>(newPost.ErrorMessage));

        var result = GeneratePostDto(newPost.Data);

        return Ok(new ApiResponse<PostReadDto>(result));
    }

    /// <summary>
    /// Search posts based on provieded queries.
    /// </summary>
    /// <param name="uid">The unique identifier of the post creator.</param>
    /// <param name="categoryName">The unique name of the category the post belongs to.</param>
    /// <param name="title">the post title or substring of post title</param>
    /// <returns>On success a list of Dtoa with information about the post, on failure HTTP 400/404 status code.</returns>
    [HttpGet("search")]
    public async Task<IActionResult> SearchPosts([FromQuery] Guid? uid, [FromQuery] string? categoryName, [FromQuery] string? title, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
    {
        var posts = await _postService.SearchPostAsync(uid, categoryName, title, fromDate, toDate);
        if (!posts.IsSuccess || posts.Data == null)
            return BadRequest(new ApiResponse<string>(posts.ErrorMessage!));

        var result = posts.Data.Select(p => GeneratePostDto(p));

        return Ok(new ApiResponse<IEnumerable<PostReadDto>>(result));
    }

    /// <summary>
    /// Get a post by its ID
    /// </summary>
    /// <param name="pid">The unique identifier of the post.</param>
    /// <returns>On success a Dto with information about the post, on failure HTTP 400/404 status code.</returns>
    [HttpGet("{pid:guid}")]
    public async Task<IActionResult> GetPostById([FromRoute] Guid pid)
    {
        var post = await _postService.GetPostByIdAsync(pid);
        if (!post.IsSuccess || post.Data == null)
            return post.ErrorMessage!.Contains(GeneralNotFound)
                ? NotFound(new ApiResponse<string>(post.ErrorMessage))
                : BadRequest(new ApiResponse<string>(post.ErrorMessage));

        var result = GeneratePostDto(post.Data);
        return Ok(new ApiResponse<PostReadDto>(result));
    }

    /// <summary>
    /// Update existing post
    /// </summary>
    /// <param name="pid"> unique identifier of the post </param>
    /// <param name="updateDto"> a dto containing the data for updating the post. </param>
    /// <returns>On success a HTTP 200 status code, on failure a HTTP 400 status code.</returns>
    [HttpPut("{pid:guid}")]
    public async Task<IActionResult> UpdatePost([FromRoute] Guid pid, [FromBody] PostUpdateDto updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var post = new ForumPost
        {
            Title = updateDto.Title,
            Content = updateDto.Content
        };

        var updatePost = await _postService.UpdatePostAsync(updateDto.UserId, pid, post);
        if (!updatePost.IsSuccess || updatePost.Data == null)
        {
            if (updatePost.ErrorMessage!.Contains(GeneralNotFound)) return NotFound(new ApiResponse<string>(updatePost.ErrorMessage));
            else if (updatePost.ErrorMessage!.Equals(NotAuthorized)) return Unauthorized(new ApiResponse<string>(updatePost.ErrorMessage));
            else return BadRequest(new ApiResponse<string>(updatePost.ErrorMessage));
        }

        var result = GeneratePostDto(updatePost.Data);
        return Ok(new ApiResponse<PostReadDto>(result));
    }


    /// <summary>
    /// Deletes an existing post
    /// </summary>
    /// <param name="pid">The unique identifier of the post.</param>
    /// <returns>On success a HTTP 200 status code, on failure a HTTP 400 status code.</returns>
    [HttpDelete("{pid:guid}")]
    public async Task<IActionResult> DeletePost([FromRoute] Guid pid, [FromQuery] Guid uid)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var deletePost = await _postService.DeletePostAsync(uid, pid);
        if (!deletePost.IsSuccess)
        {
            if (deletePost.ErrorMessage!.Contains(GeneralNotFound)) return NotFound(new ApiResponse<string>(deletePost.ErrorMessage));
            else if (deletePost.ErrorMessage!.Equals(NotAuthorized)) return Unauthorized(new ApiResponse<string>(deletePost.ErrorMessage));
            else return BadRequest(new ApiResponse<string>(deletePost.ErrorMessage));
        }

        return NoContent();
    }


    /// <summary>
    /// A helper function to create user Dto from model.
    /// </summary>
    /// <param name="user">The user model.</param>
    /// <returns>A UserReadDto.</returns>
    private UserReadDto GenerateUserReadDto(User user)
    {
        return new UserReadDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };
    }

    /// <summary>
    /// A helper function to create category Dto from model.
    /// </summary>
    /// <param name="category">The forum category model.</param>
    /// <returns>A CategoryReadDto.</returns>
    private CategoryReadDto GenerateCategoryReadDto(ForumCategory category)
    {
        return new CategoryReadDto
        {
            ForumCategoryId = category.ForumCategoryId,
            Name = category.Name,
            Description = category.Description
        };
    }

    /// <summary>
    /// A helper function to create post Dto from model.
    /// </summary>
    /// <param name="post">The forum post model.</param>
    /// <returns>A PostReadDto.</returns>
    private PostReadDto GeneratePostDto(ForumPost post)
    {
        return new PostReadDto
        {
            ForumPostId = post.ForumPostId,
            Title = post.Title,
            Content = post.Content,
            Created = post.CreatedAt,
            Updated = post.UpdatedAt,
            CommentCount = post.CommentCount,
            Category = post.Category != null
                ? GenerateCategoryReadDto(post.Category)
                : null,
            Author = post.User != null
                ? GenerateUserReadDto(post.User)
                : null
        };
    }
}
