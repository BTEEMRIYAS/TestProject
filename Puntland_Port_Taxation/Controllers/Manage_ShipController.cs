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
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(8))
                {
                    int page;
                    int page_no = 1;
                    var count = db.Ships.Count();
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
                    return View(db.Ships.OrderBy(s => s.ship_id).Skip(start_from).Take(9).ToList());
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
        // GET: /Manage_Ship/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(8))
                {
                    Ship ship = db.Ships.Find(id);
                    if (ship == null)
                    {
                        return HttpNotFound();
                    }
                    return View(ship);
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
        // GET: /Manage_Ship/Create

        public ActionResult Create()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(8))
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
        // POST: /Manage_Ship/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Ship ship)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(8))
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
        // GET: /Manage_Ship/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(8))
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
        // POST: /Manage_Ship/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Ship ship)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(8))
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
        // GET: /Manage_Ship/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(8))
                {
                    Ship ship = db.Ships.Find(id);
                    if (ship == null)
                    {
                        return HttpNotFound();
                    }
                    return View(ship);
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
        // POST: /Manage_Ship/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(8))
                {
                    Ship ship = db.Ships.Find(id);
                    db.Ships.Remove(ship);
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
        // GET: /Manage_Ship/DbSearch

        public ActionResult DbSearch()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(8))
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
        // POST: /Manage_Ship/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Ship ship)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(8))
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