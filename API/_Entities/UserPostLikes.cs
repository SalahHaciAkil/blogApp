namespace API._Entities
{
    public class UserPostLikes
    {
        public string PostrName { get; set; }
        public int PostrId { get; set; }
        public AppUser User { get; set; }
        public int UserId { get; set; }
        public Post Post { get; set; }
        public int PostId { get; set; }
        public bool Read { get; set; } = false;
    }
}