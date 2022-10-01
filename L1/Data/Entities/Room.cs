namespace L1.Data.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public int Number { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }

        public Floor Floor { get; set; }
    }
}
