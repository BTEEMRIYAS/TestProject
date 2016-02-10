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
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(7))
                {
                    int page;
                    int page_no = 1;
                    var count = db.Geographies.Count();
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
                    return View(db.Geographies.OrderBy(g => g.geography_name).Skip(start_from).Take(9).ToList());
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
        // GET: /Manage_Geography/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(7))
                {
                    Geography geography = db.Geographies.Find(id);
                    if (geography == null)
                    {
                        return HttpNotFound();
                    }
                    return View(geography);
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
        // GET: /Manage_Geography/Create

        public ActionResult Create()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(7))
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
        // POST: /Manage_Geography/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Geography geography)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(7))
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
        // GET: /Manage_Geography/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(7))
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
        // POST: /Manage_Geography/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Geography geography)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(7))
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
        // GET: /Manage_Geography/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(7))
                {
                    Geography geography = db.Geographies.Find(id);
                    if (geography == null)
                    {
                        return HttpNotFound();
                    }
                    return View(geography);
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
        // POST: /Manage_Geography/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(7))
                {
                    Geography geography = db.Geographies.Find(id);
                    db.Geographies.Remove(geography);
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
        // GET: /Manage_Geography/DbSearch

        public ActionResult DbSearch()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(7))
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
        // POST: /Manage_Geography/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Geography geography)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(7))
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