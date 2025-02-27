using BLL.Abstractions.Helpers;
using Core.Entities;

namespace UI.Rendering
{
    public class FieldRenderer
    {
        public readonly int FieldWidth = 10;
        public readonly int FieldHeight = 10;
        private readonly IShipHelper _shipHelper;

        public FieldRenderer(IShipHelper shipHelper)
        {
            _shipHelper = shipHelper;
        }

        public void RenderField(Field field, string title, bool isOpponentField = false)
        {
            Console.WriteLine($"\n{title}:");

            Console.Write("   ");
            for (int x = 0; x < FieldWidth; x++)
            {
                Console.Write(x + " ");
            }
            Console.WriteLine();

            for (int y = 0; y < FieldHeight; y++)
            {
                Console.Write(y.ToString().PadLeft(2) + " ");

                for (int x = 0; x < FieldWidth; x++)
                {
                    RenderCell(field, x, y, isOpponentField);
                }
                Console.WriteLine();
            }
        }

        private void RenderCell(Field field, int x, int y, bool isOpponentField)
        {
            foreach (var ship in field.Ships)
            {
                foreach (var point in _shipHelper.GetOccupiedPoints(ship))
                {
                    if (point.X == x && point.Y == y)
                    {
                        if (isOpponentField && ship.Health == Core.Enums.HealthStage.FullHealth)
                        {
                            Console.Write("~ ");
                            return;
                        }

                        SetShipColor(ship.Health);
                        Console.Write(GetShipSymbol(ship.Health) + " ");
                        Console.ResetColor();
                        return;
                    }
                }
            }

            Console.Write("~ ");
        }

        private void SetShipColor(Core.Enums.HealthStage health)
        {
            switch (health)
            {
                case Core.Enums.HealthStage.FullHealth:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case Core.Enums.HealthStage.Damaged:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case Core.Enums.HealthStage.Critical:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                default:
                    Console.ResetColor();
                    break;
            }
        }

        private char GetShipSymbol(Core.Enums.HealthStage health)
        {
            return health switch
            {
                Core.Enums.HealthStage.FullHealth => 'O',
                Core.Enums.HealthStage.Damaged => 'H',
                Core.Enums.HealthStage.Critical => 'C',
                _ => 'O'
            };
        }

    }
}