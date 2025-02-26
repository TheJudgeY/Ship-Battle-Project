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

        public void RenderField(Field field, string title)
        {
            Console.WriteLine($"\n{title}:");

            for (int y = 0; y < FieldHeight; y++)
            {
                for (int x = 0; x < FieldWidth; x++)
                {
                    var cell = GetCellRepresentation(field, x, y);
                    Console.Write(cell + " ");
                }
                Console.WriteLine();
            }
        }

        private char GetCellRepresentation(Field field, int x, int y)
        {
            foreach (var ship in field.Ships)
            {
                foreach (var point in _shipHelper.GetOccupiedPoints(ship))
                {
                    if (point.X == x && point.Y == y)
                    {
                        return ship.Health switch
                        {
                            Core.Enums.HealthStage.FullHealth => 'O',
                            Core.Enums.HealthStage.Damaged => 'H',
                            Core.Enums.HealthStage.Critical => 'C',
                            _ => 'O'
                        };
                    }
                }
            }
            return '~';
        }
    }
}