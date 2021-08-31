using Microsoft.AspNetCore.Http;

namespace API._DTOs
{
    public class CreatePostDto
    {
        public string PostTitle { get; set; }
        public string PostContent { get; set; }
        public IFormFile  Photo { get; set; }
    }
}