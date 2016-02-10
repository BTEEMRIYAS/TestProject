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
            var goods_sub_category = from gs in db.Goods_Subcategory
                                     join gc in db.Goods_Category on gs.goods_category_id equals gc.goods_category_id
                                     select new Goods_Sub_CategoryModel { goods_subcategory_id = gs.goods_subcategory_id, goods_subcategory_name = gs.goods_subcategory_name, goods_category = gc.goods_category_name, status_id = gs.status_id };
            return View(goods_sub_category.ToList());
        }

        //
        // GET: /Manage_Goods_Subcategory/Details/5

        public ActionResult Details(int id = 0)
        {
            Goods_Subcategory goods_subcategory = db.Goods_Subcategory.Find(id);
            if (goods_subcategory == null)
            {
                return HttpNotFound();
            }
            return View(goods_subcategory);
        }

        //
        // GET: /Manage_Goods_Subcategory/Create

        public ActionResult Create()
        {
            ViewBag.categories = new HomeController().Category();
            return View();
        }

        //
        // POST: /Manage_Goods_Subcategory/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Goods_Subcategory goods_subcategory)
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

        //
        // GET: /Manage_Goods_Subcategory/Edit/5

        public ActionResult Edit(int id = 0)
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

        //
        // POST: /Manage_Goods_Subcategory/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Goods_Subcategory goods_subcategory)
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

        //
        // GET: /Manage_Goods_Subcategory/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Goods_Subcategory goods_subcategory = db.Goods_Subcategory.Find(id);
            if (goods_subcategory == null)
            {
                return HttpNotFound();
            }
            return View(goods_subcategory);
        }

        //
        // POST: /Manage_Goods_Subcategory/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Goods_Subcategory goods_subcategory = db.Goods_Subcategory.Find(id);
            db.Goods_Subcategory.Remove(goods_subcategory);
            db.SaveChanges();
            return RedirectToAction("Index");
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
            ViewBag.categories = new HomeController().Category();
            return View();
        }

        //
        // POST: /Manage_Goods_Subcategory/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Goods_Subcategory goods_subcategory)
        {
            //Queue q = new Queue();
            if (goods_subcategory.goods_subcategory_name != null && goods_subcategory.goods_category_id != 0)
            {
                var result = from gs in db.Goods_Subcategory
                             join gc in db.Goods_Category on gs.goods_category_id equals gc.goods_category_id
                             where gs.goods_subcategory_name.StartsWith(goods_subcategory.goods_subcategory_name) && gs.goods_category_id == goods_subcategory.goods_category_id
                             select new Goods_Sub_CategoryModel { goods_subcategory_id = gs.goods_subcategory_id, goods_subcategory_name = gs.goods_subcategory_name, goods_category = gc.goods_category_name, status_id = gs.status_id };
                return View("Index", result.ToList());
            }
            else if (goods_subcategory.goods_subcategory_name == null && goods_subcategory.goods_category_id != 0)
            {
                var result = from gs in db.Goods_Subcategory
                             join gc in db.Goods_Category on gs.goods_category_id equals gc.goods_category_id
                             where gs.goods_category_id == goods_subcategory.goods_category_id
                             select new Goods_Sub_CategoryModel { goods_subcategory_id = gs.goods_subcategory_id, goods_subcategory_name = gs.goods_subcategory_name, goods_category = gc.goods_category_name, status_id = gs.status_id };
                return View("Index", result.ToList());
            }
            else if (goods_subcategory.goods_subcategory_name != null && goods_subcategory.goods_category_id == 0)
            {
                var result = from gs in db.Goods_Subcategory
                             join gc in db.Goods_Category on gs.goods_category_id equals gc.goods_category_id
                             where gs.goods_subcategory_name.StartsWith(goods_subcategory.goods_subcategory_name)
                             select new Goods_Sub_CategoryModel { goods_subcategory_id = gs.goods_subcategory_id, goods_subcategory_name = gs.goods_subcategory_name, goods_category = gc.goods_category_name, status_id = gs.status_id };
                return View("Index", result.ToList());
            }
            return RedirectToAction("Index");
        }
    }
}