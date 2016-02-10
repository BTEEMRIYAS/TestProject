using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Puntland_Port_Taxation.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Puntland_Port_Taxation.Controllers
{
    public class Export_DepartmentController : Controller
    {
        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

        //
        // GET: /Export_Department/

        public ActionResult Index(int way_bill_id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(35))
                {
                    int page;
                    int page_no = 1;
                    var count = 0;
                    if (way_bill_id == 0)
                    {
                        count = db.Exports.Where(a => a.exporting_status_id == 28 || a.exporting_status_id == 33).Count();
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
                    Session["release"] = way_bill_id;
                    if (way_bill_id != 0)
                    {
                        var way_bill = (from w in db.E_Way_Bill
                                        join e in db.Exports on w.export_id equals e.export_id
                                        join sd in db.Ship_Departure on e.ship_departure_id equals sd.ship_departure_id
                                        join ist in db.Importing_Status on e.exporting_status_id equals ist.importing_status_id
                                        where w.e_way_bill_id == way_bill_id
                                        select new Manifest_Control_SectionModel { way_bill_code = w.e_way_bill_code, import_name = e.export_code, ship_arrival_code = sd.ship_departure_code, bolleto_code = e.e_bollete_dogonale_code, way_bill_id = w.e_way_bill_id, importing_status = e.exporting_status_id }).Distinct();
                        return View(way_bill.ToList());
                    }
                    else
                    {
                        var way_bill = ((from w in db.E_Way_Bill
                                         join e in db.Exports on w.export_id equals e.export_id
                                         join sd in db.Ship_Departure on e.ship_departure_id equals sd.ship_departure_id
                                         join ist in db.Importing_Status on e.exporting_status_id equals ist.importing_status_id
                                         where e.exporting_status_id == 28 || e.exporting_status_id == 33
                                         select new Manifest_Control_SectionModel { way_bill_code = w.e_way_bill_code, import_name = e.export_code, ship_arrival_code = sd.ship_departure_code, bolleto_code = e.e_bollete_dogonale_code, way_bill_id = w.e_way_bill_id, importing_status = e.exporting_status_id }).Distinct()).OrderByDescending( a => a.way_bill_id).Skip(start_from).Take(9);
                        return View(way_bill.ToList());
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
        // GET: /Export_Department/Details/W0001

        public ActionResult Details(int id)
        { 
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(35))
                {          
                    var way_bill_code = (from w in db.E_Way_Bill where w.e_way_bill_id == id select w.e_way_bill_code).FirstOrDefault();
                    TempData["way_bill_code"] = way_bill_code;
                    TempData["way_bill_id"] = id;
                    var release = db.E_Display_Release(id);
                    var penaltized_goods = (from pgd in db.E_Penaltized_Goods_Details
                                            join g in db.Goods on pgd.goods_id equals g.goods_id
                                            join u in db.Unit_Of_Measure on pgd.unit_of_measure_id equals u.unit_id
                                            where pgd.e_way_bill_id == id
                                            select new Way_BillModel { goods = g.goods_name, quantity = pgd.goods_id, unit_of_measure = u.unit_code, is_damaged = pgd.is_damaged }).ToList();
                    ViewBag.penalty_goods = penaltized_goods;
                    return View(release.ToList());
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
        // GET: /Export_Department/Total_Goods_View/W0001

        public ActionResult Total_Goods_View(int id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(35))
                {
                    var way_bill_code = (from w in db.E_Way_Bill where w.e_way_bill_id == id select w.e_way_bill_code).FirstOrDefault();
                    TempData["way_bill_code"] = way_bill_code;
                    TempData["way_bill_id"] = id;
                    var release = db.E_Display_Release(id);
                    var penaltized_goods = (from pgd in db.E_Penaltized_Goods_Details
                                            join g in db.Goods on pgd.goods_id equals g.goods_id
                                            join u in db.Unit_Of_Measure on pgd.unit_of_measure_id equals u.unit_id
                                            where pgd.e_way_bill_id == id
                                            select new Way_BillModel { goods = g.goods_name, quantity = pgd.goods_id, unit_of_measure = u.unit_code, is_damaged = pgd.is_damaged, penaltized_goods_details_id = pgd.e_penaltized_goods_details_id }).ToList();
                    ViewBag.penalty_goods = penaltized_goods;
                    return View(release.ToList());
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
        // POST: /Export_Department/Delete/5
        public int DeleteConfirmed(int id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(35))
                {
                    E_Penaltized_Goods_Details penalty_goods = db.E_Penaltized_Goods_Details.Find(id);
                    db.E_Penaltized_Goods_Details.Remove(penalty_goods);
                    db.SaveChanges();
                }
            }
            return 1;
        }

        //
        // GET: /Export_Department/Search_Way_Bill

        public ActionResult Search_Way_Bill()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(35))
                {
                    ViewBag.way_bill = new HomeController().E_Way_Bill_Release();
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
        // POST: /Export_Department/Search_Way_Bill

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search_Way_Bill(Manifest_Control_SectionModel manifest)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(35))
                {
                    return RedirectToAction("Index", new { way_bill_id = manifest.way_bill_id });
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
        // GET: /Export_Department/Submit_Manifest/W0009

        public ActionResult Submit_Manifest(int way_bill_id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(35))
                {
                    var way_bill = (from w in db.E_Way_Bill
                                    join e in db.Exports on w.export_id equals e.export_id
                                    where w.e_way_bill_id == way_bill_id
                                    select new { w.e_way_bill_id, e.export_id }).First();
                    Export export = db.Exports.Find(way_bill.export_id);
                    export.exporting_status_id = 29;
                    db.Entry(export).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["errorMessage"] = "Export Submitted";
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
        // GET: /Export_Department/Reject_Manifest/15

        public ActionResult Reject_Manifest(int way_bill_id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(35))
                {
                    db.E_Reject_Calculated_Tax(way_bill_id, 30);
                    var way_bill = (from w in db.E_Way_Bill
                                    join e in db.Exports on w.export_id equals e.export_id
                                    where w.e_way_bill_id == way_bill_id
                                    select e.export_id).FirstOrDefault();
                    Export export = db.Exports.Find(way_bill);
                    export.e_payment_id = 1;
                    db.Entry(export).State = EntityState.Modified;
                    db.SaveChanges();
                    //var payment_details = db.Payment_details.Where(pd => pd.import_id == way_bill).ToList();
                    //foreach (var item in payment_details)
                    //{
                    //    db.Payment_details.Remove(item);
                    //}
                    //db.SaveChanges();
                    TempData["errorMessage"] = "Export Department Rejected";
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
        // GET: /Export_Department/Create_Way_Bill

        public ActionResult Create_Way_Bill(int way_bill_id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(35))
                {
                    ViewBag.categories = new HomeController().Category();
                    ViewBag.currency = new HomeController().Currency();
                    ViewBag.unif_of_measures = new HomeController().Unit_Of_Measures();
                    TempData["way_bill_id"] = way_bill_id;
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

        //To add goods using add next in create way bill page
        // POST: /Export_Department/Create_Way_Bill

        [HttpPost]
        public int Save_Penaltized_Goods(string obj)
        {
            //To make serialized string in correct Json format
            string s = "{^" + obj.Replace("&", ",^").Replace("=", "^:") + "}";
            s = s.Replace('^', '"').Replace("False", "false").Replace("True", "true");

            // JObject Jobj = JObject.Parse(s);


            JavaScriptSerializer js = new JavaScriptSerializer();
            var way_billModel = js.Deserialize<Way_BillModel>(s);
            E_Penaltized_Goods_Details penaltized_goods_details = new E_Penaltized_Goods_Details();
            //var way_bill_id = Convert.ToInt32(Session["way_bill_id"]);
            penaltized_goods_details.e_way_bill_id = way_billModel.way_bill_id;
            penaltized_goods_details.goods_id = way_billModel.goods_id;
            penaltized_goods_details.unit_of_measure_id = way_billModel.unit_of_measure_id;
            penaltized_goods_details.total_quantity = way_billModel.total_quantity;
            penaltized_goods_details.is_damaged = way_billModel.is_damaged;
            db.E_Penaltized_Goods_Details.Add(penaltized_goods_details);
            db.SaveChanges();
            return penaltized_goods_details.e_penaltized_goods_details_id;
        }
    }
}
