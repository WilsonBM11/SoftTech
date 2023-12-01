using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftTech.Data;
using SoftTech.Repositories.Abstract;

namespace SoftTech.Controllers
{
    [Authorize(Roles = "employee")]
    public class SalaryEmployeeController : Controller
    {
        private readonly IUserAdministrationService _service;
        private DataContext db = new DataContext();
        public SalaryEmployeeController(IUserAdministrationService service)
        {
            this._service = service;
        }
        // GET: SalaryEmployeeController
        public async Task<ActionResult> Index()
        {
            var user = await _service.GetUserAsync(User);
            var employee = db.Employees.FirstOrDefault(x => x.id_user == user.Id);
            return View(db.Salaries.Include(x=>x.id_employeeNavigation).Include(x=>x.id_payrollNavigation).Where(x=>x.id_employee == employee.id));
        }

        // GET: SalaryEmployeeController/Details/5
        public ActionResult Details(string id)
        {
            return View(db.Salaries.Include(x=>x.id_employeeNavigation).Include(x => x.id_payrollNavigation).FirstOrDefault(x=>x.id == id));
        }

    }
}
