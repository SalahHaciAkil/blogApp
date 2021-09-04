using System;

namespace API._DTOs
{
    public class UserPostCommentsDto
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public DateTime CommentTime { get; set; } = DateTime.Now;
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserPhoto { get; set; }
        public int PostId { get; set; }
    }
}