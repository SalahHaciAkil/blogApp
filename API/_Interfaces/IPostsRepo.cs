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
        Task<bool> SaveChangesAsync();
        void AddPostAsync(Post post);

        Task<UserPostCommentDto> AddCommentAsync(UserPostComment userPostComment);
    }
}