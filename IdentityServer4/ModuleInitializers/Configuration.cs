using IdentityServer4;
using IdentityServer4.Models;

namespace TestIdentityServer.ModuleInitializers;

public static class Configuration
{
    public static readonly IEnumerable<IdentityResource> IdentityResources = new []
    {
        (IdentityResource)new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        new IdentityResources.Email()
    };
    
    public static readonly IEnumerable<ApiScope> ApiScopes =
        new[]
        {
            // local API scope
            new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
        };

    public static readonly Client Client = new Client
    {
        ClientId = "demo",

        ClientSecrets =
        {
            new Secret("secret".Sha256())
        },

        AllowedGrantTypes = GrantTypes.Implicit,
        AllowAccessTokensViaBrowser = true, 
        RequireConsent = false,

        AccessTokenLifetime = 75,
        AlwaysIncludeUserClaimsInIdToken = true,

        RedirectUris = { 
            "https://localhost:7185/redirect"
        },
        FrontChannelLogoutUri = "https://localhost:7185/",
        PostLogoutRedirectUris = { "https://localhost:7185/" },

        AllowedScopes = 
        {
            IdentityServerConstants.StandardScopes.OpenId,
            IdentityServerConstants.StandardScopes.Profile,
            IdentityServerConstants.StandardScopes.Email,
            IdentityServerConstants.LocalApi.ScopeName
        }
    };
}