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
            var tax_calculation = (from w in db.Way_Bill
                                   join i in db.Imports on w.import_id equals i.import_id
                                   join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                   join ist in db.Importing_Status on i.importing_status_id equals ist.importing_status_id
                                   where i.importing_status_id == 6 || i.importer_id == 7 || i.importing_status_id == 8 || i.importing_status_id == 9 || i.importing_status_id == 10 || i.importing_status_id == 12 || i.importing_status_id == 16
                                   select new Tax_CalculationModel { way_bill_code = w.way_bill_code, import_code = i.import_code, ship_arrival_code = sa.ship_arrival_code, importing_status = ist.importing_status, importing_status_id = i.importing_status_id, way_bill_id = w.way_bill_id }).Distinct();
            return View(tax_calculation.ToList());
        }

        [HttpPost]
        public ActionResult SaveFile()
        {
            int updated_file_count = 0;
            HttpPostedFileBase file = Request.Files.Get("exchangefile");
            var files = Request.Files["exchangefile"];
            if (files != null)
            {
                FileInfo fil = new FileInfo(files.FileName);
                var filename = DateTime.Now.Ticks + fil.Extension;
                string path = Server.MapPath("~/Images/" + filename);
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
                if (myFile.GetCellValue(i, 1) == null)
                {
                    break;
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
            var currency1 = from c in db.Currencies
                            join e in db.Exchange_Rate on c.currency_id equals e.from_currency_id
                            where e.end_date == "9999/12/31"
                            select new { c.currency_id, c.currency_code };
            ViewBag.Currency1 = (new SelectList(currency1, "currency_id", "currency_code"));
            return View();
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
            var way_bill_code = (from w in db.Way_Bill where w.way_bill_id == way_bill_id select w.way_bill_code).First();
            TempData["way_bill_code"] = way_bill_code;
            var grand_total = db.Get_Grand_Total(way_bill_id);
            foreach (var v in grand_total)
            {
                ViewBag.grand_total = v.Value;
            }
            var tax_calculation = db.Display_Tax_Details(way_bill_id);
            return View(tax_calculation.ToList());
        }


        //
        // GET: /Manage_Tax_Calculation/Reject_Reason_View/5

        public ActionResult Reject_Reason_View(int way_bill_id)
        {
            var reason = (from r in db.Reject_Reason
                          join w in db.Way_Bill on r.way_bill_id equals w.way_bill_id
                          where r.way_bill_id == way_bill_id
                          orderby r.reject_reason_id descending
                          select new Reject_ReasonModel { reject_reason_id = r.reject_reason_id, way_bill_id = r.way_bill_id, way_bill_code = w.way_bill_code, reason = r.reject_reason, rejected_from = r.reject_from, rejected_date = r.rejected_date });
            return View(reason.ToList());
        }


        //
        // Manage_Tax_Calculation/Create

        //public ActionResult Create(int way_bill_id)
        //{
        //    db.Calculate_Tax(way_bill_id);
        //    TempData["errorMessage"] = "Tax Calculated Successfully";
        //    return RedirectToAction("Index");
        //}

        //
        // GET: /Manage_Tax_Calculation/Add_Goods_Price

        public ActionResult Add_Goods_Price(int way_bill_id)
        {
            ViewBag.currency = new HomeController().Currency();
            var end_date = Convert.ToDateTime("9999-12-31");
            TempData["way_bill_id"] = way_bill_id;
            var goods_tariff_not_percent = (from wd in db.Way_Bill_Details 
                                           join g in db.Goods on wd.goods_id equals g.goods_id
                                           join gtf in db.Goods_Tariff on g.goods_id equals gtf.goods_id
                                           join u in db.Unit_Of_Measure on wd.unit_of_measure_id equals u.unit_id
                                           where gtf.end_date ==end_date && wd.way_bill_id == way_bill_id
                                           select new Add_Goods_Price { goods_name = g.goods_name, total_quantity = wd.total_quantity, unit_of_measure = u.unit_code, goods_id_value = wd.goods_id, goods_tariff = gtf.goods_tariff, ispercentage = gtf.ispercentage, goods_price_value = wd.unit_price, currency_id_value = wd.currency_id }).ToList();
            return View(goods_tariff_not_percent);
        }

        //
        // POST: /Manage_Tax_Calculation/Add_Goods_Price
        [HttpPost]
        public ActionResult Add_Goods_Price(Add_Goods_Price add_goods_price)
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
            db.Calculate_Tax(way_bill_id);
            TempData["errorMessage"] = "Tax Calculated Successfully";
            return RedirectToAction("Index");
        }

        //
        // GET: /Manage_Tax_Calculation/DbSearch

        public ActionResult DbSearch()
        {
            return View();
        }

        //
        // POST: /Manage_Tax_Calculation/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Tax_CalculationModel tax_calculation)
        {
            //Queue q = new Queue();
            if (tax_calculation.way_bill_code != null)
            {
                var result = (from w in db.Way_Bill
                                   join i in db.Imports on w.import_id equals i.import_id
                                   join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                   join ist in db.Importing_Status on i.importing_status_id equals ist.importing_status_id
                                   where i.importing_status_id != 1 && w.way_bill_code.StartsWith(tax_calculation.way_bill_code)
                              select new Tax_CalculationModel { way_bill_code = w.way_bill_code, import_code = i.import_code, ship_arrival_code = sa.ship_arrival_code, importing_status = ist.importing_status, importing_status_id = i.importing_status_id, way_bill_id = w.way_bill_id }).Distinct();
                return View("Index", result.ToList());
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}