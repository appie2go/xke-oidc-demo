using Microsoft.AspNetCore.Authentication;
using OpenIddict.Server.AspNetCore;

namespace OpenIdDictServer;

public static class Endpoints
{
    
    public static WebApplication MapLogoutEndpoint(this WebApplication app)
    {
        app.MapGet("~/connect/logout", () =>
        {
            var authenticationSchemes = new List<string>
            {
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme
            };

            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = "/"
            };
            
            var result = Results.SignOut(authenticationProperties, authenticationSchemes);
            return Task.FromResult(result);
        });

        return app;
    }

}