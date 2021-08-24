using System.ComponentModel.DataAnnotations;

namespace API._DTOs
{
    public class RegisterDto
    {
        [Required]
        public string UserName { get; set; }

        [MinLength(8)]
        [Required]
        public string Password { get; set; }
    }
}