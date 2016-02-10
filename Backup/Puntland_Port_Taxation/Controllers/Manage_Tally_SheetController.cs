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
            var tally_sheet = (from t in db.Tally_Sheet
                              join sa in db.Ship_Arrival on t.ship_arrival_id equals sa.ship_arrival_id
                              select new Tally_SheetModel { tally_sheet_code = t.tally_sheet_code, ship_arrival_code = sa.ship_arrival_code, tally_sheet_id = t.tally_sheet_id }).Distinct();
            return View(tally_sheet.ToList());
        }

        //
        // GET: /Manage_Tally_Sheet/Details/T0001

        public ActionResult Details(int tally_sheet_id)
        {
            var tally_sheet_code = (from t in db.Tally_Sheet where t.tally_sheet_id == tally_sheet_id select t.tally_sheet_code).First();
            var tally_sheet = from t in db.Tally_Sheet
                              join td in db.Tally_Sheet_Details on t.tally_sheet_id equals td.tally_sheet_id
                               where t.tally_sheet_id == tally_sheet_id
                               select new Tally_SheetModel { tally_sheet_id = t.tally_sheet_id, way_bill_code = td.way_bill_code, goods = td.goods_name, units = td.units, quantity = td.quantity, unit_of_measure = td.unit_of_measure, total_quantity = td.total_quantity, tally_sheet_code = t.tally_sheet_code, tally_sheet_details_id = td.tally_sheet_details_id };
            Session["tally_sheet_id"] = tally_sheet_id;
            Session["tally_sheet_code"] = tally_sheet_code;
            return View(tally_sheet.ToList());
        }

        //
        // GET: /Manage_Tally_Sheet/View_Way_Bill/T0001

        public ActionResult View_Way_Bill(int id)
        {
            var tally_sheet_code = (from t in db.Tally_Sheet where t.tally_sheet_id == id select t.tally_sheet_code).First();
            Session["tally_sheet_code"] = tally_sheet_code;
            var tally_sheet = from t in db.Tally_Sheet
                              join td in db.Tally_Sheet_Details on t.tally_sheet_id equals td.tally_sheet_id
                              where td.tally_sheet_id == id
                              select new Tally_SheetModel { way_bill_code = td.way_bill_code, goods = td.goods_name, units = td.units, quantity = td.quantity, unit_of_measure = td.unit_of_measure, total_quantity = td.total_quantity };
            return View(tally_sheet.ToList());
        }

        //
        // GET: /Manage_Tally_Sheet/Create

        public ActionResult Create()
        {
            ViewBag.ship_arrivals = new HomeController().Ship_Arrival();
            return View();
        }

        //
        // POST: /Manage_Tally_Sheet/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tally_Sheet tally_sheet)
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
                    db.Tally_Sheet.Add(tally_sheet);                   
                    db.SaveChanges();
                    TempData["errorMessage"] = "Cargo Manifest Added Successfully";
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Manage_Tally_Sheet/Create_Tally_Sheet

        public ActionResult Create_Tally_Sheet()
        {
            ViewBag.categories = new HomeController().Category();
            ViewBag.currency = new HomeController().Currency();
            return View();
        }

        //
        // POST: /Manage_Tally_Sheet/Create_Tally_Sheet

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create_Tally_Sheet(Tally_SheetModel tally_sheetModel)
        {
            Tally_Sheet_Details tally_sheet_details = new Tally_Sheet_Details();
            var tally_sheet_id = Convert.ToInt32(Session["tally_sheet_id"]);
            if (ModelState.IsValid)
            {
                tally_sheet_details.tally_sheet_id = tally_sheet_id;
                tally_sheet_details.way_bill_code = tally_sheetModel.way_bill_code;
                tally_sheet_details.goods_name = tally_sheetModel.goods;
                tally_sheet_details.units = tally_sheetModel.units;
                tally_sheet_details.quantity = tally_sheetModel.quantity;
                tally_sheet_details.unit_of_measure = tally_sheetModel.unit_of_measure;
                tally_sheet_details.total_quantity = tally_sheetModel.total_quantity;
                db.Tally_Sheet_Details.Add(tally_sheet_details);
                db.SaveChanges();
                return RedirectToAction("Details", new { tally_sheet_id = tally_sheet_id });
            }
            return RedirectToAction("Details", new { tally_sheet_id = tally_sheet_id });
        }

        //
        // GET: /Manage_Tally_Sheet/Edit/5

        public ActionResult Edit(int id = 0)
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
                                    select new { sa.ship_arrival_id, sa.ship_arrival_code };
            ViewBag.ship_arrivals = new SelectList(ship_arrival_code, "ship_arrival_id", "ship_arrival_code");
            TempData["id"] = tally_sheet_details.tally_sheet_details_id;
            TempData["tally_sheet_id"] = tally_sheet_details.tally_sheet_id;
            var tally_sheet_code = from t in db.Tally_Sheet where t.tally_sheet_id == tally_sheet_details.tally_sheet_id select t.tally_sheet_code;  
            tally_sheetModel.tally_sheet_code = tally_sheet_code.First();
            tally_sheetModel.ship_arrival_id = ship_arrival_code.First().ship_arrival_id;
            tally_sheetModel.way_bill_code = tally_sheet_details.way_bill_code;
            tally_sheetModel.goods = tally_sheet_details.goods_name;
            tally_sheetModel.units = tally_sheet_details.units;
            tally_sheetModel.quantity = tally_sheet_details.quantity;
            tally_sheetModel.unit_of_measure = tally_sheet_details.unit_of_measure;
            tally_sheetModel.total_quantity = tally_sheet_details.total_quantity;
            return View(tally_sheetModel);
        }

        //
        // POST: /Manage_Tally_Sheet/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tally_SheetModel tally_sheetModel)
        {
            int id = Convert.ToInt32(TempData["id"]);
            Tally_Sheet_Details tally_sheet_details = new Tally_Sheet_Details();
            if (ModelState.IsValid)
            {
                tally_sheet_details.tally_sheet_details_id = id;
                tally_sheet_details.tally_sheet_id = Convert.ToInt32(TempData["tally_sheet_id"]);
                tally_sheet_details.way_bill_code = tally_sheetModel.way_bill_code;
                tally_sheet_details.goods_name = tally_sheetModel.goods;
                tally_sheet_details.units = tally_sheetModel.units;
                tally_sheet_details.quantity = tally_sheetModel.quantity;
                tally_sheet_details.unit_of_measure = tally_sheetModel.unit_of_measure;
                tally_sheet_details.total_quantity = tally_sheetModel.total_quantity;
                db.Entry(tally_sheet_details).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { tally_sheet_id = tally_sheet_details.tally_sheet_id });
            }
            return RedirectToAction("Details", new { tally_sheet_id = tally_sheet_details.tally_sheet_id });
        }

        [HttpPost]
        public ActionResult SaveFile(Tally_Sheet tally_sheet)
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
                    string path = Server.MapPath("~/Images/" + filename);
                    files.SaveAs(path);
                    SaveExcel(path, tally_sheet_id_hd);
                    System.IO.File.Delete(path);
                    db.SaveChanges();
                }
                TempData["errorMessage"] = " Data Updated Successfully";
            return RedirectToAction("Index", "Manage_Tally_Sheet");
        }
        public void SaveExcel(string path, string hd_tally_sheet_id)
        {
            XlsFile myFile = new XlsFile(path);
            myFile.ActiveSheet = 1;

            int rowCount = myFile.RowCount;
            int colCount = myFile.ColCount;
            for (int i = 2; i <= rowCount; i++)
            {
                if (myFile.GetCellValue(i, 1) == null)
                {
                    break;
                }
                Puntland_Port_Taxation.Tally_Sheet_Details tsheet = new Tally_Sheet_Details();
                tsheet.tally_sheet_id = int.Parse(hd_tally_sheet_id);
                tsheet.way_bill_code = myFile.GetCellValue(i, 1).ToString();
                tsheet.goods_name = myFile.GetCellValue(i, 2).ToString();
                tsheet.units = (myFile.GetCellValue(i, 3).ToString());
                tsheet.quantity = myFile.GetCellValue(i, 4).ToString();
                tsheet.unit_of_measure = (myFile.GetCellValue(i, 5).ToString());
                tsheet.total_quantity = (myFile.GetCellValue(i, 6).ToString());
                db.Tally_Sheet_Details.Add(tsheet);
                db.SaveChanges();
            }

        }

        //
        // GET: /Manage_Tally_Sheet/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Tally_Sheet tally_sheet = db.Tally_Sheet.Find(id);
            if (tally_sheet == null)
            {
                return HttpNotFound();
            }
            return View(tally_sheet);
        }

        //
        // POST: /Manage_Tally_Sheet/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tally_Sheet tally_sheet = db.Tally_Sheet.Find(id);
            db.Tally_Sheet.Remove(tally_sheet);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // GET: /Manage_Tally_Sheet/DbSearch

        public ActionResult DbSearch()
        {
            ViewBag.ship_arrivals = new HomeController().Ship_Arrival();
            ViewBag.way_bills = new HomeController().Way_Bill();
            ViewBag.goods = new HomeController().Goods();
            return View();
        }

        //
        // POST: /Manage_Tally_Sheet/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Tally_Sheet tally_sheet)
        {
            //Queue q = new Queue();
            if (tally_sheet.ship_arrival_id != 0 && tally_sheet.tally_sheet_code != null)
            {
                var result = (from t in db.Tally_Sheet
                              join sa in db.Ship_Arrival on t.ship_arrival_id equals sa.ship_arrival_id
                              where t.ship_arrival_id == tally_sheet.ship_arrival_id && t.tally_sheet_code == tally_sheet.tally_sheet_code
                              select new Tally_SheetModel { tally_sheet_code = t.tally_sheet_code, ship_arrival_code = sa.ship_arrival_code, tally_sheet_id = t.tally_sheet_id }).Distinct(); 
                return View("Index", result.ToList());
            }
            else if (tally_sheet.ship_arrival_id != 0 && tally_sheet.tally_sheet_code == null)
            {
                var result = (from t in db.Tally_Sheet
                              join sa in db.Ship_Arrival on t.ship_arrival_id equals sa.ship_arrival_id
                              where t.ship_arrival_id == tally_sheet.ship_arrival_id
                              select new Tally_SheetModel { tally_sheet_code = t.tally_sheet_code, ship_arrival_code = sa.ship_arrival_code, tally_sheet_id = t.tally_sheet_id }).Distinct(); 
                return View("Index", result.ToList());
            }
            else if (tally_sheet.ship_arrival_id == 0 && tally_sheet.tally_sheet_code != null)
            {
                var result = (from t in db.Tally_Sheet
                              join sa in db.Ship_Arrival on t.ship_arrival_id equals sa.ship_arrival_id
                              where t.tally_sheet_code == tally_sheet.tally_sheet_code
                              select new Tally_SheetModel { tally_sheet_code = t.tally_sheet_code, ship_arrival_code = sa.ship_arrival_code, tally_sheet_id = t.tally_sheet_id }).Distinct(); 
                return View("Index", result.ToList());
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Manage_Tally_Sheet/DbSearch_new

        public ActionResult DbSearch_new()
        {
            ViewBag.goods = new HomeController().Goods();
            return View();
        }

        ////
        //// POST: /Manage_Tally_Sheet/DbSearchresult_new
        ////In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult_new(Tally_Sheet_Details tally_sheet)
        {
            var tally_sheet_id = Convert.ToInt32(Session["tally_sheet_id"]);
            if (tally_sheet.goods_name != null && tally_sheet.way_bill_code != null)
            {
                var result = from t in db.Tally_Sheet
                             join td in db.Tally_Sheet_Details on t.tally_sheet_id equals td.tally_sheet_id
                             where td.goods_name == tally_sheet.goods_name && td.way_bill_code == tally_sheet.way_bill_code && td.tally_sheet_id == tally_sheet_id
                             select new Tally_SheetModel { tally_sheet_id = t.tally_sheet_id, way_bill_code = td.way_bill_code, goods = td.goods_name, units = td.units, quantity = td.quantity, unit_of_measure = td.unit_of_measure, total_quantity = td.total_quantity, tally_sheet_code = t.tally_sheet_code, tally_sheet_details_id = td.tally_sheet_details_id };
                return View("Details", result.ToList());
            }
            else if (tally_sheet.goods_name != null && tally_sheet.way_bill_code == null)
            {
                var result = from t in db.Tally_Sheet
                             join td in db.Tally_Sheet_Details on t.tally_sheet_id equals td.tally_sheet_id
                             where td.goods_name == tally_sheet.goods_name && td.tally_sheet_id == tally_sheet_id
                             select new Tally_SheetModel { tally_sheet_id = t.tally_sheet_id, way_bill_code = td.way_bill_code, goods = td.goods_name, units = td.units, quantity = td.quantity, unit_of_measure = td.unit_of_measure, total_quantity = td.total_quantity, tally_sheet_code = t.tally_sheet_code, tally_sheet_details_id = td.tally_sheet_details_id };
                return View("Details", result.ToList());
            }
            else if (tally_sheet.goods_name == null && tally_sheet.way_bill_code != null)
            {
                var result = from t in db.Tally_Sheet
                             join td in db.Tally_Sheet_Details on t.tally_sheet_id equals td.tally_sheet_id
                             where td.way_bill_code == tally_sheet.way_bill_code && td.tally_sheet_id == tally_sheet_id
                             select new Tally_SheetModel { tally_sheet_id = t.tally_sheet_id, way_bill_code = td.way_bill_code, goods = td.goods_name, units = td.units, quantity = td.quantity, unit_of_measure = td.unit_of_measure, total_quantity = td.total_quantity, tally_sheet_code = t.tally_sheet_code, tally_sheet_details_id = td.tally_sheet_details_id };
                return View("Details", result.ToList());
            }
            return RedirectToAction("Details", new { tally_sheet_id = tally_sheet_id });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}