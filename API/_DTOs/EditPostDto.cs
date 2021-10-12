using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace API._DTOs
{
    public class EditPostDto
    {

        [Required]
        public string Id { get; set; }

        // public bool IsPhotoEdited { get; set; } = false;

        public string PostTitle { get; set; }

        public string PostContent { get; set; }

        public string PostCategory { get; set; }

        public IFormFile Photo { get; set; }
    }
}