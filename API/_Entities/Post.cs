using System;

namespace API._Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string PosetrName { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public string Photo { get; set; }
        public AppUser User { get; set; }
    }
}