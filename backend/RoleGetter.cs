using System.Security.Claims;

namespace backend
{
    public static class RoleGetter
    {
        public static async Task<string> GetRoleFromClaimsPrincipal(ClaimsPrincipal claimsPrincipal)
        {
            string username = claimsPrincipal.Identity.Name;
            Database d = new Database(ConnectionStringStorer.Instance.ConnectionString);
            Login l = new Login(d);
            await d.Connection.OpenAsync();
            string userType = l.GetUserType(username).Result;
            await d.Connection.CloseAsync();
            return userType;
        }
    }
}
