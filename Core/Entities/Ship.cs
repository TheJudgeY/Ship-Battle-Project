namespace Core.Entities
{
    public abstract class Ship
    {
        public string Type { get; protected set; }
        public int Speed { get; protected set; }
        public int Length {  get; protected set; }
        public Point Position { get; protected set; }
        public int Range { get; protected set; }

        protected Ship(int speed, int length, Point position, int range) 
        {
            Speed = speed;
            Length = length;
            Position = position;
            Range = range;
        }

        public void Move(Point newPosition) 
        {
            Position = newPosition;
        }

        public override string ToString()
        {
            return $"{Type} Ship at {Position}, Speed: {Speed}, Length: {Length}";
        }
    }
}
