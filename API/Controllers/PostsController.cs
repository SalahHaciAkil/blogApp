using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API._Data;
using API._DTOs;
using API._Entities;
using API._Extensions;
using API._Helpers;
using API._Interfaces;
using API.Helpers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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
        public async Task<ActionResult<IEnumerable<PostDto>>> GetPosts([FromQuery] PostParams postParams)
        {
            var posts = await this.postsRepo.GetPostsDtoAsync(postParams);
            Response.AddPaginationHeader(posts.CurrentPage, posts.PageSize, posts.TotalCount, posts.TotalPages);
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
            if (await this.postsRepo.SaveChangesAsync())
                return Ok(this.autpMapper.Map<PostDto>(post));
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





        [HttpPost("add-like/{postId}")]

        public async Task<ActionResult> AddLike(int postId)
        {


            var userName = User.GetUserName();

            var user = await this.userRepo.GetUserAsync(userName);

            var post = await this.postsRepo.GetPostAsync(postId);

            if (post.UserId == user.Id) return BadRequest("You can't like your post");

            if (user == null || post == null) return BadRequest("User or Post doesnot exisit");

            var userPostLikes = new UserPostLikes
            {
                User = user,
                Post = post,
                PostrName = post.PostrName,
                PostrId = post.UserId
            };


            await this.userRepo.AddUserPostLike(userPostLikes);
            if (await this.userRepo.SaveChangesAsync())
            {
                return Ok();
            }
            return BadRequest("Couldn't save the changes");

        }

        [HttpGet("like-activity")]
        public async Task<ActionResult<IEnumerable<UserPostLikesDto>>> GetLikeActivities()
        {
            var userName = User.GetUserName();
            var userPostLikes = await this.postsRepo.GetLikeActivitiesAsync(userName);
            if (await this.postsRepo.SaveChangesAsync())
            {
                return Ok(this.autpMapper.Map<UserPostLikesDto[]>(userPostLikes));
            }

            return NoContent();
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
                PostrName = post.PostrName,
                PostrId = post.UserId,
            };
            await this.postsRepo.AddCommentAsync(userPostComment);
            if (await this.postsRepo.SaveChangesAsync())
            {
                return Ok(this.autpMapper.Map<UserPostCommentDto>(userPostComment));
            }
            return BadRequest("Error while creating the comment");

        }




        [HttpGet("comment-activities")]
        public async Task<ActionResult<IEnumerable<UserPostLikesDto>>> GetCommentActivities()
        {
            var userName = User.GetUserName();
            var userPostComment = await this.postsRepo.GetCommentActivitiesAsync(userName);
            if (await this.postsRepo.SaveChangesAsync())
            {
                return Ok(this.autpMapper.Map<UserPostCommentDto[]>(userPostComment));
            }

            return NoContent();
        }


        [HttpDelete("{commentId}")]
        public async Task<ActionResult> DeleteComment(int commentId)
        {
            var userId = User.GetUserId();
            var comment = await this.postsRepo.GetCommentAsync(commentId);
            if (comment is null) return BadRequest("Comment doesNot exisit");
            if (comment.UserId != userId) return BadRequest("You cannot delete other users comments");


            this.postsRepo.DeleteComment(comment);
            if (await this.postsRepo.SaveChangesAsync())
            {
                return Ok();
            }

            return BadRequest("Unexpected Error occured");






        }






    }
}