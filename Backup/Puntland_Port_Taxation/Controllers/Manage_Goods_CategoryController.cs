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
            return View(db.Goods_Category.ToList());
        }

        //
        // GET: /Manage_Goods_Category/Details/5

        public ActionResult Details(int id = 0)
        {
            Goods_Category goods_category = db.Goods_Category.Find(id);
            if (goods_category == null)
            {
                return HttpNotFound();
            }
            return View(goods_category);
        }

        //
        // GET: /Manage_Goods_Category/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Manage_Goods_Category/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Goods_Category goods_category)
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

        //
        // GET: /Manage_Goods_Category/Edit/5

        public ActionResult Edit(int id = 0)
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

        //
        // POST: /Manage_Goods_Category/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Goods_Category goods_category)
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

        //
        // GET: /Manage_Goods_Category/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Goods_Category goods_category = db.Goods_Category.Find(id);
            if (goods_category == null)
            {
                return HttpNotFound();
            }
            return View(goods_category);
        }

        //
        // POST: /Manage_Goods_Category/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Goods_Category goods_category = db.Goods_Category.Find(id);
            db.Goods_Category.Remove(goods_category);
            db.SaveChanges();
            return RedirectToAction("Index");
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
            return View();
        }

        //
        // POST: /Manage_Goods_Category/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Goods_Category goods_category)
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
    }
}