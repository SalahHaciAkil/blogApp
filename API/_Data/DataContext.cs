using System.Collections.Generic;
using API._Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API._Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int, IdentityUserClaim<int>,
     AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<AppUser>()
                                .HasMany(u => u.UserRole)
                                .WithOne(u => u.User)
                                .HasForeignKey(u => u.UserId)
                                .IsRequired();

            modelBuilder.Entity<AppRole>()
                                .HasMany(u => u.UserRole)
                                .WithOne(u => u.Role)
                                .HasForeignKey(u => u.RoleId)
                                .IsRequired();
            
            modelBuilder.Entity<Post>()
                    .HasOne(x => x.User)
                    .WithMany(x => x.Posts)
                    .HasForeignKey(x => x.UserId)
                    .IsRequired();

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




        }

        // public DbSet<AppUser> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<UserPostLikes> UsersPostLikes { get; set; }
        public DbSet<UserPostComment> UsersPostComments { get; set; }
        // public DbSet<UserActivities> UsersActivities { get; set; }


    }



}


