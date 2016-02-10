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
    public class Accounting_Re_VerificationController : Controller
    {

        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();
        //
        // GET: /Accounting_Re_Verification/

        public ActionResult Index(int way_bill_id = 0)
        {           
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(18))
                {
                    int page;
                    int page_no = 1;
                    var count = 0;
                    if (way_bill_id == 0)
                    {
                        count = db.Imports.Where(a => a.importing_status_id == 6).Count();
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
                    Session["accounting"] = way_bill_id;
                    if (way_bill_id != 0)
                    {
                        var way_bill = (from w in db.Way_Bill
                                        join i in db.Imports on w.import_id equals i.import_id
                                        join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                        join ist in db.Importing_Status on i.importing_status_id equals ist.importing_status_id
                                        where w.way_bill_id == way_bill_id && i.importing_status_id == 6
                                        select new Manifest_Control_SectionModel { way_bill_code = w.way_bill_code, import_name = i.import_code, ship_arrival_code = sa.ship_arrival_code, bolleto_code = i.bollete_dogonale_code, way_bill_id = w.way_bill_id }).Distinct();
                        return View(way_bill.ToList());
                    }
                    else
                    {
                        var way_bill = (from w in db.Way_Bill
                                        join i in db.Imports on w.import_id equals i.import_id
                                        join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                        join ist in db.Importing_Status on i.importing_status_id equals ist.importing_status_id
                                        where i.importing_status_id == 6
                                        orderby i.import_id descending
                                        select new Manifest_Control_SectionModel { way_bill_code = w.way_bill_code, import_name = i.import_code, ship_arrival_code = sa.ship_arrival_code, bolleto_code = i.bollete_dogonale_code, way_bill_id = w.way_bill_id }).Skip(start_from).Take(9).Distinct();
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
        // GET: /Accounting_Re_Verification/Search_Way_Bill

        public ActionResult Search_Way_Bill()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(18))
                {
                    ViewBag.way_bill = new HomeController().Way_Bill_Accounting();
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
        // POST: /Accounting_Re_Verification/Search_Way_Bill

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search_Way_Bill(Manifest_Control_SectionModel manifest)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(18))
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
        // GET: /Accounting_Re_Verification/View_Way_Bill/10

        public ActionResult Details(int way_bill_id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(18))
                {        
                    var way_bill_code = (from w in db.Way_Bill where w.way_bill_id == way_bill_id select w.way_bill_code).First();
                    TempData["way_bill_code"] = way_bill_code;
                    var grand_total = db.Get_Grand_Total(way_bill_id,106);
                    foreach (var v in grand_total)
                    {
                        ViewBag.grand_total = v;
                    }
                    var tax_calculation = db.Display_Tax_Details(way_bill_id,106);
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
        // GET: /Accounting_Re_Verification/Submit_Manifest/W0009

        public ActionResult Submit_Manifest(int way_bill_id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(18))
                {   
                    var way_bill = (from w in db.Way_Bill
                                    join i in db.Imports on w.import_id equals i.import_id
                                    where w.way_bill_id == way_bill_id
                                    select new { w.way_bill_id, i.import_id }).First();
                    Import import = db.Imports.Find(way_bill.import_id);
                    import.importing_status_id = 7;
                    db.Entry(import).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["errorMessage"] = "Accounting Submitted";
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
        // GET: /Accounting_Re_Verification/Reject_Waybill

        public ActionResult Reject_Waybill(int way_bill_id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(18))
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
        // GET: /Accounting_Re_Verification/Reject_Manifest/W0009

        public ActionResult Reject_Manifest(Reject_ReasonModel reject_reason_model)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(18))
                { 
                    var way_bill_id = Convert.ToInt32(TempData["way_bill_id"]);
                    db.Reject_Calculated_Tax(way_bill_id, 8, reject_reason_model.reason,"Accounting");
                    TempData["errorMessage"] = "Accounting Rejected";
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
    }
}