using System.ComponentModel.DataAnnotations;

namespace API._DTOs
{
    public class CreateCommentDto
    {

        [Required]
        public int PostId { get; set; }

        [Required]
        public string Comment { get; set; }
    }
}