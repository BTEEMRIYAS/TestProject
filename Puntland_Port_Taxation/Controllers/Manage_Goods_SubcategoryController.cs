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
    public class Manage_Goods_SubcategoryController : Controller
    {
        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

        //
        // GET: /Manage_Goods_Subcategory/

        public ActionResult Index()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(2))
                {
                    int page;
                    int page_no = 1;
                    var count = db.Goods_Subcategory.Count();
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
                    var goods_sub_category = (from gs in db.Goods_Subcategory
                                             join gc in db.Goods_Category on gs.goods_category_id equals gc.goods_category_id
                                             select new Goods_Sub_CategoryModel { goods_subcategory_id = gs.goods_subcategory_id, goods_subcategory_name = gs.goods_subcategory_name, goods_category = gc.goods_category_name, status_id = gs.status_id, goods_subcategory_code = gs.goods_subcategory_code }).OrderBy( gs => gs.goods_subcategory_id).Skip(start_from).Take(9);
                    return View(goods_sub_category.ToList());
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
        // GET: /Manage_Goods_Subcategory/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(2))
                {
                    Goods_Subcategory goods_subcategory = db.Goods_Subcategory.Find(id);
                    if (goods_subcategory == null)
                    {
                        return HttpNotFound();
                    }
                    return View(goods_subcategory);
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
        // GET: /Manage_Goods_Subcategory/Create

        public ActionResult Create()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(2))
                {
                    ViewBag.categories = new HomeController().Category();
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
        // POST: /Manage_Goods_Subcategory/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Goods_Subcategory goods_subcategory)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(2))
                {
                    if (ModelState.IsValid)
                    {
                        var is_exist = (from gs in db.Goods_Subcategory where gs.goods_subcategory_name == goods_subcategory.goods_subcategory_name select gs).Count();
                        if (is_exist > 0)
                        {
                            TempData["errorMessage"] = "This Subcategory Already Exist";
                        }
                        else
                        {
                            goods_subcategory.status_id = 1;
                            goods_subcategory.created_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                            goods_subcategory.updated_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                            db.Goods_Subcategory.Add(goods_subcategory);                    
                            db.SaveChanges();
                            TempData["errorMessage"] = "Subcategory Added Successfully";
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
        // GET: /Manage_Goods_Subcategory/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(2))
                {
                    Goods_Subcategory goods_subcategory = db.Goods_Subcategory.Find(id);
                    if (goods_subcategory == null)
                    {
                        return HttpNotFound();
                    }
                    TempData["subcategory_id"] = goods_subcategory.goods_subcategory_id;
                    var category =from gs in db.Goods_Subcategory
                                  join gc in db.Goods_Category on gs.goods_category_id equals gc.goods_category_id
                                  where gs.goods_subcategory_id == goods_subcategory.goods_subcategory_id
                                  select new { gc.goods_category_id, gc.goods_category_name }; 
                    ViewBag.status = new HomeController().Status();
                    ViewBag.categories = new SelectList(category, "goods_category_id", "goods_category_name");
                    return View(goods_subcategory);
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
        // POST: /Manage_Goods_Subcategory/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Goods_Subcategory goods_subcategory)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(2))
                {
                    var subcategory_id = Convert.ToInt32(TempData["subcategory_id"]);
                    Goods_Subcategory goods_subcategory_new = db.Goods_Subcategory.Find(subcategory_id);
                    if (ModelState.IsValid)
                    {
                        var is_exist = (from gs in db.Goods_Subcategory where gs.goods_subcategory_name == goods_subcategory.goods_subcategory_name && gs.goods_subcategory_id != subcategory_id select gs).Count();
                        if (is_exist > 0)
                        {
                            TempData["errorMessage"] = "This Subcategory Already Exist";
                        }
                        else
                        {
                            goods_subcategory_new.goods_category_id = goods_subcategory.goods_category_id;
                            goods_subcategory_new.goods_subcategory_code = goods_subcategory.goods_subcategory_code;
                            goods_subcategory_new.goods_subcategory_name = goods_subcategory.goods_subcategory_name;
                            goods_subcategory_new.status_id = goods_subcategory.status_id;
                            goods_subcategory_new.updated_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                            db.Entry(goods_subcategory_new).State = EntityState.Modified;
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
        // GET: /Manage_Goods_Subcategory/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(2))
                {
                    Goods_Subcategory goods_subcategory = db.Goods_Subcategory.Find(id);
                    if (goods_subcategory == null)
                    {
                        return HttpNotFound();
                    }
                    return View(goods_subcategory);
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
        // POST: /Manage_Goods_Subcategory/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(2))
                {
                    Goods_Subcategory goods_subcategory = db.Goods_Subcategory.Find(id);
                    db.Goods_Subcategory.Remove(goods_subcategory);
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
        // GET: /Manage_Goods_Subcategory/DbSearch

        public ActionResult DbSearch()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(2))
                {
                    ViewBag.categories = new HomeController().Category();
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
        // POST: /Manage_Goods_Subcategory/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Goods_Subcategory goods_subcategory)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(2))
                {
                    //Queue q = new Queue();
                    if (goods_subcategory.goods_subcategory_name != null && goods_subcategory.goods_category_id != 0)
                    {
                        var result = from gs in db.Goods_Subcategory
                                     join gc in db.Goods_Category on gs.goods_category_id equals gc.goods_category_id
                                     where gs.goods_subcategory_name.StartsWith(goods_subcategory.goods_subcategory_name) && gs.goods_category_id == goods_subcategory.goods_category_id
                                     select new Goods_Sub_CategoryModel { goods_subcategory_id = gs.goods_subcategory_id, goods_subcategory_name = gs.goods_subcategory_name, goods_category = gc.goods_category_name, status_id = gs.status_id, goods_subcategory_code = gs.goods_subcategory_code };
                        return View("Index", result.ToList());
                    }
                    else if (goods_subcategory.goods_subcategory_name == null && goods_subcategory.goods_category_id != 0)
                    {
                        var result = from gs in db.Goods_Subcategory
                                     join gc in db.Goods_Category on gs.goods_category_id equals gc.goods_category_id
                                     where gs.goods_category_id == goods_subcategory.goods_category_id
                                     select new Goods_Sub_CategoryModel { goods_subcategory_id = gs.goods_subcategory_id, goods_subcategory_name = gs.goods_subcategory_name, goods_category = gc.goods_category_name, status_id = gs.status_id, goods_subcategory_code = gs.goods_subcategory_code };
                        return View("Index", result.ToList());
                    }
                    else if (goods_subcategory.goods_subcategory_name != null && goods_subcategory.goods_category_id == 0)
                    {
                        var result = from gs in db.Goods_Subcategory
                                     join gc in db.Goods_Category on gs.goods_category_id equals gc.goods_category_id
                                     where gs.goods_subcategory_name.StartsWith(goods_subcategory.goods_subcategory_name)
                                     select new Goods_Sub_CategoryModel { goods_subcategory_id = gs.goods_subcategory_id, goods_subcategory_name = gs.goods_subcategory_name, goods_category = gc.goods_category_name, status_id = gs.status_id, goods_subcategory_code = gs.goods_subcategory_code };
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