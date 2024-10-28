using Authentication.Common.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Tiny.Common;
using Tiny.DI;

namespace Tiny;

internal class Program
{
    private static async Task Main(string[] args)
    {
        // 1. Initialization stage.
        var diProvider = new DiContainer();

        var authentication = diProvider.Container.GetRequiredService<IAuthenticationService>();
        await authentication.InitializeAsync(Constants.DataPath);

        // 2. Start application.
        var app = diProvider.Container.GetRequiredService<Application>();
        await app.Run();
    }
}