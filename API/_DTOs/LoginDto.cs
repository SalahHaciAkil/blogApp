using System.ComponentModel.DataAnnotations;

namespace API._DTOs
{
    public class LoginDto
    {
        [Required]public string UserName { get; set; }



        [MinLength(6)]
        [MaxLength(12)]
        [Required]public string Password { get; set; }
    }
}