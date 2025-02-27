using Microsoft.Extensions.DependencyInjection;
using BLL.Abstractions.Services;
using BLL.Abstractions.Helpers;
using BLL.Abstractions.Factories;
using BLL.Services;
using BLL.Factories;
using BLL.Helper;
using Core.Entities;
using Core.Enums;
using DAL.Abstractions;

namespace BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
        {
            services.AddSingleton<IShipHelper, ShipHelper>();

            services.AddSingleton<IShipFactory, DefaultShipFactory>();

            services.AddSingleton<IFieldService>(provider =>
                CreateFieldService(Player.Player1, provider));

            services.AddSingleton<IFieldService>(provider =>
                CreateFieldService(Player.Player2, provider));

            services.AddSingleton<IGameService>(provider =>
            {
                var fieldServices = provider.GetServices<IFieldService>().ToList();
                var dataStore = provider.GetRequiredService<IDataStore<Dictionary<string, List<Ship>>>>();

                var player1FieldService = fieldServices[0];
                var player2FieldService = fieldServices[1];

                return new GameService(player1FieldService, player2FieldService, dataStore);
            });

            return services;
        }

        private static IFieldService CreateFieldService(Player player, IServiceProvider provider)
        {
            var shipHelper = provider.GetRequiredService<IShipHelper>();
            var shipRepository = provider.GetRequiredService<IRepository<Ship>>();

            return new FieldService(player, new Field(), shipHelper, shipRepository);
        }
    }
}