using System.Security.Claims;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace OpenIdDictServer.Controllers;

public class AuthorizeController : Controller
{
    private readonly IOpenIddictScopeManager _manager;

    public AuthorizeController(IOpenIddictScopeManager manager)
    {
        _manager = manager;
    }
    
    [HttpGet("/connect/authorize")]
    public async Task<IActionResult> Get()
    {
        var sub = HttpContext.Request.Query["subject"];
        if (string.IsNullOrEmpty(sub))
        {
            return View("Index");
        }
        
        var request = HttpContext.GetOpenIddictServerRequest();
        var identity = CreateClaimsIdentity(sub);
        identity.SetScopes(request.GetScopes());
            
        var resources = await _manager.ListResourcesAsync(identity.GetScopes()).ToListAsync();
        identity.SetResources(resources);
        identity.SetDestinations(c => new [] { OpenIddictConstants.Destinations.AccessToken });

        return SignIn(new ClaimsPrincipal(identity), properties: null, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
    
    private static ClaimsIdentity CreateClaimsIdentity(string sub)
    {
        var claims = new[]
        {
            new Claim(OpenIddictConstants.Claims.Subject, sub),
            new Claim(OpenIddictConstants.Claims.Name, sub)
                .SetDestinations(OpenIddictConstants.Destinations.AccessToken),
            new Claim(OpenIddictConstants.Claims.PreferredUsername, sub)
                .SetDestinations(OpenIddictConstants.Destinations.AccessToken)
        };

        var identity = new ClaimsIdentity(TokenValidationParameters.DefaultAuthenticationType,
            OpenIddictConstants.Claims.Name,
            OpenIddictConstants.Claims.Role);

        identity.AddClaims(claims);
        return identity;
    }
}