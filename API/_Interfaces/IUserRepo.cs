using System.Collections.Generic;
using System.Threading.Tasks;
using API._Entities;

namespace API._Interfaces
{
    public interface IUserRepo
    {
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserAsync(string userName);

        Task AddUserPostLike(UserPostLikes userPostLikes);

        // Task<bool> SaveChangesAsync();
    }
}