using System.Collections.Generic;

namespace API._DTOs
{
    public class MemberDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public ICollection<PostDto> Posts { get; set; }
    }
}