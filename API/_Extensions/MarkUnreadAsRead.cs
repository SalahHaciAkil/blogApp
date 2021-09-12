using System.Linq;
using API._Entities;

namespace API._Extensions
{
    public static class MarkUnreadAsRead
    {
        public static IQueryable<UserPostLikes> MarkPostLikeRead(this IQueryable<UserPostLikes> query)
        {
            if (query.Any())
            {
                foreach (var activity in query)
                {
                    activity.Read = true;
                }
            }

            return query;
        }
        public static IQueryable<UserPostComment> MarkPostCommentRead(this IQueryable<UserPostComment> query)
        {
            if (query.Any())
            {
                foreach (var activity in query)
                {
                    activity.Read = true;
                }
            }

            return query;
        }
    }
}