using System;
using System.Collections.Generic;

namespace API._Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string PostTitle { get; set; }
        public string PostContent { get; set; }
        public string PostrName { get; set; }
        public string PostrPhoto { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public string Photo { get; set; }
        public string PhotoPublicId { get; set; }
        public AppUser User { get; set; }
        public int UserId { get; set; }
        public ICollection<UserPostLikes> LikedBy { get; set; }
        public ICollection<UserPostComment> Comments { get; set; }

    }
}