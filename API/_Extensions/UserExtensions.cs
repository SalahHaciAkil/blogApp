using System.Security.Claims;

namespace API._Extensions
{
    public static class UserExtensions
    {
        public static string GetUserName(this ClaimsPrincipal User)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier).Value; // Name represend uniqueName
            return username;
        }
    }
}