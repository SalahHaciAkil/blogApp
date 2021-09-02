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

            modelBuilder.Entity<PostsTags>().HasKey(x => new {x.PostsId, x.TagId});

            modelBuilder.Entity<PostsTags>().HasOne(pt => pt.Posts)
            .WithMany(r => r.PostsTags)
            .HasForeignKey(pt => pt.PostsId);

            modelBuilder.Entity<PostsTags>().HasOne(pt => pt.Tag)
            .WithMany(t => t.PostsTags)
            .HasForeignKey(pt => pt.TagId);



        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Post> Posts { get; set; }

        public DbSet<Posts> Postss { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }




    public class Posts
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Tag> Tags { get; set; }
        public ICollection<PostsTags> PostsTags { get; set; }
    }

    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Posts> Posts { get; set; }
        public ICollection<PostsTags> PostsTags { get; set; }


    }

    public class PostsTags
    {
        public int PostsId { get; set; }
        public Posts Posts { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}


