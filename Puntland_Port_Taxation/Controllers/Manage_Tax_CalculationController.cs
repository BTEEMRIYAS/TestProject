using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using FlexCel.Core;
using FlexCel.XlsAdapter;
using Puntland_Port_Taxation.Models;

namespace Puntland_Port_Taxation.Controllers
{
    public class Manage_Tax_CalculationController : Controller
    {
        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

        //
        // GET: /Manage_Tax_Calculation/

        public ActionResult Index()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                var end_date = Convert.ToDateTime("9999-12-31");
                var count = 0;
                if (z.Contains(14) || z.Contains(26))
                {
                    int page;
                    int page_no = 1;
                    var user_id = Convert.ToInt32(Session["user_id"]);
                    if (z.Contains(26) && !z.Contains(14))
                    {
                        count = (from i in db.Imports
                                join aw in db.Assign_Way_Bill on i.way_bill_id equals aw.way_bill_id
                                where (aw.status == false || i.importing_status_id == 18) && aw.user_id == user_id select i).Count();
                    }
                    else
                    {
                        count = db.Imports.Where(i => i.importing_status_id == 6 || i.importing_status_id == 7 || i.importing_status_id == 8 || i.importing_status_id == 9 || i.importing_status_id == 10 || i.importing_status_id == 12 || i.importing_status_id == 3 || i.importing_status_id == 18 || i.importing_status_id == 19 || i.importing_status_id == 21 || i.importing_status_id == 22).Count();
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
                    if (z.Contains(26) && !z.Contains(14))
                    {
                        var tax_calculation = ((from w in db.Way_Bill
                                                join aw in db.Assign_Way_Bill on w.way_bill_id equals aw.way_bill_id
                                                join i in db.Imports on w.import_id equals i.import_id
                                                join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                                join ist in db.Importing_Status on i.importing_status_id equals ist.importing_status_id
                                                where aw.user_id == user_id && ((i.importing_status_id == 21 && aw.status == false) || (i.importing_status_id == 19 && aw.status == false) || (i.importing_status_id == 18))
                                                select new Tax_CalculationModel { way_bill_code = w.way_bill_code, import_code = i.import_code, ship_arrival_code = sa.ship_arrival_code, importing_status = ist.importing_status, importing_status_id = i.importing_status_id, way_bill_id = w.way_bill_id }).Distinct()).OrderByDescending(a => a.way_bill_id).Skip(start_from).Take(9);
                        return View(tax_calculation.ToList());
                    }
                    else
                    {
                        var tax_calculation = ((from w in db.Way_Bill
                                                join i in db.Imports on w.import_id equals i.import_id
                                                join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                                join ist in db.Importing_Status on i.importing_status_id equals ist.importing_status_id
                                                join sub in (from aw in db.Assign_Way_Bill
                                                             join u in db.Users on aw.user_id equals u.user_id
                                                             join e in db.Employees on u.employee_id equals e.employee_id
                                                             where aw.end_date == end_date
                                                             select new { way_bill_id = aw.way_bill_id, username = e.first_name + " " + e.middle_name + " " + e.last_name }) on w.way_bill_id equals sub.way_bill_id into x
                                                from l in x.DefaultIfEmpty()
                                                where i.importing_status_id == 6 || i.importing_status_id == 7 || i.importing_status_id == 8 || i.importing_status_id == 9 || i.importing_status_id == 10 || i.importing_status_id == 12 || i.importing_status_id == 3 || i.importing_status_id == 18 || i.importing_status_id == 19 || i.importing_status_id == 21 || i.importing_status_id == 22
                                                select new Tax_CalculationModel { way_bill_code = w.way_bill_code, import_code = i.import_code, ship_arrival_code = sa.ship_arrival_code, importing_status = ist.importing_status, importing_status_id = i.importing_status_id, way_bill_id = w.way_bill_id, assigned_to = l.username }).Distinct()).OrderByDescending(a => a.way_bill_id).Skip(start_from).Take(9);
                        return View(tax_calculation.ToList());
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

        [HttpPost]
        public ActionResult SaveFile()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(14) || z.Contains(26))
                {
                    int updated_file_count = 0;
                    HttpPostedFileBase file = Request.Files.Get("exchangefile");
                    var files = Request.Files["exchangefile"];
                    if (files != null)
                    {
                        FileInfo fil = new FileInfo(files.FileName);
                        var filename = DateTime.Now.Ticks + fil.Extension;
                        string path = Server.MapPath("~/App_Data/" + filename);
                        files.SaveAs(path);
                        updated_file_count = SaveExcel(path);
                        System.IO.File.Delete(path);

                        if (!updated_file_count.Equals(0))
                            TempData["errorMessage"] = updated_file_count + " Data Updated Successfully";

                        else
                            TempData["errorMessage"] = "Data Updating Failed";
                    }
                    else
                    {
                        TempData["errorMessage"] = "Data Updating Failed !";
                    }

                    return RedirectToAction("Index", "Manage_Tax_Calculation");

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

        public int GetCurrencyId(string code)
        {
            var currency_id = (from d in db.Currencies
                               where d.currency_code == code
                               select d.currency_id);

            if (currency_id.Count() > 0)
                return currency_id.First();

            else return 0;

        }
        public string GetDateTime(string val)
        {
            string retString = string.Empty;
            try
            {
                if (CheckDouble(val).Equals(1))
                    retString = DateTime.FromOADate(double.Parse(val)).ToString("yyyy/MM/dd");

                else
                {
                    string[] date = val.Split('/');
                    retString = date[2] + "/" + date[0] + "/" + date[1];
                }

                return retString;
            }
            catch
            {
                return "0";
            }
        }
        public int CheckDouble(string val)
        {
            try
            {
                double samp = double.Parse(val);
                return 1;
            }
            catch { return 0; }
        }
        public decimal CheckDecimal(string val)
        {
            try
            {
                decimal samp = decimal.Parse(val);
                return samp;
            }
            catch { return 0; }
        }
        public int SaveExcel(string path)
        {
            XlsFile myFile = new XlsFile(path);
            myFile.ActiveSheet = 1;

            int rowCount = myFile.RowCount;
            int colCount = myFile.ColCount;
            //get from excel sheets
            
            int from_currency_id_ = 0;
            int to_currency_id_ = 0;
            decimal newrate = 0;
            string yyyymmdd = string.Empty;
            int updated_record_count = 0;

            for (int i = 2; i <= rowCount; i++)
            {
                //for (int j = 1; j <= colCount; j++)
                //{
                if (myFile.GetCellValue(i, 1) == null || myFile.GetCellValue(i, 2) == null || myFile.GetCellValue(i, 3) == null || myFile.GetCellValue(i, 4) == null || myFile.GetCellValue(i, 5) == null || myFile.GetCellValue(i, 6) == null)
                {
                    continue;
                }
                from_currency_id_ = GetCurrencyId(myFile.GetCellValue(i, 1).ToString());
                to_currency_id_ = GetCurrencyId(myFile.GetCellValue(i, 3).ToString());
                newrate = CheckDecimal(myFile.GetCellValue(i, 5).ToString());
                yyyymmdd = GetDateTime(myFile.GetCellValue(i, 6).ToString());

                if (from_currency_id_.Equals(0) || to_currency_id_.Equals(0) || yyyymmdd.Equals("0") || newrate.Equals(0))
                goto skipSave;

                updated_record_count++;

                //update the old data
                var updateOld = from ord in db.Exchange_Rate
                                where ord.from_currency_id == from_currency_id_ && ord.to_currency_id == to_currency_id_ && ord.end_date == "9999/12/31"
                                select ord;
                foreach (Exchange_Rate ord in updateOld)
                {
                    ord.end_date = yyyymmdd;
                }

                db.SaveChanges();

                //adding a new record
                Exchange_Rate myDm = new Exchange_Rate();
                myDm.from_currency_id = from_currency_id_;
                myDm.to_currency_id = to_currency_id_;
                myDm.month_id = 1;
                myDm.exchange_rate1 = newrate;
                myDm.start_date = yyyymmdd;
                myDm.end_date = "9999/12/31";
                db.Exchange_Rate.Add(myDm);
                db.SaveChanges();

                skipSave:
                    continue;
                //}
            }
            return updated_record_count;
        }

        public ActionResult ViewExchangeRate()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(14) || z.Contains(26))
                {
                    var currency1 = from c in db.Currencies
                                    join e in db.Exchange_Rate on c.currency_id equals e.from_currency_id
                                    where e.end_date == "9999/12/31"
                                    select new { c.currency_id, c.currency_code };
                    ViewBag.Currency1 = (new SelectList(currency1, "currency_id", "currency_code"));
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

        [HttpPost]
        public ActionResult getCurrency2(int id)
        {
            var values = (from c in db.Currencies
                          join e in db.Exchange_Rate on c.currency_id equals e.to_currency_id
                          where e.from_currency_id == id && e.end_date == "9999/12/31"
                          select new DropdownListModel { Id = c.currency_id, name = c.currency_code });

            if (values == null)
                return Json(null);

            List<DropdownListModel> items = (List<DropdownListModel>)values.ToList();
            return Json(items);
        }

        [HttpPost]
        public ActionResult getCurrentRate(int id1, int id2)
        {
            var values = (from c in db.Currencies
                          join e in db.Exchange_Rate on c.currency_id equals e.to_currency_id
                          where e.from_currency_id == id1 && e.to_currency_id == id2
                          && e.end_date == "9999/12/31"
                          select e.exchange_rate1).FirstOrDefault();
            return Json(values);
        }

        //
        // GET: /Manage_Tax_Calculation/Details/5

        public ActionResult Details(int way_bill_id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(14) || z.Contains(26))
                {
                    var way_bill_code = (from w in db.Way_Bill where w.way_bill_id == way_bill_id select w.way_bill_code).First();
                    TempData["way_bill_code"] = way_bill_code;
                    var grand_total = db.Get_Grand_Total(way_bill_id,106);
                    foreach (var v in grand_total)
                    {
                        ViewBag.grand_total = v;
                    }
                    var tax_calculation = db.Display_Tax_Details(way_bill_id,100);
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
        // GET: /Manage_Tax_Calculation/View_Way_Bill/19

        public ActionResult View_Way_Bill(int id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(14) || z.Contains(26))
                {
                    var way_bill = (from w in db.Way_Bill
                                    join wd in db.Way_Bill_Details on w.way_bill_id equals wd.way_bill_id
                                    join i in db.Importers on w.importer_id equals i.importer_id
                                    join g in db.Goods on wd.goods_id equals g.goods_id
                                    join u in db.Unit_Of_Measure on wd.unit_of_measure_id equals u.unit_id
                                    where w.way_bill_id == id
                                    select new Way_BillModel { way_bill_code = w.way_bill_code, goods = g.goods_name, unit_of_measure = u.unit_code, total_quantity = wd.total_quantity, importer_name = i.importer_first_name + " " + i.importer_middle_name + " " + i.importer_last_name, is_damaged = wd.is_damaged });
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
        // GET: /Manage_Tax_Calculation/Reject_Reason_View/5

        public ActionResult Reject_Reason_View(int way_bill_id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(14) || z.Contains(26))
                {
                    var reason = (from r in db.Reject_Reason
                                  join w in db.Way_Bill on r.way_bill_id equals w.way_bill_id
                                  where r.way_bill_id == way_bill_id
                                  orderby r.reject_reason_id descending
                                  select new Reject_ReasonModel { reject_reason_id = r.reject_reason_id, way_bill_id = r.way_bill_id, way_bill_code = w.way_bill_code, reason = r.reject_reason, rejected_from = r.reject_from, rejected_date = r.rejected_date });
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
        // GET: /Manage_Tax_Calculation/Add_Goods_Price

        public ActionResult Add_Goods_Price(int way_bill_id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(14) || z.Contains(26))
                {
                    ViewBag.currency = new HomeController().Currency();
                    var end_date = Convert.ToDateTime("9999-12-31");
                    TempData["way_bill_id"] = way_bill_id;
                    var goods_tariff_not_percent = (from wd in db.Way_Bill_Details 
                                                   join g in db.Goods on wd.goods_id equals g.goods_id
                                                   join gtf in db.Goods_Tariff on g.goods_id equals gtf.goods_id
                                                   join u in db.Unit_Of_Measure on wd.unit_of_measure_id equals u.unit_id
                                                   where gtf.end_date ==end_date && wd.way_bill_id == way_bill_id
                                                   select new Add_Goods_Price { goods_name = g.goods_name, total_quantity = wd.total_quantity, unit_of_measure = u.unit_code, goods_id_value = wd.goods_id, goods_tariff = gtf.goods_tariff, ispercentage_value = gtf.ispercentage, goods_price_value = wd.unit_price, currency_id_value = wd.currency_id, is_damaged = wd.is_damaged }).ToList();
                    var sos_usd = from pc in db.Payment_Config
                                  where pc.end_date > DateTime.Now
                                  select new { pc.sos, pc.usd };
                    foreach (var item in sos_usd)
                    {
                        TempData["sos"] = item.sos;
                        TempData["usd"] = item.usd;
                    }
                    return View(goods_tariff_not_percent);
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
        // POST: /Manage_Tax_Calculation/Add_Goods_Price
        [HttpPost]
        public ActionResult Add_Goods_Price(Add_Goods_Price add_goods_price)
        {
            if (Session["login_status"] != null)
            {
                var emp_id = Convert.ToInt32(Session["id"]);
                int assign_way_bill_id = 0;
                int[] z = (int[])Session["function_id"];
                if (z.Contains(14) || z.Contains(26))
                {
                    var goods_id = 0;
                    var currency_id = 0;
                    decimal goods_price = 0;
                    var way_bill_id = Convert.ToInt32(TempData["way_bill_id"]); 
                    for (var i = 0; i < add_goods_price.count; i++)
                    {
                        goods_id = add_goods_price.goods_id[i];
                        currency_id = add_goods_price.currency_id[i];
                        goods_price = add_goods_price.goods_price[i];
                        db.Update_Way_Bill_table(goods_id, currency_id, goods_price, way_bill_id);
                    }
                    var assign_way_bill = from aw in db.Assign_Way_Bill
                                                      where aw.way_bill_id == way_bill_id && aw.status == false
                                                      select aw.assign_tax_id;
                    foreach (var item in assign_way_bill)
                    {
                        assign_way_bill_id = item;
                        Assign_Way_Bill assign_way_bill_old = db.Assign_Way_Bill.Find(assign_way_bill_id);
                        assign_way_bill_old.status = true;
                        db.Entry(assign_way_bill_old).State = EntityState.Modified;                        
                    }
                    db.SaveChanges();
                    db.Calculate_Tax(way_bill_id,emp_id);
                    TempData["errorMessage"] = "Tax Calculated Successfully";
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
        // GET: /Manage_Tax_Calculation/Add_Penalty_Goods_Price

        public ActionResult Add_Penalty_Goods_Price(int way_bill_id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(14) || z.Contains(26))
                {
                    ViewBag.currency = new HomeController().Currency();
                    var end_date = Convert.ToDateTime("9999-12-31");
                    TempData["way_bill_id"] = way_bill_id;
                    var goods_tariff_not_percent = (from pgd in db.Penaltized_Goods_Details
                                                    join g in db.Goods on pgd.goods_id equals g.goods_id
                                                    join gtf in db.Goods_Tariff on g.goods_id equals gtf.goods_id
                                                    join u in db.Unit_Of_Measure on pgd.unit_of_measure_id equals u.unit_id
                                                    where gtf.end_date == end_date && pgd.way_bill_id == way_bill_id
                                                    select new Add_Goods_Price { goods_name = g.goods_name, total_quantity = pgd.total_quantity, unit_of_measure = u.unit_code, goods_id_value = pgd.goods_id, goods_tariff = gtf.goods_tariff, ispercentage_value = gtf.ispercentage, goods_price_value = pgd.unit_price, currency_id_value = pgd.currency_id, is_damaged = pgd.is_damaged }).ToList();
                    var sos_usd = from pc in db.Payment_Config
                                  where pc.end_date > DateTime.Now
                                  select new { pc.sos, pc.usd };
                    foreach (var item in sos_usd)
                    {
                        TempData["sos"] = item.sos;
                        TempData["usd"] = item.usd;
                    }
                    return View(goods_tariff_not_percent);
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
        // POST: /Manage_Tax_Calculation/Add_Penalty_Goods_Price
        [HttpPost]
        public ActionResult Add_Penalty_Goods_Price(Add_Goods_Price add_goods_price)
        {
            if (Session["login_status"] != null)
            {
                int assign_way_bill_id = 0;
                int[] z = (int[])Session["function_id"];
                if (z.Contains(14) || z.Contains(26))
                {
                    var emp_id = Convert.ToInt32(Session["id"]);
                    var goods_id = 0;
                    var currency_id = 0;
                    decimal goods_price = 0;
                    var way_bill_id = Convert.ToInt32(TempData["way_bill_id"]);
                    for (var i = 0; i < add_goods_price.count; i++)
                    {
                        goods_id = add_goods_price.goods_id[i];
                        currency_id = add_goods_price.currency_id[i];
                        goods_price = add_goods_price.goods_price[i];
                        db.Update_Penaltized_Goods_Details_table(goods_id, currency_id, goods_price, way_bill_id);
                    }
                    var assign_way_bill = from aw in db.Assign_Way_Bill
                                          where aw.way_bill_id == way_bill_id && aw.status == false
                                          select aw.assign_tax_id;
                    foreach (var item in assign_way_bill)
                    {
                        assign_way_bill_id = item;
                        Assign_Way_Bill assign_way_bill_old = db.Assign_Way_Bill.Find(assign_way_bill_id);
                        assign_way_bill_old.status = true;
                        db.Entry(assign_way_bill_old).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    db.Calculate_Penalty(way_bill_id, add_goods_price.penalty,emp_id);
                    TempData["errorMessage"] = "Tax Calculated Successfully";
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
        // GET: /Manage_Tax_Calculation/Assign_Way_Bill
        public ActionResult Assign_Way_Bill(int way_bill_id)
        {
            TempData["way_bill_id"] = way_bill_id;
            ViewBag.user = new HomeController().Way_Bill_Assign_User();
            return View();
        }

        //
        // POST: /Manage_Tax_Calculation/Assign_Way_Bill
        [HttpPost]
        public ActionResult Assign_Way_Bill(Assign_Way_Bill assign_way_bill )
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(14))
                {
                    var old_assigned = 0;
                    var end_date = Convert.ToDateTime("9999-12-31");
                    var way_bill_id = Convert.ToInt32(TempData["way_bill_id"]);
                    var way_bill_exist = from wa in db.Assign_Way_Bill
                                         where wa.way_bill_id == way_bill_id && wa.end_date == end_date
                                         select wa.assign_tax_id;
                    foreach (var item in way_bill_exist)
                    {
                        old_assigned = item;
                        Assign_Way_Bill assign_way_bill_old = db.Assign_Way_Bill.Find(old_assigned);
                        assign_way_bill_old.end_date = DateTime.Now;
                        assign_way_bill.status = true;
                        db.Entry(assign_way_bill_old).State = EntityState.Modified;                        
                    }
                    db.SaveChanges();
                    assign_way_bill.way_bill_id = way_bill_id;
                    assign_way_bill.status = false;
                    assign_way_bill.end_date = end_date;
                    db.Assign_Way_Bill.Add(assign_way_bill);
                    db.SaveChanges();
                    var import_id = (from i in db.Imports
                                     where i.way_bill_id == way_bill_id
                                     select new { i.import_id, i.importing_status_id }).First();
                    Import import = db.Imports.Find(import_id.import_id);
                    if (import_id.importing_status_id == 18)
                    {
                        import.importing_status_id = 21;
                    }
                    else
                    {
                        import.importing_status_id = 19;
                    }
                    db.Entry(import).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["errorMessage"] = "Way Bill Assigned Successfully";
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
        // GET: /Manage_Tax_Calculation/DbSearch

        public ActionResult View_Payment_Config()
        {
            var end_date = Convert.ToDateTime("9999-12-31");
            var payment_config = db.Payment_Config.Where(pc => pc.end_date == end_date).ToList();
            return View(payment_config);
        }

        //
        // GET: /Manage_Tax_Calculation/Reject_Waybill

        public ActionResult Reject_Waybill(int way_bill_id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(14) || z.Contains(26))
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
        // GET: /Manage_Tax_Calculation/Reject_Manifest/W0009

        public ActionResult Reject_Manifest(Reject_ReasonModel reject_reason_model)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(14) || z.Contains(26))
                {
                    var way_bill_id = Convert.ToInt32(TempData["way_bill_id"]);
                    db.Reject_Calculated_Tax(way_bill_id, 20, reject_reason_model.reason, "Accounting");
                    TempData["errorMessage"] = "Evaluation Rejected";
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
        // GET: /Manage_Tax_Calculation/DbSearch

        public ActionResult DbSearch()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(14) || z.Contains(26))
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
        // POST: /Manage_Tax_Calculation/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Tax_CalculationModel tax_calculation)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (!z.Contains(14) && z.Contains(26))
                {
                    var user_id = Convert.ToInt32(Session["user_id"]);
                    if (tax_calculation.way_bill_code != null)
                    {
                        var result = (from w in db.Way_Bill
                                      join i in db.Imports on w.import_id equals i.import_id
                                      join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                      join ist in db.Importing_Status on i.importing_status_id equals ist.importing_status_id
                                      where (i.importing_status_id == 19 || i.importing_status_id == 21 || i.importing_status_id == 18) && w.way_bill_code.StartsWith(tax_calculation.way_bill_code)
                                      select new Tax_CalculationModel { way_bill_code = w.way_bill_code, import_code = i.import_code, ship_arrival_code = sa.ship_arrival_code, importing_status = ist.importing_status, importing_status_id = i.importing_status_id, way_bill_id = w.way_bill_id }).Distinct();
                            return View("Index", result.ToList());
                    }
                    return RedirectToAction("Index");
                }
                else if(z.Contains(14))
                {
                    //Queue q = new Queue();
                    if (tax_calculation.way_bill_code != null)
                    {
                        var end_date = Convert.ToDateTime("9999-12-31");
                        var result = (from w in db.Way_Bill
                                                join i in db.Imports on w.import_id equals i.import_id
                                                join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                                join ist in db.Importing_Status on i.importing_status_id equals ist.importing_status_id
                                                join sub in (from aw in db.Assign_Way_Bill
                                                             join u in db.Users on aw.user_id equals u.user_id
                                                             join e in db.Employees on u.employee_id equals e.employee_id
                                                             where aw.end_date == end_date
                                                             select new { way_bill_id = aw.way_bill_id, username = e.first_name + " " + e.middle_name + " " + e.last_name }) on w.way_bill_id equals sub.way_bill_id into x
                                                from l in x.DefaultIfEmpty()
                                      where (i.importing_status_id == 6 || i.importing_status_id == 7 || i.importing_status_id == 8 || i.importing_status_id == 9 || i.importing_status_id == 10 || i.importing_status_id == 12 || i.importing_status_id == 3 || i.importing_status_id == 18 || i.importing_status_id == 19 || i.importing_status_id == 21 || i.importing_status_id == 22) && w.way_bill_code.StartsWith(tax_calculation.way_bill_code)
                                      select new Tax_CalculationModel { way_bill_code = w.way_bill_code, import_code = i.import_code, ship_arrival_code = sa.ship_arrival_code, importing_status = ist.importing_status, importing_status_id = i.importing_status_id, way_bill_id = w.way_bill_id, assigned_to = l.username }).Distinct();
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