using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Puntland_Port_Taxation.Controllers
{
    public class Manage_DepartmentController : Controller
    {
        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

        //
        // GET: /Manage_Department/

        public ActionResult Index()
        {
            return View(db.Departments.ToList());
        }

        //
        // GET: /Manage_Department/Details/5

        public ActionResult Details(int id = 0)
        {
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        //
        // GET: /Manage_Department/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Manage_Department/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {
                var is_exist = (from d in db.Departments where d.department_name == department.department_name select d).Count();
                if (is_exist > 0)
                {
                    TempData["errorMessage"] = "This Department Already Exist";
                }
                else
                {
                    department.status_id = 1;
                    department.created_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                    department.updated_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                    db.Departments.Add(department);                    
                    db.SaveChanges();
                    TempData["errorMessage"] = "Department Added Successfully";
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        //
        // GET: /Manage_Department/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            TempData["department_id"] = department.department_id;
            ViewBag.status = new HomeController().Status();
            return View(department);
        }

        //
        // POST: /Manage_Department/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Department department)
        {
            int department_id = Convert.ToInt32(TempData["department_id"]);         
            Department department_new = db.Departments.Find(department_id);
            if (ModelState.IsValid)
            {
                var is_exist = (from d in db.Departments where d.department_name == department.department_name && d.department_id != department_id select d).Count();
                if (is_exist > 0)
                {
                    TempData["errorMessage"] = "This Department Already Exist";
                }
                else
                {
                    department_new.department_name = department.department_name;
                    department_new.status_id = department.status_id;
                    department_new.updated_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                    db.Entry(department_new).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["errorMessage"] = "Edited Successfully";
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Manage_Department/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        //
        // POST: /Manage_Department/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Department department = db.Departments.Find(id);
            db.Departments.Remove(department);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        //
        // GET: /Manage_Department/DbSearch

        public ActionResult DbSearch()
        {
            return View();
        }

        //
        // POST: /Manage_Department/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Department department)
        {
            if (department.department_name != null)
            {
                var result =  from d in db.Departments
                              where d.department_name.StartsWith(department.department_name)
                              select d;
                return View("Index", result.ToList());
            }
            return RedirectToAction("Index");
        }
    }
}