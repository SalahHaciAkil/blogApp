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
        public string PostCategory { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
        public string Photo { get; set; }
        public string PhotoPublicId { get; set; }
        public AppUser User { get; set; }
        public int UserId { get; set; }
        public ICollection<UserPostLikes> LikedBy { get; set; }
        public ICollection<UserPostComment> Comments { get; set; }

    }
}

//    id: number;
    // postTitle: string;
    // postContent: string;
    // postrName: string;
    // postrPhoto: string;
    // postCategory:string;
    // createdTime?: Data;
    // photo?: string;
    // likedBy?: Array<Like>;
    // comments?: Array<Comment>;