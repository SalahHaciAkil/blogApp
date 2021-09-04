namespace API._Entities
{
    public class UserPostLikes
    {
        public int UserId { get; set; }
        public string PostrName { get; set; }
        public AppUser User { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}