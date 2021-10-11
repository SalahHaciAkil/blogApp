using System;
using System.Collections.Generic;
using API._Entities;

namespace API._DTOs
{
    public class PostDto
    {
        public int Id { get; set; }
        public string PostTitle { get; set; }
        public string PostContent { get; set; }
        public string PostrName { get; set; }
        public string PostrPhoto { get; set; }
        public string PostCategory { get; set; }

        public DateTime CreatedTime { get; set; }
        public string Photo { get; set; }
        public int UserId { get; set; }

        public string PhotoPublicId { get; set; }
        public ICollection<UserPostLikesDto> LikedBy { get; set; }
        public ICollection<UserPostCommentDto> Comments { get; set; }

    }
}