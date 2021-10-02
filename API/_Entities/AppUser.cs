using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace API._Entities
{
    public class AppUser : IdentityUser<int>
    {

        public string KnownAs { get; set; }

        public string Photo { get; set; }
        public string PhotoPublicId { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<UserPostLikes> LikesPost { get; set; }
        public ICollection<UserPostComment> Comments { get; set; }
        public ICollection<AppUserRole> UserRole { get; set; }
        // public ICollection<UserActivities> SourceActivites { get; set; }
        // public ICollection<UserActivities> DestinationActivites { get; set; }

    }
}