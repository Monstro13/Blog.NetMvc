using System;
using System.Web;

namespace iBlog
{
    public static class SessionManager
    {
        public static Int32 GetUserId()
        {
            const String userIdField = "UserId";
            var context = HttpContext.Current;
            var user = context.User.Identity;

            if (user.IsAuthenticated)
            {
                if (context.Session[userIdField] != null)
                {
                    return Int32.Parse(context.Session[userIdField].ToString());
                }
                else
                {
                    Int32 userId = 0;
                    if (Int32.TryParse(user.Name, out userId))
                    {
                        context.Session[userIdField] = userId;
                        return userId;
                    }
                }                
            }

            return 0;
        }
    }
}