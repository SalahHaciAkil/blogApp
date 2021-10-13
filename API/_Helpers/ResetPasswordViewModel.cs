using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API._Helpers
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string UserId { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 4)]
        public string NewPassword { get; set; }
    }
}