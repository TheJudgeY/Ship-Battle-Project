using Microsoft.Extensions.DependencyInjection;
using BLL.Abstractions.Services;
using BLL.Abstractions.Factories;
using BLL.Abstractions.Helpers;
using UI.Rendering;
using BLL.Factories;
using BLL.Helper;
using BLL.Services;

namespace UI
{
    public static class Startup
    {
        public static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IGameService, GameService>();

            services.AddSingleton(provider => provider.GetRequiredService<IFieldService>());

            services.AddSingleton<IShipFactory, DefaultShipFactory>();

            services.AddSingleton<IShipHelper, ShipHelper>();

            services.AddSingleton<FieldRenderer>();

            return services.BuildServiceProvider();
        }
    }
}
