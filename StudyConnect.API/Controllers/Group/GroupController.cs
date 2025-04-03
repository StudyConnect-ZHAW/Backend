using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StudyConnect.API.Controllers.Groups
{
    /// <summary>
    /// The group endpoint is used to make modifications to a group.
    /// </summary>
    [ApiController]
    public class GroupsController : BaseController
    {
        /// <summary>
        /// Create group
        /// </summary>
        /// <returns></returns>
        [Route("v1/groups")]
        [HttpPost]
        public IActionResult AddGroup(){
            return Ok();
        }

        /// <summary>
        /// Delete group
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
         [Route("v1/groups/{id}")]
         [HttpDelete]
        public IActionResult DeleteGroups([FromRoute] Guid id)
         {
             return Ok();
         }

        /// <summary>
        /// Returns value of group
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
         [Route("v1/groups/{id}")]
         [HttpGet]
        public IActionResult GetGroup([FromRoute] Guid id)
        {
            return Ok();
        }

        /// <summary>
        /// Update group
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
         [Route("v1/groups/update/{id}")]
         [HttpPut]
        public IActionResult UpdateGroup([FromRoute] Guid id)
        {
            return Ok();
        }

        /// <summary>
        /// Join group
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
         [Route("v1/groups/join/{id}")]
         [HttpPost]
        public IActionResult JoinGroup([FromRoute] Guid id)
        {
            return Ok();
        }

        /// <summary>
        /// Leave group
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
         [Route("v1/groups/{id}")]
         [HttpPost]
        public IActionResult LeaveGroup([FromRoute] Guid id)
        {
            return Ok();
        }

    }




}