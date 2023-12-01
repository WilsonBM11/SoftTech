using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoftTech.Data;
using SoftTech.Models.Entites;

namespace SoftTech.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
	{
		private DataContext db = new DataContext();
		// GET: DepartmentController
		public ActionResult Index()
		{
			return View(db.Departments.ToList());
		}

		// GET: DepartmentController/Details/5
		public ActionResult Details(string id)
		{
			return View(db.Departments.Where(x => x.id == id).FirstOrDefault());
		}

		// GET: DepartmentController/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: DepartmentController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Department depto)
		{
			try
			{
				db.Departments.Add(depto);
				db.SaveChanges();
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: DepartmentController/Edit/5
		public ActionResult Edit(string id)
		{
			return View(db.Departments.Where(x => x.id == id).FirstOrDefault());
		}

		// POST: DepartmentController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(string id, Department depto)
		{
			try
			{
				db.Departments.Update(depto);
				db.SaveChanges();
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: DepartmentController/Delete/5
		public ActionResult Delete(string id)
		{
			return View(db.Departments.Where(x => x.id == id).FirstOrDefault());
		}

		// POST: DepartmentController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(string id, Department depto)
		{
			try
			{
				db.Departments.Remove(depto);
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
