using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace API._DTOs
{
    public class RegisterDto
    {
        [Required] public string UserName { get; set; }
        [Required] public string KnownAs { get; set; }
        [Required] public string Email { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 4)]
        public string Password { get; set; }

        public IFormFile Photo { get; set; }

    }
}