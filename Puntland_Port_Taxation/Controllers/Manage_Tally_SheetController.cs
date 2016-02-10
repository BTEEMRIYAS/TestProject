using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Puntland_Port_Taxation.Models;
using System.IO;
using FlexCel.XlsAdapter;

namespace Puntland_Port_Taxation.Controllers
{
    public class Manage_Tally_SheetController : Controller
    {
        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

        //
        // GET: /Manage_Tally_Sheet/

        public ActionResult Index()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(12))
                {
                    int page;
                    int page_no = 1;
                    var count = db.Tally_Sheet.Count();
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
                    var tally_sheet = ((from t in db.Tally_Sheet
                                      join sa in db.Ship_Arrival on t.ship_arrival_id equals sa.ship_arrival_id
                                      select new Tally_SheetModel { tally_sheet_code = t.tally_sheet_code, ship_arrival_code = sa.ship_arrival_code, tally_sheet_id = t.tally_sheet_id }).Distinct()).OrderByDescending(ts => ts.tally_sheet_id).Skip(start_from).Take(9);
                    return View(tally_sheet.ToList());
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
        // GET: /Manage_Tally_Sheet/Details/T0001

        public ActionResult Details(int tally_sheet_id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(12))
                {
                    int page;
                    int page_no = 1;
                    var count = db.Tally_Sheet_Details.Where(t => t.tally_sheet_id == tally_sheet_id).Count();
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
                    var tally_sheet_code = (from t in db.Tally_Sheet where t.tally_sheet_id == tally_sheet_id select t.tally_sheet_code).First();
                    var tally_sheet = (from t in db.Tally_Sheet
                                      join td in db.Tally_Sheet_Details on t.tally_sheet_id equals td.tally_sheet_id
                                       where t.tally_sheet_id == tally_sheet_id
                                       select new Tally_SheetModel { tally_sheet_id = t.tally_sheet_id, way_bill_code = td.way_bill_code, goods = td.goods_name, unit_of_measure = td.unit_of_measure, total_quantity = td.total_quantity, tally_sheet_code = t.tally_sheet_code, tally_sheet_details_id = td.tally_sheet_details_id, importer_name = td.importer_name, is_damaged = td.is_damaged }).OrderBy( t => t.tally_sheet_details_id).Skip(start_from).Take(9);
                    Session["tally_sheet_id"] = tally_sheet_id;
                    Session["tally_sheet_code"] = tally_sheet_code;
                    return View(tally_sheet.ToList());
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
        // GET: /Manage_Tally_Sheet/View_Way_Bill/T0001

        public ActionResult View_Way_Bill(int id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(12))
                {
                    var tally_sheet_code = (from t in db.Tally_Sheet where t.tally_sheet_id == id select t.tally_sheet_code).First();
                    Session["tally_sheet_code"] = tally_sheet_code;
                    var tally_sheet = from t in db.Tally_Sheet
                                      join td in db.Tally_Sheet_Details on t.tally_sheet_id equals td.tally_sheet_id
                                      where td.tally_sheet_id == id
                                      select new Tally_SheetModel { way_bill_code = td.way_bill_code, goods = td.goods_name, unit_of_measure = td.unit_of_measure, total_quantity = td.total_quantity, is_damaged = td.is_damaged };
                    return View(tally_sheet.ToList());
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
        // GET: /Manage_Tally_Sheet/Create

        public ActionResult Create()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(12))
                {
                    ViewBag.ship_arrivals = new HomeController().Ship_Arrival();
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
        // POST: /Manage_Tally_Sheet/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tally_Sheet tally_sheet)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(12))
                {
                    if (ModelState.IsValid)
                    {
                        var is_cargomanifest_code_exist = from t in db.Tally_Sheet where t.tally_sheet_code == tally_sheet.tally_sheet_code select t;
                        var is_ship_arrival_exist = from t in db.Tally_Sheet where t.ship_arrival_id == tally_sheet.ship_arrival_id select t;
                        if (is_cargomanifest_code_exist.Count() > 0)
                        {
                            TempData["errorMessage"] = "Cargo Manifest Code Already Exists, Please Enter Another One";
                            return RedirectToAction("Index");
                        }
                        else if (is_ship_arrival_exist.Count() > 0)
                        {
                            TempData["errorMessage"] = "Only One Cargo Manifest Allowed For a Shipp Arrival, Please Choose Another One";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            tally_sheet.employee_id = Convert.ToInt32(Session["id"]);
                            tally_sheet.tally_sheet_code = "CM_";
                            db.Tally_Sheet.Add(tally_sheet);                   
                            db.SaveChanges();
                            if (tally_sheet.tally_sheet_id < 10)
                            {
                                tally_sheet.tally_sheet_code = tally_sheet.tally_sheet_code + tally_sheet.tally_sheet_id.ToString("00");
                            }
                            else
                            {
                                tally_sheet.tally_sheet_code = tally_sheet.tally_sheet_code + tally_sheet.tally_sheet_id;
                            }
                            db.Entry(tally_sheet).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["errorMessage"] = "Cargo Manifest Added Successfully";
                            return RedirectToAction("Index");
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
        // GET: /Manage_Tally_Sheet/Create_Tally_Sheet

        public ActionResult Create_Tally_Sheet()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(12))
                {
                    ViewBag.categories = new HomeController().Category();
                    ViewBag.importer = new HomeController().Importer_Name();
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
        // POST: /Manage_Tally_Sheet/Create_Tally_Sheet

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create_Tally_Sheet(Tally_SheetModel tally_sheetModel)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(12))
                {
                    Tally_Sheet_Details tally_sheet_details = new Tally_Sheet_Details();
                    var tally_sheet_id = Convert.ToInt32(Session["tally_sheet_id"]);
                    if (ModelState.IsValid)
                    {
                        tally_sheet_details.tally_sheet_id = tally_sheet_id;
                        tally_sheet_details.way_bill_code = tally_sheetModel.way_bill_code;
                        tally_sheet_details.importer_name = tally_sheetModel.importer_name;
                        tally_sheet_details.goods_name = tally_sheetModel.goods;
                        tally_sheet_details.unit_of_measure = tally_sheetModel.unit_of_measure;
                        tally_sheet_details.total_quantity = tally_sheetModel.total_quantity;
                        tally_sheet_details.is_damaged = tally_sheetModel.is_damaged;
                        db.Tally_Sheet_Details.Add(tally_sheet_details);
                        db.SaveChanges();
                        return RedirectToAction("Details", new { tally_sheet_id = tally_sheet_id });
                    }
                    return RedirectToAction("Details", new { tally_sheet_id = tally_sheet_id });
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
        // GET: /Manage_Tally_Sheet/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(12))
                {
                    Tally_Sheet_Details tally_sheet_details = db.Tally_Sheet_Details.Find(id);
                    Tally_SheetModel tally_sheetModel = new Tally_SheetModel();
                    if (tally_sheet_details == null)
                    {
                        return HttpNotFound();
                    }
                    var ship_arrival_code = from t in db.Tally_Sheet
                                            join td in db.Tally_Sheet_Details on t.tally_sheet_id equals td.tally_sheet_id
                                            join sa in db.Ship_Arrival on t.ship_arrival_id equals sa.ship_arrival_id
                                            where td.tally_sheet_details_id == tally_sheet_details.tally_sheet_details_id
                                            select new { sa.ship_arrival_id, sa.ship_arrival_code, td.importer_name };
                    ViewBag.ship_arrivals = new SelectList(ship_arrival_code, "ship_arrival_id", "ship_arrival_code");
                    ViewBag.importer = new SelectList(ship_arrival_code, "importer_name", "importer_name");
                    TempData["id"] = tally_sheet_details.tally_sheet_details_id;
                    TempData["tally_sheet_id"] = tally_sheet_details.tally_sheet_id;
                    var goods = from g in db.Goods
                                join gt in db.Goods_Type on g.goods_type_id equals gt.goods_type_id
                                join gs in db.Goods_Subcategory on gt.goods_subcategory_id equals gs.goods_subcategory_id
                                join gc in db.Goods_Category on gs.goods_category_id equals gc.goods_category_id
                                where g.goods_name == tally_sheet_details.goods_name
                                select new { g.goods_id, g.goods_name, gt.goods_type_id, gt.goods_type_name, gs.goods_subcategory_id, gs.goods_subcategory_name, gc.goods_category_id };
                    var unit_id = from u in db.Unit_Of_Measure
                                  where u.unit_code == tally_sheet_details.unit_of_measure
                                  select u;
                    ViewBag.unit_of_measure = new SelectList(unit_id, "unit_code", "unit_code");
                    ViewBag.subcategories = new SelectList(goods, "goods_subcategory_id", "goods_subcategory_name");
                    ViewBag.goods_type = new SelectList(goods, "goods_type_id", "goods_type_name");
                    ViewBag.goods = new SelectList(goods, "goods_name", "goods_name");
                    ViewBag.categories = new HomeController().Category();                   
                    var tally_sheet_code = from t in db.Tally_Sheet where t.tally_sheet_id == tally_sheet_details.tally_sheet_id select t.tally_sheet_code;  
                    tally_sheetModel.tally_sheet_code = tally_sheet_code.FirstOrDefault();
                    tally_sheetModel.ship_arrival_id = ship_arrival_code.FirstOrDefault().ship_arrival_id;
                    tally_sheetModel.way_bill_code = tally_sheet_details.way_bill_code;
                    tally_sheetModel.importer_name = tally_sheet_details.importer_name;
                    tally_sheetModel.goods_category_id = goods.First().goods_category_id;
                    tally_sheetModel.goods_subcategory_id = goods.First().goods_subcategory_id;
                    tally_sheetModel.goods_type_id = goods.First().goods_type_id;
                    tally_sheetModel.goods = tally_sheet_details.goods_name;
                    tally_sheetModel.unit_of_measure = tally_sheet_details.unit_of_measure;
                    tally_sheetModel.total_quantity = Convert.ToInt32(tally_sheet_details.total_quantity);
                    tally_sheetModel.is_damaged = tally_sheet_details.is_damaged;
                    return View(tally_sheetModel);
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
        // POST: /Manage_Tally_Sheet/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tally_SheetModel tally_sheetModel)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(12))
                {
                    int id = Convert.ToInt32(TempData["id"]);
                    Tally_Sheet_Details tally_sheet_details = new Tally_Sheet_Details();
                    if (ModelState.IsValid)
                    {
                        tally_sheet_details.tally_sheet_details_id = id;
                        tally_sheet_details.tally_sheet_id = Convert.ToInt32(TempData["tally_sheet_id"]);
                        tally_sheet_details.way_bill_code = tally_sheetModel.way_bill_code;
                        tally_sheet_details.importer_name = tally_sheetModel.importer_name;
                        tally_sheet_details.goods_name = tally_sheetModel.goods;
                        tally_sheet_details.unit_of_measure = tally_sheetModel.unit_of_measure;
                        tally_sheet_details.total_quantity = tally_sheetModel.total_quantity;
                        tally_sheet_details.is_damaged = tally_sheetModel.is_damaged;
                        db.Entry(tally_sheet_details).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Details", new { tally_sheet_id = tally_sheet_details.tally_sheet_id });
                    }
                    return RedirectToAction("Details", new { tally_sheet_id = tally_sheet_details.tally_sheet_id });
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

        [HttpPost]
        public ActionResult SaveFile(Tally_Sheet tally_sheet)
        {        
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(12))
                {
                    var tally_sheet_id_hd = Request.Form["tally_sheet_id_hd"].ToString();
                    var tally_sheet_id = Convert.ToInt32(Request.Form["tally_sheet_id_hd"]);
                    db.Update_Tally_Sheet_Details(tally_sheet_id);
                        //int updated_file_count = 0;
                        HttpPostedFileBase file = Request.Files.Get("exchangefile");
                        var files = Request.Files["exchangefile"];
                        if (files != null)
                        {
                            FileInfo fil = new FileInfo(files.FileName);
                            var filename = DateTime.Now.Ticks + fil.Extension;
                            string path = Server.MapPath("~/App_Data/" + filename);
                            files.SaveAs(path);
                            SaveExcel(path, tally_sheet_id_hd);
                            System.IO.File.Delete(path);
                            db.SaveChanges();                            
                        }
                        TempData["errorMessage"] = " Data Updated Successfully";
                        return RedirectToAction("Index", "Manage_Tally_Sheet");
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

        public void SaveExcel(string path, string hd_tally_sheet_id)
        {
            XlsFile myFile = new XlsFile(path);
            myFile.ActiveSheet = 1;

            int rowCount = myFile.RowCount;
            int colCount = myFile.ColCount;

            for (int i = 2; i <= rowCount; i++)
            {
                if (myFile.GetCellValue(i, 1) == null || myFile.GetCellValue(i, 2) == null || myFile.GetCellValue(i, 3) == null || myFile.GetCellValue(i, 4) == null)
                {
                    continue;
                }
                Puntland_Port_Taxation.Tally_Sheet_Details tsheet = new Tally_Sheet_Details();
                tsheet.tally_sheet_id = int.Parse(hd_tally_sheet_id);
                tsheet.way_bill_code = myFile.GetCellValue(i, 1).ToString().Trim();
                tsheet.importer_name = myFile.GetCellValue(i, 2).ToString().Trim();
                tsheet.goods_name = myFile.GetCellValue(i, 3).ToString();
                tsheet.total_quantity = Convert.ToInt32((myFile.GetCellValue(i, 4).ToString()));
                tsheet.unit_of_measure = (myFile.GetCellValue(i, 5).ToString());
                var damaged = (myFile.GetCellValue(i, 6).ToString());
                damaged = damaged.ToLower();
                if (damaged == "yes")
                {
                    tsheet.is_damaged = true;
                }
                else
                {
                    tsheet.is_damaged = false;
                }
                db.Tally_Sheet_Details.Add(tsheet);
                db.SaveChanges();
            }
        }

        //
        // GET: /Manage_Tally_Sheet/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(12))
                {
                    Tally_Sheet_Details tally_sheet_details = db.Tally_Sheet_Details.Find(id);
                    if (tally_sheet_details == null)
                    {
                        return HttpNotFound();
                    }
                    var goods = (from g in db.Goods where g.goods_name == tally_sheet_details.goods_name select g.goods_name).FirstOrDefault();
                    ViewBag.goods = goods;
                    return View(tally_sheet_details);
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
        // POST: /Manage_Tally_Sheet/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(12))
                {
                    var tally_sheet_id = Convert.ToInt32(Session["tally_sheet_id"]);
                    Tally_Sheet_Details tally_sheet_details = db.Tally_Sheet_Details.Find(id);
                    db.Tally_Sheet_Details.Remove(tally_sheet_details);
                    db.SaveChanges();
                    return RedirectToAction("Details", new { tally_sheet_id = tally_sheet_id });
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
        // GET: /Manage_Tally_Sheet/DbSearch

        public ActionResult DbSearch()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(12))
                {
                    ViewBag.ship_arrivals = new HomeController().Ship_Arrival();
                    ViewBag.way_bills = new HomeController().Way_Bill();
                    ViewBag.goods = new HomeController().Goods();
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
        // POST: /Manage_Tally_Sheet/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Tally_SheetModel tally_sheet)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(12))
                {
                    //Queue q = new Queue();
                    if (tally_sheet.ship_arrival_code != null && tally_sheet.tally_sheet_code != null)
                    {
                        var result = (from t in db.Tally_Sheet
                                      join sa in db.Ship_Arrival on t.ship_arrival_id equals sa.ship_arrival_id
                                      where sa.ship_arrival_code.Contains(tally_sheet.ship_arrival_code) && t.tally_sheet_code.Contains(tally_sheet.tally_sheet_code)
                                      select new Tally_SheetModel { tally_sheet_code = t.tally_sheet_code, ship_arrival_code = sa.ship_arrival_code, tally_sheet_id = t.tally_sheet_id }).Distinct(); 
                        return View("Index", result.ToList());
                    }
                    else if (tally_sheet.ship_arrival_code != null && tally_sheet.tally_sheet_code == null)
                    {
                        var result = (from t in db.Tally_Sheet
                                      join sa in db.Ship_Arrival on t.ship_arrival_id equals sa.ship_arrival_id
                                      where sa.ship_arrival_code.Contains(tally_sheet.ship_arrival_code)
                                      select new Tally_SheetModel { tally_sheet_code = t.tally_sheet_code, ship_arrival_code = sa.ship_arrival_code, tally_sheet_id = t.tally_sheet_id }).Distinct(); 
                        return View("Index", result.ToList());
                    }
                    else if (tally_sheet.ship_arrival_code == null && tally_sheet.tally_sheet_code != null)
                    {
                        var result = (from t in db.Tally_Sheet
                                      join sa in db.Ship_Arrival on t.ship_arrival_id equals sa.ship_arrival_id
                                      where t.tally_sheet_code.Contains(tally_sheet.tally_sheet_code)
                                      select new Tally_SheetModel { tally_sheet_code = t.tally_sheet_code, ship_arrival_code = sa.ship_arrival_code, tally_sheet_id = t.tally_sheet_id }).Distinct(); 
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

        //
        // GET: /Manage_Tally_Sheet/DbSearch_new

        public ActionResult DbSearch_new()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(12))
                {
                    ViewBag.goods = new HomeController().Goods();
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

        ////
        //// POST: /Manage_Tally_Sheet/DbSearchresult_new
        ////In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult_new(Tally_Sheet_Details tally_sheet)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(12))
                {
                    var tally_sheet_id = Convert.ToInt32(Session["tally_sheet_id"]);
                    if (tally_sheet.goods_name != null && tally_sheet.way_bill_code != null)
                    {
                        var result = from t in db.Tally_Sheet
                                     join td in db.Tally_Sheet_Details on t.tally_sheet_id equals td.tally_sheet_id
                                     where td.goods_name == tally_sheet.goods_name && td.way_bill_code == tally_sheet.way_bill_code && td.tally_sheet_id == tally_sheet_id
                                     select new Tally_SheetModel { tally_sheet_id = t.tally_sheet_id, way_bill_code = td.way_bill_code, goods = td.goods_name, unit_of_measure = td.unit_of_measure, total_quantity = td.total_quantity, tally_sheet_code = t.tally_sheet_code, tally_sheet_details_id = td.tally_sheet_details_id, importer_name = td.importer_name, is_damaged = td.is_damaged };
                        return View("Details", result.ToList());
                    }
                    else if (tally_sheet.goods_name != null && tally_sheet.way_bill_code == null)
                    {
                        var result = from t in db.Tally_Sheet
                                     join td in db.Tally_Sheet_Details on t.tally_sheet_id equals td.tally_sheet_id
                                     where td.goods_name == tally_sheet.goods_name && td.tally_sheet_id == tally_sheet_id
                                     select new Tally_SheetModel { tally_sheet_id = t.tally_sheet_id, way_bill_code = td.way_bill_code, goods = td.goods_name, unit_of_measure = td.unit_of_measure, total_quantity = td.total_quantity, tally_sheet_code = t.tally_sheet_code, tally_sheet_details_id = td.tally_sheet_details_id, importer_name = td.importer_name, is_damaged = td.is_damaged };
                        return View("Details", result.ToList());
                    }
                    else if (tally_sheet.goods_name == null && tally_sheet.way_bill_code != null)
                    {
                        var result = from t in db.Tally_Sheet
                                     join td in db.Tally_Sheet_Details on t.tally_sheet_id equals td.tally_sheet_id
                                     where td.way_bill_code == tally_sheet.way_bill_code && td.tally_sheet_id == tally_sheet_id
                                     select new Tally_SheetModel { tally_sheet_id = t.tally_sheet_id, way_bill_code = td.way_bill_code, goods = td.goods_name, unit_of_measure = td.unit_of_measure, total_quantity = td.total_quantity, tally_sheet_code = t.tally_sheet_code, tally_sheet_details_id = td.tally_sheet_details_id, importer_name = td.importer_name, is_damaged = td.is_damaged };
                        return View("Details", result.ToList());
                    }
                    return RedirectToAction("Details", new { tally_sheet_id = tally_sheet_id });
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