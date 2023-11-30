using SoftTech.Models.Domain;

namespace SoftTech.Models
{
    public class UserInformation
    {
        public ApplicationUser User { get; set; }
        public List<string> Roles { get; set; }
    }
}
