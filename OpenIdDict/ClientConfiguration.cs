using OpenIddict.Abstractions;

namespace OpenIdDictServer;

public class ClientConfiguration : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public ClientConfiguration(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.EnsureCreatedAsync(cancellationToken);
        
        await EnsureClientCreated(scope, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    
    private static async Task EnsureClientCreated(AsyncServiceScope scope, CancellationToken cancellationToken)
    {
        const string clientId = "demo";
        
        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

        // <Hacky hacky>
        var client = await manager.FindByClientIdAsync(clientId, cancellationToken);
        if (client != null)
        {
            await manager.DeleteAsync(client, cancellationToken);
        }
        // </ Hacky hacky>

        if (await manager.FindByClientIdAsync(clientId, cancellationToken) == null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = clientId,
                ClientType = OpenIddictConstants.ClientTypes.Public,
                RedirectUris =
                {
                    new Uri("https://localhost:7185/redirect")
                },
                PostLogoutRedirectUris =
                {
                    new Uri("https://localhost:7185/")
                },
                
                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Authorization,
                    OpenIddictConstants.Permissions.Endpoints.Logout,
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.GrantTypes.Implicit,
                    OpenIddictConstants.Permissions.ResponseTypes.IdToken,
                    OpenIddictConstants.Permissions.ResponseTypes.IdTokenToken,
                    OpenIddictConstants.Permissions.Scopes.Email,
                    OpenIddictConstants.Permissions.Scopes.Profile,
                    OpenIddictConstants.Permissions.ResponseTypes.Token,
                    OpenIddictConstants.Scopes.Email,
                    OpenIddictConstants.Scopes.Profile
                }
            }, cancellationToken);
        }
    }
}