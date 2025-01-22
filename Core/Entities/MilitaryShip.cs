namespace Core.Entities
{
    public class MilitaryShip : Ship
    {
        internal MilitaryShip(int speed, int length, Point position, int range) : base(speed, length, position, range)
        {
            Type = "Military";
        }

        public void Shoot()
        {
            Console.WriteLine($"Military ship at {Position} is shooting!");
        }
    }
}
