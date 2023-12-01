using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SoftTech.Data;
using SoftTech.Models;
using SoftTech.Models.Domain;
using SoftTech.Models.DTO;
using SoftTech.Repositories.Abstract;

namespace SoftTech.Controllers
{
    public class UserAuthenticationController : Controller
    {
        //Se instancia el servicio de UserAuthentication y el UserManager
        private readonly IUserAuthenticationService _service;
        private readonly UserManager<ApplicationUser> _userManager;
        TestUCRContext db = new TestUCRContext();

        public UserAuthenticationController(IUserAuthenticationService service, UserManager<ApplicationUser> userManager)
        {
            this._service = service;
            this._userManager = userManager;
        }

        public IActionResult Authentication()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.Role = "client";
            //Crea el usuario
            var result = await _service.RegistrationAsync(model);

            TempData["msg"] = result.Message;       

            return RedirectToAction(nameof(Authentication));
        }

        //Valida las credenciales del usuario y le permite loguearse
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //Utiliza el servicio de UserAuthentication para validar el logueo
            var result = await _service.LoginAsync(model);
            if (result.StatusCode == 1)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["msg"] = result.Message;
                return RedirectToAction(nameof(Authentication));
            }
        }

        //Permite al usuario desloguearse
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _service.LogoutAsync();
            return RedirectToAction(nameof(Authentication));
        }

        //Registro de administrador
        public async Task<IActionResult> RegAdmin()
        {
            var model = new RegistrationModel
            {
                UserName = "Admin",
                Name = "Administrador",
                Email = "fio.mn1911@gmail.com",
                Password = "Admin2023!",
            };
            model.Role = "admin";
            var result = await _service.RegistrationAsync(model);
            return Ok(result);
        }

    }
}
