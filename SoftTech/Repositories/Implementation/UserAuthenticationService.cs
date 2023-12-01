using Microsoft.AspNetCore.Identity;
using SoftTech.Data;
using SoftTech.Models;
using SoftTech.Models.Domain;
using SoftTech.Models.DTO;
using SoftTech.Repositories.Abstract;
using System.Security.Claims;

namespace SoftTech.Repositories.Implementation
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        TestUCRContext db = new TestUCRContext();

        public UserAuthenticationService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.signInManager = signInManager;

        }
        public async Task<Status> LoginAsync(LoginModel model)
        {
            var status = new Status();
            var user = await userManager.FindByNameAsync(model.UserName);
            var userX = new List<ApplicationUser>();
            if (user == null)
            {
                status.StatusCode = 0;
                status.Message = "Invalid UserName";
                return status;
            }
            if (!await userManager.CheckPasswordAsync(user, model.Password))
            {
                status.StatusCode = 0;
                status.Message = "Invalid Password";
                return status;
            }

            var signInResult = await signInManager.PasswordSignInAsync(user, model.Password, false, true);
            if (signInResult.Succeeded)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName)

                };
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                status.StatusCode = 1;
                status.Message = "Logged In Successfully";
                return status;
            }
            else if (signInResult.IsLockedOut)
            {
                status.StatusCode = 0;
                status.Message = "User Locked Out";
                return status;
            }
            else
            {
                status.StatusCode = 0;
                status.Message = "Error On Loggin In";
                return status;
            }
        }

        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<Status> RegistrationAsync(RegistrationModel model)
        {
            var status = new Status();
            var userExists = await userManager.FindByNameAsync(model.UserName);
            if (userExists != null)
            {
                status.StatusCode = 0;
                status.Message = "User Already Exists";
                return status;
            }

            ApplicationUser user = new ApplicationUser
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                Name = model.Name,
                Email = model.Email,
                UserName = model.UserName,
                EmailConfirmed = true,

            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                status.StatusCode = 0;
                status.Message = "User Creation Failed";
                return status;
            }

            //Role Managment
            if (!await roleManager.RoleExistsAsync(model.Role))
            {
                await roleManager.CreateAsync(new IdentityRole(model.Role));
            }

            if (await roleManager.RoleExistsAsync(model.Role))
            {
                await userManager.AddToRoleAsync(user, model.Role);
            }

            var client = new Client
            {
                id = user.Id,
                name = model.Name,
                email = model.Email,
                phone_number = model.Phone_Number,
                address = model.Address,
                userName=model.UserName
            };
            db.Client.Add(client);
            db.SaveChanges();

            status.StatusCode = 1;
            status.Message = "User Has Registered Successfully";
            return status;
        }

        //edit
        public async Task<Status> EditAsync(RegistrationModel model)
        {
            var status = new Status();
            var userExists = await userManager.FindByNameAsync(model.UserName);
            if (userExists == null)
            {
                status.StatusCode = 0;
                status.Message = "User doesn't exist";
                return status;
            }

            userExists.UserName = model.UserName;
            userExists.Email = model.Email;
            userExists.Name = model.Name;
            userExists.Address = model.Address;
            userExists.Phone_Number = model.Phone_Number;

            if (!string.IsNullOrEmpty(model.Password))
            {
                userExists.PasswordHash = userManager.PasswordHasher.HashPassword(userExists, model.Password);
            }
            var result = await userManager.UpdateAsync(userExists); //updated user

            if (result.Succeeded)
            {
                status.StatusCode = 1;
                status.Message = "User updated successfully";
            }
            else
            {
                // Error 
                status.StatusCode = 0;
                status.Message = "Error updating user";
            }
            return status;
        }

        //delete
        public async Task<Status> RemoveAsync(string id)
        {
            var status = new Status();
            var userExists = await userManager.FindByIdAsync(id);
            if (userExists == null)
            {
                status.StatusCode = 0;
                status.Message = "User doesn't exist";
                return status;
            }

            var result = await userManager.DeleteAsync(userExists); //deleted user
            if (result.Succeeded)
            {
                status.StatusCode = 1;
                status.Message = "User deleted successfully";
            }
            else
            {
                // Error 
                status.StatusCode = 0;
                status.Message = "Error deleting user";
            }
            return status;
        }
    }
}
