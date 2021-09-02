using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API._DTOs;
using API._Entities;
using API._Interfaces;
using API.Helpers;
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

        public async Task<PagedList<PostDto>> GetUserPosts(string userName,[FromQuery] int pageNumber,[FromQuery]  int pageSize)
        {
            var query = this.context.Posts.Where(p => p.PostrName == userName).OrderByDescending(x => x.CreatedTime).AsNoTracking();
            if (query == null) return null;

            var posts = await PagedList<PostDto>.CreateAsync(query.ProjectTo<PostDto>(this.autoMapepr.ConfigurationProvider), pageNumber, pageSize);
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