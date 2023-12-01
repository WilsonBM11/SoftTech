using SoftTech.Models.DTO;

namespace SoftTech.Repositories.Abstract
{
    public interface IUserAuthenticationService
    {
        Task<Status> LoginAsync(LoginModel model);
        Task<Status> RegistrationAsync(RegistrationModel model);
        Task<Status> EditAsync(RegistrationModel model);
        Task<Status> RemoveAsync(string id);
        Task LogoutAsync();

    }
}
