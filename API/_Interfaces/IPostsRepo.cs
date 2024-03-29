using System.Collections.Generic;
using System.Threading.Tasks;
using API._DTOs;
using API._Entities;
using API._Helpers;
using API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API._Interfaces
{
    public interface IPostsRepo
    {
        Task<PagedList<PostDto>> GetPostsDtoAsync(PostParams postParams);
        Task<PagedList<PostDto>> GetUserPostsDtoAsync(string userName, int pageNumber, int PageSize);


        Task<Post> GetPostAsync(int postId);
        Task<PostDto> GetPostDtoAsync(int postId);
        Task<IEnumerable<Post>> GetPosts();
        Task<bool> SaveChangesAsync();
        void AddPostAsync(Post post);
        void DeletePost(Post post);
        Task AddCommentAsync(UserPostComment userPostComment);

        Task<UserPostComment> GetCommentAsync(int commentId);
        void DeleteComment(UserPostComment userPostComment);
        Task<IEnumerable<UserPostLikes>> GetLikeActivitiesAsync(string userName);
        Task<UserPostLikes> GetLikeActivitiyAsync(int PostId, int userId);
        Task<IEnumerable<UserPostComment>> GetCommentActivitiesAsync(string userName);
        // void EditPost(Post post);
        // Task AddActivityAsync(UserActivities userActivities);

    }
}