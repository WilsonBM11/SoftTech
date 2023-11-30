using Microsoft.AspNetCore.Identity;

namespace SoftTech.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Address { get; set; }
        public string? Phone_Number { get; set; }
    }
}
