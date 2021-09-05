using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API._Data;
using API._DTOs;
using API._Entities;
using API._Extensions;
using API._Interfaces;
using API.Helpers;
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

            var posts = await this.postsRepo.GetPostsDtoAsync();
            return Ok(posts);
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
            this.postsRepo.AddPostAsync(post);
            var postResult = this.autpMapper.Map<PostDto>(post);
            if (await this.postsRepo.SaveChangesAsync())
                return Ok(postResult);
            return BadRequest("Error while creating the post");

        }


        [HttpGet("post-detail/{postId}")]
        public async Task<ActionResult<PostDto>> GetPost(int postId)
        {
            var post = await this.postsRepo.GetPostDtoAsync(postId);
            if (post == null) return BadRequest("Post doesnot exisit");

            return Ok(post);



        }

        [HttpGet("{userName}")]
        public async Task<ActionResult<PagedList<PostDto>>> GetUserPosts(string userName, int pageNumber, int pageSize)
        {

            var posts = await this.postsRepo.GetUserPostsDtoAsync(userName, pageNumber, pageSize);
            if (posts == null) return BadRequest("Unvalid userName");
            Response.AddPaginationHeader(posts.CurrentPage, posts.PageSize, posts.TotalCount, posts.TotalPages);
            return Ok(posts);
        }


        [HttpPost("add-comment")]
        public async Task<ActionResult<UserPostCommentDto>> AddComment(CreateCommentDto createCommentDto)
        {
            var postId = createCommentDto.PostId;
            var comment = createCommentDto.Comment;
            var userName = User.GetUserName();
            var user = await this.userRepo.GetUserAsync(userName);
            var post = await this.postsRepo.GetPostAsync(postId);

            if (user == null || post == null) return BadRequest("User or post doesnot exisit");

            var userPostComment = new UserPostComment
            {
                User = user,
                UserName = user.UserName,
                UserPhoto = user.Photo,
                Post = post,
                Comment = comment,
            };

            var userPostCommentDto = await this.postsRepo.AddCommentAsync(userPostComment);
            if (await this.postsRepo.SaveChangesAsync()) return Ok(userPostCommentDto);
            return BadRequest("Error while creating the comment");

        }






    }
}