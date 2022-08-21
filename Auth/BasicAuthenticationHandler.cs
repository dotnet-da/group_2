// BasicAuthenticationHandler.cs
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Text;
using System;
using System.Security.Claims;
using backend;
using Microsoft.AspNetCore.Mvc;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock
        )
: base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        Response.Headers.Add("WWW-Authenticate", "Basic");

        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return Task.FromResult(AuthenticateResult.Fail("Authorization header missing."));
        }

        // Get authorization key
        var authorizationHeader = Request.Headers["Authorization"].ToString();
        var authHeaderRegex = new Regex(@"Basic (.*)");

        if (!authHeaderRegex.IsMatch(authorizationHeader))
        {
            return Task.FromResult(AuthenticateResult.Fail("Authorization code not formatted properly."));
        }

        var authBase64 = Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderRegex.Replace(authorizationHeader, "$1")));
        var authSplit = authBase64.Split(Convert.ToChar(":"), 2);
        var authUsername = authSplit[0];
        var authPassword = authSplit.Length > 1 ? authSplit[1] : throw new Exception("Unable to get password");


        /////////////////////////////////////////////////////////
        //if (authUsername != "admin" || authPassword != "1234")
        //{
        //    return Task.FromResult(AuthenticateResult.Fail("The username or password is not correct."));
        //}
        /////////////////////////////////////////////////////////


        //MY SOLUTION
        ConnectionStringStorer c = ConnectionStringStorer.Instance;
        Database d = new Database(c.ConnectionString);

        Login login = new Login(d);
        d.Connection.Open();  //Needed to do this explicitly
        var passwordFromDatabase = login.GetPassword(authUsername).Result;  //await entfernt
        d.Connection.Close();
        AuthenticatedUser authenticatedUser;
        ClaimsPrincipal claimsPrincipal;

        if (passwordFromDatabase != null && BCrypt.Net.BCrypt.Verify(authPassword, passwordFromDatabase)) //Inverted condition for checking for correct and not for false
        {
            authenticatedUser = new AuthenticatedUser("BasicAuthentication", true, authUsername);
            claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(authenticatedUser));

            return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
        }
        else
        {
            if (authUsername != "admin" || authPassword != "1234")
            {
                return Task.FromResult(AuthenticateResult.Fail("The username or password is not correct."));
            }
            else
            {
                authenticatedUser = new AuthenticatedUser("BasicAuthentication", true, "admin");
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(authenticatedUser));
            }
        }
        //MY SOLUTION END


        return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
    }
}