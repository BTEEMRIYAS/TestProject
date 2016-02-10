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
    public class Manage_StateController : Controller
    {
        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

        //
        // GET: /Manage_State/

        public ActionResult Index()
        {
            var state = from s in db.States
                        join c in db.Countries on s.country_id equals c.country_id
                        join g in db.Geographies on c.geography_id equals g.geography_id
                        select new StateModel { country_name = c.country_name, state_name = s.state_name, state_id = s.state_id, status_id = s.status_id, geography_name = g.geography_name };
            return View(state.ToList());
        }

        //
        // GET: /Manage_State/Details/5

        public ActionResult Details(int id = 0)
        {
            State state = db.States.Find(id);
            if (state == null)
            {
                return HttpNotFound();
            }
            return View(state);
        }

        //
        // GET: /Manage_State/Create

        public ActionResult Create()
        {
            ViewBag.geography = new HomeController().Geography();
            return View();
        }

        //
        // POST: /Manage_State/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GeographyModel stateModel)
        {
            var isexist = from s in db.States
                          where s.state_name == stateModel.state_name
                          select s;
            if (isexist.Count() > 0)
            {
                TempData["errorMessage"] = "This State Already Exist";
                return RedirectToAction("Index");
            }
            State state = new State();
            if (ModelState.IsValid)
            {
                state.country_id = stateModel.country_id;
                state.state_name = stateModel.state_name;
                state.status_id = 1;
                state.created_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                state.updated_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                db.States.Add(state);
                db.SaveChanges();
                TempData["errorMessage"] = "State Added Successfully";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Manage_State/Edit/5

        public ActionResult Edit(int id = 0)
        {
            State state = db.States.Find(id);
            if (state == null)
            {
                return HttpNotFound();
            }
            var geography = from s in db.States
                            join c in db.Countries on s.country_id equals c.country_id
                            join g in db.Geographies on c.geography_id equals g.geography_id
                            where s.state_id == state.state_id
                            select new { country_name = c.country_name, country_id = c.country_id, geography_id = g.geography_id };
            ViewBag.country = new SelectList(geography, "country_id", "country_name");
            GeographyModel stateModel = new GeographyModel();
            stateModel.country_id = state.country_id;
            stateModel.state_name = state.state_name;
            stateModel.status_id = state.status_id;
            stateModel.state_id = state.state_id;
            stateModel.geography_id = geography.First().geography_id;
            ViewBag.geography = new HomeController().Geography();
            ViewBag.status = new HomeController().Status();
            TempData["id"] = state.state_id;
            TempData["created_date"] = state.created_date;
            return View(stateModel);
        }

        //
        // POST: /Manage_State/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GeographyModel stateModel)
        {
            int id = Convert.ToInt32(TempData["id"]);
            var isexist = from s in db.States
                          where s.state_name == stateModel.state_name && s.state_id != id
                          select s;
            if (isexist.Count() > 0)
            {
                TempData["errorMessage"] = "This State Already Exist";
                return RedirectToAction("Index");
            }
            var created_date = TempData["created_date"].ToString();
            State state = new State();
            if (ModelState.IsValid)
            {
                state.country_id = stateModel.country_id;
                state.state_name = stateModel.state_name;
                state.status_id = stateModel.status_id;
                state.state_id = id;
                state.created_date = created_date;
                state.updated_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                db.Entry(state).State = EntityState.Modified;
                db.SaveChanges();
                TempData["errorMessage"] = "Edited Successfully";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Manage_State/Delete/5

        public ActionResult Delete(int id = 0)
        {
            State state = db.States.Find(id);
            if (state == null)
            {
                return HttpNotFound();
            }
            return View(state);
        }

        //
        // POST: /Manage_State/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            State state = db.States.Find(id);
            db.States.Remove(state);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // GET: /Manage_State/DbSearch

        public ActionResult DbSearch()
        {
            ViewBag.geography = new HomeController().Geography();
            ViewBag.country = new HomeController().Country();
            return View();
        }

        //
        // POST: /Manage_State/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(GeographyModel state)
        {
            if (state.geography_id != 0 && state.country_id != 0 && state.state_name != null)
            {
                var result = from s in db.States
                             join c in db.Countries on s.country_id equals c.country_id
                             join g in db.Geographies on c.geography_id equals g.geography_id
                             where g.geography_id == state.geography_id && s.country_id == state.country_id && s.state_name.StartsWith(state.state_name)
                             select new StateModel { country_name = c.country_name, state_name = s.state_name, state_id = s.state_id, status_id = s.status_id, geography_name = g.geography_name };
                return View("Index", result.ToList());
            }
            else if (state.geography_id != 0 && state.country_id != 0 && state.state_name == null)
            {
                var result = from s in db.States
                             join c in db.Countries on s.country_id equals c.country_id
                             join g in db.Geographies on c.geography_id equals g.geography_id
                             where g.geography_id == state.geography_id && s.country_id == state.country_id
                             select new StateModel { country_name = c.country_name, state_name = s.state_name, state_id = s.state_id, status_id = s.status_id, geography_name = g.geography_name };
                return View("Index", result.ToList());
            }
            else if (state.geography_id != 0 && state.country_id == 0 && state.state_name != null)
            {
                var result = from s in db.States
                             join c in db.Countries on s.country_id equals c.country_id
                             join g in db.Geographies on c.geography_id equals g.geography_id
                             where g.geography_id == state.geography_id && s.state_name.StartsWith(state.state_name)
                             select new StateModel { country_name = c.country_name, state_name = s.state_name, state_id = s.state_id, status_id = s.status_id, geography_name = g.geography_name };
                return View("Index", result.ToList());
            }
            else if (state.geography_id != 0 && state.country_id == 0 && state.state_name == null)
            {
                var result = from s in db.States
                             join c in db.Countries on s.country_id equals c.country_id
                             join g in db.Geographies on c.geography_id equals g.geography_id
                             where g.geography_id == state.geography_id
                             select new StateModel { country_name = c.country_name, state_name = s.state_name, state_id = s.state_id, status_id = s.status_id, geography_name = g.geography_name };
                return View("Index", result.ToList());
            }
            else if (state.geography_id == 0 && state.country_id != 0 && state.state_name != null)
            {
                var result = from s in db.States
                             join c in db.Countries on s.country_id equals c.country_id
                             join g in db.Geographies on c.geography_id equals g.geography_id
                             where s.country_id == state.country_id && s.state_name.StartsWith(state.state_name)
                             select new StateModel { country_name = c.country_name, state_name = s.state_name, state_id = s.state_id, status_id = s.status_id, geography_name = g.geography_name };
                return View("Index", result.ToList());
            }
            else if (state.geography_id == 0 && state.country_id != 0 && state.state_name == null)
            {
                var result = from s in db.States
                             join c in db.Countries on s.country_id equals c.country_id
                             join g in db.Geographies on c.geography_id equals g.geography_id
                             where s.country_id == state.country_id
                             select new StateModel { country_name = c.country_name, state_name = s.state_name, state_id = s.state_id, status_id = s.status_id, geography_name = g.geography_name };
                return View("Index", result.ToList());
            }
            else if (state.geography_id == 0 && state.country_id == 0 && state.state_name != null)
            {
                var result = from s in db.States
                             join c in db.Countries on s.country_id equals c.country_id
                             join g in db.Geographies on c.geography_id equals g.geography_id
                             where s.state_name.StartsWith(state.state_name)
                             select new StateModel { country_name = c.country_name, state_name = s.state_name, state_id = s.state_id, status_id = s.status_id, geography_name = g.geography_name };
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