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
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper autoMapper;
        private readonly IPhotoService photoService;

        public PostsController(IUnitOfWork unitOfWork, IMapper autoMapper,
        IPhotoService photoService)
        {

            this.unitOfWork = unitOfWork;
            this.autoMapper = autoMapper;
            this.photoService = photoService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetPosts([FromQuery] PostParams postParams)
        {
            var posts = await this.unitOfWork.PostRepo.GetPostsDtoAsync(postParams);
            Response.AddPaginationHeader(posts.CurrentPage, posts.PageSize, posts.TotalCount, posts.TotalPages);
            return Ok(posts);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<PostDto>> AddPost([FromForm] CreatePostDto createPostDto)
        {
            var userName = User.GetUserName();
            var user = await this.unitOfWork.UserRepo.GetUserAsync(userName);

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
            this.unitOfWork.PostRepo.AddPostAsync(post);
            if (await this.unitOfWork.PostRepo.SaveChangesAsync())
                return Ok(this.autoMapper.Map<PostDto>(post));
            return BadRequest("Error while creating the post");

        }


        [HttpGet("post-detail/{postId}")]
        public async Task<ActionResult<PostDto>> GetPost(int postId)
        {
            var post = await this.unitOfWork.PostRepo.GetPostDtoAsync(postId);
            if (post == null) return BadRequest("Post doesnot exisit");

            return Ok(post);



        }

        [HttpGet("{userName}")]
        public async Task<ActionResult<PagedList<PostDto>>> GetUserPosts(string userName, int pageNumber, int pageSize)
        {

            var posts = await this.unitOfWork.PostRepo.GetUserPostsDtoAsync(userName, pageNumber, pageSize);
            if (posts == null) return BadRequest("Unvalid userName");
            Response.AddPaginationHeader(posts.CurrentPage, posts.PageSize, posts.TotalCount, posts.TotalPages);
            return Ok(posts);
        }




        [Authorize]

        [HttpPost("add-like/{postId}")]

        public async Task<ActionResult> AddLike(int postId)
        {


            var userName = User.GetUserName();

            var user = await this.unitOfWork.UserRepo.GetUserAsync(userName);

            var post = await this.unitOfWork.PostRepo.GetPostAsync(postId);

            if (post.UserId == user.Id) return BadRequest("You can't like your post");

            if (user == null || post == null) return BadRequest("User or Post doesnot exisit");

            var userPostLikes = new UserPostLikes
            {
                User = user,
                Post = post,
                PostrName = post.PostrName,
                PostrId = post.UserId,
                UserPhoto = user.Photo,
                UserName = userName
            };


            await this.unitOfWork.UserRepo.AddUserPostLike(userPostLikes);
            if (await this.unitOfWork.Complete())
            {
                return Ok();
            }
            return BadRequest("Couldn't save the changes");

        }
        [Authorize]

        [HttpGet("like-activity")]
        public async Task<ActionResult<IEnumerable<UserPostLikesDto>>> GetLikeActivities()
        {
            var userName = User.GetUserName();
            var userPostLikes = await this.unitOfWork.PostRepo.GetLikeActivitiesAsync(userName);
            if (await this.unitOfWork.PostRepo.SaveChangesAsync())
            {
                return Ok(this.autoMapper.Map<UserPostLikesDto[]>(userPostLikes));
            }

            return NoContent();
        }
        [Authorize]

        [HttpPost("add-comment")]
        public async Task<ActionResult<UserPostCommentDto>> AddComment(CreateCommentDto createCommentDto)
        {
            var postId = createCommentDto.PostId;
            var comment = createCommentDto.Comment;
            var userName = User.GetUserName();
            var user = await this.unitOfWork.UserRepo.GetUserAsync(userName);
            var post = await this.unitOfWork.PostRepo.GetPostAsync(postId);

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
            await this.unitOfWork.PostRepo.AddCommentAsync(userPostComment);
            if (await this.unitOfWork.PostRepo.SaveChangesAsync())
            {
                return Ok(this.autoMapper.Map<UserPostCommentDto>(userPostComment));
            }
            return BadRequest("Error while creating the comment");

        }



        [Authorize]

        [HttpGet("comment-activities")]
        public async Task<ActionResult<IEnumerable<UserPostLikesDto>>> GetCommentActivities()
        {
            var userName = User.GetUserName();
            var userPostComment = await this.unitOfWork.PostRepo.GetCommentActivitiesAsync(userName);
            if (await this.unitOfWork.PostRepo.SaveChangesAsync())
            {
                return Ok(this.autoMapper.Map<UserPostCommentDto[]>(userPostComment));
            }

            return NoContent();
        }

        [Authorize]

        [HttpDelete("{commentId}")]
        public async Task<ActionResult> DeleteComment(int commentId)
        {
            var userId = User.GetUserId();
            var comment = await this.unitOfWork.PostRepo.GetCommentAsync(commentId);
            if (comment is null) return BadRequest("Comment doesNot exisit");
            if (comment.UserId != userId) return BadRequest("You cannot delete other users comments");


            this.unitOfWork.PostRepo.DeleteComment(comment);
            if (await this.unitOfWork.PostRepo.SaveChangesAsync())
            {
                return Ok();
            }

            return BadRequest("Unexpected Error occured");


        }






    }
}