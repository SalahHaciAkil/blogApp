using System;

namespace API._DTOs
{
    public class UserPostLikesDto
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public DateTime LikedTime { get; set; }
        public string UserPhoto { get; set; }
        public string PostrName { get; set; }
        public int PostrId { get; set; }
        public string UserName { get; set; }

    }
}