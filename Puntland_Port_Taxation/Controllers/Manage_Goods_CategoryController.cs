using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Puntland_Port_Taxation.Controllers
{
    public class Manage_Goods_CategoryController : Controller
    {
        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

        //
        // GET: /Manage_Goods_Category/

        public ActionResult Index()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(2))
                {
                    int page;
                    int page_no = 1;
                    var count = db.Goods_Category.Count();
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
                    return View(db.Goods_Category.OrderBy(gc => gc.goods_category_id).Skip(start_from).Take(9).ToList());
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
        // GET: /Manage_Goods_Category/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(2))
                {
                    Goods_Category goods_category = db.Goods_Category.Find(id);
                    if (goods_category == null)
                    {
                        return HttpNotFound();
                    }
                    return View(goods_category);
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
        // GET: /Manage_Goods_Category/Create

        public ActionResult Create()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(2))
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
        // POST: /Manage_Goods_Category/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Goods_Category goods_category)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(2))
                {
                    if (ModelState.IsValid)
                    {
                        var is_exist = (from gc in db.Goods_Category where gc.goods_category_name == goods_category.goods_category_name select gc).Count();
                        if (is_exist > 0)
                        {
                            TempData["errorMessage"] = "This Category Already Exist";
                        }
                        else
                        {
                            goods_category.status_id = 1;
                            goods_category.created_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                            goods_category.updated_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                            db.Goods_Category.Add(goods_category);                
                            db.SaveChanges();
                            TempData["errorMessage"] = "Category Added Successfully";
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
        // GET: /Manage_Goods_Category/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(2))
                {
                    Goods_Category goods_category = db.Goods_Category.Find(id);
                    if (goods_category == null)
                    {
                        return HttpNotFound();
                    }
                    TempData["category_id"] = goods_category.goods_category_id;
                    ViewBag.status = new HomeController().Status();
                    return View(goods_category);
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
        // POST: /Manage_Goods_Category/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Goods_Category goods_category)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(2))
                {
                    var category_id = Convert.ToInt32(TempData["category_id"]); 
                    Goods_Category goods_category_new = db.Goods_Category.Find(category_id);
                    if (ModelState.IsValid)
                    {
                        var is_exist = (from gc in db.Goods_Category where gc.goods_category_name == goods_category.goods_category_name && gc.goods_category_id != category_id select gc).Count();
                        if (is_exist > 0)
                        {
                            TempData["errorMessage"] = "This Category Already Exist";
                        }
                        else
                        {
                            goods_category_new.goods_category_name = goods_category.goods_category_name;
                            goods_category_new.goods_category_code = goods_category.goods_category_code;
                            goods_category_new.status_id = goods_category.status_id;
                            goods_category_new.updated_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                            db.Entry(goods_category_new).State = EntityState.Modified;
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
        // GET: /Manage_Goods_Category/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(2))
                {
                    Goods_Category goods_category = db.Goods_Category.Find(id);
                    if (goods_category == null)
                    {
                        return HttpNotFound();
                    }
                    return View(goods_category);
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
        // POST: /Manage_Goods_Category/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(2))
                {
                    Goods_Category goods_category = db.Goods_Category.Find(id);
                    db.Goods_Category.Remove(goods_category);
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
        // GET: /Manage_Goods_Category/DbSearch

        public ActionResult DbSearch()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(2))
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
        // POST: /Manage_Goods_Category/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Goods_Category goods_category)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(2))
                {
                    if (goods_category.goods_category_name != null)
                    {
                        var result = from g in db.Goods_Category
                                     where g.goods_category_name.StartsWith(goods_category.goods_category_name)
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
    }
}