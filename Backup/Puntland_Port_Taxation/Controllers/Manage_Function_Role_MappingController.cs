using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Puntland_Port_Taxation.Models;

namespace Puntland_Port_Taxation.Controllers
{
    public class Manage_Function_Role_MappingController : Controller
    {
        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

        //
        // GET: /Manage_Function_Role_Mapping/

        public ActionResult Index()
        {
            var model = (from r in db.Roles
                         select new Function_Role_MapModel { role_id = r.role_id, role_name = r.role_name }).OrderBy(e => e.role_id);
            return View(model);
        }

        //
        // GET: /Manage_Function_Role_Mapping/Details/5

        public ActionResult Details(int id = 0)
        {
            var details = from c in db.Roles
                          join f in db.Function_Role_Map on c.role_id equals f.role_id
                          join fm in db.Functions on f.function_id equals fm.function_id
                          where c.role_id == id
                          select new Function_Role_MapModel { role_id = c.role_id, function_id = f.function_id, status_id = 1, role_name = c.role_name, function_name = fm.function_name, created_date = f.created_date, updated_date = f.updated_date };
            return View(details);
        }

        //
        // GET: /Manage_Function_Role_Mapping/Create

        public ActionResult Create()
        {
            ViewBag.roles = new HomeController().Roles();
            ViewBag.functions = new HomeController().Function();
            return View();
        }


        //
        // POST: /Manage_Function_Role_Mapping/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Function_Role_Map function_role_map)
        {
            string dtNow = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");

            if (ModelState.IsValid)
            {
                //delete existing data
                var deleteExistingData = from details in db.Function_Role_Map where details.role_id == function_role_map.role_id select details;
                foreach (var detail in deleteExistingData)
                {
                    db.Function_Role_Map.Remove(detail);
                }
                db.SaveChanges();

                //save new 

                if (!string.IsNullOrEmpty(Request.Form["rolefunction"]))
                {
                    string[] temp = Request.Form["rolefunction"].Split(',');
                    for (int i = 0; i < temp.Count(); i++)
                    {
                        function_role_map.function_id = int.Parse(temp[i].ToString());
                        function_role_map.status_id = 1;
                        function_role_map.created_date = dtNow;
                        function_role_map.updated_date = dtNow;
                        db.Function_Role_Map.Add(function_role_map);
                        db.SaveChanges();
                        TempData["errorMessage"] = "Role Maped Successfully";
                    }
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Manage_Function_Role_Mapping/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Role role_dm = new Role();
            role_dm.role_id = id;
            ViewBag.roles = new HomeController().Roles();
            ViewBag.functions = new HomeController().Function();
            ViewBag.existingfunctions = new HomeController().Function_Roles(id);
            return View(role_dm);
        }

        //
        // POST: /Manage_Function_Role_Mapping/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Function_Role_Map function_role_map)
        {
            if (ModelState.IsValid)
            {
                string dtNow = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                //delete existing data
                var deleteExistingData = from details in db.Function_Role_Map where details.role_id == function_role_map.role_id select details;
                foreach (var detail in deleteExistingData)
                {
                    db.Function_Role_Map.Remove(detail);
                }
                db.SaveChanges();

                //save new 

                if (!string.IsNullOrEmpty(Request.Form["rolefunction"]))
                {
                    string[] temp = Request.Form["rolefunction"].Split(',');
                    for (int i = 0; i < temp.Count(); i++)
                    {
                        function_role_map.function_id = int.Parse(temp[i].ToString());
                        function_role_map.status_id = 1;
                        function_role_map.updated_date = dtNow;
                        function_role_map.created_date = dtNow;
                        db.Function_Role_Map.Add(function_role_map);
                        db.SaveChanges();
                        TempData["errorMessage"] = "Edited Successfully";
                    }
                }
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Manage_Function_Role_Mapping/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Function_Role_Map function_role_map = db.Function_Role_Map.Find(id);
            if (function_role_map == null)
            {
                return HttpNotFound();
            }
            return View(function_role_map);
        }

        //
        // POST: /Manage_Function_Role_Mapping/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Function_Role_Map function_role_map = db.Function_Role_Map.Find(id);
            db.Function_Role_Map.Remove(function_role_map);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        //
        // GET: /Manage_Function_Role_Mapping/DbSearch

        public ActionResult DbSearch()
        {
            ViewBag.roles = new HomeController().Roles();
            // ViewBag.functions = new HomeController().Function();
            return View();
        }

        //
        // POST: /Manage_Function_Role_Mapping/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Function_Role_Map function_role_map)
        {
            if (function_role_map.role_id != 0)
            {
                var result = from r in db.Roles
                             where r.role_id == function_role_map.role_id
                             select new Function_Role_MapModel { role_name = r.role_name, role_id = r.role_id };
                return View("Index", result.ToList());
            }
            return RedirectToAction("Index");
        }
    }
}