using API._Entities;

namespace API._Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}