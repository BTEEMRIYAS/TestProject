using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using System.IO;
using System.Data.Objects.SqlClient;
using Puntland_Port_Taxation.Models;

namespace Puntland_Port_Taxation.Controllers
{
    public class PaymentController : Controller
    {

        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

        //
        // GET: /Payment/

        public ActionResult Index(int way_bill_id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(20))
                {
                    int page;
                    int page_no = 1;
                    var count = 0;
                    if (way_bill_id == 0)
                    {
                        count = db.Imports.Where(a => a.importing_status_id == 9 || a.importing_status_id == 22).Count();
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
                    Session["payment"] = way_bill_id;
                    var payment_details = (db.Display_Payment(way_bill_id)).OrderByDescending( p => p.way_bill_id).Skip(start_from).Take(9);
                    return View(payment_details.ToList());
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
        // GET: /Payment/Search_Way_Bill

        public ActionResult Search_Way_Bill()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(20))
                {
                    ViewBag.way_bill = new HomeController().Way_Bill_payment();
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
        // POST: /Payment/Search_Way_Bill

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search_Way_Bill(Manifest_Control_SectionModel manifest)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(20))
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

        public ActionResult Submit_Payment(int way_bill_id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(20))
                {
                    var grand_total_value = "";
                    TempData["way_bill_id"] = way_bill_id;
                    ViewBag.currency = new HomeController().Sos_Usd_Currency();
                    ViewBag.payment_mode = new HomeController().Payment_Mode();
                    var grand_total = db.Get_Grand_Total(way_bill_id, 106);
                    foreach(var v in grand_total)
                    {
                        grand_total_value = v;
                    }
                    Payment_details payment_deatils = new Payment_details();
                    payment_deatils.amount_tobe_paid = grand_total_value;
                    payment_deatils.currency_id_tobe_paid = 106;
                    var payment_parts = db.Calculated_Payment_Config.Where(cpc => cpc.way_bill_id == way_bill_id).ToList();
                    ViewBag.payment_partition = payment_parts;
                    foreach (var item in payment_parts)
                    {
                        payment_deatils.sos_part_total = item.calculated_sos_amount;
                        payment_deatils.usd_part_total = item.calculated_usd_amount;
                        TempData["sos"] = item.calculated_sos_part;
                        TempData["usd"] = item.calculated_usd_part;
                    }
                    return View(payment_deatils);
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
        // GET: /Payment/Submit_Manifest/W0009

        public ActionResult Submit_Manifest(Payment_details payment_details, int way_bill_id, decimal sos, decimal usd)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(20))
                {
                    if (((payment_details.sos_by_cash + payment_details.sos_by_cheque) != sos) || ((payment_details.usd_by_cash + payment_details.usd_by_cheque) != usd))
                    {
                        TempData["errorMessage"] = "Payment Failed";
                        return RedirectToAction("Index");
                    }
                    else
                    {                      
                        var import_id = (from i in db.Imports
                                         where i.way_bill_id == way_bill_id
                                         select i.import_id).First();
                        var payment_details_old = db.Payment_details.Where(pd => pd.import_id == import_id).ToList();
                        foreach (var item in payment_details_old)
                        {
                            db.Payment_details.Remove(item);
                        }
                        db.SaveChanges();
                        payment_details.paid_date = DateTime.Now;
                        payment_details.import_id = import_id;
                        db.Payment_details.Add(payment_details);
                        db.SaveChanges();
                        Import import = db.Imports.Find(import_id);
                        import.importing_status_id = 10;
                        import.payment_id = 2;
                        db.Entry(import).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["errorMessage_payment"] = "Payment Submitted";
                        TempData["way_bill_id"] = way_bill_id;
                        //return RedirectToAction("Home_Print_Bolleto", "Home", new { currency_id = 100, way_bill_id = way_bill_id });
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

        public ActionResult Submit_Penalty_Payment(int way_bill_id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(20))
                {
                    var grand_total_value = "";
                    TempData["way_bill_id"] = way_bill_id;
                    ViewBag.currency = new HomeController().Sos_Usd_Currency();
                    ViewBag.payment_mode = new HomeController().Payment_Mode();
                    var grand_total = db.Get_Penalty_Grand_Total(way_bill_id, 106);
                    foreach (var v in grand_total)
                    {
                        grand_total_value = v.Grand_total;
                    }
                    Payment_Penalty_details payment_penalty_deatils = new Payment_Penalty_details();
                    payment_penalty_deatils.amount_tobe_paid = grand_total_value;
                    payment_penalty_deatils.currency_id_tobe_paid = 106;
                    var payment_parts = db.Calculated_Penalty_Config.Where(cpc => cpc.way_bill_id == way_bill_id).ToList();
                    ViewBag.payment_partition = payment_parts;
                    foreach (var item in payment_parts)
                    {
                        payment_penalty_deatils.sos_part_total = item.calculated_sos_amount;
                        payment_penalty_deatils.usd_part_total = item.calculated_usd_amount;
                        TempData["sos"] = item.calculated_sos_part;
                        TempData["usd"] = item.calculated_usd_part;
                    }
                    return View(payment_penalty_deatils);
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
        // GET: /Payment/Submit_Manifest/W0009

        public ActionResult Submit_Penalty_Manifest(Payment_Penalty_details payment_penalty_deatils, int way_bill_id, decimal sos, decimal usd)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(20))
                {
                    if (((payment_penalty_deatils.sos_by_cash + payment_penalty_deatils.sos_by_cheque) != sos) || ((payment_penalty_deatils.usd_by_cash + payment_penalty_deatils.usd_by_cheque) != usd))
                    {
                        TempData["errorMessage"] = "Tax And Fine Payment Failed";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        var import_id = (from i in db.Imports
                                         where i.way_bill_id == way_bill_id
                                         select i.import_id).First();
                        var payment_details_old = db.Payment_Penalty_details.Where(pd => pd.import_id == import_id).ToList();
                        foreach (var item in payment_details_old)
                        {
                            db.Payment_Penalty_details.Remove(item);
                        }
                        db.SaveChanges();
                        payment_penalty_deatils.paid_date = DateTime.Now;
                        payment_penalty_deatils.import_id = import_id;
                        db.Payment_Penalty_details.Add(payment_penalty_deatils);
                        db.SaveChanges();
                        Import import = db.Imports.Find(import_id);
                        import.importing_status_id = 23;
                        import.payment_id = 2;
                        db.Entry(import).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["errorMessage_payment"] = "Tax And Fine Payment Submitted";
                        TempData["way_bill_id"] = way_bill_id;
                        //return RedirectToAction("Home_Print_Bolleto", "Home", new { currency_id = 100, way_bill_id = way_bill_id });
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

        public ActionResult Payment_Print_Receipt(int currency_id, int way_bill_id)
        {
            if (Session["login_status"] != null)
            {
                var prev_url = Request.UrlReferrer.ToString();
                if (prev_url.Contains("Payment"))
                {
                    TempData["payment"] = "true";
                }
                var grand_total = db.Get_Grand_Total(way_bill_id, currency_id);
                foreach (var v in grand_total)
                {
                    ViewBag.grand_total = v;
                }
                db.Display_Tax_Details(way_bill_id, currency_id);
                var display1 = from d1 in db.TempDisplay1
                               where d1.way_bill_id == way_bill_id
                               select d1;
                var display2 = (from d2 in db.TempDisplay2
                                where d2.way_bill_id == way_bill_id
                                select d2).ToList();
                ViewBag.display2 = display2;
                var bollete_dogonale = (from c in db.Calculated_Levi
                                        join w in db.Way_Bill on c.way_bill_id equals w.way_bill_id
                                        join i in db.Imports on w.import_id equals i.import_id
                                        join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                        join cpc in db.Calculated_Payment_Config on w.way_bill_id equals cpc.way_bill_id
                                        where c.way_bill_id == way_bill_id
                                        select new Bolleto_DogonaleModel { way_bill_code = w.way_bill_code, import_code = i.import_code, ship_arrival_code = sa.ship_arrival_code, bolleto_dogonale_code = i.bollete_dogonale_code, date = cpc.calculated_date, mark = w.mark }).Distinct();
                ViewBag.bolleto = bollete_dogonale;
                var get_payment_config = db.Calculated_Payment_Config.Where(cpc => cpc.way_bill_id == way_bill_id).ToList();
                var employee = from cpc in db.Calculated_Payment_Config
                               join e in db.Employees on cpc.employee_id equals e.employee_id
                               where cpc.way_bill_id == way_bill_id
                               select new { employee = e.first_name + " " + e.middle_name + " " + e.last_name };
                foreach (var item in employee)
                {
                    TempData["employee"] = item.employee;
                }                 
                foreach (var item in get_payment_config)
                {
                    TempData["sos_amount"] = item.calculated_sos_amount;
                    TempData["usd_amount"] = item.calculated_usd_amount;
                    TempData["sos"] = item.calculated_sos_part;
                    TempData["usd"] = item.calculated_usd_part;
                }
                return View(display1.ToList());
            }
            else
            {
                return RedirectToAction("../Home");
            }
        }

        public ActionResult Payment_Print_Penalty_Receipt(int currency_id, int way_bill_id)
        {
            if (Session["login_status"] != null)
            {
                var prev_url = Request.UrlReferrer.ToString();
                if (prev_url.Contains("Payment"))
                {
                    TempData["payment"] = "true";
                }
                var grand_total = db.Get_Penalty_Grand_Total(way_bill_id, currency_id);
                foreach (var v in grand_total)
                {
                    ViewBag.grand_total = v.Grand_total;
                    ViewBag.Penalty = v.Penalty;
                    ViewBag.total = v.total;
                    ViewBag.Total_Penalty = v.Total_Penalty;
                }
                db.Display_Penalty_Details(way_bill_id, currency_id);
                var display1 = from d1 in db.Temp_Penalty_Display1
                               where d1.way_bill_id == way_bill_id
                               select d1;
                var display2 = (from d2 in db.Temp_penalty_Display2
                                where d2.way_bill_id == way_bill_id
                                select d2).ToList();
                ViewBag.display2 = display2;
                var bollete_dogonale = (from c in db.Calculated_Penalty
                                        join w in db.Way_Bill on c.way_bill_id equals w.way_bill_id
                                        join i in db.Imports on w.import_id equals i.import_id
                                        join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                        join cpc in db.Calculated_Penalty_Config on w.way_bill_id equals cpc.way_bill_id
                                        where c.way_bill_id == way_bill_id
                                        select new Bolleto_DogonaleModel { way_bill_code = w.way_bill_code, import_code = i.import_code, ship_arrival_code = sa.ship_arrival_code, bolleto_dogonale_code = i.bollete_dogonale_code, date = cpc.calculated_date, mark = w.mark }).Distinct();
                ViewBag.bolleto = bollete_dogonale;
                var get_payment_config = db.Calculated_Penalty_Config.Where(cpc => cpc.way_bill_id == way_bill_id).ToList();
                var employee = from cpc in db.Calculated_Penalty_Config
                               join e in db.Employees on cpc.employee_id equals e.employee_id
                               where cpc.way_bill_id == way_bill_id
                               select new { employee = e.first_name + " " + e.middle_name + " " + e.last_name };
                foreach (var item in employee)
                {
                    TempData["employee"] = item.employee;
                }                  
                foreach (var item in get_payment_config)
                {
                    TempData["sos_amount"] = item.calculated_sos_amount;
                    TempData["usd_amount"] = item.calculated_usd_amount;
                    TempData["sos"] = item.calculated_sos_part;
                    TempData["usd"] = item.calculated_usd_part;
                }
                return View(display1.ToList());
            }
            else
            {
                return RedirectToAction("../Home");
            }
        }

        public ActionResult Error_Message(string error_message, int way_bill_id)
        {
            TempData["errorMessage_payment"] = error_message;
            TempData["way_bill_id"] = way_bill_id;
            return View();
        }

        [HttpPost]
        public ActionResult Get_Grand_Total(int id, int way_bill_id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(20))
                {
                    var grand_total = "";
                    var grand_total_value = db.Get_Grand_Total(way_bill_id, id);
                    foreach (var v in grand_total_value)
                    {
                        grand_total = v;
                    }
                    return Json(grand_total);
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
        public ActionResult Get_Penalty_Grand_Total(int id, int way_bill_id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(20))
                {
                    var grand_total = "";
                    var grand_total_value = db.Get_Penalty_Grand_Total(way_bill_id, id);
                    foreach (var v in grand_total_value)
                    {
                        grand_total = v.Grand_total;
                    }
                    return Json(grand_total);
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

