namespace SoftTech.Models.Domain
{
    public class UserInformation
    {
        public ApplicationUser User { get; set; }
        public List<string> Roles { get; set; }
    }
}
