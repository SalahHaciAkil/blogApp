using System.Collections.Generic;

namespace API._Entities
{
    public class Blog
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}