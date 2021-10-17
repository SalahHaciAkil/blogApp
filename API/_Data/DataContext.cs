using System;
using System.Collections.Generic;
using API._Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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


            modelBuilder.ApplyUtcDateTimeConverter();


        }

        // public DbSet<AppUser> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<UserPostLikes> UsersPostLikes { get; set; }
        public DbSet<UserPostComment> UsersPostComments { get; set; }
        // public DbSet<UserActivities> UsersActivities { get; set; }


    }



    public static class UtcDateAnnotation
    {
        private const string IsUtcAnnotation = "IsUtc";
        private static readonly ValueConverter<DateTime, DateTime> UtcConverter =
            new ValueConverter<DateTime, DateTime>(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        public static PropertyBuilder<TProperty> IsUtc<TProperty>(this PropertyBuilder<TProperty> builder, bool isUtc = true) =>
            builder.HasAnnotation(IsUtcAnnotation, isUtc);

        public static bool IsUtc(this IMutableProperty property) =>
            ((bool?)property.FindAnnotation(IsUtcAnnotation)?.Value) ?? true;

        /// <summary>
        /// Make sure this is called after configuring all your entities.
        /// </summary>
        public static void ApplyUtcDateTimeConverter(this ModelBuilder builder)
        {
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (!property.IsUtc())
                    {
                        continue;
                    }

                    if (property.ClrType == typeof(DateTime) ||
                        property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(UtcConverter);
                    }
                }
            }
        }
    }



}


