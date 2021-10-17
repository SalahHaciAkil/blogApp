using System;

namespace API._Entities
{
    public class UserPostComment
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public DateTime CommentTime { get; set; } = DateTime.UtcNow;
        public AppUser User { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }

        public string UserPhoto { get; set; }
        public Post Post { get; set; }
        public int PostId { get; set; }

        public int PostrId { get; set; }
        public string PostrName { get; set; }
        public bool Read { get; set; } = false;

    }
}