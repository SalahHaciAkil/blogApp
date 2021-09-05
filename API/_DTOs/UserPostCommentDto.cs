using System;

namespace API._DTOs
{
    public class UserPostCommentDto
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public DateTime CommentTime { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserPhoto { get; set; }
        public int PostId { get; set; }
    }
}