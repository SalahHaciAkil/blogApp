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

        public async Task<IEnumerable<PostDto>> GetPostsDtoAsync()
        {
            var posts = await this.context.Posts
                .Include(x => x.LikedBy)
                .ProjectTo<PostDto>(this.autoMapepr.ConfigurationProvider)
                .ToListAsync();

            return posts;
        }

        public async Task<PagedList<PostDto>> GetUserPostsDtoAsync(string userName, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var query = this.context.Posts.Where(p => p.PostrName == userName).OrderByDescending(x => x.CreatedTime).AsNoTracking();
            if (query == null) return null;

            var posts = await PagedList<PostDto>.CreateAsync(query.ProjectTo<PostDto>(this.autoMapepr.ConfigurationProvider), pageNumber, pageSize);
            return posts;
        }


        public async Task<Post> GetPostAsync(int postId)
        {
            var post = await this.context.Posts
                .Include(x => x.LikedBy)
                .FirstOrDefaultAsync(x => x.Id == postId);

            return post;

        }

        public async Task<PostDto> GetPostDtoAsync(int postId)
        {
            var post = await this.context.Posts
                .Include(x => x.LikedBy).ProjectTo<PostDto>(this.autoMapepr.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == postId);

            return post;

        }

        public async void AddPostAsync(Post post)
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