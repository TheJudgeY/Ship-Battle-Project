namespace Core.Entities
{
    public class MixedShip : Ship
    {
        internal MixedShip(int speed, int length, Point position, int range) : base(speed, length, position, range) 
        {
            Type = "Mixed";
        }
        public void Shoot()
        {
            Console.WriteLine($"Mixed ship at {Position} is shooting!");
        }

        public void Repair()
        {
            Console.WriteLine($"Mixed ship at {Position} is repearing!");
        }
    }
}
