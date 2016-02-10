using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using Puntland_Port_Taxation.Models;

namespace Puntland_Port_Taxation.Controllers
{
    public class Manage_GoodsController : Controller
    {
        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

        //
        // GET: /Manage_Goods/

        public ActionResult Index()
        {
            var end_date = Convert.ToDateTime("9999-12-31");
            var goods = from g in db.Goods
                        join gp in db.Goods_Type on g.goods_type_id equals gp.goods_type_id
                        join gs in db.Goods_Subcategory on gp.goods_subcategory_id equals gs.goods_subcategory_id
                        join gc in db.Goods_Category on gs.goods_category_id equals gc.goods_category_id
                        join gtf in db.Goods_Tariff on g.goods_id equals gtf.goods_id
                        join u in db.Unit_Of_Measure on gtf.unit_of_measure_id equals u.unit_id
                        where gtf.end_date == end_date
                        select new GoodsModel { goods_id = g.goods_id, goods_name = g.goods_name, goods_type = gp.goods_type_name, goods_sub_category = gs.goods_subcategory_name, goods_category = gc.goods_category_name, goods_tariff = gtf.goods_tariff, ispercentage = gtf.ispercentage, unit_of_measure = u.unit_code };
            return View(goods.ToList());
        }

        //
        // GET: /Manage_Goods/Details/5

        public ActionResult Details(int id = 0)
        {
            Good good = db.Goods.Find(id);
            if (good == null)
            {
                return HttpNotFound();
            }
            return View(good);
        }

        //
        // GET: /Manage_Goods/Create

        public ActionResult Create()
        {
            ViewBag.categories = new HomeController().Category();
            ViewBag.unif_of_measures = new HomeController().Unit_Of_Measures();
            return View();
        }

