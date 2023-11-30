using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SoftTech.Models.Domain
{
    public class DataBaseSecurityContext : IdentityDbContext<ApplicationUser>
    {
        public DataBaseSecurityContext(DbContextOptions<DataBaseSecurityContext> options) : base(options)
        {

        }

    }
}
