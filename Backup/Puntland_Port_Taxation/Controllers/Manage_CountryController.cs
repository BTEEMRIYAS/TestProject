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
    public class Manage_CountryController : Controller
    {
        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

        //
        // GET: /Manage_Country/

        public ActionResult Index()
        {
            var country = from c in db.Countries
                          join g in db.Geographies on c.geography_id equals g.geography_id
                          select new GeographyModel { geography_name = g.geography_name, country_name = c.country_name, country_id = c.country_id, status_id = c.status_id };
            return View(country.ToList());
        }

        //
        // GET: /Manage_Country/Details/5

        public ActionResult Details(int id = 0)
        {
            Country country = db.Countries.Find(id);
            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }

        //
        // GET: /Manage_Country/Create

        public ActionResult Create()
        {
            ViewBag.geogaphy = new HomeController().Geography();
            return View();
        }

        //
        // POST: /Manage_Country/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Country country)
        {
            var isexist = from c in db.Countries
                          where c.country_name == country.country_name
                          select c;
            if (isexist.Count() > 0)
            {
                TempData["errorMessage"] = "This Country Already Exist";
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                country.status_id = 1;
                country.created_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                country.updated_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                db.Countries.Add(country);
                db.SaveChanges();
                TempData["errorMessage"] = "Country Added Successfully";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Manage_Country/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Country country = db.Countries.Find(id);
            if (country == null)
            {
                return HttpNotFound();
            }
            ViewBag.status = new HomeController().Status();
            ViewBag.geogaphy = new HomeController().Geography();
            TempData["id"] = country.country_id;
            TempData["created_date"] = country.country_id;
            return View(country);
        }

        //
        // POST: /Manage_Country/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Country country)
        {
            int id = Convert.ToInt32(TempData["id"]);
            var isexist = from c in db.Countries
                          where c.country_name == country.country_name && c.country_id != id
                          select c;
            if (isexist.Count() > 0)
            {
                TempData["errorMessage"] = "This Country Already Exist";
                return RedirectToAction("Index");
            }
            var created_date = TempData["created_date"].ToString();
            if (ModelState.IsValid)
            {
                country.country_id = id;
                country.created_date = created_date;
                country.updated_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                db.Entry(country).State = EntityState.Modified;
                db.SaveChanges();
                TempData["errorMessage"] = "Edited Successfully";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Manage_Country/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Country country = db.Countries.Find(id);
            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }

        //
        // POST: /Manage_Country/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Country country = db.Countries.Find(id);
            db.Countries.Remove(country);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // GET: /Manage_Country/DbSearch

        public ActionResult DbSearch()
        {
            ViewBag.geogaphy = new HomeController().Geography();
            return View();
        }

        //
        // POST: /Manage_Country/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Country country)
        {
            if (country.geography_id != 0 && country.country_name != null)
            {
                var result = from c in db.Countries
                             join g in db.Geographies on c.geography_id equals g.geography_id
                             where c.geography_id == country.geography_id && c.country_name == country.country_name
                             select new GeographyModel { geography_name = g.geography_name, country_name = c.country_name, country_id = c.country_id, status_id = c.status_id };
                return View("Index", result.ToList());
            }
            else if (country.geography_id != 0 && country.country_name == null)
            {
                var result = from c in db.Countries
                             join g in db.Geographies on c.geography_id equals g.geography_id
                             where c.geography_id == country.geography_id
                             select new GeographyModel { geography_name = g.geography_name, country_name = c.country_name, country_id = c.country_id, status_id = c.status_id };
                return View("Index", result.ToList());
            }
            else if (country.geography_id == 0 && country.country_name != null)
            {
                var result = from c in db.Countries
                             join g in db.Geographies on c.geography_id equals g.geography_id
                             where c.country_name == country.country_name
                             select new GeographyModel { geography_name = g.geography_name, country_name = c.country_name, country_id = c.country_id, status_id = c.status_id };
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