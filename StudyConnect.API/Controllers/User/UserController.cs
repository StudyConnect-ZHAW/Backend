using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace StudyConnect.API.Controllers.Users
{

    [ApiController]
    public class UserController : BaseController
    {
        /// <summary>
        /// Create a new user
        /// </summary>
        /// <returns></returns>
        [Route("v1/users")]
        [HttpPost]
        public IActionResult AddUser() {
            return Ok();
        }

        /// <summary>
        /// Get a user by id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns></returns>
        [Route("v1/users/{id}")]
        [HttpGet]
        public IActionResult GetUserById([FromRoute] Guid id ){
            string user = "{\r\n  \"name\": \"Anna M�ller\",\r\n  \"email\": \"anna.mueller@example.com\",\r\n  \"family_name\": \"M�ller\",\r\n  \"given_name\": \"Anna\" \t \r\n}\r\n";

            return Ok(user);
        }

        /// <summary>
        /// Update a user
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns></returns>
        [Route("v1/users/{id}")]
        [HttpPut]
        public IActionResult UpdateUser([FromRoute] Guid id)
        {
            return Ok();
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
            return Ok();
        }


    }
}
