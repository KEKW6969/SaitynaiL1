namespace L1.Data.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public int Number { get; set; }

        public Floor Floor { get; set; }
    }
}
