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
    public class Manage_Unit_Of_MeasureController : Controller
    {
        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();
        //
        // GET: /Manage_Unit_Of_Measure/

        public ActionResult Index()
        {
            return View(db.Unit_Of_Measure.ToList());
        }

        //
        // GET: /Manage_Unit_Of_Measure/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Manage_Unit_Of_Measure/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Manage_Unit_Of_Measure/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Unit_Of_Measure unit_Of_Measure)
        {
            if (ModelState.IsValid)
            {
                var is_exist = (from u in db.Unit_Of_Measure where u.unit_name == unit_Of_Measure.unit_name select u).Count();
                if (is_exist > 0)
                {
                    TempData["errorMessage"] = "This Unit Of Measure Already Exist";
                }
                else
                {
                    db.Unit_Of_Measure.Add(unit_Of_Measure);
                    db.SaveChanges();
                    TempData["errorMessage"] = "Unit Of Measure Added Successfully";
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        //
        // GET: /Manage_Unit_Of_Measure/Edit/5

        public ActionResult Edit(int id=0)
        {
            Unit_Of_Measure unit_Of_Measure = db.Unit_Of_Measure.Find(id);
            
            if (unit_Of_Measure == null)
            {
                return HttpNotFound();
            }
            TempData["id"] = unit_Of_Measure.unit_id;
            return View(unit_Of_Measure);
        }


        //
        // POST: /Manage_Unit_Of_Measure/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Unit_Of_Measure unit_Of_Measure)
        {                             
            int id = Convert.ToInt32(TempData["id"]);
            if (ModelState.IsValid)
            {
                var is_exist = (from u in db.Unit_Of_Measure where u.unit_name == unit_Of_Measure.unit_name && u.unit_id != id select u).Count();
                if (is_exist > 0)
                {
                    TempData["errorMessage"] = "This Unit Of Measure Already Exist";
                }
                else
                {
                    unit_Of_Measure.unit_id = id;
                    db.Entry(unit_Of_Measure).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["errorMessage"] = "Edited Successfully";
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        //
        // GET: /Manage_Department/DbSearch

        public ActionResult DbSearch()
        {
            return View();
        }

        ////
        //// POST: /Manage_Department/DbSearchresult
        ////In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Unit_Of_Measure Unit_Of_Measure)
        {
            if (Unit_Of_Measure.unit_name != null && Unit_Of_Measure.unit_code != null)
            {
                var result = from u in db.Unit_Of_Measure
                             where u.unit_name.StartsWith(Unit_Of_Measure.unit_name) && u.unit_code.StartsWith(Unit_Of_Measure.unit_code)
                             select u;
                return View("Index", result.ToList());
            }
            else if (Unit_Of_Measure.unit_name != null && Unit_Of_Measure.unit_code == null)
            {
                var result = from u in db.Unit_Of_Measure
                             where u.unit_name.StartsWith(Unit_Of_Measure.unit_name)
                             select u;
                return View("Index", result.ToList());
            }
            else if (Unit_Of_Measure.unit_name == null && Unit_Of_Measure.unit_code != null)
            {
                var result = from u in db.Unit_Of_Measure
                             where u.unit_code.StartsWith(Unit_Of_Measure.unit_code)
                             select u;
                return View("Index", result.ToList());
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Manage_Unit_Of_Measure/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Manage_Unit_Of_Measure/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public object[] unit_id { get; set; }
    }
}
