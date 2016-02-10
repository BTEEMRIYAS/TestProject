using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Puntland_Port_Taxation.Controllers
{
    public class Manage_RoleController : Controller
    {
        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

        //
        // GET: /Manage_Role/

        public ActionResult Index()
        {
            return View(db.Roles.ToList());
        }

        //
        // GET: /Manage_Role/Details/5

        public ActionResult Details(int id = 0)
        {
            Role role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        //
        // GET: /Manage_Role/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Manage_Role/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Role role)
        {
            if (ModelState.IsValid)
            {
                var is_exist = (from r in db.Roles where r.role_name == role.role_name select r).Count();
                if (is_exist > 0)
                {
                    TempData["errorMessage"] = "This Role Already Exist";
                }
                else
                {
                    role.status_id = 1;
                    role.created_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                    role.updated_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                    db.Roles.Add(role);                    
                    db.SaveChanges();
                    TempData["errorMessage"] = "Role Added Successfully";
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        //
        // GET: /Manage_Role/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Role role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            TempData["role_id"] = role.role_id;
            ViewBag.status = new HomeController().Status();
            return View(role);
        }

        //
        // POST: /Manage_Role/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Role role)
        {
            int role_id = Convert.ToInt32(TempData["role_id"]);
            Role role_new = db.Roles.Find(role_id);
            if (ModelState.IsValid)
            {
                var is_exist = (from r in db.Roles where r.role_name == role.role_name && r.role_id != role_id select r).Count();
                if (is_exist > 0)
                {
                    TempData["errorMessage"] = "This Role Already Exist";
                }
                else
                {
                    role_new.role_name = role.role_name;
                    role_new.status_id = role.status_id;
                    role_new.updated_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                    db.Entry(role_new).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["errorMessage"] = "Edited Successfully";
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Manage_Role/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Role role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        //
        // POST: /Manage_Role/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Role role = db.Roles.Find(id);
            db.Roles.Remove(role);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        //
        // GET: /Manage_Role/DbSearch

        public ActionResult DbSearch()
        {
            return View();
        }

        //
        // POST: /Manage_Role/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Role role)
        {
            if (role.role_name != null)
            {
                var result = from r in db.Roles
                             where r.role_name.StartsWith(role.role_name)
                             select r;
                return View("Index", result.ToList());
            }
            return RedirectToAction("Index");
        }
    }
}