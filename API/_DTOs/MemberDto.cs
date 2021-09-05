using System.Collections.Generic;

namespace API._DTOs
{
    public class MemberDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string KnownAs { get; set; }
        public string Photo { get; set; }
        public string PhotoPublicId { get; set; }
        public ICollection<PostDto> Posts { get; set; }
        public ICollection<UserPostCommentDto> Comments { get; set; }
    }
}