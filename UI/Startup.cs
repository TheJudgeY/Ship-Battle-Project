using Microsoft.Extensions.DependencyInjection;
using CompositionRoot;

namespace UI
{
    public class Startup
    {
        public static ServiceProvider ConfigureServices()
        {
            return DependencyInjection.ConfigureServices();
        }
    }
}