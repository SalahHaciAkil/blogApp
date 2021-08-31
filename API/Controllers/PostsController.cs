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
        private readonly IPhotoService photoService;

        public PostsController(IPostsRepo postsRepo, IUserRepo userRepo, IMapper autpMapper,
        IPhotoService photoService)
        {
            this.postsRepo = postsRepo;
            this.userRepo = userRepo;
            this.autpMapper = autpMapper;
            this.photoService = photoService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetPosts()
        {
            var posts = await this.postsRepo.GetPosts();
            var postsDto = this.autpMapper.Map<PostDto[]>(posts);
            return Ok(postsDto);
        }

        [HttpPost]
        public async Task<ActionResult<PostDto>> AddPost([FromForm] CreatePostDto createPostDto)
        {
            var userName = User.GetUserName();
            var user = await this.userRepo.GetUserAsync(userName);

            if (user is null) return NotFound("User doesnot exisit");

            var uploadResult = await this.photoService.AddCloudPhotoAsync(createPostDto.Photo);
            if (uploadResult != null && uploadResult.Error != null) return BadRequest("Image couldnot be uploaded");



            var post = new Post
            {
                PostTitle = createPostDto.PostTitle,
                PostContent = createPostDto.PostContent,
                Photo = uploadResult.Url.AbsoluteUri,
                PhotoPublicId = uploadResult.PublicId,
                PostrName = userName,
                PostrPhoto = user.Photo,
                User = user,
            };
            this.postsRepo.AddPost(post);
            var postResult = this.autpMapper.Map<PostDto>(post);
            if (await this.postsRepo.SaveChangesAsync())
                return Ok(postResult);
            return BadRequest("Error while creating the post");

        }


        [HttpGet("post-detail/{postId}")]
        public async Task<ActionResult<PostDto>> GetPost(int postId){
            var post = await this.postsRepo.GetPost(postId);
            if(post == null)return BadRequest("Post doesnot exisit");

            return Ok(this.autpMapper.Map<PostDto>(post));

            

        }
    }
}