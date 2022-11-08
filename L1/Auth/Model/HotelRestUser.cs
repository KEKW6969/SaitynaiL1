using Microsoft.AspNetCore.Identity;

namespace L1.Auth.Model
{
    public class HotelRestUser : IdentityUser
    {
        [PersonalData]
        public string? AdditionalInfo { get; set; }
    }
}
