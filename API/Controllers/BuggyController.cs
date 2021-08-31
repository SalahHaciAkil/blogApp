using API._Data;
using API._Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly DataContext context;

        public BuggyController(DataContext context)
        {
            this.context = context;
        }


        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFoundError()
        {
            return NotFound("User is not exisit");
        }


        [HttpGet("bad-request")]
        public ActionResult<AppUser> GetBadRequestError()
        {
            return BadRequest("this is bad-request");
        }


        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            var user = this.context.Users.Find(-1);
            var s = user.ToString();
            if (s is not null) return Ok(s);
            return BadRequest("zzzz");

        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<AppUser> GetUnAuthorizeError()
        {
            return Ok();
        }

    }
}