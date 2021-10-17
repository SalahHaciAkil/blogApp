using System;

namespace API._Entities
{
    public class UserPostLikes
    {
        public string PostrName { get; set; }
        public int PostrId { get; set; }
        public AppUser User { get; set; }
        public int UserId { get; set; }

        public string UserPhoto { get; set; }
        public Post Post { get; set; }
        public int PostId { get; set; }
        public string UserName { get; set; }
        public DateTime LikedTime { get; set; } = DateTime.UtcNow;
        public bool Read { get; set; } = false;
    }
}