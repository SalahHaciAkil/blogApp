using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace API._Entities
{

    public class AppRole : IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRole { get; set; }    
    }
}