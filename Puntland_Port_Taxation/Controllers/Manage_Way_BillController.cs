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
    public class Manage_Way_BillController : Controller
    {
        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

        //
        // GET: /Manage_Way_Bill/

        public ActionResult Index()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(13))
                {
                    int page;
                    int page_no = 1;
                    var count = db.Way_Bill.Count();
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
                    var way_bill = (from w in db.Way_Bill
                                    join i in db.Imports on w.import_id equals i.import_id
                                    join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                    select new Way_BillModel { way_bill_code = w.way_bill_code, import_name = i.import_code, ship_arrival_code = sa.ship_arrival_code, import_status_id = i.importing_status_id, way_bill_id = w.way_bill_id, mark = w.mark }).OrderByDescending( wy => wy.way_bill_id).Skip(start_from).Take(9);
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
        // GET: /Manage_Way_Bill/Details/W0001

        public ActionResult Details(int way_bill_id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(13) || z.Contains(24))
                {
                    int page;
                    int page_no = 1;
                    var count = db.Way_Bill_Details.Where( w => w.way_bill_id == way_bill_id).Count();
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
                    var way_bill = (from w in db.Way_Bill
                                    join wd in db.Way_Bill_Details on w.way_bill_id equals wd.way_bill_id
                                    join imps in db.Imports on w.import_id equals imps.import_id
                                    join i in db.Importers on w.importer_id equals i.importer_id
                                    join g in db.Goods on wd.goods_id equals g.goods_id
                                    join u in db.Unit_Of_Measure on wd.unit_of_measure_id equals u.unit_id
                                    where w.way_bill_id == way_bill_id
                                    select new Way_BillModel { way_bill_id = w.way_bill_id, importer_name = i.importer_first_name + " " + i.importer_middle_name + " " + i.importer_last_name, way_bill_code = w.way_bill_code, way_bill_name = w.way_bill_name, goods = g.goods_name, unit_of_measure = u.unit_code, total_quantity = wd.total_quantity, import_status_id = imps.importing_status_id, goods_id = wd.goods_id, way_bill_details_id = wd.way_bill_details_id, is_damaged = wd.is_damaged }).OrderBy( a => a.way_bill_details_id).Skip(start_from).Take(9);
                    var way_bill_code = (from w in db.Way_Bill where w.way_bill_id == way_bill_id select w.way_bill_code).First();
                    Session["way_bill_code"] = way_bill_code;
                    Session["way_bill_id"] = way_bill_id;
                    TempData["importer"] = (from w in db.Way_Bill
                                           join i in db.Importers on w.importer_id equals i.importer_id
                                            where w.way_bill_id == way_bill_id
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
        // GET: /Manage_Way_Bill/View_Way_Bill/W0001

        public ActionResult View_Way_Bill(int id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(13))
                {
                    var way_bill = (from w in db.Way_Bill
                                    join wd in db.Way_Bill_Details on w.way_bill_id equals wd.way_bill_id
                                    join i in db.Importers on w.importer_id equals i.importer_id
                                    join g in db.Goods on wd.goods_id equals g.goods_id
                                    join u in db.Unit_Of_Measure on wd.unit_of_measure_id equals u.unit_id
                                    where wd.way_bill_id == id
                                    select new Way_BillModel { goods = g.goods_name, unit_of_measure = u.unit_code, total_quantity = wd.total_quantity, importer_name = i.importer_first_name + " " + i.importer_middle_name + " " + i.importer_last_name, is_damaged = wd.is_damaged });            
                    var way_biil_code = (from w in db.Way_Bill where w.way_bill_id == id select w.way_bill_code).FirstOrDefault();
                    ViewBag.way_bill_code = way_biil_code;
                    TempData["importer"] = (from w in db.Way_Bill
                                            join i in db.Imports on w.import_id equals i.import_id
                                            join ir in db.Importers on i.importer_id equals ir.importer_id
                                            where w.way_bill_id == id
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
        // GET: /Manage_Way_Bill/Create

        public ActionResult Create()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(13))
                {
                    ViewBag.import = new HomeController().Import();
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
        // POST: /Manage_Way_Bill/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Way_Bill way_bill)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(13))
                {
                    var importer_id = 0;
                    var ship_arrival_id = 0;
                    var importer = from i in db.Imports
                                   join ir in db.Importers on i.importer_id equals ir.importer_id
                                   join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                   where i.import_id == way_bill.import_id
                                   select new { ir.importer_id, sa.ship_arrival_id };
                    foreach (var item in importer)
                    {
                        importer_id = item.importer_id;
                        ship_arrival_id = item.ship_arrival_id;
                    }
                    var way_bill_exist = (from w in db.Way_Bill
                                          join i in db.Imports on w.import_id equals i.import_id
                                          join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                          where w.way_bill_code == way_bill.way_bill_code && sa.ship_arrival_id == ship_arrival_id
                                          select w).Count();
                    var import_exist = (from w in db.Way_Bill
                                        where w.import_id == way_bill.import_id
                                        select w).Count();
                    if (import_exist > 0)
                    {
                        TempData["errorMessage"] = "Only One Way Bill Allowed For an Import, Please Choose Another Import";
                        return RedirectToAction("Index");
                    }
                    else if (way_bill_exist > 0)
                    {
                        TempData["errorMessage"] = "Way Bill Already Exists For This Ship Arrival,Please Enter Another One";
                        return RedirectToAction("Index");
                    }
                    if (ModelState.IsValid)
                    {
                        way_bill.importer_id = importer_id;
                        way_bill.way_bill_name = "100";
                        way_bill.is_calculated = false;
                        db.Way_Bill.Add(way_bill);
                        db.SaveChanges();
                        way_bill.way_bill_name = way_bill.way_bill_name + way_bill.way_bill_id;
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
        // GET: /Manage_Way_Bill/Create_Way_Bill

        public ActionResult Create_Way_Bill()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(13) || z.Contains(24))
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

        //Manage_Way_Bill/way_bill_already

        public int way_bill_already()
        {
            return 0;
        }


        //To add goods using add next in create way bill page
        // POST: /Manage_Way_Bill/Create_Way_Bill

        [HttpPost]
        public int Create_Way_Bill_new(string obj)
        {
            //To make serialized string in correct Json format
            string s = "{^"+obj.Replace("&", ",^").Replace("=", "^:")+"}";
            s = s.Replace('^', '"').Replace("False","false").Replace("True","true");

           // JObject Jobj = JObject.Parse(s);
           
           
            JavaScriptSerializer js = new JavaScriptSerializer();
            var way_billModel = js.Deserialize<Way_BillModel>(s);
            Way_Bill_Details way_bill_details = new Way_Bill_Details();
            var way_bill_id = Convert.ToInt32(Session["way_bill_id"]);
            way_bill_details.way_bill_id = way_bill_id;
            way_bill_details.goods_id = way_billModel.goods_id;
            way_bill_details.unit_of_measure_id = way_billModel.unit_of_measure_id;
            way_bill_details.total_quantity = way_billModel.total_quantity;
            way_bill_details.is_damaged = way_billModel.is_damaged;
            db.Way_Bill_Details.Add(way_bill_details);
            db.SaveChanges();
            return 1;
        }

        [HttpPost]
        public ActionResult Create_Way_Bill(Way_BillModel way_billModel)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(13) || z.Contains(24))
                {
                    Way_Bill_Details way_bill_details = new Way_Bill_Details();
                    var way_bill_id = Convert.ToInt32(Session["way_bill_id"]);
                    if (ModelState.IsValid)
                    {
                        way_bill_details.way_bill_id = way_bill_id;
                        way_bill_details.goods_id = way_billModel.goods_id;
                        way_bill_details.unit_of_measure_id = way_billModel.unit_of_measure_id;
                        way_bill_details.total_quantity = way_billModel.total_quantity;
                        way_bill_details.is_damaged = way_billModel.is_damaged;
                        db.Way_Bill_Details.Add(way_bill_details);
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
        // GET: /Manage_Way_Bill/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(13) || z.Contains(24))
                {
                    Way_Bill_Details way_bill_details = db.Way_Bill_Details.Find(id);
                    Way_BillModel way_billModel = new Way_BillModel();
                    if (way_bill_details == null)
                    {
                        return HttpNotFound();
                    }
                    var import_code = from w in db.Way_Bill
                                      join wd in db.Way_Bill_Details on w.way_bill_id equals wd.way_bill_id
                                      join i in db.Imports on w.import_id equals i.import_id
                                      where wd.way_bill_details_id == way_bill_details.way_bill_details_id
                                      select new { i.import_id, i.import_code };
                    ViewBag.import = new SelectList(import_code, "import_id", "import_code");
                    var way_bill_master_details = (from w in db.Way_Bill
                                                   where w.way_bill_id == way_bill_details.way_bill_id
                                                   select new { w.way_bill_name, w.import_id, w.importer_id, w.is_calculated, w.way_bill_code, w.way_bill_id, w.mark }).First();
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
                    TempData["id"] = way_bill_details.way_bill_details_id;
                    TempData["way_bill_id"] = way_bill_details.way_bill_id;
                    ViewBag.categories = new HomeController().Category();
                    ViewBag.currency = new HomeController().Currency();
                    way_billModel.way_bill_code = way_bill_master_details.way_bill_code;
                    way_billModel.import_id = way_bill_master_details.import_id;
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
        // POST: /Manage_Way_Bill/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Way_BillModel way_billModel)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(13) || z.Contains(24))
                {
                    Way_Bill_Details way_bill_details = new Way_Bill_Details();
                    int id = Convert.ToInt32(TempData["id"]);
                    if (ModelState.IsValid)
                    {
                        way_bill_details.way_bill_details_id = id;
                        way_bill_details.way_bill_id = Convert.ToInt32(TempData["way_bill_id"]);
                        way_bill_details.goods_id = way_billModel.goods_id;
                        way_bill_details.unit_of_measure_id = way_billModel.unit_of_measure_id;
                        way_bill_details.total_quantity = way_billModel.total_quantity;
                        way_bill_details.currency_id = way_billModel.currency_id;
                        way_bill_details.is_damaged = way_billModel.is_damaged;
                        db.Entry(way_bill_details).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Details", new { way_bill_id = way_bill_details.way_bill_id });
                    }
                    return RedirectToAction("Details", new { way_bill_id = way_bill_details.way_bill_id });
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
        // GET: /Manage_Way_Bill/Submit_Way_Bill/W0009

        public ActionResult Submit_Way_Bill()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(13) || z.Contains(24))
                {
                    var way_bill_code = Session["way_bill_code"].ToString();
                    var way_bill_id = Convert.ToInt32(Session["way_bill_id"]);
                    var way_bill = (from w in db.Way_Bill
                                    join i in db.Imports on w.import_id equals i.import_id
                                     where w.way_bill_id == way_bill_id
                                    select new { w.way_bill_id, i.import_id, i.importing_status_id }).FirstOrDefault();
                    Import import = db.Imports.Find(way_bill.import_id);
                    import.way_bill_id = way_bill_id;
                    var imp_status = way_bill.importing_status_id;
                    if (way_bill.importing_status_id == 1 || way_bill.importing_status_id == 2 || way_bill.importing_status_id == 15)
                    {
                        import.importing_status_id = 2;
                    }
                    else if (way_bill.importing_status_id == 13 || way_bill.importing_status_id == 17)
                    {
                        import.importing_status_id = 14;
                    }
                    db.Entry(import).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["errorMessage"] = "Way Bill Submitted";
                    if (imp_status == 2 || imp_status == 14 || imp_status == 15)
                    {
                        return RedirectToAction("Index", "Examination_Unit");
                    }
                    else
                    {
                        return RedirectToAction("Index");
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
        // GET: /Manage_Way_Bill/Reject_Reason_View/5

        public ActionResult Reject_Reason_View(int way_bill_id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(13))
                {
                    var reason = (from r in db.Reject_Reason
                                 join w in db.Way_Bill on r.way_bill_id equals w.way_bill_id
                                 join i in db.Imports on r.way_bill_id equals i.way_bill_id
                                 join eu in db.EU_Check on r.way_bill_id equals eu.way_bill_id into eua
                                 from k in eua.DefaultIfEmpty()
                                 where r.way_bill_id == way_bill_id
                                 orderby r.reject_reason_id descending, k.eu_check_id descending
                                 select new Reject_ReasonModel { reject_reason_id = r.reject_reason_id, way_bill_id = r.way_bill_id, way_bill_code = w.way_bill_code, reason = r.reject_reason, rejected_from = r.reject_from, rejected_date = r.rejected_date, rechecked_by = k.eu_checked_by, reject_number = k.reject_number, import_status = i.importing_status_id });
                    return View(reason.ToList());
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
        // GET: /Manage_Way_Bill/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(13) || z.Contains(24))
                {
                    Way_Bill_Details way_bill_details = db.Way_Bill_Details.Find(id);
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
        // POST: /Manage_Way_Bill/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(13) || z.Contains(24))
                {
                    var way_bill_id = Convert.ToInt32(Session["way_bill_id"]);
                    Way_Bill_Details way_bill_details = db.Way_Bill_Details.Find(id);
                    db.Way_Bill_Details.Remove(way_bill_details);
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
        // GET: /Manage_Way_Bill/DbSearch

        public ActionResult DbSearch()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(13))
                {
                    ViewBag.import = new HomeController().Import();
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
        // POST: /Manage_Way_Bill/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Way_BillModel way_bill)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(13))
                {
                    //Queue q = new Queue();
                    if ( way_bill.way_bill_code != null && way_bill.import_code != null && way_bill.ship_arrival_code != null)
                    {
                        var result = (from w in db.Way_Bill
                                      join i in db.Imports on w.import_id equals i.import_id
                                      join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                      where w.way_bill_code.Contains(way_bill.way_bill_code) && i.import_code.Contains(way_bill.import_code) && sa.ship_arrival_code.Contains(way_bill.ship_arrival_code)
                                      select new Way_BillModel { way_bill_code = w.way_bill_code, import_name = i.import_code, ship_arrival_code = sa.ship_arrival_code, import_status_id = i.importing_status_id, way_bill_id = w.way_bill_id, mark = w.mark });
                        return View("Index", result.ToList());
                    }
                    else if (way_bill.way_bill_code != null && way_bill.import_code != null && way_bill.ship_arrival_code == null)
                    {
                        var result = (from w in db.Way_Bill
                                      join i in db.Imports on w.import_id equals i.import_id
                                      join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                      where w.way_bill_code.Contains(way_bill.way_bill_code) && i.import_code.Contains(way_bill.import_code)
                                      select new Way_BillModel { way_bill_code = w.way_bill_code, import_name = i.import_code, ship_arrival_code = sa.ship_arrival_code, import_status_id = i.importing_status_id, way_bill_id = w.way_bill_id, mark = w.mark });
                        return View("Index", result.ToList());
                    }
                    else if (way_bill.way_bill_code != null && way_bill.import_code == null && way_bill.ship_arrival_code != null)
                    {
                        var result = (from w in db.Way_Bill
                                      join i in db.Imports on w.import_id equals i.import_id
                                      join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                      where w.way_bill_code.Contains(way_bill.way_bill_code) && sa.ship_arrival_code.Contains(way_bill.ship_arrival_code)
                                      select new Way_BillModel { way_bill_code = w.way_bill_code, import_name = i.import_code, ship_arrival_code = sa.ship_arrival_code, import_status_id = i.importing_status_id, way_bill_id = w.way_bill_id, mark = w.mark });
                        return View("Index", result.ToList());
                    }
                    else if (way_bill.way_bill_code != null && way_bill.import_code == null && way_bill.ship_arrival_code == null)
                    {
                        var result = (from w in db.Way_Bill
                                      join i in db.Imports on w.import_id equals i.import_id
                                      join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                      where w.way_bill_code.Contains(way_bill.way_bill_code)
                                      select new Way_BillModel { way_bill_code = w.way_bill_code, import_name = i.import_code, ship_arrival_code = sa.ship_arrival_code, import_status_id = i.importing_status_id, way_bill_id = w.way_bill_id, mark = w.mark });
                        return View("Index", result.ToList());
                    }
                    else if (way_bill.way_bill_code == null && way_bill.import_code != null && way_bill.ship_arrival_code != null)
                    {
                        var result = (from w in db.Way_Bill
                                      join i in db.Imports on w.import_id equals i.import_id
                                      join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                      where i.import_code.Contains(way_bill.import_code) && sa.ship_arrival_code.Contains(way_bill.ship_arrival_code)
                                      select new Way_BillModel { way_bill_code = w.way_bill_code, import_name = i.import_code, ship_arrival_code = sa.ship_arrival_code, import_status_id = i.importing_status_id, way_bill_id = w.way_bill_id, mark = w.mark });
                        return View("Index", result.ToList());
                    }
                    else if (way_bill.way_bill_code == null && way_bill.import_code != null && way_bill.ship_arrival_code == null)
                    {
                        var result = (from w in db.Way_Bill
                                      join i in db.Imports on w.import_id equals i.import_id
                                      join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                      where i.import_code.Contains(way_bill.import_code)
                                      select new Way_BillModel { way_bill_code = w.way_bill_code, import_name = i.import_code, ship_arrival_code = sa.ship_arrival_code, import_status_id = i.importing_status_id, way_bill_id = w.way_bill_id, mark = w.mark });
                        return View("Index", result.ToList());
                    }
                    else if (way_bill.way_bill_code == null && way_bill.import_code == null && way_bill.ship_arrival_code != null)
                    {
                        var result = (from w in db.Way_Bill
                                      join i in db.Imports on w.import_id equals i.import_id
                                      join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                      where sa.ship_arrival_code.Contains(way_bill.ship_arrival_code)
                                      select new Way_BillModel { way_bill_code = w.way_bill_code, import_name = i.import_code, ship_arrival_code = sa.ship_arrival_code, import_status_id = i.importing_status_id, way_bill_id = w.way_bill_id, mark = w.mark });
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
        // GET: /Manage_Way_Bill/DbSearch_new

        public ActionResult DbSearch_new()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(13) || z.Contains(24))
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

        //
        // POST: /Manage_Way_Bill/DbSearchresult_new
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult_new(Way_BillModel way_bill)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(13) || z.Contains(24))
                {
                    var way_bill_id = Convert.ToInt32(Session["way_bill_id"]);
                    if (way_bill.goods != null)
                    {
                        var result = (from w in db.Way_Bill
                                      join wd in db.Way_Bill_Details on w.way_bill_id equals wd.way_bill_id
                                      join imps in db.Imports on w.import_id equals imps.import_id
                                      join i in db.Importers on w.importer_id equals i.importer_id
                                      join g in db.Goods on wd.goods_id equals g.goods_id
                                      join u in db.Unit_Of_Measure on wd.unit_of_measure_id equals u.unit_id
                                      where g.goods_name.Contains(way_bill.goods) && wd.way_bill_id == way_bill_id
                                      select new Way_BillModel { way_bill_id = w.way_bill_id, importer_name = i.importer_first_name, way_bill_code = w.way_bill_code, way_bill_name = w.way_bill_name, goods = g.goods_name, unit_of_measure = u.unit_code, total_quantity = wd.total_quantity, import_status_id = imps.importing_status_id, goods_id = wd.goods_id, way_bill_details_id = wd.way_bill_details_id, is_damaged = wd.is_damaged });
                        TempData["importer"] = (from w in db.Way_Bill
                                                join i in db.Importers on w.importer_id equals i.importer_id
                                                where w.way_bill_id == way_bill_id
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