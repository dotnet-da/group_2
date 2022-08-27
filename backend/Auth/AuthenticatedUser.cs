//AuthenticatedUser.cs
using System.Security.Principal;
public class AuthenticatedUser : IIdentity
{
    public AuthenticatedUser(string authenticationType, bool isAuthenticated, string name, string role)
    {
        AuthenticationType = authenticationType;
        IsAuthenticated = isAuthenticated;
        Name = name;
        Role = role;
    }

    public string AuthenticationType { get; }

    public bool IsAuthenticated { get; }

    public string Name { get; }

    public string Role { get;  }
}