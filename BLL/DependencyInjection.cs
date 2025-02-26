using Microsoft.Extensions.DependencyInjection;
using BLL.Abstractions.Factories;
using BLL.Abstractions.Helpers;
using BLL.Abstractions.Services;
using BLL.Factories;
using BLL.Helper;
using BLL.Services;
using DAL.Abstractions;
using Core.Entities;

namespace BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
        {
            services.AddSingleton<IShipHelper, ShipHelper>();

            services.AddSingleton<IShipFactory, DefaultShipFactory>();

            services.AddTransient<IFieldService, FieldService>(provider =>
            {
                var shipHelper = provider.GetRequiredService<IShipHelper>();
                var shipRepository = provider.GetRequiredService<IRepository<Ship>>();
                return new FieldService(new Field(), shipHelper, shipRepository);
            });

            services.AddSingleton<IGameService, GameService>(provider =>
            {
                var player1FieldService = provider.GetRequiredService<IFieldService>();
                var player2FieldService = provider.GetRequiredService<IFieldService>();
                return new GameService(player1FieldService, player2FieldService);
            });

            return services;
        }
    }
}
