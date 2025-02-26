using Microsoft.Extensions.DependencyInjection;
using BLL.Abstractions.Services;
using BLL.Abstractions.Factories;
using BLL.Abstractions.Helpers;
using UI.Rendering;
using CompositionRoot;

namespace UI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Ship Battle!");

            var serviceProvider = DependencyInjection.ConfigureServices();

            var gameService = serviceProvider.GetRequiredService<IGameService>();
            var shipFactory = serviceProvider.GetRequiredService<IShipFactory>();
            var shipHelper = serviceProvider.GetRequiredService<IShipHelper>();

            var fieldRenderer = new FieldRenderer(shipHelper);
            var gameFlowManager = new GameFlowManager(gameService, shipFactory, shipHelper, fieldRenderer);

            gameFlowManager.StartGame();
        }
    }
}
