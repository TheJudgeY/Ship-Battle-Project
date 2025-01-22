namespace Core.Entities
{
    public struct Point
    {
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public double DistanceFromCenter()
        {
            return Math.Sqrt(X * X + Y * Y);
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}
