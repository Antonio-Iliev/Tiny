using Authentication.Common.Abstractions;
using Authentication.Services;
using Authentication.Utils;
using Microsoft.Extensions.DependencyInjection;
using Slot.Common.Abstractions;
using Slot.Games;
using Tiny.Utils;
using Wallet.Common.Abstracts;
using Wallet.Services;

namespace Tiny.DI;

/// <summary>
/// Dependency Injection container.
/// </summary>
public class DiContainer
{
    public DiContainer()
    {
        var configurations = ConfigureServices();
        Container = configurations.BuildServiceProvider();
    }

    public ServiceProvider Container { get; }

    /// <summary>
    /// Configures the dependency injection services.
    /// </summary>
    private ServiceCollection ConfigureServices()
    {
        var serviceProvider = new ServiceCollection();

        // Register application-level services.
        serviceProvider.AddSingleton<Application>();
        serviceProvider.AddSingleton<CommandParser>();
        serviceProvider.AddSingleton<IWalletService, WalletService>();
        serviceProvider.AddSingleton<IAuthenticationService, AuthenticationService>();

        // Register transient services that will be created each time they are requested.
        serviceProvider.AddTransient<IDataProvider, DataProvider>();
        serviceProvider.AddTransient<IHasher, BasicHasher>();
        serviceProvider.AddTransient<ISlotGame, BasicSlotGame>();

        return serviceProvider;
    }
}