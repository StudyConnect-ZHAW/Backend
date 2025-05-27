using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyConnect.Core.Interfaces;
using StudyConnect.Data.Repositories;
using StudyConnect.Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StudyConnect.API.Dtos.Requests.User;
using System.ComponentModel.DataAnnotations;
using StudyConnect.API.Dtos.Responses.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using StudyConnect.Core.Common;
using System.Security.Claims;
using System.Linq;

namespace StudyConnect.API.Controllers.Users
{
    /// <summary>
    /// The user endpoint is used to make modifications to a user.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="UserController"/> class.
    /// </remarks>
    /// <param name="userRepository">The user repository to interact with user data.</param>
    [ApiController]
    public class UserController(IUserRepository userRepository) : BaseController
    {
        /// <summary>
        /// The user repository to interact with user data.
        /// </summary>
        protected readonly IUserRepository _userRepository = userRepository;

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <returns></returns>
        [Route("v1/users")]
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserCreateDto userCreateDto)
        {

            // Validate the incoming request
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Map the DTO to the User entity
            User user = new()
            {
                UserGuid = userCreateDto.UserGuid,
                FirstName = userCreateDto.FirstName,
                LastName = userCreateDto.LastName,
                Email = userCreateDto.Email
            };

            // Add the user to the repository
            var result = await _userRepository.AddAsync(user);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            return NoContent();
        }
        
        /// <summary>
        /// Add a new user based on the claims in the token.
        /// </summary>
        /// <returns></returns>
        [Route("v2/users")]
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddUser()
        {
            UserCreateDto userCreateDto = new()
            {
                UserGuid = Guid.TryParse(HttpContext.User.GetObjectId(), out var userGuid) ? userGuid : Guid.Empty,
                FirstName = HttpContext.User.FindFirst(ClaimTypes.GivenName)?.Value ?? string.Empty,
                LastName = HttpContext.User.FindFirst(ClaimTypes.Surname)?.Value ?? string.Empty,
                Email = HttpContext.User.FindFirst(ClaimTypes.Upn)?.Value ?? string.Empty
            };

            // Validate the incoming request
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            User user = new()
            {
                UserGuid = userCreateDto.UserGuid,
                FirstName = userCreateDto.FirstName,
                LastName = userCreateDto.LastName,
                Email = userCreateDto.Email
            };

            var result = await _userRepository.AddAsync(user);
            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);
            

            return Ok(userCreateDto);
        }

        /// <summary>
        /// Get the current user based on oid claim in the token
        /// This endpoint retrieves the user information of the currently authenticated user.
        /// </summary>
        /// <returns></returns>
        [Route("v1/users/me")]
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMe()
        {
            //Read the ObjectId claim from the token
            var oidClaim = HttpContext.User.GetObjectId();
            if (string.IsNullOrEmpty(oidClaim))
                return Unauthorized();

            var result = await _userRepository.GetByIdAsync(Guid.Parse(oidClaim));

            // Check if the user was found
            if (result.Data == null)
                return NotFound(ErrorMessages.UserNotFound);


            // Map the User entity to a DTO if necessary
            var userDto = new UserReadDto
            {
                Oid = result.Data.UserGuid,
                FirstName = result.Data.FirstName,
                LastName = result.Data.LastName,
                Email = result.Data.Email
            };

            return Ok(userDto);
        }

        /// <summary>
        /// Get a user by id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns></returns>
        [Route("v1/users/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetUserById([FromRoute] Guid id)
        {

            var result = await _userRepository.GetByIdAsync(id);

            // Check if the user was found
            if (result.Data == null)
            {
                return NotFound("User not found.");
            }

            // Map the User entity to a DTO if necessary
            var userDto = new UserReadDto
            {
                Oid = result.Data.UserGuid,
                FirstName = result.Data.FirstName,
                LastName = result.Data.LastName,
                Email = result.Data.Email
            };

            return Ok(userDto);
        }

        /// <summary>
        /// Update a user
        /// </summary>
        /// <param name="id">User id</param>
        /// <param name="userUpdateDto">User with updated properties</param>
        /// <returns></returns>
        [Route("v1/users/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromBody] UserUpdateDto userUpdateDto)
        {
            // Validate the incoming request
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Create a new User object with the updated information
            var user = new User
            {
                UserGuid = id,
                FirstName = userUpdateDto.FirstName,
                LastName = userUpdateDto.LastName,
                Email = userUpdateDto.Email
            };

            var result = await _userRepository.UpdateAsync(user);

            // Check if the operation was successful
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            // Check if the operation was successful but the user was not found
            if (!result.Data)
            {
                return NotFound("User not found.");
            }

            return Ok("User updated successfully.");
        }


        /// <summary>
        /// Update a user
        /// </summary>
        /// <param name="userUpdateDto">User with updated properties</param>
        /// <response code="204">User was updated</response>
        [Route("v1/users")]
        [HttpPut]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDto userUpdateDto)
        {
            //Read the ObjectId claim from the token
            var oidClaim = HttpContext.User.GetObjectId();
            if (string.IsNullOrEmpty(oidClaim))
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Create a updated User object with the new information
            var user = new User
            {
                UserGuid = Guid.Parse(oidClaim),
                FirstName = userUpdateDto.FirstName,
                LastName = userUpdateDto.LastName,
                Email = userUpdateDto.Email
            };

            var result = await _userRepository.UpdateAsync(user);

            // Check if the operation was successful
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            // Check if the operation was successful but the user was not found
            if (!result.Data)
            {
                return NotFound("User not found.");
            }

            return NoContent();
        }


        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns></returns>
        [Route("v1/users/{id}")]
        [HttpDelete]
        public IActionResult DeleteUser([FromRoute] Guid id)
        {
            //Not implemented yet
            return BadRequest("Not implemented yet.");
        }
    }
}

