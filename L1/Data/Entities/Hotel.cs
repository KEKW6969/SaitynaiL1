using L1.Auth.Model;
using System.ComponentModel.DataAnnotations;

namespace L1.Data.Entities
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        [Required]
        public string UserId { get; set; }
        public HotelRestUser User { get; set; }
    }
}
