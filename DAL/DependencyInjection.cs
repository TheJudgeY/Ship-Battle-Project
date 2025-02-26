using Microsoft.Extensions.DependencyInjection;
using DAL.Abstractions;
using DAL.Repositories;
using DAL.Storage;
using Core.Entities;

namespace DAL
{
    public static class DependencyInjection
    {
        public static void AddDataAccessServices(this IServiceCollection services)
        {
            services.AddSingleton<IDataStore<Ship>>(provider => new JsonDataStore<Ship>("ships.json"));
            services.AddSingleton<IRepository<Ship>>(provider => new JsonRepository<Ship>(provider.GetRequiredService<IDataStore<Ship>>()));
        }
    }
}
