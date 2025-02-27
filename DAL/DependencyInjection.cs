using DAL.Abstractions;
using DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Core.Entities;

namespace DAL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccessServices(this IServiceCollection services)
        {
            services.AddSingleton<IDataStore<Dictionary<string, List<Ship>>>>(provider =>
                new JsonDataStore<Dictionary<string, List<Ship>>>("game_state.json"));

            services.AddSingleton<IRepository<Ship>>(provider =>
                new JsonRepository<Ship>(provider.GetRequiredService<IDataStore<Dictionary<string, List<Ship>>>>()));

            return services;
        }
    }
}