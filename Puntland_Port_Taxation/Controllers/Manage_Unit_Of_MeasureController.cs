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
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(23))
                {
                    int page;
                    int page_no = 1;
                    var count = db.Unit_Of_Measure.Count();
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
                    return View(db.Unit_Of_Measure.OrderBy(u => u.unit_id).Skip(start_from).Take(9).ToList());
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
        // GET: /Manage_Unit_Of_Measure/Details/5

        public ActionResult Details(int id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(23))
                {
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
        // GET: /Manage_Unit_Of_Measure/Create

        public ActionResult Create()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(23))
                {
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
        // POST: /Manage_Unit_Of_Measure/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Unit_Of_Measure unit_Of_Measure)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(23))
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
        // GET: /Manage_Unit_Of_Measure/Edit/5

        public ActionResult Edit(int id=0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(23))
                {
                    Unit_Of_Measure unit_Of_Measure = db.Unit_Of_Measure.Find(id);
            
                    if (unit_Of_Measure == null)
                    {
                        return HttpNotFound();
                    }
                    TempData["id"] = unit_Of_Measure.unit_id;
                    return View(unit_Of_Measure);
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
        // POST: /Manage_Unit_Of_Measure/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Unit_Of_Measure unit_Of_Measure)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(23))
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
        // GET: /Manage_Department/DbSearch

        public ActionResult DbSearch()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(23))
                {
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

        ////
        //// POST: /Manage_Department/DbSearchresult
        ////In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Unit_Of_Measure Unit_Of_Measure)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(23))
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
    }
}
