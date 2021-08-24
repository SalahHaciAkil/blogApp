using System.Collections.Generic;
using System.Threading.Tasks;
using API._DTOs;
using API._Entities;
using Microsoft.AspNetCore.Mvc;

namespace API._Interfaces
{
    public interface IPostsRepo
    {
        Task<IEnumerable<Post>> GetPosts();
        Task<bool> SaveChangesAsync();
        void AddPost(Post post);
    }
}