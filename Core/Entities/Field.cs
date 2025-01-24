namespace Core.Entities
{
    public class Field
    {
        public List<Ship> Ships { get; }

        public Field() 
        { 
            Ships = new List<Ship>();
        }
    }
}
