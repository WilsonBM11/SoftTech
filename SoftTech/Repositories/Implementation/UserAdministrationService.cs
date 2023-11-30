using Microsoft.AspNetCore.Identity;
using SoftTech.Data;
using SoftTech.Models;
using SoftTech.Models.Domain;
using SoftTech.Models.DTO;
using SoftTech.Repositories.Abstract;

namespace SoftTech.Repositories.Implementation
{
    public class UserAdministrationService : IUserAdministrationService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        TestUCRContext db = new TestUCRContext();

        public UserAdministrationService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<List<UserInformation>> GetUsersByRoleAsync(string role)
        {
            var userIList = new List<UserInformation>();
            var userList = (List<ApplicationUser>)await userManager.GetUsersInRoleAsync(role);
            foreach (var user in userList)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                userIList.Add(new UserInformation() { User= user, Roles = (List<string>) userRoles});
            }
            return userIList;
        }
        public async Task<UserInformation> GetUserByIdAsync(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            var userRoles = await userManager.GetRolesAsync(user);
            return new UserInformation() { User = user, Roles = (List<string>) userRoles};
        }

        public async Task<Status> UpdateAsyncSA(UserInformation user)
        {
            var model = await userManager.FindByIdAsync(user.User.Id);
            model.Name = user.User.Name;
            model.UserName = user.User.UserName;
            model.Email = user.User.Email;
            model.Phone_Number= user.User.Phone_Number;
            model.Address = user.User.Address;

            var status = new Status();
            var update = await userManager.UpdateAsync(model);
            if (!update.Succeeded)
            {
                status.StatusCode = 0;
                status.Message = "User Update Failed";
                return status;
            }

            var userRoles = await userManager.GetRolesAsync(model);
            var removeRoles = await userManager.RemoveFromRolesAsync(model, userRoles);
            //Roles Managment
            
            if (removeRoles.Succeeded)
            {
                if (!await roleManager.RoleExistsAsync(user.Roles[0]))
                {
                    await roleManager.CreateAsync(new IdentityRole(user.Roles[0]));
                }

                if (await roleManager.RoleExistsAsync(user.Roles[0]))
                {
                    await userManager.AddToRoleAsync(model, user.Roles[0]);
                }
            }

            status.StatusCode = 1;
            status.Message = "User Has Updated Successfully";
            return status;
        }
        public async Task<Status> DeleteAsync(ApplicationUser user)
        {
            var status = new Status();
            var model = await userManager.FindByIdAsync(user.Id);
            var delete = await userManager.DeleteAsync(model);
            if (!delete.Succeeded)
            {
                status.StatusCode = 0;
                status.Message = "User Delete Failed";
                return status;
            }
            status.StatusCode = 1;
            status.Message = "User Has Deleted Successfully";
            return status;
        }

        public async Task<Status> CreateAsync(RegistrationModel model)
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
                Phone_Number = model.Phone_Number,
                Address = model.Address,
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

            //guardar en BD
            var client = new Client
            {
                client_name = model.Name,
                email = model.Email,
                tel = model.Phone_Number,
                dir = model.Address
            };
            db.Client.Add(client);
            db.SaveChanges();

            status.StatusCode = 1;
            status.Message = "User Has Registered Successfully";
            return status;
        }

        public async Task<Status> UpdateAsyncA(UserInformation user)
        {
            var model = await userManager.FindByIdAsync(user.User.Id);
            model.Name = user.User.Name;
            model.UserName = user.User.UserName;
            model.Email = user.User.Email;
            model.Phone_Number = user.User.Phone_Number;
            model.Address = user.User.Address;

            var status = new Status();
            var update = await userManager.UpdateAsync(model);
            if (!update.Succeeded)
            {
                status.StatusCode = 0;
                status.Message = "User Update Failed";
                return status;
            }

            status.StatusCode = 1;
            status.Message = "User Has Updated Successfully";
            return status;
        }
    }
}
