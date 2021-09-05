using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API._Data;
using API._DTOs;
using API._Entities;
using API._Extensions;
using API._Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IUserRepo userRepo;
        private readonly IPostsRepo postsRepo;
        private readonly IMapper autoMapper;

        public UsersController(IUserRepo userRepo, IPostsRepo postsRepo, IMapper autoMapper)
        {
            this.userRepo = userRepo;
            this.postsRepo = postsRepo;
            this.autoMapper = autoMapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await this.userRepo.GetUsersAsync();
            return Ok(this.autoMapper.Map<MemberDto[]>(users));
        }



        [HttpGet("{userName}")]

        public async Task<ActionResult<MemberDto>> GetUser(string userName)
        {
            var user = await this.userRepo.GetUserAsync(userName);
            if (user is null) { return NotFound("user is not exisit"); }
            return Ok(this.autoMapper.Map<MemberDto>(user));
        }


        [HttpPost("add-like/{postId}")]

        public async Task<ActionResult> AddLike(int postId)
        {


            var userName = User.GetUserName();

            var user = await this.userRepo.GetUserAsync(userName);
            var post = await this.postsRepo.GetPostAsync(postId);

            if (user == null || post == null) return BadRequest("User or Post doesnot exisit");

            var userPostLikes = new UserPostLikes
            {
                User = user,
                Post = post,
                PostrName = userName,
            };


            await this.userRepo.AddUserPostLike(userPostLikes);
            if (await this.userRepo.SaveChangesAsync())
            {
                return Ok();
            }
            return BadRequest("Couldn't save the changes");





        }





    }
}