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
    public class Manage_Function_Role_MappingController : Controller
    {
        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

        //
        // GET: /Manage_Function_Role_Mapping/

        public ActionResult Index()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(6))
                {
                    int page;
                    int page_no = 1;
                    var count = db.Roles.Count();
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
                    var model = (from r in db.Roles
                                 select new Function_Role_MapModel { role_id = r.role_id, role_name = r.role_name }).OrderBy(e => e.role_id).Skip(start_from).Take(9);
                    return View(model);
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
        // GET: /Manage_Function_Role_Mapping/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(6))
                {
                    var details = from c in db.Roles
                                  join f in db.Function_Role_Map on c.role_id equals f.role_id
                                  join fm in db.Functions on f.function_id equals fm.function_id
                                  where c.role_id == id
                                  select new Function_Role_MapModel { role_id = c.role_id, function_id = f.function_id, status_id = 1, role_name = c.role_name, function_name = fm.function_name, created_date = f.created_date, updated_date = f.updated_date };
                    return View(details);
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
        // GET: /Manage_Function_Role_Mapping/Create

        public ActionResult Create()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(6))
                {
                    ViewBag.roles = new HomeController().Roles();
                    ViewBag.functions = new HomeController().Function();
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
        // POST: /Manage_Function_Role_Mapping/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Function_Role_Map function_role_map)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(6))
                {
                    string dtNow = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");

                    if (ModelState.IsValid)
                    {
                        //delete existing data
                        var deleteExistingData = from details in db.Function_Role_Map where details.role_id == function_role_map.role_id select details;
                        foreach (var detail in deleteExistingData)
                        {
                            db.Function_Role_Map.Remove(detail);
                        }
                        db.SaveChanges();

                        //save new 

                        if (!string.IsNullOrEmpty(Request.Form["rolefunction"]))
                        {
                            string[] temp = Request.Form["rolefunction"].Split(',');
                            for (int i = 0; i < temp.Count(); i++)
                            {
                                function_role_map.function_id = int.Parse(temp[i].ToString());
                                function_role_map.status_id = 1;
                                function_role_map.created_date = dtNow;
                                function_role_map.updated_date = dtNow;
                                db.Function_Role_Map.Add(function_role_map);
                                db.SaveChanges();
                                TempData["errorMessage"] = "Role Maped Successfully";
                            }
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
        // GET: /Manage_Function_Role_Mapping/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(6))
                {
                    Role role_dm = new Role();
                    role_dm.role_id = id;
                    var roles = from r in db.Roles
                                where r.role_id == id
                                select r;
                    ViewBag.roles = new SelectList(roles, "role_id", "role_name");
                    ViewBag.functions = new HomeController().Function();
                    ViewBag.existingfunctions = new HomeController().Function_Roles(id);
                    return View(role_dm);
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
        // POST: /Manage_Function_Role_Mapping/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Function_Role_Map function_role_map)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(6))
                {
                    if (ModelState.IsValid)
                    {
                        string dtNow = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                        //delete existing data
                        var deleteExistingData = from details in db.Function_Role_Map where details.role_id == function_role_map.role_id select details;
                        foreach (var detail in deleteExistingData)
                        {
                            db.Function_Role_Map.Remove(detail);
                        }
                        db.SaveChanges();

                        //save new 

                        if (!string.IsNullOrEmpty(Request.Form["rolefunction"]))
                        {
                            string[] temp = Request.Form["rolefunction"].Split(',');
                            for (int i = 0; i < temp.Count(); i++)
                            {
                                function_role_map.function_id = int.Parse(temp[i].ToString());
                                function_role_map.status_id = 1;
                                function_role_map.updated_date = dtNow;
                                function_role_map.created_date = dtNow;
                                db.Function_Role_Map.Add(function_role_map);
                                db.SaveChanges();
                                TempData["errorMessage"] = "Edited Successfully";
                            }
                        }
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
        // GET: /Manage_Function_Role_Mapping/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(6))
                {
                    Function_Role_Map function_role_map = db.Function_Role_Map.Find(id);
                    if (function_role_map == null)
                    {
                        return HttpNotFound();
                    }
                    return View(function_role_map);
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
        // POST: /Manage_Function_Role_Mapping/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(6))
                {
                    Function_Role_Map function_role_map = db.Function_Role_Map.Find(id);
                    db.Function_Role_Map.Remove(function_role_map);
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

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        //
        // GET: /Manage_Function_Role_Mapping/DbSearch

        public ActionResult DbSearch()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(6))
                {
                    ViewBag.roles = new HomeController().Roles();
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
        // POST: /Manage_Function_Role_Mapping/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Function_Role_Map function_role_map)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(6))
                {
                    if (function_role_map.role_id != 0)
                    {
                        var result = from r in db.Roles
                                     where r.role_id == function_role_map.role_id
                                     select new Function_Role_MapModel { role_name = r.role_name, role_id = r.role_id };
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