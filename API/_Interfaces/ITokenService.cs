using System.Threading.Tasks;
using API._Entities;

namespace API._Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}