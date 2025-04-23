using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyConnect.Core.Interfaces;
using StudyConnect.Data.Repositories;
using StudyConnect.Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StudyConnect.API.Dtos.Requests.User;
using System.ComponentModel.DataAnnotations;
using StudyConnect.API.Dtos.Responses.User;

namespace StudyConnect.API.Controllers.Users
{
    /// <summary>
    /// The user endpoint is used to make modifications to a user.
    /// </summary>
    [ApiController]
    public class UserController : BaseController
    {   
        /// <summary>
        /// The user repository to interact with user data.
        /// </summary>
        protected readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository to interact with user data.</param>
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }   

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <returns></returns>
        [Route("v1/users")]
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserCreateDto userCreateDto) {

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
        /// Get a user by id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns></returns>
        [Route("v1/users/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetUserById([FromRoute] Guid id ){
            
            var result = await _userRepository.GetByIdAsync(id);

            // Check if the user was found
            if (result.Data == null)
            {
                return NotFound("User not found.");
            }

            // Map the User entity to a DTO if necessary
            var userDto = new UserReadDto
            {
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
        public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromBody] UserUpdateDto userUpdateDto) {
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

            var result =  await _userRepository.UpdateAsync(user);

            // Check if the operation was successful
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok("User updated successfully.");
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

