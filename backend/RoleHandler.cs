using Microsoft.AspNetCore.Mvc;

namespace backend
{
    public static class RoleHandler
    {
        public static ActionResult? OnlyThisRoleCanAccess(string roleWantingAccess, string roleWithAccess)
        {
            if(!roleWantingAccess.Equals(roleWithAccess))
            {
                return new ForbidResult();
            }
            return null;
        }
    }
}
