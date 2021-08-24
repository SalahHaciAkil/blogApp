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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class PostsController : BaseApiController
    {
        private readonly IPostsRepo postsRepo;
        private readonly IUserRepo userRepo;
        private readonly IMapper autpMapper;
        public PostsController(IPostsRepo postsRepo, IUserRepo userRepo, IMapper autpMapper)
        {
            this.postsRepo = postsRepo;
            this.userRepo = userRepo;
            this.autpMapper = autpMapper;
        }


        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetPosts()
        {
            var posts = await this.postsRepo.GetPosts();
            return Ok(this.autpMapper.Map<PostDto[]>(posts));
        }

        [HttpPost]
        public async Task<ActionResult<PostDto>> AddPost(CreatePostDto createPostDto)
        {
            var userName = User.GetUserName();
            var user = await this.userRepo.GetUserAsync(userName);

            if (user is null) return NotFound("User doesnot exisit");

            var post = new Post
            {
                Title = createPostDto.Title,
                Content = createPostDto.Content,
                PosetrName = userName,
                User = user,
            };
            this.postsRepo.AddPost(post);
            if (await this.postsRepo.SaveChangesAsync())
                return Ok(this.autpMapper.Map<PostDto>(post));
            return BadRequest("Error while creating the post");

        }
    }
}