using System;

namespace API._Entities
{
    public class UserPostComments
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public DateTime CommentTime { get; set; } = DateTime.Now;
        public AppUser User { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }

        public string UserPhoto { get; set; }

        public Post Post { get; set; }
        public int PostId { get; set; }

    }
}