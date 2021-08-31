using System.Collections.Generic;
using System.Threading.Tasks;
using API._DTOs;
using API._Entities;
using API._Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API._Data
{
    public class PostsRepo : IPostsRepo
    {
        private readonly DataContext context;
        private readonly IMapper autoMapepr;

        public PostsRepo(DataContext context, IMapper autoMapepr)
        {
            this.context = context;
            this.autoMapepr = autoMapepr;
        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            var posts = await this.context.Posts
            .ToListAsync();
            return posts;
        }

        public async Task<Post> GetPost(int postId)
        {
            var post = await this.context.Posts.FindAsync(postId);
            return post;

        }

        public async void AddPost(Post post)
        {
            await this.context.Posts.AddAsync(post);
        }






        public async Task<bool> SaveChangesAsync()
        {
            if (await this.context.SaveChangesAsync() > 0) return true;
            return false;
        }
    }
}