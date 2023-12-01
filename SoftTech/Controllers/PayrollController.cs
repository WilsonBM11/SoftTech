using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoftTech.Data;
using SoftTech.Models.Entites;

namespace SoftTech.Controllers
{
    [Authorize(Roles = "human_resources")]
    public class PayrollController : Controller
    {
        private DataContext db = new DataContext();
        // GET: PayrollController
        public ActionResult Index()
        {
            return View(db.Payrolls.ToList());
        }

        // GET: PayrollController/Details/5
        public ActionResult Details(string id)
        {
            return View(db.Payrolls.FirstOrDefault(x => x.id == id));
        }

        // POST: PayrollController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create()
        {
            try
            {
                var new_payroll = new Payroll() {
                    date_payroll = DateTime.Now,
                    income_tax = 0,
                    employee_ccss = 0,
                    employer_ccss = 0,
                    total_salary = 0,   
                    total_payment = 0
                };

                db.Payrolls.Add(new_payroll);
                db.SaveChanges();

                return RedirectToAction("Index", "Salary", new { id_payroll = new_payroll.id });
            }
            catch
            {
                return View();
            }
        }

        // GET: PayrollController/Delete/5
        public ActionResult Delete(string id)
        {
            return View(db.Payrolls.FirstOrDefault(x => x.id == id));
        }

        // POST: PayrollController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, Payroll payroll)
        {
            try
            {
                var salaries = db.Salaries.Where(x => x.id_payroll == payroll.id);
                foreach (var salary in salaries)
                {
                    db.Salaries.Remove(salary);
                }
                db.Payrolls.Remove(payroll);
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
