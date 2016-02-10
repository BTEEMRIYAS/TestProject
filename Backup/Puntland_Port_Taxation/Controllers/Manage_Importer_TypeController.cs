using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Puntland_Port_Taxation.Controllers
{
    public class Manage_Importer_TypeController : Controller
    {
        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

        //
        // GET: /Manage_Importer_Type/

        public ActionResult Index()
        {
            return View(db.Importer_Type.ToList());
        }

        //
        // GET: /Manage_Importer_Type/Details/5

        public ActionResult Details(int id = 0)
        {
            Importer_Type importer_type = db.Importer_Type.Find(id);
            if (importer_type == null)
            {
                return HttpNotFound();
            }
            return View(importer_type);
        }

        //
        // GET: /Manage_Importer_Type/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Manage_Importer_Type/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Importer_Type importer_type)
        {
            if (ModelState.IsValid)
            {
                var is_exist = (from it in db.Importer_Type where it.importer_type_name == importer_type.importer_type_name select it).Count();
                if (is_exist > 0)
                {
                    TempData["errorMessage"] = "This Importer Type Already Exist";
                }
                else
                {
                    importer_type.status_id = 1;
                    importer_type.created_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                    importer_type.updated_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                    db.Importer_Type.Add(importer_type);
                    TempData["errorMessage"] = "Importer Type Added Successfully";
                    db.SaveChanges();
                }
              return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Manage_Importer_Type/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Importer_Type importer_type = db.Importer_Type.Find(id);
            if (importer_type == null)
            {
                return HttpNotFound();
            }
            TempData["id"] = importer_type.importer_type_id;
            TempData["created_date"] = importer_type.created_date;
            ViewBag.status = new HomeController().Status();
            return View(importer_type);
        }

        //
        // POST: /Manage_Importer_Type/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Importer_Type importer_type)
        {
            int id = Convert.ToInt32(TempData["id"]);
            var created_date = TempData["created_date"].ToString();
            if (ModelState.IsValid)
            {
                var is_exist = (from it in db.Importer_Type where it.importer_type_name == importer_type.importer_type_name && it.importer_type_id != id select it).Count();
                if (is_exist > 0)
                {
                    TempData["errorMessage"] = "This Importer Type Already Exist";
                }
                else
                {
                    importer_type.importer_type_id = id;
                    importer_type.created_date = created_date;
                    importer_type.updated_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                    db.Entry(importer_type).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["errorMessage"] = "Edited Successfully";
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Manage_Importer_Type/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Importer_Type importer_type = db.Importer_Type.Find(id);
            if (importer_type == null)
            {
                return HttpNotFound();
            }
            return View(importer_type);
        }

        //
        // POST: /Manage_Importer_Type/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Importer_Type importer_type = db.Importer_Type.Find(id);
            db.Importer_Type.Remove(importer_type);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // GET: /Manage_Importer_Type/DbSearch

        public ActionResult DbSearch()
        {
            return View();
        }

        //
        // POST: /Manage_Importer_Type/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Importer_Type importer_type)
        {
            if (importer_type.importer_type_name != null)
            {
                var result = from i in db.Importer_Type
                             where i.importer_type_name.StartsWith(importer_type.importer_type_name)
                             select i;
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