        //
        // POST: /Manage_Goods/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Goods_TypeModel good_typeModel)
        {
            Good good = new Good();
            Goods_Tariff goods_tariff = new Goods_Tariff();
            if (ModelState.IsValid)
            {
                var is_exist = (from g in db.Goods where g.goods_name == good_typeModel.goods_name select g).Count();
                if (is_exist > 0)
                {
                    TempData["errorMessage"] = "This Goods Already Exist";
                }
                else
                {
                    good.goods_type_id = good_typeModel.goods_type_id;
                    good.goods_name = good_typeModel.goods_name;
                    good.status_id = 1;
                    good.created_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                    good.updated_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                    db.Goods.Add(good);
                    db.SaveChanges();
                    goods_tariff.goods_id = good.goods_id;
                    goods_tariff.goods_tariff = good_typeModel.goods_tariff;
                    goods_tariff.ispercentage = good_typeModel.ispercentage;
                    goods_tariff.unit_of_measure_id = good_typeModel.unit_of_measure_id;
                    goods_tariff.created_date = DateTime.Now;
                    goods_tariff.end_date = Convert.ToDateTime("9999-12-31");
                    db.Goods_Tariff.Add(goods_tariff);
                    db.SaveChanges();
                    db.Update_Levi_Entry();
                    db.Update_Levi();
                    TempData["errorMessage"] = "Goods Added Successfully";
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Manage_Goods/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Good good = db.Goods.Find(id);
            Goods_TypeModel goods_typeModel = new Goods_TypeModel();
            if (good == null)
            {
                return HttpNotFound();
            }
            var goods = from g in db.Goods
                        join gt in db.Goods_Type on g.goods_type_id equals gt.goods_type_id
                        join gs in db.Goods_Subcategory on gt.goods_subcategory_id equals gs.goods_subcategory_id
                        join gc in db.Goods_Category on gs.goods_category_id equals gc.goods_category_id
                        where g.goods_id == good.goods_id
                        select new { gt.goods_type_id, gt.goods_type_name, gs.goods_subcategory_id, gs.goods_subcategory_name, gc.goods_category_id, gc.goods_category_name };
            TempData["type_id"] = good.goods_type_id;
            TempData["created_date"] = good.created_date;
            TempData["goods_id"] = good.goods_id;
            ViewBag.status = new HomeController().Status();
            ViewBag.categories = new SelectList(goods, "goods_category_id", "goods_category_name");
            ViewBag.subcategories = new SelectList(goods, "goods_subcategory_id", "goods_subcategory_name");
            ViewBag.goods_type = new SelectList(goods, "goods_type_id", "goods_type_name");
            ViewBag.unif_of_measures = new HomeController().Unit_Of_Measures();
            goods_typeModel.goods_category_id = goods.First().goods_category_id;
            goods_typeModel.goods_subcategory_id = goods.First().goods_subcategory_id;
            goods_typeModel.goods_type_id = good.goods_type_id;
            goods_typeModel.goods_name = good.goods_name;
            goods_typeModel.status_id = good.status_id;
            var end_date = Convert.ToDateTime("9999-12-31");
            var goods_tariff = (from gtf in db.Goods_Tariff
                               where gtf.goods_id == good.goods_id && gtf.end_date == end_date
                               select new { gtf.goods_tariff_id, gtf.goods_tariff, gtf.ispercentage, gtf.unit_of_measure_id }).First();
            goods_typeModel.goods_tariff = goods_tariff.goods_tariff;
            goods_typeModel.unit_of_measure_id = goods_tariff.unit_of_measure_id;
            goods_typeModel.ispercentage = goods_tariff.ispercentage;
            TempData["goods_tariff_id"] = goods_tariff.goods_tariff_id;
            return View(goods_typeModel);
        }

        //
        // POST: /Manage_Goods/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Goods_TypeModel good_typeModel)
        {
            int good_id = Convert.ToInt32(TempData["goods_id"]);
            Good good_new = db.Goods.Find(good_id);
            int goods_tariff_id = Convert.ToInt32(TempData["goods_tariff_id"]);
            Goods_Tariff goods_tariff = db.Goods_Tariff.Find(goods_tariff_id);
            Goods_Tariff goods_tariff_new = new Goods_Tariff();
            if (ModelState.IsValid)
            {
                var is_exist = (from g in db.Goods where g.goods_name == good_typeModel.goods_name && g.goods_id != good_id select g).Count();
                if (is_exist > 0)
                {
                    TempData["errorMessage"] = "This Goods Already Exist";
                }
                else
                {
                    good_new.goods_type_id = good_typeModel.goods_type_id;
                    good_new.goods_name = good_typeModel.goods_name;
                    good_new.status_id = good_typeModel.status_id;
                    good_new.created_date = TempData["created_date"].ToString();
                    good_new.updated_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                    db.Entry(good_new).State = EntityState.Modified;
                    db.SaveChanges();
                    goods_tariff.end_date = DateTime.Now;
                    db.Entry(goods_tariff).State = EntityState.Modified;
                    db.SaveChanges();
                    goods_tariff_new.goods_id = good_new.goods_id;
                    goods_tariff_new.goods_tariff = good_typeModel.goods_tariff;
                    goods_tariff_new.ispercentage = good_typeModel.ispercentage;
                    goods_tariff_new.unit_of_measure_id = good_typeModel.unit_of_measure_id;
                    goods_tariff_new.created_date = DateTime.Now;
                    goods_tariff_new.end_date = Convert.ToDateTime("9999-12-31");
                    db.Goods_Tariff.Add(goods_tariff_new);
                    db.SaveChanges();
                    TempData["errorMessage"] = "Edited Successfully";
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Manage_Goods/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Good good = db.Goods.Find(id);
            if (good == null)
            {
                return HttpNotFound();
            }
            return View(good);
        }

        //
        // POST: /Manage_Goods/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Good good = db.Goods.Find(id);
            db.Goods.Remove(good);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        //
        // GET: /Manage_Goods/DbSearch

        public ActionResult DbSearch()
        {
            ViewBag.goods_type = new HomeController().Goods_Type();
            ViewBag.subcategories = new HomeController().Subcategory();
            ViewBag.categories = new HomeController().Category();
            return View();
        }

        //
        // POST: /Manage_Goods/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Goods_TypeModel good)
        {
            var end_date = Convert.ToDateTime("9999-12-31");
            //Queue q = new Queue();
            if (good.goods_name != null && good.goods_type_id != 0 && good.goods_subcategory_id != 0 && good.goods_category_id != 0)
            {
                var result = from g in db.Goods
                             join gp in db.Goods_Type on g.goods_type_id equals gp.goods_type_id
                             join gs in db.Goods_Subcategory on gp.goods_subcategory_id equals gs.goods_subcategory_id
                             join gc in db.Goods_Category on gs.goods_category_id equals gc.goods_category_id
                             join gtf in db.Goods_Tariff on g.goods_id equals gtf.goods_id
                             join u in db.Unit_Of_Measure on gtf.unit_of_measure_id equals u.unit_id
                             where gtf.end_date == end_date && g.goods_name.StartsWith(good.goods_name) && g.goods_type_id == good.goods_type_id && gp.goods_subcategory_id == good.goods_subcategory_id && gs.goods_category_id == good.goods_category_id
                             select new GoodsModel { goods_id = g.goods_id, goods_name = g.goods_name, goods_type = gp.goods_type_name, goods_sub_category = gs.goods_subcategory_name, goods_category = gc.goods_category_name, goods_tariff = gtf.goods_tariff, ispercentage = gtf.ispercentage, unit_of_measure = u.unit_code };
                return View("Index", result.ToList());
            }
            else if (good.goods_name != null && good.goods_type_id != 0 && good.goods_subcategory_id != 0 && good.goods_category_id == 0)
            {
                var result = from g in db.Goods
                             join gp in db.Goods_Type on g.goods_type_id equals gp.goods_type_id
                             join gs in db.Goods_Subcategory on gp.goods_subcategory_id equals gs.goods_subcategory_id
                             join gc in db.Goods_Category on gs.goods_category_id equals gc.goods_category_id
                             join gtf in db.Goods_Tariff on g.goods_id equals gtf.goods_id
                             join u in db.Unit_Of_Measure on gtf.unit_of_measure_id equals u.unit_id
                             where gtf.end_date == end_date && g.goods_name.StartsWith(good.goods_name) && g.goods_type_id == good.goods_type_id && gp.goods_subcategory_id == good.goods_subcategory_id
                             select new GoodsModel { goods_id = g.goods_id, goods_name = g.goods_name, goods_type = gp.goods_type_name, goods_sub_category = gs.goods_subcategory_name, goods_category = gc.goods_category_name, goods_tariff = gtf.goods_tariff, ispercentage = gtf.ispercentage, unit_of_measure = u.unit_code };
                return View("Index", result.ToList());
            }
            else if (good.goods_name != null && good.goods_type_id != 0 && good.goods_subcategory_id == 0 && good.goods_category_id != 0)
            {
                var result = from g in db.Goods
                             join gp in db.Goods_Type on g.goods_type_id equals gp.goods_type_id
                             join gs in db.Goods_Subcategory on gp.goods_subcategory_id equals gs.goods_subcategory_id
                             join gc in db.Goods_Category on gs.goods_category_id equals gc.goods_category_id
                             join gtf in db.Goods_Tariff on g.goods_id equals gtf.goods_id
                             join u in db.Unit_Of_Measure on gtf.unit_of_measure_id equals u.unit_id
                             where gtf.end_date == end_date && g.goods_name.StartsWith(good.goods_name) && g.goods_type_id == good.goods_type_id && gs.goods_category_id == good.goods_category_id
                             select new GoodsModel { goods_id = g.goods_id, goods_name = g.goods_name, goods_type = gp.goods_type_name, goods_sub_category = gs.goods_subcategory_name, goods_category = gc.goods_category_name, goods_tariff = gtf.goods_tariff, ispercentage = gtf.ispercentage, unit_of_measure = u.unit_code };
                return View("Index", result.ToList());
            }
            if (good.goods_name != null && good.goods_type_id != 0 && good.goods_subcategory_id == 0 && good.goods_category_id == 0)
            {
                var result = from g in db.Goods
                             join gp in db.Goods_Type on g.goods_type_id equals gp.goods_type_id
                             join gs in db.Goods_Subcategory on gp.goods_subcategory_id equals gs.goods_subcategory_id
                             join gc in db.Goods_Category on gs.goods_category_id equals gc.goods_category_id
                             join gtf in db.Goods_Tariff on g.goods_id equals gtf.goods_id
                             join u in db.Unit_Of_Measure on gtf.unit_of_measure_id equals u.unit_id
                             where gtf.end_date == end_date && g.goods_name.StartsWith(good.goods_name) && g.goods_type_id == good.goods_type_id
                             select new GoodsModel { goods_id = g.goods_id, goods_name = g.goods_name, goods_type = gp.goods_type_name, goods_sub_category = gs.goods_subcategory_name, goods_category = gc.goods_category_name, goods_tariff = gtf.goods_tariff, ispercentage = gtf.ispercentage, unit_of_measure = u.unit_code };
                return View("Index", result.ToList());
            }
            if (good.goods_name != null && good.goods_type_id == 0 && good.goods_subcategory_id != 0 && good.goods_category_id != 0)
            {
                var result = from g in db.Goods
                             join gp in db.Goods_Type on g.goods_type_id equals gp.goods_type_id
                             join gs in db.Goods_Subcategory on gp.goods_subcategory_id equals gs.goods_subcategory_id
                             join gc in db.Goods_Category on gs.goods_category_id equals gc.goods_category_id
                             join gtf in db.Goods_Tariff on g.goods_id equals gtf.goods_id
                             join u in db.Unit_Of_Measure on gtf.unit_of_measure_id equals u.unit_id
                             where gtf.end_date == end_date && g.goods_name.StartsWith(good.goods_name) && gp.goods_subcategory_id == good.goods_subcategory_id && gs.goods_category_id == good.goods_category_id
                             select new GoodsModel { goods_id = g.goods_id, goods_name = g.goods_name, goods_type = gp.goods_type_name, goods_sub_category = gs.goods_subcategory_name, goods_category = gc.goods_category_name, goods_tariff = gtf.goods_tariff, ispercentage = gtf.ispercentage, unit_of_measure = u.unit_code };
                return View("Index", result.ToList());
            }
            if (good.goods_name != null && good.goods_type_id == 0 && good.goods_subcategory_id != 0 && good.goods_category_id == 0)
            {
                var result = from g in db.Goods
                             join gp in db.Goods_Type on g.goods_type_id equals gp.goods_type_id
                             join gs in db.Goods_Subcategory on gp.goods_subcategory_id equals gs.goods_subcategory_id
                             join gc in db.Goods_Category on gs.goods_category_id equals gc.goods_category_id
                             join gtf in db.Goods_Tariff on g.goods_id equals gtf.goods_id
                             join u in db.Unit_Of_Measure on gtf.unit_of_measure_id equals u.unit_id
                             where gtf.end_date == end_date && g.goods_name.StartsWith(good.goods_name) && gp.goods_subcategory_id == good.goods_subcategory_id
                             select new GoodsModel { goods_id = g.goods_id, goods_name = g.goods_name, goods_type = gp.goods_type_name, goods_sub_category = gs.goods_subcategory_name, goods_category = gc.goods_category_name, goods_tariff = gtf.goods_tariff, ispercentage = gtf.ispercentage, unit_of_measure = u.unit_code };
                return View("Index", result.ToList());
            }
            if (good.goods_name != null && good.goods_type_id == 0 && good.goods_subcategory_id == 0 && good.goods_category_id != 0)
            {
                var result = from g in db.Goods
                             join gp in db.Goods_Type on g.goods_type_id equals gp.goods_type_id
                             join gs in db.Goods_Subcategory on gp.goods_subcategory_id equals gs.goods_subcategory_id
                             join gc in db.Goods_Category on gs.goods_category_id equals gc.goods_category_id
                             join gtf in db.Goods_Tariff on g.goods_id equals gtf.goods_id
                             join u in db.Unit_Of_Measure on gtf.unit_of_measure_id equals u.unit_id
                             where gtf.end_date == end_date && g.goods_name.StartsWith(good.goods_name) && gs.goods_category_id == good.goods_category_id
                             select new GoodsModel { goods_id = g.goods_id, goods_name = g.goods_name, goods_type = gp.goods_type_name, goods_sub_category = gs.goods_subcategory_name, goods_category = gc.goods_category_name, goods_tariff = gtf.goods_tariff, ispercentage = gtf.ispercentage, unit_of_measure = u.unit_code };
                return View("Index", result.ToList());
            }
            if (good.goods_name != null && good.goods_type_id == 0 && good.goods_subcategory_id == 0 && good.goods_category_id == 0)
            {
                var result = from g in db.Goods
                             join gp in db.Goods_Type on g.goods_type_id equals gp.goods_type_id
                             join gs in db.Goods_Subcategory on gp.goods_subcategory_id equals gs.goods_subcategory_id
                             join gc in db.Goods_Category on gs.goods_category_id equals gc.goods_category_id
                             join gtf in db.Goods_Tariff on g.goods_id equals gtf.goods_id
                             join u in db.Unit_Of_Measure on gtf.unit_of_measure_id equals u.unit_id
                             where gtf.end_date == end_date && g.goods_name.StartsWith(good.goods_name)
                             select new GoodsModel { goods_id = g.goods_id, goods_name = g.goods_name, goods_type = gp.goods_type_name, goods_sub_category = gs.goods_subcategory_name, goods_category = gc.goods_category_name, goods_tariff = gtf.goods_tariff, ispercentage = gtf.ispercentage, unit_of_measure = u.unit_code };
                return View("Index", result.ToList());
            }
            if (good.goods_name == null && good.goods_type_id != 0 && good.goods_subcategory_id != 0 && good.goods_category_id != 0)
            {
                var result = from g in db.Goods
                             join gp in db.Goods_Type on g.goods_type_id equals gp.goods_type_id
                             join gs in db.Goods_Subcategory on gp.goods_subcategory_id equals gs.goods_subcategory_id
                             join gc in db.Goods_Category on gs.goods_category_id equals gc.goods_category_id
                             join gtf in db.Goods_Tariff on g.goods_id equals gtf.goods_id
                             join u in db.Unit_Of_Measure on gtf.unit_of_measure_id equals u.unit_id
                             where gtf.end_date == end_date && g.goods_type_id == good.goods_type_id && gp.goods_subcategory_id == good.goods_subcategory_id && gs.goods_category_id == good.goods_category_id
                             select new GoodsModel { goods_id = g.goods_id, goods_name = g.goods_name, goods_type = gp.goods_type_name, goods_sub_category = gs.goods_subcategory_name, goods_category = gc.goods_category_name, goods_tariff = gtf.goods_tariff, ispercentage = gtf.ispercentage, unit_of_measure = u.unit_code };
                return View("Index", result.ToList());
            }
            if (good.goods_name == null && good.goods_type_id != 0 && good.goods_subcategory_id != 0 && good.goods_category_id == 0)
            {
                var result = from g in db.Goods
                             join gp in db.Goods_Type on g.goods_type_id equals gp.goods_type_id
                             join gs in db.Goods_Subcategory on gp.goods_subcategory_id equals gs.goods_subcategory_id
                             join gc in db.Goods_Category on gs.goods_category_id equals gc.goods_category_id
                             join gtf in db.Goods_Tariff on g.goods_id equals gtf.goods_id
                             join u in db.Unit_Of_Measure on gtf.unit_of_measure_id equals u.unit_id
                             where gtf.end_date == end_date && g.goods_type_id == good.goods_type_id && gp.goods_subcategory_id == good.goods_subcategory_id
                             select new GoodsModel { goods_id = g.goods_id, goods_name = g.goods_name, goods_type = gp.goods_type_name, goods_sub_category = gs.goods_subcategory_name, goods_category = gc.goods_category_name, goods_tariff = gtf.goods_tariff, ispercentage = gtf.ispercentage, unit_of_measure = u.unit_code };
                return View("Index", result.ToList());
            }
            if (good.goods_name == null && good.goods_type_id != 0 && good.goods_subcategory_id == 0 && good.goods_category_id != 0)
            {
                var result = from g in db.Goods
                             join gp in db.Goods_Type on g.goods_type_id equals gp.goods_type_id
                             join gs in db.Goods_Subcategory on gp.goods_subcategory_id equals gs.goods_subcategory_id
                             join gc in db.Goods_Category on gs.goods_category_id equals gc.goods_category_id
                             join gtf in db.Goods_Tariff on g.goods_id equals gtf.goods_id
                             join u in db.Unit_Of_Measure on gtf.unit_of_measure_id equals u.unit_id
                             where gtf.end_date == end_date && g.goods_type_id == good.goods_type_id && gs.goods_category_id == good.goods_category_id
                             select new GoodsModel { goods_id = g.goods_id, goods_name = g.goods_name, goods_type = gp.goods_type_name, goods_sub_category = gs.goods_subcategory_name, goods_category = gc.goods_category_name, goods_tariff = gtf.goods_tariff, ispercentage = gtf.ispercentage, unit_of_measure = u.unit_code };
                return View("Index", result.ToList());
            }
            if (good.goods_name == null && good.goods_type_id != 0 && good.goods_subcategory_id == 0 && good.goods_category_id == 0)
            {
                var result = from g in db.Goods
                             join gp in db.Goods_Type on g.goods_type_id equals gp.goods_type_id
                             join gs in db.Goods_Subcategory on gp.goods_subcategory_id equals gs.goods_subcategory_id
                             join gc in db.Goods_Category on gs.goods_category_id equals gc.goods_category_id
                             join gtf in db.Goods_Tariff on g.goods_id equals gtf.goods_id
                             join u in db.Unit_Of_Measure on gtf.unit_of_measure_id equals u.unit_id
                             where gtf.end_date == end_date && g.goods_type_id == good.goods_type_id
                             select new GoodsModel { goods_id = g.goods_id, goods_name = g.goods_name, goods_type = gp.goods_type_name, goods_sub_category = gs.goods_subcategory_name, goods_category = gc.goods_category_name, goods_tariff = gtf.goods_tariff, ispercentage = gtf.ispercentage, unit_of_measure = u.unit_code };
                return View("Index", result.ToList());
            }
            if (good.goods_name == null && good.goods_type_id == 0 && good.goods_subcategory_id != 0 && good.goods_category_id != 0)
            {
                var result = from g in db.Goods
                             join gp in db.Goods_Type on g.goods_type_id equals gp.goods_type_id
                             join gs in db.Goods_Subcategory on gp.goods_subcategory_id equals gs.goods_subcategory_id
                             join gc in db.Goods_Category on gs.goods_category_id equals gc.goods_category_id
                             join gtf in db.Goods_Tariff on g.goods_id equals gtf.goods_id
                             join u in db.Unit_Of_Measure on gtf.unit_of_measure_id equals u.unit_id
                             where gtf.end_date == end_date && gp.goods_subcategory_id == good.goods_subcategory_id && gs.goods_category_id == good.goods_category_id
                             select new GoodsModel { goods_id = g.goods_id, goods_name = g.goods_name, goods_type = gp.goods_type_name, goods_sub_category = gs.goods_subcategory_name, goods_category = gc.goods_category_name, goods_tariff = gtf.goods_tariff, ispercentage = gtf.ispercentage, unit_of_measure = u.unit_code };
                return View("Index", result.ToList());
            }
            if (good.goods_name == null && good.goods_type_id == 0 && good.goods_subcategory_id != 0 && good.goods_category_id == 0)
            {
                var result = from g in db.Goods
                             join gp in db.Goods_Type on g.goods_type_id equals gp.goods_type_id
                             join gs in db.Goods_Subcategory on gp.goods_subcategory_id equals gs.goods_subcategory_id
                             join gc in db.Goods_Category on gs.goods_category_id equals gc.goods_category_id
                             join gtf in db.Goods_Tariff on g.goods_id equals gtf.goods_id
                             join u in db.Unit_Of_Measure on gtf.unit_of_measure_id equals u.unit_id
                             where gtf.end_date == end_date && gp.goods_subcategory_id == good.goods_subcategory_id
                             select new GoodsModel { goods_id = g.goods_id, goods_name = g.goods_name, goods_type = gp.goods_type_name, goods_sub_category = gs.goods_subcategory_name, goods_category = gc.goods_category_name, goods_tariff = gtf.goods_tariff, ispercentage = gtf.ispercentage, unit_of_measure = u.unit_code };
                return View("Index", result.ToList());
            }
            if (good.goods_name == null && good.goods_type_id == 0 && good.goods_subcategory_id == 0 && good.goods_category_id != 0)
            {
                var result = from g in db.Goods
                             join gp in db.Goods_Type on g.goods_type_id equals gp.goods_type_id
                             join gs in db.Goods_Subcategory on gp.goods_subcategory_id equals gs.goods_subcategory_id
                             join gc in db.Goods_Category on gs.goods_category_id equals gc.goods_category_id
                             join gtf in db.Goods_Tariff on g.goods_id equals gtf.goods_id
                             join u in db.Unit_Of_Measure on gtf.unit_of_measure_id equals u.unit_id
                             where gtf.end_date == end_date && gs.goods_category_id == good.goods_category_id
                             select new GoodsModel { goods_id = g.goods_id, goods_name = g.goods_name, goods_type = gp.goods_type_name, goods_sub_category = gs.goods_subcategory_name, goods_category = gc.goods_category_name, goods_tariff = gtf.goods_tariff, ispercentage = gtf.ispercentage, unit_of_measure = u.unit_code };
                return View("Index", result.ToList());
            }
            //if (good.goods_name != null)
            //{
            //    q.Enqueue("g.goods_name.StartsWith(" + good.goods_name + ")");
            //}
            //if (good.goods_type_id != 0)
            //{
            //    q.Enqueue("g.goods_type_id == " + good.goods_type_id);
            //}
            //var str = String.Join("&&", q);
            ////var query = "from g in db.Goods where " + str + " select g";
            //var result = db.Goods.Where(str);
            return RedirectToAction("Index");
        }
    }
}