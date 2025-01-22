using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class AuxiliaryShip : Ship
    {
        internal AuxiliaryShip(int speed, int length, Point position, int range) : base(speed, length, position, range)
        {
            Type = "Auxiliary";
        }

        public void Repair()
        {
            Console.WriteLine($"Auxiliary ship at {Position} is repearing!");
        }
    }
}
