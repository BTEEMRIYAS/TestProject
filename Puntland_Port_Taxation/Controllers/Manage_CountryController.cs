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
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(7))
                {
                    int page;
                    int page_no = 1;
                    var count = db.Countries.Count();
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
                    var country = (from c in db.Countries
                                  join g in db.Geographies on c.geography_id equals g.geography_id
                                  orderby c.country_name ascending
                                  select new GeographyModel { geography_name = g.geography_name, country_name = c.country_name, country_id = c.country_id, status_id = c.status_id }).Skip(start_from).Take(9);
                    return View(country.ToList());
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
        // GET: /Manage_Country/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(7))
                {
                    Country country = db.Countries.Find(id);
                    if (country == null)
                    {
                        return HttpNotFound();
                    }
                    return View(country);
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
        // GET: /Manage_Country/Create

        public ActionResult Create()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(7))
                {
                    ViewBag.geogaphy = new HomeController().Geography();
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
        // POST: /Manage_Country/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Country country)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(7))
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
        // GET: /Manage_Country/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(7))
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
        // POST: /Manage_Country/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Country country)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(7))
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
        // GET: /Manage_Country/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(7))
                {
                    Country country = db.Countries.Find(id);
                    if (country == null)
                    {
                        return HttpNotFound();
                    }
                    return View(country);
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
        // POST: /Manage_Country/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(7))
                {
                    Country country = db.Countries.Find(id);
                    db.Countries.Remove(country);
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
        // GET: /Manage_Country/DbSearch

        public ActionResult DbSearch()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(7))
                {
                    ViewBag.geogaphy = new HomeController().Geography();
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
        // POST: /Manage_Country/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Country country)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(7))
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