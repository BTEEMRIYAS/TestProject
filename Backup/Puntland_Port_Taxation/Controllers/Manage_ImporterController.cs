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
    public class Manage_ImporterController : Controller
    {
        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

        //
        // GET: /Manage_Importer/

        public ActionResult Index()
        {
            var importer = from i in db.Importers
                           join c in db.Countries on i.importer_country_id equals c.country_id
                           join t in db.Importer_Type on i.importer_type_id equals t.importer_type_id
                           select new ImporterModel { importer_id = i.importer_id, importer_first_name = i.importer_first_name, importer_middle_name = i.importer_middle_name, importer_last_name = i.importer_last_name, importer_email_id = i.importer_email_id, importer_mob_no = i.importer_mob_no, status_id = i.status_id, importer_country_name = c.country_name, importer_type_name = t.importer_type_name }; 
            return View(importer.ToList());
        }

        //
        // GET: /Manage_Importer/Details/5

        public ActionResult Details(int id = 0)
        {
            Importer importer = db.Importers.Find(id);
            if (importer == null)
            {
                return HttpNotFound();
            }
            return View(importer);
        }

        //
        // GET: /Manage_Importer/Create

        public ActionResult Create()
        {
            ViewBag.importer_type = new HomeController().Importer_Type();
            ViewBag.country = new HomeController().Country();
            return View();
        }

        //
        // POST: /Manage_Importer/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Importer importer)
        {
            if (ModelState.IsValid)
            {
                var is_exist = (from i in db.Importers where i.importer_mob_no == importer.importer_mob_no select i).Count();
                if (is_exist > 0)
                {
                    TempData["errorMessage"] = "This Mobile Number Already Exist";
                }
                else
                {
                    importer.status_id = 1;
                    importer.created_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                    importer.updated_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");   
                    db.Importers.Add(importer);                    
                    db.SaveChanges();
                    TempData["errorMessage"] = "Importer Added Successfully";
                }
                return RedirectToAction("Index");                                       
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Manage_Importer/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Importer importer = db.Importers.Find(id);
            if (importer == null)
            {
                return HttpNotFound();
            }
            ViewBag.importer_type = new HomeController().Importer_Type();
            ViewBag.country = new HomeController().Country();
            ViewBag.status = new HomeController().Status();
            TempData["id"] = importer.importer_id;
            TempData["created_date"] = importer.created_date;
            return View(importer);
        }

        //
        // POST: /Manage_Importer/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Importer importer)
        {
            int id = Convert.ToInt32(TempData["id"]);
            var created_date = TempData["created_date"].ToString(); 
            if (ModelState.IsValid)
            {
                var is_exist = (from i in db.Importers where i.importer_mob_no == importer.importer_mob_no && i.importer_id != id select i).Count();
                if (is_exist > 0)
                {
                    TempData["errorMessage"] = "This Mobile Number Already Exist";
                }
                else
                {
                    importer.importer_id = id;
                    importer.created_date = created_date;
                    importer.updated_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                    db.Entry(importer).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["errorMessage"] = "Edited Successfully";
                }
                return RedirectToAction("Index");
            }
            return View(importer);
        }

        //
        // GET: /Manage_Importer/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Importer importer = db.Importers.Find(id);
            if (importer == null)
            {
                return HttpNotFound();
            }
            return View(importer);
        }

        //
        // POST: /Manage_Importer/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Importer importer = db.Importers.Find(id);
            db.Importers.Remove(importer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // GET: /Manage_Importer/DbSearch

        public ActionResult DbSearch()
        {
            return View();
        }

        //
        // POST: /Manage_Importer/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Importer importer)
        {
            if (importer.importer_first_name != null && importer.importer_middle_name != null && importer.importer_last_name != null)
            {
                var result = from i in db.Importers
                             join c in db.Countries on i.importer_country_id equals c.country_id
                             join t in db.Importer_Type on i.importer_type_id equals t.importer_type_id
                             where i.importer_first_name.StartsWith(importer.importer_first_name) && i.importer_middle_name.StartsWith(importer.importer_middle_name) && i.importer_last_name.StartsWith(importer.importer_last_name)
                             select new ImporterModel { importer_id = i.importer_id, importer_first_name = i.importer_first_name, importer_middle_name = i.importer_middle_name, importer_last_name = i.importer_last_name, importer_email_id = i.importer_email_id, importer_mob_no = i.importer_mob_no, status_id = i.status_id, importer_country_name = c.country_name, importer_type_name = t.importer_type_name };
                return View("Index", result.ToList());
            }
            else if (importer.importer_first_name != null && importer.importer_middle_name != null && importer.importer_last_name == null)
            {
                var result = from i in db.Importers
                             join c in db.Countries on i.importer_country_id equals c.country_id
                             join t in db.Importer_Type on i.importer_type_id equals t.importer_type_id
                             where i.importer_first_name.StartsWith(importer.importer_first_name) && i.importer_middle_name.StartsWith(importer.importer_middle_name)
                             select new ImporterModel { importer_id = i.importer_id, importer_first_name = i.importer_first_name, importer_middle_name = i.importer_middle_name, importer_last_name = i.importer_last_name, importer_email_id = i.importer_email_id, importer_mob_no = i.importer_mob_no, status_id = i.status_id, importer_country_name = c.country_name, importer_type_name = t.importer_type_name };
                return View("Index", result.ToList());
            }
            else if (importer.importer_first_name != null && importer.importer_middle_name == null && importer.importer_last_name != null)
            {
                var result = from i in db.Importers
                             join c in db.Countries on i.importer_country_id equals c.country_id
                             join t in db.Importer_Type on i.importer_type_id equals t.importer_type_id
                             where i.importer_first_name.StartsWith(importer.importer_first_name) && i.importer_last_name.StartsWith(importer.importer_last_name)
                             select new ImporterModel { importer_id = i.importer_id, importer_first_name = i.importer_first_name, importer_middle_name = i.importer_middle_name, importer_last_name = i.importer_last_name, importer_email_id = i.importer_email_id, importer_mob_no = i.importer_mob_no, status_id = i.status_id, importer_country_name = c.country_name, importer_type_name = t.importer_type_name };
                return View("Index", result.ToList());
            }
            else if (importer.importer_first_name != null && importer.importer_middle_name == null && importer.importer_last_name == null)
            {
                var result = from i in db.Importers
                             join c in db.Countries on i.importer_country_id equals c.country_id
                             join t in db.Importer_Type on i.importer_type_id equals t.importer_type_id
                             where i.importer_first_name.StartsWith(importer.importer_first_name)
                             select new ImporterModel { importer_id = i.importer_id, importer_first_name = i.importer_first_name, importer_middle_name = i.importer_middle_name, importer_last_name = i.importer_last_name, importer_email_id = i.importer_email_id, importer_mob_no = i.importer_mob_no, status_id = i.status_id, importer_country_name = c.country_name, importer_type_name = t.importer_type_name };
                return View("Index", result.ToList());
            }
            else if (importer.importer_first_name == null && importer.importer_middle_name != null && importer.importer_last_name != null)
            {
                var result = from i in db.Importers
                             join c in db.Countries on i.importer_country_id equals c.country_id
                             join t in db.Importer_Type on i.importer_type_id equals t.importer_type_id
                             where i.importer_middle_name.StartsWith(importer.importer_middle_name) && i.importer_last_name.StartsWith(importer.importer_last_name)
                             select new ImporterModel { importer_id = i.importer_id, importer_first_name = i.importer_first_name, importer_middle_name = i.importer_middle_name, importer_last_name = i.importer_last_name, importer_email_id = i.importer_email_id, importer_mob_no = i.importer_mob_no, status_id = i.status_id, importer_country_name = c.country_name, importer_type_name = t.importer_type_name };
                return View("Index", result.ToList());
            }
            else if (importer.importer_first_name == null && importer.importer_middle_name != null && importer.importer_last_name == null)
            {
                var result = from i in db.Importers
                             join c in db.Countries on i.importer_country_id equals c.country_id
                             join t in db.Importer_Type on i.importer_type_id equals t.importer_type_id
                             where i.importer_middle_name.StartsWith(importer.importer_middle_name)
                             select new ImporterModel { importer_id = i.importer_id, importer_first_name = i.importer_first_name, importer_middle_name = i.importer_middle_name, importer_last_name = i.importer_last_name, importer_email_id = i.importer_email_id, importer_mob_no = i.importer_mob_no, status_id = i.status_id, importer_country_name = c.country_name, importer_type_name = t.importer_type_name };
                return View("Index", result.ToList());
            }
            else if (importer.importer_first_name == null && importer.importer_middle_name == null && importer.importer_last_name != null)
            {
                var result = from i in db.Importers
                             join c in db.Countries on i.importer_country_id equals c.country_id
                             join t in db.Importer_Type on i.importer_type_id equals t.importer_type_id
                             where i.importer_last_name.StartsWith(importer.importer_last_name)
                             select new ImporterModel { importer_id = i.importer_id, importer_first_name = i.importer_first_name, importer_middle_name = i.importer_middle_name, importer_last_name = i.importer_last_name, importer_email_id = i.importer_email_id, importer_mob_no = i.importer_mob_no, status_id = i.status_id, importer_country_name = c.country_name, importer_type_name = t.importer_type_name };
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