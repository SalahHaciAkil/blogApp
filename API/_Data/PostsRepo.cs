using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API._DTOs;
using API._Entities;
using API._Extensions;
using API._Helpers;
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

        public async Task<PagedList<PostDto>> GetPostsDtoAsync(PostParams postParams)
        {
            var postsQuery = this.context.Posts
                .OrderByDescending(x => x.CreatedTime)
                .Include(x => x.LikedBy)
                .Include(x => x.Comments)
                .AsNoTracking();

            var posts = await PagedList<PostDto>.CreateAsync(
                postsQuery.ProjectTo<PostDto>(this.autoMapepr.ConfigurationProvider), postParams.PageNumber, postParams.PageSize);


            return posts;
            // .ProjectTo<PostDto>(this.autoMapepr.ConfigurationProvider)
            // .ToListAsync();

            // return posts;
        }

        public async Task<PagedList<PostDto>> GetUserPostsDtoAsync(string userName, int pageNumber, int pageSize)
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
                .Include(x => x.Comments)
                .FirstOrDefaultAsync(x => x.Id == postId);

            return post;

        }

        public async Task<PostDto> GetPostDtoAsync(int postId)
        {
            var post = await this.context.Posts
                .Include(x => x.LikedBy)
                .Include(x => x.Comments)
                .ProjectTo<PostDto>(this.autoMapepr.ConfigurationProvider)
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

        public async Task AddCommentAsync(UserPostComment userPostComment)
        {
            await this.context.UsersPostComments.AddAsync(userPostComment);
            // return this.autoMapepr.Map<UserPostCommentDto>(userPostComment);
        }

        public async Task<IEnumerable<UserPostLikes>> GetLikeActivitiesAsync(string userName)
        {
            var userPostLikes = await this.context.UsersPostLikes
            .Where(x => x.PostrName == userName && x.Read == false)
            .MarkPostLikeRead()
            .ToListAsync();

            return userPostLikes;
        }

        public async Task<UserPostLikes> GetLikeActivitiyAsync(int postId, int userId)
        {
            var like = await this.context.UsersPostLikes
            .Where(x => x.UserId == userId && x.PostId ==postId)
            .FirstOrDefaultAsync();

            return like;
        }

        public async Task<IEnumerable<UserPostComment>> GetCommentActivitiesAsync(string userName)
        {
            var userPostComments = await this.context.UsersPostComments
            .Where(x => x.PostrName == userName && x.PostrName != x.UserName && x.Read == false)
            .MarkPostCommentRead()
            .ToListAsync();


            return userPostComments;
        }


        public void DeleteComment(UserPostComment userPostComment)
        {
            this.context.UsersPostComments.Remove(userPostComment);
        }
        public async Task<UserPostComment> GetCommentAsync(int commentId)
        {
           var comment = await this.context.UsersPostComments.FindAsync(commentId);
           return comment;
        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            var posts =  await this.context.Posts.ToListAsync();
            return posts;
        }

        public void DeletePost(Post post)
        {
            this.context.Posts.Remove(post);
        }



        // public async Task AddActivityAsync(UserActivities userActivities)
        // {
        //     await this.context.UsersActivities.AddAsync(userActivities);
        // }

    }
}