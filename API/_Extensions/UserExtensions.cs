using System.Security.Claims;

namespace API._Extensions
{
    public static class UserExtensions
    {



        public static int GetUserId(this ClaimsPrincipal User)
        {
            var stringId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var id = int.Parse(stringId);
            return id;
        }

        public static string GetUserName(this ClaimsPrincipal User)
        {
            var username = User.FindFirst(ClaimTypes.Name).Value; // Name represend uniqueName
            return username;                                      // NameIdentifer represent NameId

        }
    }
}