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
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(22))
                {
                    int page;
                    int page_no = 1;
                    var count = db.Importer_Type.Count();
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
                    return View(db.Importer_Type.OrderBy(it => it.importer_type_id).Skip(start_from).Take(9).ToList());
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
        // GET: /Manage_Importer_Type/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(22))
                {
                    Importer_Type importer_type = db.Importer_Type.Find(id);
                    if (importer_type == null)
                    {
                        return HttpNotFound();
                    }
                    return View(importer_type);
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
        // GET: /Manage_Importer_Type/Create

        public ActionResult Create()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(22))
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
        // POST: /Manage_Importer_Type/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Importer_Type importer_type)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(22))
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
        // GET: /Manage_Importer_Type/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(22))
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
        // POST: /Manage_Importer_Type/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Importer_Type importer_type)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(22))
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
        // GET: /Manage_Importer_Type/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(22))
                {
                    Importer_Type importer_type = db.Importer_Type.Find(id);
                    if (importer_type == null)
                    {
                        return HttpNotFound();
                    }
                    return View(importer_type);
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
        // POST: /Manage_Importer_Type/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(22))
                {
                    Importer_Type importer_type = db.Importer_Type.Find(id);
                    db.Importer_Type.Remove(importer_type);
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
        // GET: /Manage_Importer_Type/DbSearch

        public ActionResult DbSearch()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(22))
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
        // POST: /Manage_Importer_Type/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Importer_Type importer_type)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(22))
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