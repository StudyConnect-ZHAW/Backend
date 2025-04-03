using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StudyConnect.API.Controllers.Groups
{
    [ApiController]
    public class GroupsController : ControllerBase
    {
        /// create Groups
        [Route("v1/groups")]
        [HttpPost]
        public IActionResult AddGroup(){
            return Ok();
        }

        /// delete Groups by id
         [Route("v1/groups/{id}")]
         [HttpDelete]
        public IActionResult DeleteGroups([FromRoute] Guid id)
         {
             return Ok();
         }

        /// info Group
         [Route("v1/groups/{id}")]
         [HttpGet]
        public IActionResult GetGroup([FromRoute] Guid id)
        {
            return Ok();
        }

        /// update Group
         [Route("v1/groups/update/{id}")]
         [HttpPut]
        public IActionResult UpdateGroup([FromRoute] Guid id) /// name , description, public/private
        {
            return Ok();
        }

        /// join Group
         [Route("v1/groups/join/{id}")]
         [HttpPost]
        public IActionResult JoinGroup([FromRoute] Guid id)
        {
            return Ok();
        }

        /// leave Group
         [Route("v1/groups/{id}")]
         [HttpPost]
        public IActionResult LeaveGroup([FromRoute] Guid id)
        {
            return Ok();
        }

    }




}