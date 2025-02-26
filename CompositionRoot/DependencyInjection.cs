using Microsoft.Extensions.DependencyInjection;
using BLL;
using DAL;

namespace CompositionRoot
{
    public static class DependencyInjection
    {
        public static ServiceProvider ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDataAccessServices();

            serviceCollection.AddBusinessLogicServices();

            return serviceCollection.BuildServiceProvider();
        }
    }
}