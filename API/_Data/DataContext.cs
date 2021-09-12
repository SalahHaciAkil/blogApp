using System.Collections.Generic;
using API._Entities;
using Microsoft.EntityFrameworkCore;

namespace API._Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {



            // modelBuilder.Entity<Posts>().HasMany(p => p.Tags)
            // .WithMany(t => t.Posts).UsingEntity<PostsTags>(
            //     j => j.HasOne(q => q.Tag)
            //     .WithMany(t => t.PostsTags)
            //     .HasForeignKey(po => po.TagId),
            //     j => j.HasOne(q => q.Posts)
            //     .WithMany(po => po.PostsTags)
            //     .HasForeignKey(x => x.PostsId),

            //     j => j.HasKey( x => new {x.PostsId, x.TagId})

            // );




            modelBuilder.Entity<UserPostLikes>()
                .HasKey(x => new { x.UserId, x.PostId });

            modelBuilder.Entity<UserPostLikes>()
                .HasOne(x => x.Post)
                .WithMany(x => x.LikedBy)
                .HasForeignKey(x => x.PostId);

            modelBuilder.Entity<UserPostLikes>()
                .HasOne(x => x.User)
                .WithMany(x => x.LikesPost)
                .HasForeignKey(x => x.UserId);



            modelBuilder.Entity<UserPostComment>()
                .HasOne(x => x.Post)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.PostId);

            modelBuilder.Entity<UserPostComment>()
                .HasOne(x => x.User)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.UserId);


            // modelBuilder.Entity<UserActivities>()
            // .HasOne(x => x.FromUser)
            // .WithMany(x => x.SourceActivites)
            // .HasForeignKey(x => x.FromUserId)
            // .OnDelete(DeleteBehavior.Cascade);



            // modelBuilder.Entity<UserActivities>()
            // .HasOne(x => x.ToUser)
            // .WithMany(x => x.DestinationActivites)
            // .HasForeignKey(x => x.ToUserId)
            // .OnDelete(DeleteBehavior.Cascade);

        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<UserPostLikes> UsersPostLikes { get; set; }
        public DbSet<UserPostComment> UsersPostComments { get; set; }
        // public DbSet<UserActivities> UsersActivities { get; set; }


    }



}


