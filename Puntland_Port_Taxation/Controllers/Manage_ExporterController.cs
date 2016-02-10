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
    public class Manage_ExporterController : Controller
    {
        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

        //
        // GET: /Manage_Exporter/

        public ActionResult Index()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(29))
                {
                    int page;
                    int page_no = 1;
                    var count = db.Importers.Count();
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
                    var importer = (from i in db.Importers
                                    join c in db.Countries on i.importer_country_id equals c.country_id
                                    join t in db.Importer_Type on i.importer_type_id equals t.importer_type_id
                                    select new ImporterModel { importer_id = i.importer_id, importer_first_name = i.importer_first_name, importer_middle_name = i.importer_middle_name, importer_last_name = i.importer_last_name, importer_email_id = i.importer_email_id, importer_mob_no = i.importer_mob_no, status_id = i.status_id, importer_country_name = c.country_name, importer_type_name = t.importer_type_name, multiple_way_bill = t.is_multiple_allowed }).OrderBy(ir => ir.importer_id).Skip(start_from).Take(9);
                    return View(importer.ToList());
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
        // GET: /Manage_Exporter/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(29))
                {
                    Importer importer = db.Importers.Find(id);
                    if (importer == null)
                    {
                        return HttpNotFound();
                    }
                    return View(importer);
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
        // GET: /Manage_Exporter/Create

        public ActionResult Create()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(29))
                {
                    ViewBag.importer_type = new HomeController().Importer_Type();
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
        // POST: /Manage_Exporter/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Importer importer)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(29))
                {
                    if (ModelState.IsValid)
                    {
                        var is_exist = (from i in db.Importers where i.importer_mob_no == importer.importer_mob_no select i).Count();
                        if (is_exist > 0)
                        {
                            TempData["errorMessage"] = "This Mobile Number Already Exist";
                        }
                        else
                        {
                            importer.importer_first_name = importer.importer_first_name.Trim();
                            importer.importer_middle_name = importer.importer_middle_name.Trim();
                            importer.importer_last_name = importer.importer_last_name.Trim();
                            importer.status_id = 1;
                            importer.created_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                            importer.updated_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                            db.Importers.Add(importer);
                            db.SaveChanges();
                            TempData["errorMessage"] = "Exporter Added Successfully";
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
        // GET: /Manage_Exporter/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(29))
                {
                    Importer importer = db.Importers.Find(id);
                    if (importer == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.importer_type = new HomeController().Importer_Type();
                    ViewBag.country = new HomeController().Country();
                    ViewBag.status = new HomeController().Status();
                    TempData["id"] = importer.importer_id;
                    TempData["created_date"] = importer.created_date;
                    return View(importer);
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
        // POST: /Manage_Exporter/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Importer importer)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(29))
                {
                    int id = Convert.ToInt32(TempData["id"]);
                    var created_date = TempData["created_date"].ToString();
                    if (ModelState.IsValid)
                    {
                        var is_exist = (from i in db.Importers where i.importer_mob_no == importer.importer_mob_no && i.importer_id != id select i).Count();
                        if (is_exist > 0)
                        {
                            TempData["errorMessage"] = "This Mobile Number Already Exist";
                        }
                        else
                        {
                            importer.importer_first_name = importer.importer_first_name.Trim();
                            importer.importer_middle_name = importer.importer_middle_name.Trim();
                            importer.importer_last_name = importer.importer_last_name.Trim();
                            importer.importer_id = id;
                            importer.created_date = created_date;
                            importer.updated_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                            db.Entry(importer).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["errorMessage"] = "Edited Successfully";
                        }
                        return RedirectToAction("Index");
                    }
                    return View(importer);
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
        // GET: /Manage_Exporter/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(29))
                {
                    Importer importer = db.Importers.Find(id);
                    if (importer == null)
                    {
                        return HttpNotFound();
                    }
                    return View(importer);
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
        // POST: /Manage_Exporter/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(29))
                {
                    Importer importer = db.Importers.Find(id);
                    db.Importers.Remove(importer);
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
        // GET: /Manage_Exporter/DbSearch

        public ActionResult DbSearch()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(29))
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
        // POST: /Manage_Exporter/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Importer importer)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(29))
                {
                    if (importer.importer_first_name != null && importer.importer_middle_name != null && importer.importer_last_name != null)
                    {
                        var result = from i in db.Importers
                                     join c in db.Countries on i.importer_country_id equals c.country_id
                                     join t in db.Importer_Type on i.importer_type_id equals t.importer_type_id
                                     where i.importer_first_name.StartsWith(importer.importer_first_name) && i.importer_middle_name.StartsWith(importer.importer_middle_name) && i.importer_last_name.StartsWith(importer.importer_last_name)
                                     select new ImporterModel { importer_id = i.importer_id, importer_first_name = i.importer_first_name, importer_middle_name = i.importer_middle_name, importer_last_name = i.importer_last_name, importer_email_id = i.importer_email_id, importer_mob_no = i.importer_mob_no, status_id = i.status_id, importer_country_name = c.country_name, importer_type_name = t.importer_type_name };
                        return View("Index", result.ToList());
                    }
                    else if (importer.importer_first_name != null && importer.importer_middle_name != null && importer.importer_last_name == null)
                    {
                        var result = from i in db.Importers
                                     join c in db.Countries on i.importer_country_id equals c.country_id
                                     join t in db.Importer_Type on i.importer_type_id equals t.importer_type_id
                                     where i.importer_first_name.StartsWith(importer.importer_first_name) && i.importer_middle_name.StartsWith(importer.importer_middle_name)
                                     select new ImporterModel { importer_id = i.importer_id, importer_first_name = i.importer_first_name, importer_middle_name = i.importer_middle_name, importer_last_name = i.importer_last_name, importer_email_id = i.importer_email_id, importer_mob_no = i.importer_mob_no, status_id = i.status_id, importer_country_name = c.country_name, importer_type_name = t.importer_type_name };
                        return View("Index", result.ToList());
                    }
                    else if (importer.importer_first_name != null && importer.importer_middle_name == null && importer.importer_last_name != null)
                    {
                        var result = from i in db.Importers
                                     join c in db.Countries on i.importer_country_id equals c.country_id
                                     join t in db.Importer_Type on i.importer_type_id equals t.importer_type_id
                                     where i.importer_first_name.StartsWith(importer.importer_first_name) && i.importer_last_name.StartsWith(importer.importer_last_name)
                                     select new ImporterModel { importer_id = i.importer_id, importer_first_name = i.importer_first_name, importer_middle_name = i.importer_middle_name, importer_last_name = i.importer_last_name, importer_email_id = i.importer_email_id, importer_mob_no = i.importer_mob_no, status_id = i.status_id, importer_country_name = c.country_name, importer_type_name = t.importer_type_name };
                        return View("Index", result.ToList());
                    }
                    else if (importer.importer_first_name != null && importer.importer_middle_name == null && importer.importer_last_name == null)
                    {
                        var result = from i in db.Importers
                                     join c in db.Countries on i.importer_country_id equals c.country_id
                                     join t in db.Importer_Type on i.importer_type_id equals t.importer_type_id
                                     where i.importer_first_name.StartsWith(importer.importer_first_name)
                                     select new ImporterModel { importer_id = i.importer_id, importer_first_name = i.importer_first_name, importer_middle_name = i.importer_middle_name, importer_last_name = i.importer_last_name, importer_email_id = i.importer_email_id, importer_mob_no = i.importer_mob_no, status_id = i.status_id, importer_country_name = c.country_name, importer_type_name = t.importer_type_name };
                        return View("Index", result.ToList());
                    }
                    else if (importer.importer_first_name == null && importer.importer_middle_name != null && importer.importer_last_name != null)
                    {
                        var result = from i in db.Importers
                                     join c in db.Countries on i.importer_country_id equals c.country_id
                                     join t in db.Importer_Type on i.importer_type_id equals t.importer_type_id
                                     where i.importer_middle_name.StartsWith(importer.importer_middle_name) && i.importer_last_name.StartsWith(importer.importer_last_name)
                                     select new ImporterModel { importer_id = i.importer_id, importer_first_name = i.importer_first_name, importer_middle_name = i.importer_middle_name, importer_last_name = i.importer_last_name, importer_email_id = i.importer_email_id, importer_mob_no = i.importer_mob_no, status_id = i.status_id, importer_country_name = c.country_name, importer_type_name = t.importer_type_name };
                        return View("Index", result.ToList());
                    }
                    else if (importer.importer_first_name == null && importer.importer_middle_name != null && importer.importer_last_name == null)
                    {
                        var result = from i in db.Importers
                                     join c in db.Countries on i.importer_country_id equals c.country_id
                                     join t in db.Importer_Type on i.importer_type_id equals t.importer_type_id
                                     where i.importer_middle_name.StartsWith(importer.importer_middle_name)
                                     select new ImporterModel { importer_id = i.importer_id, importer_first_name = i.importer_first_name, importer_middle_name = i.importer_middle_name, importer_last_name = i.importer_last_name, importer_email_id = i.importer_email_id, importer_mob_no = i.importer_mob_no, status_id = i.status_id, importer_country_name = c.country_name, importer_type_name = t.importer_type_name };
                        return View("Index", result.ToList());
                    }
                    else if (importer.importer_first_name == null && importer.importer_middle_name == null && importer.importer_last_name != null)
                    {
                        var result = from i in db.Importers
                                     join c in db.Countries on i.importer_country_id equals c.country_id
                                     join t in db.Importer_Type on i.importer_type_id equals t.importer_type_id
                                     where i.importer_last_name.StartsWith(importer.importer_last_name)
                                     select new ImporterModel { importer_id = i.importer_id, importer_first_name = i.importer_first_name, importer_middle_name = i.importer_middle_name, importer_last_name = i.importer_last_name, importer_email_id = i.importer_email_id, importer_mob_no = i.importer_mob_no, status_id = i.status_id, importer_country_name = c.country_name, importer_type_name = t.importer_type_name };
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
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace Puntland_Port_Taxation.Controllers
//{
//    public class Manage_ExporterController : Controller
//    {
//        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

//        //
//        // GET: /Manage_Exporter/

//        public ActionResult Index()
//        {
//            return View(db.Importers.ToList());
//        }

//        //
//        // GET: /Manage_Exporter/Details/5

//        public ActionResult Details(int id = 0)
//        {
//            Importer importer = db.Importers.Find(id);
//            if (importer == null)
//            {
//                return HttpNotFound();
//            }
//            return View(importer);
//        }

//        //
//        // GET: /Manage_Exporter/Create

//        public ActionResult Create()
//        {
//            return View();
//        }

//        //
//        // POST: /Manage_Exporter/Create

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create(Importer importer)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Importers.Add(importer);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            return View(importer);
//        }

//        //
//        // GET: /Manage_Exporter/Edit/5

//        public ActionResult Edit(int id = 0)
//        {
//            Importer importer = db.Importers.Find(id);
//            if (importer == null)
//            {
//                return HttpNotFound();
//            }
//            return View(importer);
//        }

//        //
//        // POST: /Manage_Exporter/Edit/5

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit(Importer importer)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(importer).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(importer);
//        }

//        //
//        // GET: /Manage_Exporter/Delete/5

//        public ActionResult Delete(int id = 0)
//        {
//            Importer importer = db.Importers.Find(id);
//            if (importer == null)
//            {
//                return HttpNotFound();
//            }
//            return View(importer);
//        }

//        //
//        // POST: /Manage_Exporter/Delete/5

//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            Importer importer = db.Importers.Find(id);
//            db.Importers.Remove(importer);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            db.Dispose();
//            base.Dispose(disposing);
//        }
//    }
//}