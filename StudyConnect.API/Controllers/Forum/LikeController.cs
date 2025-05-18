using Microsoft.AspNetCore.Mvc;
using StudyConnect.Core.Interfaces.Services;
using StudyConnect.API.Dtos.Requests.Forum;
using StudyConnect.API.Dtos.Responses.Forum;
using StudyConnect.API.Dtos;
using static StudyConnect.Core.Common.ErrorMessages;

namespace StudyConnect.API.Controllers.Forum;

/// <summary>
/// Controller for managing the forum likes
/// Provides endpoints to create, retrieve,and delete likes.
/// </summary>
public class LikeController : BaseController
{
    /// <summary>
    ///  Service responsible for handling business logic related to likes. 
    /// </summary>
    protected readonly ILikeService _likeService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryController"/> class.
    /// </summary>
    /// <param name="likeService">
    /// The like service used to perform business operations and coordinate data access.
    /// </param>
    public LikeController(ILikeService likeService)
    {
        _likeService = likeService;
    }

    /// <summary>
    /// Add a like to a post
    /// </summary>
    /// <param name="likeDto">A DTO containing information how to make a like.</param>
    [Route("v1/posts/likes")]
    [HttpPost]
    public async Task<IActionResult> AddPostLike([FromBody] LikeCreateDto likeDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _likeService.LeaveLikeAsync(likeDto.UserId, likeDto.PostId, null);
        if (!result.IsSuccess)
            return result.ErrorMessage!.Contains(GeneralNotFound)
                ? NotFound(new ApiResponse<string>(result.ErrorMessage))
                : BadRequest(new ApiResponse<string>(result.ErrorMessage));


        return NoContent();
    }

    /// <summary>
    /// Remove a like from a post.
    /// </summary>
    /// <param name="pid">The unique identifier of the post.</param>
    /// <param name="uid">the unique identifier of the user.</param>
    [Route("v1/posts/likes/{pid:guid}")]
    [HttpDelete]
    public async Task<IActionResult> DeletePostLike([FromRoute] Guid pid, [FromQuery] Guid uid)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _likeService.RemoveLikeAsync(uid, pid, null);
        if (!result.IsSuccess)
        {
            if (result.ErrorMessage!.Contains(GeneralNotFound)) return NotFound(new ApiResponse<string>(result.ErrorMessage));
            else if (result.ErrorMessage!.Equals(NotAuthorized)) return Unauthorized(new ApiResponse<string>(result.ErrorMessage));
            else return BadRequest(new ApiResponse<string>(result.ErrorMessage));
        }

        return NoContent();
    }

    /// <summary>
    /// Get like count of a specific post.
    /// </summary>
    /// <param name="pid">he unique identifier of the post.</param>
    /// <returns>On success a DTO with information about the like, on failure HTTP 400/404 status code.</returns>
    [Route("v1/posts/likes/{pid:guid}")]
    [HttpGet]
    public async Task<IActionResult> GetPostLikeCount([FromRoute] Guid pid)
    {
        var count = await _likeService.GetLikeCountAsync(pid, null);
        if (!count.IsSuccess)
            return count.ErrorMessage!.Contains(GeneralNotFound)
                ? NotFound(new ApiResponse<string>(count.ErrorMessage))
                : BadRequest(new ApiResponse<string>(count.ErrorMessage));

        var result = new LikeReadDto { PostLikeCount = count.Data };
        return Ok(new ApiResponse<LikeReadDto>(result));
    }


    /// <summary>
    /// Add a like to a comment.
    /// </summary>
    /// <param name="likeDto">A DTO containing information how to make a like.</param>
    [Route("v1/comments/likes")]
    [HttpPost]
    public async Task<IActionResult> AddCommentLike([FromBody] LikeCreateDto likeDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _likeService.LeaveLikeAsync(likeDto.UserId, null, likeDto.CommentId);
        if (!result.IsSuccess)
            return result.ErrorMessage!.Contains(GeneralNotFound)
                ? NotFound(new ApiResponse<string>(result.ErrorMessage))
                : BadRequest(new ApiResponse<string>(result.ErrorMessage));


        return NoContent();
    }

    /// <summary>
    /// Remove a like from a comment.
    /// </summary>
    /// <param name="cmid">The unique identifier of the comment.</param>
    /// <param name="uid">the unique identifier of the user.</param>
    [Route("v1/comments/likes/{cmid:guid}")]
    [HttpDelete]
    public async Task<IActionResult> DeleteCommentLike([FromRoute] Guid cmid, [FromQuery] Guid uid)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _likeService.RemoveLikeAsync(uid, null, cmid);
        if (!result.IsSuccess)
        {
            if (result.ErrorMessage!.Contains(GeneralNotFound)) return NotFound(new ApiResponse<string>(result.ErrorMessage));
            else if (result.ErrorMessage!.Equals(NotAuthorized)) return Unauthorized(new ApiResponse<string>(result.ErrorMessage));
            else return BadRequest(new ApiResponse<string>(result.ErrorMessage));
        }

        return NoContent();
    }

    /// <summary>
    /// Get like count of a specific post.
    /// </summary>
    /// <param name="cmid">he unique identifier of the comment.</param>
    /// <returns>On success a DTO with information about the like, on failure HTTP 400/404 status code.</returns>
    [Route("v1/comments/likes/{cmid:guid}")]
    [HttpGet]
    public async Task<IActionResult> GetCommentLikeCount([FromRoute] Guid cmid)
    {
        var count = await _likeService.GetLikeCountAsync(null, cmid);
        if (!count.IsSuccess)
            return count.ErrorMessage!.Contains(GeneralNotFound)
                ? NotFound(new ApiResponse<string>(count.ErrorMessage))
                : BadRequest(new ApiResponse<string>(count.ErrorMessage));

        var result = new LikeReadDto { CommentLikeCount = count.Data };
        return Ok(new ApiResponse<LikeReadDto>(result));
    }


}
