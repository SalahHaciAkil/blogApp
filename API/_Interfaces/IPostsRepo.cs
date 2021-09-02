using System.Collections.Generic;
using System.Threading.Tasks;
using API._DTOs;
using API._Entities;
using API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API._Interfaces
{
    public interface IPostsRepo
    {
        Task<IEnumerable<Post>> GetPosts();
        Task<PagedList<PostDto>> GetUserPosts(string userName, int pageNumber, int PageSize);
        Task<Post> GetPost(int postId);
        Task<bool> SaveChangesAsync();
        void AddPost(Post post);
    }
}