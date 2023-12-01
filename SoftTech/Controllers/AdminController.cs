
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SoftTech.Data;
using SoftTech.Models;
using SoftTech.Models.Domain;
using SoftTech.Models.DTO;
using SoftTech.Repositories.Abstract;
using System.Security.Cryptography;

namespace SoftTech.Controllers
{
    //[Authorize(Roles = "admin")] //just admin can use this controller
    public class AdminController : Controller
    {
        private TestUCRContext db = new TestUCRContext(); //database context
        private readonly IUserAuthenticationService _service; //database context authentication
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(IUserAuthenticationService service, UserManager<ApplicationUser> userManager)
        {
            this._service = service;
            this._userManager = userManager;
        }


        // GET: AdminController        
        public async Task<ActionResult> Client_List()
        {
            List<Client> clients = db.Client.ToList();
            return View(clients);

        }

        // GET: AdminController/Details/5
        public ActionResult Details(string id)
        {
            using (var db = new TestUCRContext())
            {
                var client = db.Client.FirstOrDefault(c => c.id.Equals(id));

                if (client == null)
                {
                    return NotFound();
                }
                return View(client);
            }
        }


        // GET: AdminController/Create
        public ActionResult Create()
        {
            return View();
        }

        // Genera una contraseña aleatoria 
        public static string GenerateRandomPassword(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()-=_+";

            using (var cryptoProvider = new RNGCryptoServiceProvider())
            {
                byte[] data = new byte[length];
                cryptoProvider.GetBytes(data);

                var result = new char[length];
                for (int i = 0; i < length; i++)
                {
                    result[i] = chars[data[i] % chars.Length];
                }

                return new string(result);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Create(Client client, RegistrationModel model)
        {
            try
            {
                model.Role = "client";
                model.Password = GenerateRandomPassword(8);
                model.PasswordConfirm = model.Password;
                
                var result = await _service.RegistrationAsync(model); 
                return RedirectToAction(nameof(Client_List));
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        //edit user client
        public async Task<ActionResult> Edit(string id)
        {
            Client client = db.Client.FirstOrDefault(c => c.id.Equals(id));
            return View(client);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Client client, RegistrationModel model)
        {
            try
            {
                var result = await _service.EditAsync(model);
                db.Client.Update(client); //save clients in testUCR
                db.SaveChanges();
                return RedirectToAction(nameof(Client_List));
            }
            catch (Exception ex)
            {
                return View();
            }
        }


        // GET: AdminController/Delete/5
        public ActionResult Delete(string id)
        {
            Client client = db.Client.FirstOrDefault(c => c.id.Equals(id));
            return View(client);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Client client, RegistrationModel model)
        {
            try
            {                
                var result = await _service.RemoveAsync(client.id);
                db.Client.Remove(client);
                db.SaveChanges();
                return RedirectToAction(nameof(Client_List));
            }
            catch (Exception ex)
            {
                return View();
            }

        }
    }
}
