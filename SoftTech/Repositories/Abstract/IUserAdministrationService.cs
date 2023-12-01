using SoftTech.Models;
using SoftTech.Models.Domain;
using SoftTech.Models.DTO;
using System.Security.Claims;

namespace SoftTech.Repositories.Abstract
{
    public interface IUserAdministrationService
    {
        Task<List<UserInformation>> GetUsersByRoleAsync(string role);
        Task<UserInformation> GetUserByIdAsync(string id);
        Task<Status> CreateAsync(RegistrationModel model);
        Task<Status> UpdateAsyncSA(UserInformation user);
        Task<Status> UpdateAsyncA(UserInformation user);
        Task<Status> DeleteAsync(ApplicationUser user);
        Task<UserInformation> GetUserByEmailAsync(string email);
        Task<ApplicationUser> GetUserAsync(ClaimsPrincipal current_user);
    }
}
