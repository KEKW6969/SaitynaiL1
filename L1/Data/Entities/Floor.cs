namespace L1.Data.Entities
{
    public class Floor
    {
        public int Id { get; set; }
        public int Number { get; set; }

        public Hotel Hotel { get; set; }
    }
}
