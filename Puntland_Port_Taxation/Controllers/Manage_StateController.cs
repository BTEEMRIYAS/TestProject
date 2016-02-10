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
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(7))
                {
                    int page;
                    int page_no = 1;
                    var count = db.States.Count();
                    double cnt = (double)count / 9;
                    var no_of_pages = Math.Ceiling(cnt);
                    int last_page = Convert.ToInt32(no_of_pages);
                    string value = Request["page"];
                    bool res = int.TryParse(value, out page);
                    if (res == true)
                    {
                        page_no = Convert.ToInt32(value);
                    }
                    if (!String.IsNullOrEmpty(value))
                    {
                        if (page_no > no_of_pages)
                        {
                            page = last_page;
                        }
                        else if (page_no <= 0)
                        {
                            page = 1;
                        }
                        else
                        {
                            page = Convert.ToInt32(value);
                        }
                    }
                    else
                    {
                        page = 1;
                    }
                    int start_from = ((page - 1) * 9);
                    ViewBag.start_from = start_from;
                    int end_to = start_from + 8;
                    ViewBag.total_page = count;
                    ViewBag.page = page;
                    var state = (from s in db.States
                                join c in db.Countries on s.country_id equals c.country_id
                                join g in db.Geographies on c.geography_id equals g.geography_id
                                select new StateModel { country_name = c.country_name, state_name = s.state_name, state_id = s.state_id, status_id = s.status_id, geography_name = g.geography_name }).OrderBy(s => s.state_name).Skip(start_from).Take(9);
                    return View(state.ToList());
                }
                else
                {
                    return RedirectToAction("../Home/Dashboard");
                }
            }
            else
            {
                return RedirectToAction("../Home");
            }
        }

        //
        // GET: /Manage_State/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(7))
                {
                    State state = db.States.Find(id);
                    if (state == null)
                    {
                        return HttpNotFound();
                    }
                    return View(state);
                }
                else
                {
                    return RedirectToAction("../Home/Dashboard");
                }
            }
            else
            {
                return RedirectToAction("../Home");
            }
        }

        //
        // GET: /Manage_State/Create

        public ActionResult Create()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(7))
                {
                    ViewBag.geography = new HomeController().Geography();
                    return View();
                }
                else
                {
                    return RedirectToAction("../Home/Dashboard");
                }
            }
            else
            {
                return RedirectToAction("../Home");
            }
        }

        //
        // POST: /Manage_State/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GeographyModel stateModel)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(7))
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
                else
                {
                    return RedirectToAction("../Home/Dashboard");
                }
            }
            else
            {
                return RedirectToAction("../Home");
            }
        }

        //
        // GET: /Manage_State/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(7))
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
                else
                {
                    return RedirectToAction("../Home/Dashboard");
                }
            }
            else
            {
                return RedirectToAction("../Home");
            }
        }

        //
        // POST: /Manage_State/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GeographyModel stateModel)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(7))
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
                else
                {
                    return RedirectToAction("../Home/Dashboard");
                }
            }
            else
            {
                return RedirectToAction("../Home");
            }
        }

        //
        // GET: /Manage_State/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(7))
                {
                    State state = db.States.Find(id);
                    if (state == null)
                    {
                        return HttpNotFound();
                    }
                    return View(state);
                }
                else
                {
                    return RedirectToAction("../Home/Dashboard");
                }
            }
            else
            {
                return RedirectToAction("../Home");
            }
        }

        //
        // POST: /Manage_State/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(7))
                {
                    State state = db.States.Find(id);
                    db.States.Remove(state);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("../Home/Dashboard");
                }
            }
            else
            {
                return RedirectToAction("../Home");
            }
        }

        //
        // GET: /Manage_State/DbSearch

        public ActionResult DbSearch()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(7))
                {
                    ViewBag.geography = new HomeController().Geography();
                    ViewBag.country = new HomeController().Country();
                    return View();
                }
                else
                {
                    return RedirectToAction("../Home/Dashboard");
                }
            }
            else
            {
                return RedirectToAction("../Home");
            }
        }

        //
        // POST: /Manage_State/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(GeographyModel state)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(7))
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
                else
                {
                    return RedirectToAction("../Home/Dashboard");
                }
            }
            else
            {
                return RedirectToAction("../Home");
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}