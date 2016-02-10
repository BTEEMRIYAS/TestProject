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
using FlexCel.Render;


namespace Puntland_Port_Taxation.Controllers
{
    public class Tally_Sheet_ExportController : Controller
    {
        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

        //
        // GET: /Tally_Sheet_Export/

        public ActionResult Index()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(38))
                {
                    int page;
                    int page_no = 1;
                    var count = db.E_Tally_Sheet.Count();
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
                    var tally_sheet = ((from t in db.E_Tally_Sheet
                                        join sd in db.Ship_Departure on t.ship_departure_id equals sd.ship_departure_id
                                        join e in db.Employees on t.employee_id equals e.employee_id
                                        where sd.status_id==2
                                        select new Tally_SheetModel { tally_sheet_code = t.tally_sheet_code, ship_arrival_code = sd.ship_departure_code, tally_sheet_id = t.tally_sheet_id, employee_name = e.first_name + " " + e.middle_name + " " + e.last_name  }).Distinct()).OrderByDescending(ts => ts.tally_sheet_id).Skip(start_from).Take(9);
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
        // GET: /Tally_Sheet_Export/Details/T0001

        public ActionResult Download_CM(int id)
        {
            //temp file name 
            string copy_file_name = DateTime.Now.Ticks + ".xlsx";
            //template file name 
            string Orginal_file_path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Report_Templates"), "Cargo_Manifest_Export.xlsx");
            //temp file path
            string copy_file_path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/App_Data"), copy_file_name);

            XlsFile cargo = new XlsFile(Orginal_file_path);

            cargo.ActiveSheet = 1;

            cargo.SetCellValue(1, 2, "Cargo Manifest - " + id);

            var cargo_details = from t in db.E_Tally_Sheet
                                join sd in db.Ship_Departure on t.ship_departure_id equals sd.ship_departure_id
                                join s in db.Ships on sd.shipp_id equals s.ship_id
                                join e in db.Employees on t.employee_id equals e.employee_id
                                where t.tally_sheet_id == id
                                select new { t.tally_sheet_id, s.ship_name, sd.starting_date, sd.ship_departure_code, e.first_name, e.middle_name, e.last_name };

            foreach (var item in cargo_details)
            {
                cargo.SetCellValue(2, 2, item.ship_name);
                cargo.SetCellValue(3, 2, item.starting_date);
                cargo.SetCellValue(4, 2, item.ship_departure_code);
                cargo.SetCellValue(5, 2, item.first_name + " " + item.middle_name + " " + item.last_name);
            }
            var cargo_goods_details = db.Cargo_Manifest_Export(id).OrderBy(e => e.way_bill_id);

            var iRowCnt = 8;

            foreach (var item in cargo_goods_details)
            {

                cargo.SetCellValue(iRowCnt, 1, item.way_bill_code);
                cargo.SetCellValue(iRowCnt, 2, item.exporter);
                cargo.SetCellValue(iRowCnt, 3, item.goods);
                cargo.SetCellValue(iRowCnt, 4, item.quantity);
                cargo.SetCellValue(iRowCnt, 5, item.unit_of_measure);
                cargo.SetCellValue(iRowCnt, 6, item.is_damaged);
                iRowCnt = iRowCnt + 1;
            }

            FlexCelPdfExport flexpdf = new FlexCelPdfExport();
            flexpdf.Workbook = cargo;
            flexpdf.AllowOverwritingFiles = true;
            flexpdf.Export(copy_file_path.Replace(".xlsx",".pdf"));

            var save_name = "Cargo_Manifest_" + id+".pdf";

            this.DownLoadFile(save_name, copy_file_name);
            this.DeleteFile(copy_file_name);

            return RedirectToAction("Index");
        }

        protected void DownLoadFile(string save_name, string copy_file_name)
        {
            string sPath = Server.MapPath("~/App_Data\\");
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + save_name);
            Response.TransmitFile(sPath + copy_file_name.Replace(".xlsx",".pdf"));
            Response.End();
        }

