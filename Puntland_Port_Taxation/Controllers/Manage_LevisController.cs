using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Collections;
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
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(16))
                {
                    var end_date = "9999-12-31";
                    int page;
                    int page_no = 1;
                    var count = 0;
                    if (levi_entry_id == 0)
                    {
                        count = db.Levi_Entry.Where(l => l.end_date == end_date).Count();
                    }
                    else
                    {
                        count = 1;
                    }
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
                    if (levi_entry_id == 0)
                    {
                        var levi_entry_display = (db.Display_Levi_Entries_View(null)).OrderBy( l => l.Levi_entry_id).Skip(start_from).Take(9).ToList();
                        return View(levi_entry_display);
                    }
                    else
                    {
                        var levi_entry_display = db.Display_Levi_Entries_View(levi_entry_id).ToList();
                        return View(levi_entry_display);
                    }
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
        // GET: /Manage_Levis/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(16))
                {
                    var levi_entry_display = db.Display_Levi_Entries_View(id).ToList();
                    return View(levi_entry_display);
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
        // GET: /Manage_Levis/Create

        public ActionResult Create()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(16))
                {
                    ViewBag.categories = new HomeController().Category();
                    ViewBag.levi_type = new HomeController().Levi_Type();
                    ViewBag.currency = new HomeController().Sos_Usd_Currency();
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
        // POST: /Manage_Levis/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Levi_EntryModel levi_entryModel)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                var is_perentage_no = 0;
                if (z.Contains(16))
                {
                    var file_name = "";
                    var end_date = "9999-12-31";
                    var date_end = Convert.ToDateTime("9999-12-31");
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
                            if (levi_entryModel.upload_document != null)
                            {
                                //following code is used for document adding and storing in App_Data folder
                                HttpPostedFileBase file = Request.Files.Get("upload_document");

                                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                                {
                                    file_name = file.FileName;
                                    string fileContentType = file.ContentType;
                                    byte[] fileBytes = new byte[file.ContentLength];
                                    file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                                    var FileLocation = Path.Combine(Server.MapPath("~/App_Data"), file_name);
                                    file.SaveAs(FileLocation);
                                }
                            }
                            levi_entry.levi_name = levi_entryModel.levi_name;
                            levi_entry.description = levi_entryModel.description;
                            levi_entry.levi_type_id = levi_entryModel.levi_type_id;
                            if (levi_entryModel.goods_category_id == 0)
                            {
                                levi_entry.goods_heirarchy_id = 0;
                                levi_entry.goods_item = null;
                                if (levi_entryModel.ispercentage == false)
                                {
                                    TempData["errorMessage"] = "You can't add levy at flat rate here, there are multiple unit of measures";
                                    return RedirectToAction("Index");
                                }
                            }
                            else if (levi_entryModel.goods_subcategory_id == 0)
                            {
                                is_perentage_no = (from gt in db.Goods_Tariff
                                                   join g in db.Goods on gt.goods_id equals g.goods_id
                                                   join gtp in db.Goods_Type on g.goods_type_id equals gtp.goods_type_id
                                                   join gs in db.Goods_Subcategory on gtp.goods_subcategory_id equals gs.goods_subcategory_id
                                                   join gc in db.Goods_Category on gs.goods_category_id equals gc.goods_category_id
                                                   where gc.goods_category_id == levi_entryModel.goods_category_id && gt.end_date == date_end
                                                   select gt.unit_of_measure_id).Distinct().Count();
                                if (is_perentage_no > 1 && levi_entryModel.ispercentage == false)
                                {
                                    TempData["errorMessage"] = "You can't add levy at flat rate here, there are multiple unit of measures";
                                    return RedirectToAction("Index");
                                }
                                levi_entry.goods_heirarchy_id = 1;
                                levi_entry.goods_item = levi_entryModel.goods_category_id;
                            }
                            else if (levi_entryModel.goods_type_id == 0)
                            {
                                is_perentage_no = (from gt in db.Goods_Tariff
                                                   join g in db.Goods on gt.goods_id equals g.goods_id
                                                   join gtp in db.Goods_Type on g.goods_type_id equals gtp.goods_type_id
                                                   join gs in db.Goods_Subcategory on gtp.goods_subcategory_id equals gs.goods_subcategory_id
                                                   where gs.goods_subcategory_id == levi_entryModel.goods_subcategory_id && gt.end_date == date_end
                                                   select gt.unit_of_measure_id).Distinct().Count();
                                if (is_perentage_no > 1 && levi_entryModel.ispercentage == false)
                                {
                                    TempData["errorMessage"] = "You can't add levy at flat rate here, there are multiple unit of measures";
                                    return RedirectToAction("Index");
                                }
                                levi_entry.goods_heirarchy_id = 2;
                                levi_entry.goods_item = levi_entryModel.goods_subcategory_id;
                            }
                            else if (levi_entryModel.goods_item == 0)
                            {                     
                                is_perentage_no = (from gt in db.Goods_Tariff
                                                   join g in db.Goods on gt.goods_id equals g.goods_id
                                                   join gtp in db.Goods_Type on g.goods_type_id equals gtp.goods_type_id
                                                   where gtp.goods_type_id == levi_entryModel.goods_type_id && gt.end_date == date_end
                                                   select gt.unit_of_measure_id).Distinct().Count();
                                if (is_perentage_no > 1 && levi_entryModel.ispercentage == false)
                                {
                                    TempData["errorMessage"] = "You can't add levy at flat rate here, there are multiple unit of measures";
                                    return RedirectToAction("Index");
                                }
                                levi_entry.goods_heirarchy_id = 3;
                                levi_entry.goods_item = levi_entryModel.goods_type_id;
                            }
                            else
                            {
                                levi_entry.goods_heirarchy_id = 4;
                                levi_entry.goods_item = levi_entryModel.goods_item;
                            }
                            levi_entry.ispercentage = levi_entryModel.ispercentage;
                            levi_entry.is_on_subtotal = Convert.ToInt32(levi_entryModel.is_on_subtotal);
                            levi_entry.levi = levi_entryModel.levi;
                            levi_entry.currency_id = levi_entryModel.currency_id;
                            levi_entry.status_id = 1;
                            levi_entry.start_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                            levi_entry.end_date = "9999-12-31";
                            levi_entry.isprocessed = false;
                            levi_entry.document_name = file_name;
                            db.Levi_Entry.Add(levi_entry);
                            db.SaveChanges();
                            db.Update_Levi();
                            TempData["errorMessage"] = "Levy Added Successfully";
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
        // GET: /Manage_Levis/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(16))
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
                    ViewBag.currency = new HomeController().Sos_Usd_Currency();
                    levi_entryModel.Levi_entry_id = levi_entry.Levi_entry_id;
                    levi_entryModel.levi_name = levi_entry.levi_name;
                    levi_entryModel.description = levi_entry.description;
                    levi_entryModel.levi_type_id = levi_entry.levi_type_id;
                    levi_entryModel.goods_heirarchy_id = levi_entry.goods_heirarchy_id;
                    levi_entryModel.goods_item = levi_entry.goods_item;
                    levi_entryModel.ispercentage = levi_entry.ispercentage;
                    levi_entryModel.is_on_subtotal = Convert.ToBoolean(levi_entry.is_on_subtotal);
                    levi_entryModel.levi = levi_entry.levi;
                    levi_entryModel.currency_id = levi_entry.currency_id;
                    levi_entryModel.status_id = levi_entry.status_id;
                    levi_entryModel.start_date = levi_entry.start_date;
                    levi_entryModel.end_date = levi_entry.end_date;
                    ViewBag.status = new HomeController().Status();
                    ViewBag.levi_type = new HomeController().Levi_Type();
                    ViewBag.categories = new HomeController().Category();
                    TempData["document_name"] = levi_entry.document_name;
                    TempData["levi_entry_id"] = levi_entry.Levi_entry_id;
                    TempData["levi_type_id"] = levi_entry.levi_type_id;
                    return View(levi_entryModel);
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
        // POST: /Manage_Levis/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Levi_EntryModel levi_entryModel)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(16))
                {
                    var file_name = "";
                    var levi_entry_id = Convert.ToInt32(TempData["levi_entry_id"]);
                    var end_date = "9999-12-31";
                    Levi_Entry levi_entry = db.Levi_Entry.Find(levi_entry_id);
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
                            if (levi_entryModel.upload_document != null)
                            {
                                //following code is used for document adding and storing in App_Data folder
                                HttpPostedFileBase file = Request.Files.Get("upload_document");

                                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                                {
                                    file_name = file.FileName;
                                    string fileContentType = file.ContentType;
                                    byte[] fileBytes = new byte[file.ContentLength];
                                    file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                                    var FileLocation = Path.Combine(Server.MapPath("~/App_Data"), file_name);
                                    file.SaveAs(FileLocation);
                                }
                                levi_entry.document_name = file_name;
                            }
                            else
                            {
                                levi_entry.document_name = TempData["document_name"].ToString();
                            }
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
                            levi_entry.is_on_subtotal = Convert.ToInt32(levi_entryModel.is_on_subtotal);
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
        // GET: /Manage_Levis/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(16))
                {
                    Levi_Entry levi_entry = db.Levi_Entry.Find(id);
                    if (levi_entry == null)
                    {
                        return HttpNotFound();
                    }
                    return View(levi_entry);
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
        // POST: /Manage_Levis/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(16))
                {
                    Levi_Entry levi_entry = db.Levi_Entry.Find(id);
                    db.Levi_Entry.Remove(levi_entry);
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
        // GET: /Manage_Levis/DbSearch

        public ActionResult DbSearch()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(16))
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
        // POST: /Manage_Levis/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Levi_Entry levi_entry)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(16))
                {
                    var end_date = "9999-12-31";
                    if (levi_entry.levi_name != null)
                    {
                        var result = from l in db.Levi_Entry
                                     where l.levi_name == levi_entry.levi_name && l.end_date == end_date
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