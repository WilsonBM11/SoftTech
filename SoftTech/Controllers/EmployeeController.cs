using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftTech.Data;
using SoftTech.Models.Entites;
using SoftTech.Repositories.Abstract;
using SoftTech.Utilities;
using System.Data;

namespace SoftTech.Controllers
{
    [Authorize(Roles = "human_resources")]
    public class EmployeeController : Controller
	{
        private readonly IUserAdministrationService _service;
        private DataContext db = new DataContext();

        public EmployeeController(IUserAdministrationService service)
        {
            this._service = service;
        }

        // GET: EmployeeController
        public ActionResult Index()
		{
			return View(db.Employees.Include(x => x.id_deptoNavigation).ToList());
		}

		// GET: EmployeeController/Details/5
		public ActionResult Details(string id)
		{
			return View(db.Employees.Include(x => x.id_deptoNavigation).FirstOrDefault(x => x.id == id));
		}

		// GET: EmployeeController/Create
		public ActionResult Create()
		{
            ViewBag.Departments = db.Departments.ToList();
            return View();
		}

		// POST: EmployeeController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(Employee employee)
		{
			try
			{
				string secure_password = Utilities.Utilities.GenerateSecurePassword(15);
				string user_name = employee.email.Split("@")[0];
				string user_id = Guid.NewGuid().ToString();

                await _service.CreateAsync(new Models.DTO.RegistrationModel()
				{
					Name = employee.name_emp,
					Email = employee.email,
					UserName = user_name,
					Password = secure_password,
					PasswordConfirm = secure_password,
					Role = "employee",
					UserId = user_id
				});

				employee.id_user = user_id;
                db.Employees.Add(employee);
				db.SaveChanges();

				// Se envia correo para brindar los datos de autenticacion
                string body = Utilities.Utilities.getEmailBody(employee.name_emp, user_name, secure_password, "employee");
                Utilities.Utilities.SendEmail(employee.email, "Bienvenido a Nuestro Equipo", body);

                return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: EmployeeController/Edit/5
		public ActionResult Edit(string id)
		{
            ViewBag.Departments = db.Departments.ToList();
            return View(db.Employees.Where(x => x.id == id).FirstOrDefault());
		}

		// POST: EmployeeController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(string id, Employee employee)
		{
			try
			{
				db.Employees.Update(employee);
				db.SaveChanges();
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: EmployeeController/Delete/5
		public ActionResult Delete(string id)
		{
			return View(db.Employees.Include(x => x.id_deptoNavigation).FirstOrDefault(x => x.id == id));
		}

		// POST: EmployeeController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Delete(string id, string id_user, Employee employee)
		{
			try
			{
				await _service.DeleteAsync(new Models.Domain.ApplicationUser() { Id = id_user });

				db.Employees.Remove(employee);
				db.SaveChanges();
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
	}
}
