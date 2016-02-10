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
    public class Manage_GeographyController : Controller
    {
        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

        //
        // GET: /Manage_Geography/

        public ActionResult Index()
        {
            return View(db.Geographies.ToList());
        }

        //
        // GET: /Manage_Geography/Details/5

        public ActionResult Details(int id = 0)
        {
            Geography geography = db.Geographies.Find(id);
            if (geography == null)
            {
                return HttpNotFound();
            }
            return View(geography);
        }

        //
        // GET: /Manage_Geography/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Manage_Geography/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Geography geography)
        {
            var isexist = from g in db.Geographies
                          where g.geography_name == geography.geography_name
                          select g;
            if (isexist.Count() > 0)
            {
                TempData["errorMessage"] = "This Geography Already Exist";
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                geography.status_id = 1;
                geography.created_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                geography.updated_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                db.Geographies.Add(geography);
                db.SaveChanges();
                TempData["errorMessage"] = "Geography Added Successfully";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Manage_Geography/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Geography geography = db.Geographies.Find(id);
            if (geography == null)
            {
                return HttpNotFound();
            }
            TempData["id"] = geography.geography_id;
            TempData["created_date"] = geography.created_date;
            ViewBag.status = new HomeController().Status();
            return View(geography);
        }

        //
        // POST: /Manage_Geography/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Geography geography)
        {
            int id = Convert.ToInt32(TempData["id"]);
            var isexist = from g in db.Geographies
                          where g.geography_name == geography.geography_name && g.geography_id != id
                          select g;
            if (isexist.Count() > 0)
            {
                TempData["errorMessage"] = "This Geography Already Exist";
                return RedirectToAction("Index");
            }           
            var created_date = TempData["created_date"].ToString();
            if (ModelState.IsValid)
            {
                geography.geography_id = id;
                geography.created_date = created_date;
                geography.updated_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                db.Entry(geography).State = EntityState.Modified;
                db.SaveChanges();
                TempData["errorMessage"] = "Edited Successfully";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Manage_Geography/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Geography geography = db.Geographies.Find(id);
            if (geography == null)
            {
                return HttpNotFound();
            }
            return View(geography);
        }

        //
        // POST: /Manage_Geography/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Geography geography = db.Geographies.Find(id);
            db.Geographies.Remove(geography);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // GET: /Manage_Geography/DbSearch

        public ActionResult DbSearch()
        {
            return View();
        }

        //
        // POST: /Manage_Geography/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Geography geography)
        {
            if (geography.geography_name != null)
            {
                var result = from g in db.Geographies
                             where g.geography_name.StartsWith(geography.geography_name)
                             select g;
                return View("Index", result.ToList());
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}