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
    public class Manage_LevisController : Controller
    {
        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

        //
        // GET: /Manage_Levis/

        public ActionResult Index(int levi_entry_id = 0)
        {
            if (levi_entry_id == 0)
            {
                var levi_entry_display = db.Display_Levi_Entries_View(null).ToList();
                return View(levi_entry_display);
            }
            else
            {
                var levi_entry_display = db.Display_Levi_Entries_View(levi_entry_id).ToList();
                return View(levi_entry_display);
            }           
        }

        //
        // GET: /Manage_Levis/Details/5

        public ActionResult Details(int id = 0)
        {
            var levi_entry_display = db.Display_Levi_Entries_View(id).ToList();
            return View(levi_entry_display);
        }

        //
        // GET: /Manage_Levis/Create

        public ActionResult Create()
        {
            ViewBag.categories = new HomeController().Category();
            ViewBag.levi_type = new HomeController().Levi_Type();
            return View();
        }

        //
        // POST: /Manage_Levis/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Levi_EntryModel levi_entryModel)
        {
            var end_date = "9999-12-31";
            Levi_Entry levi_entry = new Levi_Entry();
            if (ModelState.IsValid)
            {
                var is_exist = (from le in db.Levi_Entry where le.levi_name == levi_entryModel.levi_name && le.end_date == end_date select le).Count();
                if (is_exist > 0)
                {
                    TempData["errorMessage"] = "This Levy Name Already Exist";
                }
                else
                {
                    levi_entry.levi_name = levi_entryModel.levi_name;
                    levi_entry.description = levi_entryModel.description;
                    levi_entry.levi_type_id = levi_entryModel.levi_type_id;
                    if (levi_entryModel.goods_category_id == 0)
                    {
                        levi_entry.goods_heirarchy_id = 0;
                        levi_entry.goods_item = null;
                    }
                    else if (levi_entryModel.goods_subcategory_id == 0)
                    {
                        levi_entry.goods_heirarchy_id = 1;
                        levi_entry.goods_item = levi_entryModel.goods_category_id;
                    }
                    else if (levi_entryModel.goods_type_id == 0)
                    {
                        levi_entry.goods_heirarchy_id = 2;
                        levi_entry.goods_item = levi_entryModel.goods_subcategory_id;
                    }
                    else if (levi_entryModel.goods_item == 0)
                    {
                        levi_entry.goods_heirarchy_id = 3;
                        levi_entry.goods_item = levi_entryModel.goods_type_id;
                    }
                    else
                    {
                        levi_entry.goods_heirarchy_id = 4;
                        levi_entry.goods_item = levi_entryModel.goods_item;
                    }
                    levi_entry.ispercentage = levi_entryModel.ispercentage;
                    levi_entry.levi = levi_entryModel.levi;
                    levi_entry.status_id = 1;
                    levi_entry.start_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                    levi_entry.end_date = "9999-12-31";
                    levi_entry.isprocessed = false;
                    db.Levi_Entry.Add(levi_entry);
                    db.SaveChanges();
                    db.Update_Levi();
                    TempData["errorMessage"] = "Levy Added Successfully";
                }
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        //
        // GET: /Manage_Levis/Edit/5

        public ActionResult Edit(int id = 0)
        {
            var goods_category_id = 0;
            var goods_subcategory_id = 0;
            var goods_subcategory_name = "";
            var goods_type_id = 0;
            var goods_type_name = "";
            var goods_id = 0;
            var goods_name = "";            
            Levi_Entry levi_entry = db.Levi_Entry.Find(id);
            Levi_EntryModel levi_entryModel = new Levi_EntryModel();
            if (levi_entry.goods_heirarchy_id == 0)
            {
                goods_category_id = 0;
                goods_subcategory_id = 0;
                goods_subcategory_name = "All";
                goods_type_id = 0;
                goods_type_name = "All";
                goods_id = 0;
                goods_name = "All";
            }
            else if (levi_entry.goods_heirarchy_id == 1)
            {
                goods_category_id = Convert.ToInt32(levi_entry.goods_item);
                goods_subcategory_id = 0;
                goods_subcategory_name = "All";
                goods_type_id = 0;
                goods_type_name = "All";
                goods_id = 0;
                goods_name = "All";
            }
            else if (levi_entry.goods_heirarchy_id == 2)
            {
                var cateogry = from s in db.Goods_Subcategory
                               join c in db.Goods_Category on s.goods_category_id equals c.goods_category_id
                               where s.goods_subcategory_id == levi_entry.goods_item
                               select new { c.goods_category_id, s.goods_subcategory_name };
                goods_category_id = cateogry.First().goods_category_id;
                goods_subcategory_id = Convert.ToInt32(levi_entry.goods_item);
                goods_subcategory_name = cateogry.First().goods_subcategory_name;
                goods_type_id = 0;
                goods_type_name = "All";
                goods_id = 0;
                goods_name = "All";
            }
            else if (levi_entry.goods_heirarchy_id == 3)
            {
                var cateogry = from t in db.Goods_Type
                               join s in db.Goods_Subcategory on t.goods_subcategory_id equals s.goods_subcategory_id
                               join c in db.Goods_Category on s.goods_category_id equals c.goods_category_id
                               where t.goods_type_id == levi_entry.goods_item
                               select new { c.goods_category_id, s.goods_subcategory_id, s.goods_subcategory_name, t.goods_type_name };
                goods_category_id = cateogry.First().goods_category_id;
                goods_subcategory_id = cateogry.First().goods_subcategory_id;
                goods_subcategory_name = cateogry.First().goods_subcategory_name;
                goods_type_id = Convert.ToInt32(levi_entry.goods_item);
                goods_type_name = cateogry.First().goods_type_name;
                goods_id = 0;
                goods_name = "All";
            }
            else if (levi_entry.goods_heirarchy_id == 4)
            {
                var cateogry = from g in db.Goods
                               join t in db.Goods_Type on g.goods_type_id equals t.goods_type_id
                               join s in db.Goods_Subcategory on t.goods_subcategory_id equals s.goods_subcategory_id
                               join c in db.Goods_Category on s.goods_category_id equals c.goods_category_id
                               where g.goods_id == levi_entry.goods_item
                               select new { c.goods_category_id, s.goods_subcategory_id, t.goods_type_id, s.goods_subcategory_name, t.goods_type_name, g.goods_name };
                goods_category_id = cateogry.First().goods_category_id;
                goods_subcategory_id = cateogry.First().goods_subcategory_id;
                goods_subcategory_name = cateogry.First().goods_subcategory_name;
                goods_type_id = cateogry.First().goods_type_id;
                goods_type_name = cateogry.First().goods_type_name;
                goods_id = Convert.ToInt32(levi_entry.goods_item);
                goods_name = cateogry.First().goods_name;
            }
            levi_entryModel.goods_category_id = goods_category_id;
            levi_entryModel.goods_subcategory_id = goods_subcategory_id;
            levi_entryModel.goods_type_id = goods_type_id;
            levi_entryModel.goods_item = goods_id;
            IList<SelectListItem> subactegory = new List<SelectListItem>
            {
                new SelectListItem{Text = goods_subcategory_name, Value = goods_subcategory_id.ToString()},
            };
            IList<SelectListItem> type = new List<SelectListItem>
            {
                new SelectListItem{Text = goods_type_name, Value = goods_type_id.ToString()},
            };
            IList<SelectListItem> goods_item = new List<SelectListItem>
            {
                new SelectListItem{Text = goods_name, Value = goods_id.ToString()},
            };
            var subcategory_items = from s in db.Goods_Subcategory
                                    where s.goods_category_id == goods_category_id
                                    select new { s.goods_subcategory_id, s.goods_subcategory_name };
            var type_items = from t in db.Goods_Type
                             where t.goods_subcategory_id == goods_subcategory_id
                             select new { t.goods_type_id, t.goods_type_name };
            var goodses = from g in db.Goods
                          where g.goods_type_id == goods_type_id
                          select new { g.goods_id, g.goods_name };
            if (levi_entry.goods_heirarchy_id == 0)
            {
                ViewBag.subcategories = new SelectList(subactegory, "Value", "Text");
                ViewBag.goods_type = new SelectList(type, "Value", "Text");
                ViewBag.goods = new SelectList(goods_item, "Value", "Text");
            }
            if (levi_entry.goods_heirarchy_id == 1)
            {
                ViewBag.subcategories = new SelectList(subcategory_items, "goods_subcategory_id", "goods_subcategory_name");
                ViewBag.goods_type = new SelectList(type, "Value", "Text");
                ViewBag.goods = new SelectList(goods_item, "Value", "Text");
            }
            if (levi_entry.goods_heirarchy_id == 2)
            {
                ViewBag.subcategories = new SelectList(subcategory_items, "goods_subcategory_id", "goods_subcategory_name");
                ViewBag.goods_type = new SelectList(type_items, "goods_type_id", "goods_type_name");
                ViewBag.goods = new SelectList(goods_item, "Value", "Text");
            }
            if (levi_entry.goods_heirarchy_id == 3 || levi_entry.goods_heirarchy_id == 4)
            {
                ViewBag.subcategories = new SelectList(subcategory_items, "goods_subcategory_id", "goods_subcategory_name");
                ViewBag.goods_type = new SelectList(type_items, "goods_type_id", "goods_type_name");
                ViewBag.goods = new SelectList(goodses, "goods_id", "goods_name");
            }
            levi_entryModel.Levi_entry_id = levi_entry.Levi_entry_id;
            levi_entryModel.levi_name = levi_entry.levi_name;
            levi_entryModel.description = levi_entry.description;
            levi_entryModel.levi_type_id = levi_entry.levi_type_id;
            levi_entryModel.goods_heirarchy_id = levi_entry.goods_heirarchy_id;
            levi_entryModel.goods_item = levi_entry.goods_item;
            levi_entryModel.ispercentage = levi_entry.ispercentage;
            levi_entryModel.levi = levi_entry.levi;
            levi_entryModel.status_id = levi_entry.status_id;
            levi_entryModel.start_date = levi_entry.start_date;
            levi_entryModel.end_date = levi_entry.end_date;
            ViewBag.status = new HomeController().Status();
            ViewBag.levi_type = new HomeController().Levi_Type();
            ViewBag.categories = new HomeController().Category();
            TempData["levi_entry_id"] = levi_entry.Levi_entry_id;
            TempData["levi_type_id"] = levi_entry.levi_type_id;
            return View(levi_entryModel);
        }

        //
        // POST: /Manage_Levis/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Levi_EntryModel levi_entryModel)
        {
            var levi_entry_id = Convert.ToInt32(TempData["levi_entry_id"]);
            var end_date = "9999-12-31";
            Levi_Entry levi_entry = db.Levi_Entry.Find(levi_entry_id);
            //if ( Convert.ToInt32(TempData["levi_type_id"]) == 1)
            //{
            //    levi_entry.end_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
            //    db.Entry(levi_entry).State = EntityState.Modified;
            //    db.SaveChanges();
            //    levi_entry.start_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
            //    levi_entry.end_date = "9999-12-31";
            //    levi_entry.levi = levi_entryModel.levi;
            //    levi_entry.isprocessed = false;
            //    db.Levi_Entry.Add(levi_entry);
            //    db.SaveChanges();
            //    db.Update_Levi();
            //    return RedirectToAction("Index");
            //}
            levi_entry.end_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
            db.Entry(levi_entry).State = EntityState.Modified;
            db.SaveChanges();
            if (ModelState.IsValid)
            {
                var is_exist = (from le in db.Levi_Entry where le.levi_name == levi_entryModel.levi_name && le.Levi_entry_id != levi_entry_id && le.end_date == end_date select le).Count();
                if (is_exist > 0)
                {
                    TempData["errorMessage"] = "This Levy Name Already Exist";
                }
                else
                {
                    levi_entry.levi_name = levi_entryModel.levi_name;
                    levi_entry.description = levi_entryModel.description;
                    levi_entry.levi_type_id = levi_entryModel.levi_type_id;
                    if (levi_entryModel.goods_category_id == 0)
                    {
                        levi_entry.goods_heirarchy_id = 0;
                        levi_entry.goods_item = null;
                    }
                    else if (levi_entryModel.goods_subcategory_id == 0)
                    {
                        levi_entry.goods_heirarchy_id = 1;
                        levi_entry.goods_item = levi_entryModel.goods_category_id;
                    }
                    else if (levi_entryModel.goods_type_id == 0)
                    {
                        levi_entry.goods_heirarchy_id = 2;
                        levi_entry.goods_item = levi_entryModel.goods_subcategory_id;
                    }
                    else if (levi_entryModel.goods_item == 0)
                    {
                        levi_entry.goods_heirarchy_id = 3;
                        levi_entry.goods_item = levi_entryModel.goods_type_id;
                    }
                    else
                    {
                        levi_entry.goods_heirarchy_id = 4;
                        levi_entry.goods_item = levi_entryModel.goods_item;
                    }
                    levi_entry.ispercentage = levi_entryModel.ispercentage;
                    levi_entry.levi = levi_entryModel.levi;
                    levi_entry.status_id = levi_entryModel.status_id;
                    levi_entry.start_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                    levi_entry.end_date = "9999-12-31";
                    levi_entry.isprocessed = false;
                    db.Levi_Entry.Add(levi_entry);
                    db.SaveChanges();
                    db.Update_Levi();
                    TempData["errorMessage"] = "Edited Successfully";
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Manage_Levis/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Levi_Entry levi_entry = db.Levi_Entry.Find(id);
            if (levi_entry == null)
            {
                return HttpNotFound();
            }
            return View(levi_entry);
        }

        //
        // POST: /Manage_Levis/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Levi_Entry levi_entry = db.Levi_Entry.Find(id);
            db.Levi_Entry.Remove(levi_entry);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // GET: /Manage_Levis/DbSearch

        public ActionResult DbSearch()
        {
            return View();
        }

        //
        // POST: /Manage_Levis/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Levi_Entry levi_entry)
        {
            if (levi_entry.levi_name != null)
            {
                var result = from l in db.Levi_Entry
                             where l.levi_name == levi_entry.levi_name
                             select l.Levi_entry_id;
                if (result.Count() > 0)
                {
                    return RedirectToAction("Index", new { levi_entry_id = result.First() });
                }
                else
                {
                    return RedirectToAction("Index", new { levi_entry_id = -1 });
                }                
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}