        protected void DeleteFile(string copy_file_name)
        {
            string fullPath = Request.MapPath("~/App_Data/" + copy_file_name.Replace(".xlsx", ".pdf"));
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }

        //
        // GET: /Tally_Sheet_Export/Create

        public ActionResult Create()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(38))
                {
                    ViewBag.ship_arrivals = new HomeController().Ship_Departure();
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
        // POST: /Tally_Sheet_Export/Create_Carogo_Manifest

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(E_Tally_Sheet tally_sheet)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(38))
                {
                    if (ModelState.IsValid)
                    {
                        var is_ship_departure_exist = from t in db.E_Tally_Sheet where t.ship_departure_id == tally_sheet.ship_departure_id select t;
                        if (is_ship_departure_exist.Count() > 0)
                        {
                            TempData["errorMessage"] = "Only One Cargo Manifest Allowed For a Shipp Arrival, Please Choose Another One";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            tally_sheet.employee_id = Convert.ToInt32(Session["id"]);
                            tally_sheet.tally_sheet_code = "CME_";
                            db.E_Tally_Sheet.Add(tally_sheet);
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
                            Ship_Departure ship_departure = db.Ship_Departure.Find(tally_sheet.ship_departure_id);
                            ship_departure.status_id = 2;
                            db.Entry(ship_departure).State = EntityState.Modified;
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
        // GET: /Tally_Sheet_Export/DbSearch

        public ActionResult DbSearch()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(38))
                {
                    ViewBag.ship_arrivals = new HomeController().Ship_Departure();
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
        // POST: /Tally_Sheet_Export/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Tally_SheetModel tally_sheet)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(38))
                {
                    //Queue q = new Queue();
                    if (tally_sheet.ship_arrival_code != null && tally_sheet.tally_sheet_code != null)
                    {
                        var result = (from t in db.E_Tally_Sheet
                                      join sd in db.Ship_Departure on t.ship_departure_id equals sd.ship_departure_id
                                      join e in db.Employees on t.employee_id equals e.employee_id
                                      where sd.ship_departure_code.Contains(tally_sheet.ship_arrival_code) && t.tally_sheet_code.Contains(tally_sheet.tally_sheet_code)
                                      select new Tally_SheetModel { tally_sheet_code = t.tally_sheet_code, ship_arrival_code = sd.ship_departure_code, tally_sheet_id = t.tally_sheet_id, employee_name = e.first_name + " " + e.middle_name + " " + e.last_name }).Distinct();
                        return View("Index", result.ToList());
                    }
                    else if (tally_sheet.ship_arrival_code != null && tally_sheet.tally_sheet_code == null)
                    {
                        var result = (from t in db.E_Tally_Sheet
                                      join sd in db.Ship_Departure on t.ship_departure_id equals sd.ship_departure_id
                                      join e in db.Employees on t.employee_id equals e.employee_id
                                      where sd.ship_departure_code.Contains(tally_sheet.ship_arrival_code)
                                      select new Tally_SheetModel { tally_sheet_code = t.tally_sheet_code, ship_arrival_code = sd.ship_departure_code, tally_sheet_id = t.tally_sheet_id, employee_name = e.first_name + " " + e.middle_name + " " + e.last_name }).Distinct();
                        return View("Index", result.ToList());
                    }
                    else if (tally_sheet.ship_arrival_code == null && tally_sheet.tally_sheet_code != null)
                    {
                        var result = (from t in db.E_Tally_Sheet
                                      join sd in db.Ship_Departure on t.ship_departure_id equals sd.ship_departure_id
                                      join e in db.Employees on t.employee_id equals e.employee_id
                                      where t.tally_sheet_code.Contains(tally_sheet.tally_sheet_code)
                                      select new Tally_SheetModel { tally_sheet_code = t.tally_sheet_code, ship_arrival_code = sd.ship_departure_code, tally_sheet_id = t.tally_sheet_id, employee_name = e.first_name + " " + e.middle_name + " " + e.last_name }).Distinct();
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