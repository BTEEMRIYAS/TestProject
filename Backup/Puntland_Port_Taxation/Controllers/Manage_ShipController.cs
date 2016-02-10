using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Puntland_Port_Taxation.Controllers
{
    public class Manage_ShipController : Controller
    {
        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

        //
        // GET: /Manage_Ship/

        public ActionResult Index()
        {
            return View(db.Ships.ToList());
        }

        //
        // GET: /Manage_Ship/Details/5

        public ActionResult Details(int id = 0)
        {
            Ship ship = db.Ships.Find(id);
            if (ship == null)
            {
                return HttpNotFound();
            }
            return View(ship);
        }

        //
        // GET: /Manage_Ship/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Manage_Ship/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Ship ship)
        {
            if (ModelState.IsValid)
            {
                var is_exist = (from s in db.Ships where s.ship_name == ship.ship_name && s.ship_code_1 == ship.ship_code_1 && s.ship_code_2 == ship.ship_code_2 && s.ship_code_3 == ship.ship_code_3 select s).Count();
                if (is_exist > 0)
                {
                    TempData["errorMessage"] = "This Ship Already Exist";
                }
                else
                {
                    ship.status_id = 1;
                    ship.created_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                    ship.updated_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                    db.Ships.Add(ship);
                    db.SaveChanges();
                    TempData["errorMessage"] = "Ship Added Successfully";
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Manage_Ship/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Ship ship = db.Ships.Find(id);
            if (ship == null)
            {
                return HttpNotFound();
            }
            TempData["id"] = ship.ship_id;
            TempData["created_date"] = ship.created_date;
            ViewBag.status = new HomeController().Status();
            return View(ship);
        }

        //
        // POST: /Manage_Ship/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Ship ship)
        {
            int id = Convert.ToInt32(TempData["id"]);
            var created_date = TempData["created_date"].ToString();
            if (ModelState.IsValid)
            {
                var is_exist = (from s in db.Ships where s.ship_name == ship.ship_name && s.ship_code_1 == ship.ship_code_1 && s.ship_code_2 == ship.ship_code_2 && s.ship_code_3 == ship.ship_code_3 && s.ship_id != id select s).Count();
                if (is_exist > 0)
                {
                    TempData["errorMessage"] = "This Ship Already Exist";
                }
                else
                {
                    ship.ship_id = id;
                    ship.created_date = created_date;
                    ship.updated_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                    db.Entry(ship).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["errorMessage"] = "Edited Successfully";
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Manage_Ship/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Ship ship = db.Ships.Find(id);
            if (ship == null)
            {
                return HttpNotFound();
            }
            return View(ship);
        }

        //
        // POST: /Manage_Ship/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ship ship = db.Ships.Find(id);
            db.Ships.Remove(ship);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // GET: /Manage_Ship/DbSearch

        public ActionResult DbSearch()
        {
            return View();
        }

        //
        // POST: /Manage_Ship/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Ship ship)
        {
            if (ship.ship_name != null && ship.ship_code_1 != null && ship.ship_code_2 != null)
            {
                var result = from s in db.Ships
                             where s.ship_name.StartsWith(ship.ship_name) && s.ship_code_1.StartsWith(ship.ship_code_1) && s.ship_code_2.StartsWith(ship.ship_code_2)
                             select s;
                return View("Index", result.ToList());
            }
            else if (ship.ship_name != null && ship.ship_code_1 != null && ship.ship_code_2 == null)
            {
                var result = from s in db.Ships
                             where s.ship_name.StartsWith(ship.ship_name) && s.ship_code_1.StartsWith(ship.ship_code_1)
                             select s;
                return View("Index", result.ToList());
            }
            else if (ship.ship_name != null && ship.ship_code_1 == null && ship.ship_code_2 != null)
            {
                var result = from s in db.Ships
                             where s.ship_name.StartsWith(ship.ship_name) && s.ship_code_2.StartsWith(ship.ship_code_2)
                             select s;
                return View("Index", result.ToList());
            }
            else if (ship.ship_name != null && ship.ship_code_1 == null && ship.ship_code_2 == null)
            {
                var result = from s in db.Ships
                             where s.ship_name.StartsWith(ship.ship_name)
                             select s;
                return View("Index", result.ToList());
            }
            else if (ship.ship_name == null && ship.ship_code_1 != null && ship.ship_code_2 != null)
            {
                var result = from s in db.Ships
                             where s.ship_code_1.StartsWith(ship.ship_code_1) && s.ship_code_2.StartsWith(ship.ship_code_2)
                             select s;
                return View("Index", result.ToList());
            }
            else if (ship.ship_name == null && ship.ship_code_1 != null && ship.ship_code_2 == null)
            {
                var result = from s in db.Ships
                             where s.ship_code_1.StartsWith(ship.ship_code_1)
                             select s;
                return View("Index", result.ToList());
            }
            else if (ship.ship_name == null && ship.ship_code_1 == null && ship.ship_code_2 != null)
            {
                var result = from s in db.Ships
                             where s.ship_code_2.StartsWith(ship.ship_code_2)
                             select s;
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