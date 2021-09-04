using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API._Entities
{
    public class AppUser
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Email { get; set; }
        public string KnownAs { get; set; }

        public string Photo { get; set; }
        public string PhotoPublicId { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<UserPostLikes> LikesPost { get; set; }
        public ICollection<UserPostComments> Comments { get; set; }

    }
}