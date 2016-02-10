using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
//using System.Web.Script.Serialization.JavaScriptSerializer;
using Puntland_Port_Taxation.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

//using System.Runtime.Serialization.Json.DataContractJsonSerializer;

namespace Puntland_Port_Taxation.Controllers
{
    public class Way_Bill_ExportController : Controller
    {
        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

        //
        // GET: /Way_Bill_Export/

        public ActionResult Index()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(31))
                {
                    int page;
                    int page_no = 1;
                    var count = db.E_Way_Bill.Count();
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
                    var way_bill = (from w in db.E_Way_Bill
                                    join e in db.Exports on w.export_id equals e.export_id
                                    join sd in db.Ship_Departure on e.ship_departure_id equals sd.ship_departure_id
                                    select new Way_BillModel { way_bill_code = w.e_way_bill_code, import_name = e.export_code, ship_arrival_code = sd.ship_departure_code, import_status_id = e.exporting_status_id, way_bill_id = w.e_way_bill_id, mark = w.mark }).OrderByDescending(wy => wy.way_bill_id).Skip(start_from).Take(9);
                    return View(way_bill.ToList());
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
        // GET: /Way_Bill_Export/Details/W0001

        public ActionResult Details(int way_bill_id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(31))
                {
                    int page;
                    int page_no = 1;
                    var count = db.E_Way_Bill_Details.Where(w => w.e_way_bill_id == way_bill_id).Count();
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
                    var way_bill = (from w in db.E_Way_Bill
                                    join wd in db.E_Way_Bill_Details on w.e_way_bill_id equals wd.e_way_bill_id
                                    join e in db.Exports on w.export_id equals e.export_id
                                    join i in db.Importers on w.exporter_id equals i.importer_id
                                    join g in db.Goods on wd.goods_id equals g.goods_id
                                    join u in db.Unit_Of_Measure on wd.unit_of_measure_id equals u.unit_id
                                    where w.e_way_bill_id == way_bill_id
                                    select new Way_BillModel { way_bill_id = w.e_way_bill_id, importer_name = i.importer_first_name + " " + i.importer_middle_name + " " + i.importer_last_name, way_bill_code = w.e_way_bill_code, way_bill_name = w.e_way_bill_name, goods = g.goods_name, unit_of_measure = u.unit_code, total_quantity = wd.total_quantity, import_status_id = e.exporting_status_id, goods_id = wd.goods_id, way_bill_details_id = wd.e_way_bill_details_id, is_damaged = wd.is_damaged }).OrderBy(a => a.way_bill_details_id).Skip(start_from).Take(9);
                    var way_bill_code = (from w in db.E_Way_Bill where w.e_way_bill_id == way_bill_id select w.e_way_bill_code).First();
                    Session["way_bill_code"] = way_bill_code;
                    Session["way_bill_id"] = way_bill_id;
                    TempData["exporter"] = (from w in db.E_Way_Bill
                                            join i in db.Importers on w.exporter_id equals i.importer_id
                                            where w.e_way_bill_id == way_bill_id
                                            select i.importer_first_name + " " + i.importer_middle_name + " " + i.importer_last_name).First();
                    return View(way_bill.ToList());
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
        // GET: /Way_Bill_Export/View_Way_Bill/W0001

        public ActionResult View_Way_Bill(int id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(31))
                {
                    var way_bill = (from w in db.E_Way_Bill
                                    join wd in db.E_Way_Bill_Details on w.e_way_bill_id equals wd.e_way_bill_id
                                    join i in db.Importers on w.exporter_id equals i.importer_id
                                    join g in db.Goods on wd.goods_id equals g.goods_id
                                    join u in db.Unit_Of_Measure on wd.unit_of_measure_id equals u.unit_id
                                    where wd.e_way_bill_id == id
                                    select new Way_BillModel { goods = g.goods_name, unit_of_measure = u.unit_code, total_quantity = wd.total_quantity, importer_name = i.importer_first_name + " " + i.importer_middle_name + " " + i.importer_last_name, is_damaged = wd.is_damaged });
                    var way_biil_code = (from w in db.E_Way_Bill where w.e_way_bill_id == id select w.e_way_bill_code).FirstOrDefault();
                    ViewBag.way_bill_code = way_biil_code;
                    TempData["exporter"] = (from w in db.E_Way_Bill
                                            join e in db.Exports on w.export_id equals e.export_id
                                            join ir in db.Importers on e.exporter_id equals ir.importer_id
                                            where w.e_way_bill_id == id
                                            select ir.importer_first_name + " " + ir.importer_middle_name + " " + ir.importer_last_name).FirstOrDefault();
                    return View(way_bill.ToList());
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
        // GET: /Way_Bill_Export/Create

        public ActionResult Create()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(31))
                {
                    ViewBag.export = new HomeController().Export();
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
        // POST: /Way_Bill_Export/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(E_Way_Bill way_bill)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(31))
                {
                    var exporter_id = 0;
                    var ship_departure_id = 0;
                    var exporter = from e in db.Exports
                                   join ir in db.Importers on e.exporter_id equals ir.importer_id
                                   join sd in db.Ship_Departure on e.ship_departure_id equals sd.ship_departure_id
                                   where e.export_id == way_bill.export_id
                                   select new { ir.importer_id, sd.ship_departure_id };
                    foreach (var item in exporter)
                    {
                        exporter_id = item.importer_id;
                        ship_departure_id = item.ship_departure_id;
                    }
                    var way_bill_exist = (from w in db.E_Way_Bill
                                          join e in db.Exports on w.export_id equals e.export_id
                                          join sd in db.Ship_Departure on e.ship_departure_id equals sd.ship_departure_id
                                          where w.e_way_bill_code == way_bill.e_way_bill_code && sd.ship_departure_id == ship_departure_id
                                          select w).Count();
                    var export_exist = (from w in db.E_Way_Bill
                                        where w.export_id == way_bill.export_id
                                        select w).Count();
                    if (export_exist > 0)
                    {
                        TempData["errorMessage"] = "Only One Way Bill Allowed For an Export, Please Choose Another Export";
                        return RedirectToAction("Index");
                    }
                    else if (way_bill_exist > 0)
                    {
                        TempData["errorMessage"] = "Way Bill Already Exists For This Ship Departure,Please Enter Another One";
                        return RedirectToAction("Index");
                    }
                    if (ModelState.IsValid)
                    {
                        way_bill.exporter_id = exporter_id;
                        way_bill.e_way_bill_name = "100";
                        way_bill.is_calculated = false;
                        db.E_Way_Bill.Add(way_bill);
                        db.SaveChanges();
                        way_bill.e_way_bill_name = way_bill.e_way_bill_name + way_bill.e_way_bill_id;
                        db.Entry(way_bill).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["errorMessage"] = "Way Bill Added Successfully";
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
        // GET: /Way_Bill_Export/Create_Way_Bill

        public ActionResult Create_Way_Bill()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(31))
                {
                    ViewBag.categories = new HomeController().Category();
                    ViewBag.currency = new HomeController().Currency();
                    ViewBag.unif_of_measures = new HomeController().Unit_Of_Measures();
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

        //Way_Bill_Export/way_bill_already

        public int way_bill_already()
        {
            return 0;
        }


        //To add goods using add next in create way bill page
        // POST: /Way_Bill_Export/Create_Way_Bill

        [HttpPost]
        public int Create_Way_Bill_new(string obj)
        {
            //To make serialized string in correct Json format
            string s = "{^" + obj.Replace("&", ",^").Replace("=", "^:") + "}";
            s = s.Replace('^', '"').Replace("False", "false").Replace("True", "true");

            // JObject Jobj = JObject.Parse(s);


            JavaScriptSerializer js = new JavaScriptSerializer();
            var way_billModel = js.Deserialize<Way_BillModel>(s);
            E_Way_Bill_Details way_bill_details = new E_Way_Bill_Details();
            var way_bill_id = Convert.ToInt32(Session["way_bill_id"]);
            way_bill_details.e_way_bill_id = way_bill_id;
            way_bill_details.goods_id = way_billModel.goods_id;
            way_bill_details.unit_of_measure_id = way_billModel.unit_of_measure_id;
            way_bill_details.total_quantity = way_billModel.total_quantity;
            way_bill_details.is_damaged = way_billModel.is_damaged;
            db.E_Way_Bill_Details.Add(way_bill_details);
            db.SaveChanges();
            return 1;
        }

        [HttpPost]
        public ActionResult Create_Way_Bill(Way_BillModel way_billModel)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(31))
                {
                    E_Way_Bill_Details way_bill_details = new E_Way_Bill_Details();
                    var way_bill_id = Convert.ToInt32(Session["way_bill_id"]);
                    if (ModelState.IsValid)
                    {
                        way_bill_details.e_way_bill_id = way_bill_id;
                        way_bill_details.goods_id = way_billModel.goods_id;
                        way_bill_details.unit_of_measure_id = way_billModel.unit_of_measure_id;
                        way_bill_details.total_quantity = way_billModel.total_quantity;
                        way_bill_details.is_damaged = way_billModel.is_damaged;
                        db.E_Way_Bill_Details.Add(way_bill_details);
                        db.SaveChanges();
                        return RedirectToAction("Details", new { way_bill_id = way_bill_id });
                    }
                    return RedirectToAction("Details", new { way_bill_id = way_bill_id });
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
        // GET: /Way_Bill_Export/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(31))
                {
                    E_Way_Bill_Details way_bill_details = db.E_Way_Bill_Details.Find(id);
                    Way_BillModel way_billModel = new Way_BillModel();
                    if (way_bill_details == null)
                    {
                        return HttpNotFound();
                    }
                    var export_code = from w in db.E_Way_Bill
                                      join wd in db.E_Way_Bill_Details on w.e_way_bill_id equals wd.e_way_bill_id
                                      join e in db.Exports on w.export_id equals e.export_id
                                      where wd.e_way_bill_details_id == way_bill_details.e_way_bill_details_id
                                      select new { e.export_id, e.export_code };
                    ViewBag.export = new SelectList(export_code, "export_id", "export_code");
                    var way_bill_master_details = (from w in db.E_Way_Bill
                                                   where w.e_way_bill_id == way_bill_details.e_way_bill_id
                                                   select new { w.e_way_bill_name, w.export_id, w.exporter_id, w.is_calculated, w.e_way_bill_code, w.e_way_bill_id, w.mark }).First();
                    var goods = from g in db.Goods
                                join gt in db.Goods_Type on g.goods_type_id equals gt.goods_type_id
                                join gs in db.Goods_Subcategory on gt.goods_subcategory_id equals gs.goods_subcategory_id
                                join gc in db.Goods_Category on gs.goods_category_id equals gc.goods_category_id
                                where g.goods_id == way_bill_details.goods_id
                                select new { g.goods_id, g.goods_name, gt.goods_type_id, gt.goods_type_name, gs.goods_subcategory_id, gs.goods_subcategory_name, gc.goods_category_id };
                    var unit_id = from u in db.Unit_Of_Measure
                                  where u.unit_id == way_bill_details.unit_of_measure_id
                                  select u;
                    ViewBag.unit_of_measure = new SelectList(unit_id, "unit_id", "unit_code");
                    ViewBag.subcategories = new SelectList(goods, "goods_subcategory_id", "goods_subcategory_name");
                    ViewBag.goods_type = new SelectList(goods, "goods_type_id", "goods_type_name");
                    ViewBag.goods = new SelectList(goods, "goods_id", "goods_name");
                    TempData["id"] = way_bill_details.e_way_bill_details_id;
                    TempData["way_bill_id"] = way_bill_details.e_way_bill_id;
                    ViewBag.categories = new HomeController().Category();
                    ViewBag.currency = new HomeController().Currency();
                    way_billModel.way_bill_code = way_bill_master_details.e_way_bill_code;
                    way_billModel.import_id = way_bill_master_details.export_id;
                    way_billModel.mark = way_bill_master_details.mark;
                    way_billModel.goods_category_id = goods.First().goods_category_id;
                    way_billModel.goods_subcategory_id = goods.First().goods_subcategory_id;
                    way_billModel.goods_type_id = goods.First().goods_type_id;
                    way_billModel.goods_id = way_bill_details.goods_id;
                    way_billModel.unit_of_measure_id = way_bill_details.unit_of_measure_id;
                    way_billModel.is_damaged = way_bill_details.is_damaged;
                    way_billModel.total_quantity = Convert.ToInt32(way_bill_details.total_quantity);
                    return View(way_billModel);
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
        // POST: /Way_Bill_Export/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Way_BillModel way_billModel)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(31))
                {
                    E_Way_Bill_Details way_bill_details = new E_Way_Bill_Details();
                    int id = Convert.ToInt32(TempData["id"]);
                    if (ModelState.IsValid)
                    {
                        way_bill_details.e_way_bill_details_id = id;
                        way_bill_details.e_way_bill_id = Convert.ToInt32(TempData["way_bill_id"]);
                        way_bill_details.goods_id = way_billModel.goods_id;
                        way_bill_details.unit_of_measure_id = way_billModel.unit_of_measure_id;
                        way_bill_details.total_quantity = way_billModel.total_quantity;
                        way_bill_details.currency_id = way_billModel.currency_id;
                        way_bill_details.is_damaged = way_billModel.is_damaged;
                        db.Entry(way_bill_details).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Details", new { way_bill_id = way_bill_details.e_way_bill_id });
                    }
                    return RedirectToAction("Details", new { way_bill_id = way_bill_details.e_way_bill_id });
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
        // GET: /Way_Bill_Export/Submit_Way_Bill/W0009

        public ActionResult Submit_Way_Bill()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(31))
                {
                    var way_bill_code = Session["way_bill_code"].ToString();
                    var way_bill_id = Convert.ToInt32(Session["way_bill_id"]);
                    var way_bill = (from w in db.E_Way_Bill
                                    join e in db.Exports on w.export_id equals e.export_id
                                    where w.e_way_bill_id == way_bill_id
                                    select new { w.e_way_bill_id, e.export_id, e.exporting_status_id }).FirstOrDefault();
                    Export export = db.Exports.Find(way_bill.export_id);
                    export.e_way_bill_id = way_bill_id;
                    export.exporting_status_id = 25;
                    db.Entry(export).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["errorMessage"] = "Way Bill Submitted";
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
        // GET: /Way_Bill_Export/Reject_Reason_View/5

        //public ActionResult Reject_Reason_View(int way_bill_id)
        //{
        //    if (Session["login_status"] != null)
        //    {
        //        int[] z = (int[])Session["function_id"];
        //        if (z.Contains(13))
        //        {
        //            var reason = (from r in db.Reject_Reason
        //                          join w in db.Way_Bill on r.way_bill_id equals w.way_bill_id
        //                          join i in db.Imports on r.way_bill_id equals i.way_bill_id
        //                          join eu in db.EU_Check on r.way_bill_id equals eu.way_bill_id into eua
        //                          from k in eua.DefaultIfEmpty()
        //                          where r.way_bill_id == way_bill_id
        //                          orderby r.reject_reason_id descending, k.eu_check_id descending
        //                          select new Reject_ReasonModel { reject_reason_id = r.reject_reason_id, way_bill_id = r.way_bill_id, way_bill_code = w.way_bill_code, reason = r.reject_reason, rejected_from = r.reject_from, rejected_date = r.rejected_date, rechecked_by = k.eu_checked_by, reject_number = k.reject_number, import_status = i.importing_status_id });
        //            return View(reason.ToList());
        //        }
        //        else
        //        {
        //            return RedirectToAction("../Home/Dashboard");
        //        }
        //    }
        //    else
        //    {
        //        return RedirectToAction("../Home");
        //    }
        //}

        //
        // GET: /Way_Bill_Export/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(31))
                {
                    E_Way_Bill_Details way_bill_details = db.E_Way_Bill_Details.Find(id);
                    if (way_bill_details == null)
                    {
                        return HttpNotFound();
                    }
                    var goods = (from g in db.Goods where g.goods_id == way_bill_details.goods_id select g.goods_name).First();
                    ViewBag.goods = goods;
                    return View(way_bill_details);
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
        // POST: /Way_Bill_Export/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(31))
                {
                    var way_bill_id = Convert.ToInt32(Session["way_bill_id"]);
                    E_Way_Bill_Details way_bill_details = db.E_Way_Bill_Details.Find(id);
                    db.E_Way_Bill_Details.Remove(way_bill_details);
                    db.SaveChanges();
                    return RedirectToAction("Details", new { way_bill_id = way_bill_id });
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
        // GET: /Way_Bill_Export/DbSearch

        public ActionResult DbSearch()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(31))
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
        // POST: /Way_Bill_Export/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Way_BillModel way_bill)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(31))
                {
                    //Queue q = new Queue();
                    if (way_bill.way_bill_code != null && way_bill.import_code != null && way_bill.ship_arrival_code != null)
                    {
                        var result = (from w in db.E_Way_Bill
                                      join e in db.Exports on w.export_id equals e.export_id
                                      join sd in db.Ship_Departure on e.ship_departure_id equals sd.ship_departure_id
                                      where w.e_way_bill_code.Contains(way_bill.way_bill_code) && e.export_code.Contains(way_bill.import_code) && sd.ship_departure_code.Contains(way_bill.ship_arrival_code)
                                      select new Way_BillModel { way_bill_code = w.e_way_bill_code, import_name = e.export_code, ship_arrival_code = sd.ship_departure_code, import_status_id = e.exporting_status_id, way_bill_id = w.e_way_bill_id, mark = w.mark });
                        return View("Index", result.ToList());
                    }
                    else if (way_bill.way_bill_code != null && way_bill.import_code != null && way_bill.ship_arrival_code == null)
                    {
                        var result = (from w in db.E_Way_Bill
                                      join e in db.Exports on w.export_id equals e.export_id
                                      join sd in db.Ship_Departure on e.ship_departure_id equals sd.ship_departure_id
                                      where w.e_way_bill_code.Contains(way_bill.way_bill_code) && e.export_code.Contains(way_bill.import_code)
                                      select new Way_BillModel { way_bill_code = w.e_way_bill_code, import_name = e.export_code, ship_arrival_code = sd.ship_departure_code, import_status_id = e.exporting_status_id, way_bill_id = w.e_way_bill_id, mark = w.mark });
                        return View("Index", result.ToList());
                    }
                    else if (way_bill.way_bill_code != null && way_bill.import_code == null && way_bill.ship_arrival_code != null)
                    {
                        var result = (from w in db.E_Way_Bill
                                      join e in db.Exports on w.export_id equals e.export_id
                                      join sd in db.Ship_Departure on e.ship_departure_id equals sd.ship_departure_id
                                      where w.e_way_bill_code.Contains(way_bill.way_bill_code) && sd.ship_departure_code.Contains(way_bill.ship_arrival_code)
                                      select new Way_BillModel { way_bill_code = w.e_way_bill_code, import_name = e.export_code, ship_arrival_code = sd.ship_departure_code, import_status_id = e.exporting_status_id, way_bill_id = w.e_way_bill_id, mark = w.mark });
                        return View("Index", result.ToList());
                    }
                    else if (way_bill.way_bill_code != null && way_bill.import_code == null && way_bill.ship_arrival_code == null)
                    {
                        var result = (from w in db.E_Way_Bill
                                      join e in db.Exports on w.export_id equals e.export_id
                                      join sd in db.Ship_Departure on e.ship_departure_id equals sd.ship_departure_id
                                      where w.e_way_bill_code.Contains(way_bill.way_bill_code)
                                      select new Way_BillModel { way_bill_code = w.e_way_bill_code, import_name = e.export_code, ship_arrival_code = sd.ship_departure_code, import_status_id = e.exporting_status_id, way_bill_id = w.e_way_bill_id, mark = w.mark });
                        return View("Index", result.ToList());
                    }
                    else if (way_bill.way_bill_code == null && way_bill.import_code != null && way_bill.ship_arrival_code != null)
                    {
                        var result = (from w in db.E_Way_Bill
                                      join e in db.Exports on w.export_id equals e.export_id
                                      join sd in db.Ship_Departure on e.ship_departure_id equals sd.ship_departure_id
                                      where e.export_code.Contains(way_bill.import_code) && sd.ship_departure_code.Contains(way_bill.ship_arrival_code)
                                      select new Way_BillModel { way_bill_code = w.e_way_bill_code, import_name = e.export_code, ship_arrival_code = sd.ship_departure_code, import_status_id = e.exporting_status_id, way_bill_id = w.e_way_bill_id, mark = w.mark });
                        return View("Index", result.ToList());
                    }
                    else if (way_bill.way_bill_code == null && way_bill.import_code != null && way_bill.ship_arrival_code == null)
                    {
                        var result = (from w in db.E_Way_Bill
                                      join e in db.Exports on w.export_id equals e.export_id
                                      join sd in db.Ship_Departure on e.ship_departure_id equals sd.ship_departure_id
                                      where e.export_code.Contains(way_bill.import_code)
                                      select new Way_BillModel { way_bill_code = w.e_way_bill_code, import_name = e.export_code, ship_arrival_code = sd.ship_departure_code, import_status_id = e.exporting_status_id, way_bill_id = w.e_way_bill_id, mark = w.mark });
                        return View("Index", result.ToList());
                    }
                    else if (way_bill.way_bill_code == null && way_bill.import_code == null && way_bill.ship_arrival_code != null)
                    {
                        var result = (from w in db.E_Way_Bill
                                      join e in db.Exports on w.export_id equals e.export_id
                                      join sd in db.Ship_Departure on e.ship_departure_id equals sd.ship_departure_id
                                      where sd.ship_departure_code.Contains(way_bill.ship_arrival_code)
                                      select new Way_BillModel { way_bill_code = w.e_way_bill_code, import_name = e.export_code, ship_arrival_code = sd.ship_departure_code, import_status_id = e.exporting_status_id, way_bill_id = w.e_way_bill_id, mark = w.mark });
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
        // GET: /Way_Bill_Export/DbSearch_new

        public ActionResult DbSearch_new()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(31))
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
        // POST: /Way_Bill_Export/DbSearchresult_new
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult_new(Way_BillModel way_bill)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(31))
                {
                    var way_bill_id = Convert.ToInt32(Session["way_bill_id"]);
                    if (way_bill.goods != null)
                    {
                        var result = (from w in db.E_Way_Bill
                                      join wd in db.E_Way_Bill_Details on w.e_way_bill_id equals wd.e_way_bill_id
                                      join e in db.Exports on w.export_id equals e.export_id
                                      join i in db.Importers on w.exporter_id equals i.importer_id
                                      join g in db.Goods on wd.goods_id equals g.goods_id
                                      join u in db.Unit_Of_Measure on wd.unit_of_measure_id equals u.unit_id
                                      where g.goods_name.Contains(way_bill.goods) && wd.e_way_bill_id == way_bill_id
                                      select new Way_BillModel { way_bill_id = w.e_way_bill_id, importer_name = i.importer_first_name, way_bill_code = w.e_way_bill_code, way_bill_name = w.e_way_bill_name, goods = g.goods_name, unit_of_measure = u.unit_code, total_quantity = wd.total_quantity, import_status_id = e.exporting_status_id, goods_id = wd.goods_id, way_bill_details_id = wd.e_way_bill_details_id, is_damaged = wd.is_damaged });
                        TempData["exporter"] = (from w in db.E_Way_Bill
                                                join i in db.Importers on w.exporter_id equals i.importer_id
                                                where w.e_way_bill_id == way_bill_id
                                                select i.importer_first_name + " " + i.importer_middle_name + " " + i.importer_last_name).First();
                        return View("Details", result.ToList());
                    }
                    return RedirectToAction("Details", new { way_bill_id = way_bill_id });
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
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace Puntland_Port_Taxation.Controllers
//{
//    public class Way_Bill_ExportController : Controller
//    {
//        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

//        //
//        // GET: /Way_Bill_Export/

//        public ActionResult Index()
//        {
//            return View(db.E_Way_Bill.ToList());
//        }

//        //
//        // GET: /Way_Bill_Export/Details/5

//        public ActionResult Details(int id = 0)
//        {
//            E_Way_Bill e_way_bill = db.E_Way_Bill.Find(id);
//            if (e_way_bill == null)
//            {
//                return HttpNotFound();
//            }
//            return View(e_way_bill);
//        }

//        //
//        // GET: /Way_Bill_Export/Create

//        public ActionResult Create()
//        {
//            return View();
//        }

//        //
//        // POST: /Way_Bill_Export/Create

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create(E_Way_Bill e_way_bill)
//        {
//            if (ModelState.IsValid)
//            {
//                db.E_Way_Bill.Add(e_way_bill);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            return View(e_way_bill);
//        }

//        //
//        // GET: /Way_Bill_Export/Edit/5

//        public ActionResult Edit(int id = 0)
//        {
//            E_Way_Bill e_way_bill = db.E_Way_Bill.Find(id);
//            if (e_way_bill == null)
//            {
//                return HttpNotFound();
//            }
//            return View(e_way_bill);
//        }

//        //
//        // POST: /Way_Bill_Export/Edit/5

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit(E_Way_Bill e_way_bill)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(e_way_bill).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(e_way_bill);
//        }

//        //
//        // GET: /Way_Bill_Export/Delete/5

//        public ActionResult Delete(int id = 0)
//        {
//            E_Way_Bill e_way_bill = db.E_Way_Bill.Find(id);
//            if (e_way_bill == null)
//            {
//                return HttpNotFound();
//            }
//            return View(e_way_bill);
//        }

//        //
//        // POST: /Way_Bill_Export/Delete/5

//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            E_Way_Bill e_way_bill = db.E_Way_Bill.Find(id);
//            db.E_Way_Bill.Remove(e_way_bill);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            db.Dispose();
//            base.Dispose(disposing);
//        }
//    }
//}