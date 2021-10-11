using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace API._DTOs
{
    public class CreatePostDto
    {
        [Required]
        public string PostTitle { get; set; }

        [Required]
        public string PostContent { get; set; }

        [Required]
        public string PostCategory { get; set; }

        public IFormFile Photo { get; set; }
    }
}