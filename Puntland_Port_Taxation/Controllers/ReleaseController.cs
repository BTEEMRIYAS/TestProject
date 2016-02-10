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
    public class ReleaseController : Controller
    {

        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

        //
        // GET: /Release/

        public ActionResult Index(int way_bill_id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(21))
                {
                    int page;
                    int page_no = 1;
                    var count = 0;
                    if (way_bill_id == 0)
                    {
                        count = db.Imports.Where(a => a.importing_status_id == 10 || a.importing_status_id == 23).Count();
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
                        var way_bill = (from w in db.Way_Bill
                                        join i in db.Imports on w.import_id equals i.import_id
                                        join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                        join ist in db.Importing_Status on i.importing_status_id equals ist.importing_status_id
                                        where w.way_bill_id == way_bill_id
                                        select new Manifest_Control_SectionModel { way_bill_code = w.way_bill_code, import_name = i.import_code, ship_arrival_code = sa.ship_arrival_code, bolleto_code = i.bollete_dogonale_code, way_bill_id = w.way_bill_id, importing_status = i.importing_status_id }).Distinct();
                        return View(way_bill.ToList());
                    }
                    else
                    {
                        var way_bill = ((from w in db.Way_Bill
                                        join i in db.Imports on w.import_id equals i.import_id
                                        join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                        join ist in db.Importing_Status on i.importing_status_id equals ist.importing_status_id
                                        where i.importing_status_id == 10 || i.importing_status_id == 23
                                        select new Manifest_Control_SectionModel { way_bill_code = w.way_bill_code, import_name = i.import_code, ship_arrival_code = sa.ship_arrival_code, bolleto_code = i.bollete_dogonale_code, way_bill_id = w.way_bill_id, importing_status = i.importing_status_id }).Distinct()).OrderByDescending( a => a.way_bill_id).Skip(start_from).Take(9);
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
        // GET: /Release/Details/W0001

        public ActionResult Details(int id)
        { 
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(21))
                {          
                    var way_bill_code = (from w in db.Way_Bill where w.way_bill_id == id select w.way_bill_code).FirstOrDefault();
                    TempData["way_bill_code"] = way_bill_code;
                    TempData["way_bill_id"] = id;
                    var release = db.Display_Release(id);
                    var penaltized_goods = (from pgd in db.Penaltized_Goods_Details
                                            join g in db.Goods on pgd.goods_id equals g.goods_id
                                            join u in db.Unit_Of_Measure on pgd.unit_of_measure_id equals u.unit_id
                                            where pgd.way_bill_id == id
                                            select new Way_BillModel { goods = g.goods_name, quantity = pgd.total_quantity, unit_of_measure = u.unit_code, is_damaged = pgd.is_damaged }).ToList();
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
        // GET: /Release/Total_Goods_View/W0001

        public ActionResult Total_Goods_View(int id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(21))
                {
                    var way_bill_code = (from w in db.Way_Bill where w.way_bill_id == id select w.way_bill_code).FirstOrDefault();
                    TempData["way_bill_code"] = way_bill_code;
                    TempData["way_bill_id"] = id;
                    var release = db.Display_Release(id);
                    var penaltized_goods = (from pgd in db.Penaltized_Goods_Details
                                            join g in db.Goods on pgd.goods_id equals g.goods_id
                                            join u in db.Unit_Of_Measure on pgd.unit_of_measure_id equals u.unit_id
                                            where pgd.way_bill_id == id
                                            select new Way_BillModel { goods = g.goods_name, quantity = pgd.goods_id, unit_of_measure = u.unit_code, is_damaged = pgd.is_damaged, penaltized_goods_details_id = pgd.penaltized_goods_details_id }).ToList();
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
        // POST: /Release/Delete/5
        public int DeleteConfirmed(int id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(21))
                {
                    Penaltized_Goods_Details penalty_goods = db.Penaltized_Goods_Details.Find(id);
                    db.Penaltized_Goods_Details.Remove(penalty_goods);
                    db.SaveChanges();
                }
            }
            return 1;
        }

        //
        // GET: /Release/Search_Way_Bill

        public ActionResult Search_Way_Bill()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(21))
                {
                    ViewBag.way_bill = new HomeController().Way_Bill_Release();
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
        // POST: /Release/Search_Way_Bill

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search_Way_Bill(Manifest_Control_SectionModel manifest)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(21))
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
        // GET: /Release/Print_Release/13

        public ActionResult Print_Release(int id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(21))
                {
                    var grand_total = db.Get_Grand_Total(id,106);
                    foreach (var v in grand_total)
                    {
                        ViewBag.grand_total = v;
                    }
                    var tax_calculation = db.Display_Tax_Details(id,106);
                    var bollete_dogonale = (from c in db.Calculated_Levi
                                            join w in db.Way_Bill on c.way_bill_id equals w.way_bill_id
                                            join i in db.Imports on w.import_id equals i.import_id
                                            join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                            where c.way_bill_id == id
                                            select new Bolleto_DogonaleModel { way_bill_code = w.way_bill_code, import_code = i.import_code, ship_arrival_code = sa.ship_arrival_code, bolleto_dogonale_code = i.bollete_dogonale_code }).Distinct();
                    ViewBag.bolleto = bollete_dogonale;
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
        // GET: /Release/Submit_Manifest/W0009

        public ActionResult Submit_Manifest(int way_bill_id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(21))
                {
                    var way_bill = (from w in db.Way_Bill
                                    join i in db.Imports on w.import_id equals i.import_id
                                    where w.way_bill_id == way_bill_id
                                    select new { w.way_bill_id, i.import_id }).First();
                    Import import = db.Imports.Find(way_bill.import_id);
                    import.importing_status_id = 11;
                    db.Entry(import).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["errorMessage"] = "Release Submitted";
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
        // GET: /Release/Reject_Waybill

        public ActionResult Reject_Waybill(int way_bill_id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(21))
                {
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

        //
        // GET: /Release/Reject_Manifest/15

        public ActionResult Reject_Manifest(int way_bill_id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(21))
                {
                    db.Reject_Calculated_Tax(way_bill_id, 18, "Goods Details Are Incorrect", "Release");            
                    var way_bill = (from w in db.Way_Bill
                                    join i in db.Imports on w.import_id equals i.import_id
                                    where w.way_bill_id == way_bill_id
                                    select i.import_id).FirstOrDefault();
                    Import import = db.Imports.Find(way_bill);
                    import.payment_id = 1;
                    db.Entry(import).State = EntityState.Modified;
                    db.SaveChanges();
                    //var payment_details = db.Payment_details.Where(pd => pd.import_id == way_bill).ToList();
                    //foreach (var item in payment_details)
                    //{
                    //    db.Payment_details.Remove(item);
                    //}
                    //db.SaveChanges();
                    TempData["errorMessage"] = "Release Rejected";
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
        // GET: /Release/Create_Way_Bill

        public ActionResult Create_Way_Bill(int way_bill_id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(21))
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
        // POST: /Manage_Way_Bill/Create_Way_Bill

        [HttpPost]
        public int Save_Penaltized_Goods(string obj)
        {
            //To make serialized string in correct Json format
            string s = "{^" + obj.Replace("&", ",^").Replace("=", "^:") + "}";
            s = s.Replace('^', '"').Replace("False", "false").Replace("True", "true");

            // JObject Jobj = JObject.Parse(s);


            JavaScriptSerializer js = new JavaScriptSerializer();
            var way_billModel = js.Deserialize<Way_BillModel>(s);
            Penaltized_Goods_Details penaltized_goods_details = new Penaltized_Goods_Details();
            //var way_bill_id = Convert.ToInt32(Session["way_bill_id"]);
            penaltized_goods_details.way_bill_id = way_billModel.way_bill_id;
            penaltized_goods_details.goods_id = way_billModel.goods_id;
            penaltized_goods_details.unit_of_measure_id = way_billModel.unit_of_measure_id;
            penaltized_goods_details.total_quantity = way_billModel.total_quantity;
            penaltized_goods_details.is_damaged = way_billModel.is_damaged;
            db.Penaltized_Goods_Details.Add(penaltized_goods_details);
            db.SaveChanges();
            return penaltized_goods_details.penaltized_goods_details_id;
        }
    }
}
