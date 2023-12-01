using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftTech.Data;
using SoftTech.Models.Entites;
using SoftTech.Models.ViewBags;

namespace SoftTech.Controllers
{
    [Authorize(Roles = "human_resources")]
    public class SalaryController : Controller
    {
        private DataContext db = new DataContext();
        // GET: SalaryController
        public ActionResult Index(string id_payroll)
        {
            ViewBag.Payroll = new PayrollData() { id = id_payroll};
            return View(db.Salaries.Include(x=>x.id_employeeNavigation).Where(x=> x.id_payroll == id_payroll));
        }

        // GET: SalaryController/Details/5
        public ActionResult Details(string id)
        {
            return View(db.Salaries.Include(x => x.id_employeeNavigation).FirstOrDefault(x => x.id == id));
        }

        // GET: SalaryController/Create
        public ActionResult Create(string id_payroll)
        {
            ViewBag.Employees = db.Employees.ToList();
            ViewBag.Payroll = new PayrollData() { id = id_payroll };
            return View();
        }

        // POST: SalaryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Salary salary)
        {
            try
            {
                var employee = db.Employees.FirstOrDefault(x => x.id == salary.id_employee);
                
                var hourly_salary = employee.salary / 30;
                
                salary.gross_salary = (hourly_salary * 15) + (hourly_salary * salary.horas_e * 1.5);
                salary.ccss_amount = salary.gross_salary * 0.1067;
                salary.income_tax =  Utilities.Utilities.CalculateIncomeTax(salary.gross_salary);
                salary.net_salary = salary.gross_salary - salary.income_tax - salary.ccss_amount;

                db.Salaries.Add(salary);
                db.SaveChanges();

                var payroll = db.Payrolls.Include(x=>x.Salaries).FirstOrDefault(x => x.id == salary.id_payroll);
                payroll.income_tax = 0; payroll.employee_ccss = 0; payroll.employer_ccss = 0; payroll.total_salary = 0; payroll.total_payment = 0;
                foreach (var item in payroll.Salaries)
                {
                    payroll.income_tax += item.income_tax;
                    payroll.employee_ccss += item.ccss_amount;
                    payroll.total_salary += item.gross_salary;
                }
                payroll.employer_ccss = payroll.total_salary * 0.2667;
                payroll.total_payment = payroll.employer_ccss + payroll.employee_ccss + payroll.income_tax + payroll.total_salary;

                db.Payrolls.Update(payroll);

                db.SaveChanges();

                return RedirectToAction("Index", new { id_payroll = salary.id_payroll });
            }
            catch
            {
                return View();
            }
        }

        // GET: SalaryController/Delete/5
        public ActionResult Delete(string id)
        {
            return View(db.Salaries.Include(x => x.id_employeeNavigation).FirstOrDefault(x => x.id == id));
        }

        // POST: SalaryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, string id_payroll,Salary salary)
        {
            try
            {
                db.Salaries.Remove(salary);
                db.SaveChanges();

                var payroll = db.Payrolls.Include(x => x.Salaries).FirstOrDefault(x => x.id == salary.id_payroll);
                payroll.income_tax = 0; payroll.employee_ccss = 0; payroll.employer_ccss = 0; payroll.total_salary = 0; payroll.total_payment = 0;
                foreach (var item in payroll.Salaries)
                {
                    payroll.income_tax += item.income_tax;
                    payroll.employee_ccss += item.ccss_amount;
                    payroll.total_salary += item.gross_salary;
                }
                payroll.employer_ccss = payroll.total_salary * 0.2667;
                payroll.total_payment = payroll.employer_ccss + payroll.employee_ccss + payroll.income_tax + payroll.total_salary;

                db.Payrolls.Update(payroll);

                db.SaveChanges();

                return RedirectToAction("Index", new { id_payroll = salary.id_payroll });
            }
            catch
            {
                return View();
            }
        }
    }
}
