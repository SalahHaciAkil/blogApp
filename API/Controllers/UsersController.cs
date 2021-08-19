using System.Collections.Generic;
using System.Threading.Tasks;
using API._Data;
using API._Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly DataContext context;

        public UsersController(DataContext context)
        {
            this.context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> getUsers()
        {
            var users = await this.context.Users.ToListAsync();

            return Ok(users);
        }

    }
}