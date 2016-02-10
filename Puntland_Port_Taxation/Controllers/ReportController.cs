using FlexCel.Render;
using FlexCel.XlsAdapter;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using FlexCel.Core;
using Puntland_Port_Taxation;
using Puntland_Port_Taxation.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.io;


namespace Puntland_Port_Taxation.Controllers
{
    public class ReportController : Controller
    {
        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();


        public ActionResult Index()
        {
            DeleteOlderFiles();
            TempData["Warning"] = null;
            ViewBag.DDL_FilterType = FilterTypeDDL();
            ViewBag.DDL_Month = Month_DDL();
            return View();
        }

        #region "Generate report"
        [HttpPost]
        public ActionResult Index(Filter_Models fmodel)
        {
            ViewBag.DDL_FilterType = FilterTypeDDL();
            ViewBag.DDL_Month = Month_DDL();


            try
            {
                string last_date = "9999/12/31";
                var exchangerate = (from d in db.Exchange_Rate where d.end_date == last_date && d.from_currency_id == 100 && d.to_currency_id == 106 select d.exchange_rate1).FirstOrDefault();//db.GetExchangeRate(dtExchangeRateDate).FirstOrDefault();


                TempData["Warning"] = null;

                DateTime default_date = new DateTime(0001, 01, 01);

                int Year_Id = 0;
                int Month_Id = 0;

                Year_Id = GetYearId(fmodel.Year);
                Month_Id = GetMonthId(fmodel.Month, fmodel.Year.ToString());
                // DateTime dtExchangeRateDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                int imp = 0, exp = 0;

                IEnumerable<GetReport_Levy_Transactions_Import_Rep2_Result> imp_list = db.GetReport_Levy_Transactions_Import_Rep2(null, null, null, null, null);
                IEnumerable<GetReport_Levy_Transactions_Export_Rep2_Result> exp_list = db.GetReport_Levy_Transactions_Export_Rep2(null, null, null, null, null);

                IEnumerable<Report_4_Import_Result> imp_list4 = db.Report_4_Import(null, null, null, null, null);
                IEnumerable<Report_4_Export_Result> exp_list4 = db.Report_4_Export(null, null, null, null, null);


                switch (fmodel.Report)
                {

                    case 1:
                        Report_1(fmodel.On_Date, fmodel.Rep_Type_hd);
                        break;
                    case 2:

                       
                        //get files w:r:to type(pdf/excel)

                        string Orginal_file_path = string.Empty;

                        if (fmodel.Rep_Type_hd.Equals("pdf"))
                            Orginal_file_path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Report_Templates"), "Report_2_B.xlsx");

                        else
                            Orginal_file_path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Report_Templates"), "Report_2_A.xlsx");



                        //temp file name 
                        string copy_file_name = DateTime.Now.Ticks + ".xlsx";
                        //temp file path
                        string copy_file_path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Images"), copy_file_name);


                      

                        #region "Report 2"
                        if (fmodel.On_Date != default_date && fmodel.FilterType == 1)
                        {
                            //Report_2(db.GetReport_Levy_Transactions_Import_Rep2(fmodel.On_Date, null, null, null, null), db.GetReport_Levy_Transactions_Export_Rep2(fmodel.On_Date, null, null, null, null), fmodel.On_Date.ToString("dd/MM/yyyy"), exchangerate.ToString(), fmodel.Rep_Type_hd);
                            try
                            {
                                imp_list = db.GetReport_Levy_Transactions_Import_Rep2(fmodel.On_Date, null, null, null, null);
                                imp = 1;
                            }
                            catch { }

                            try
                            {
                                exp_list = db.GetReport_Levy_Transactions_Export_Rep2(fmodel.On_Date, null, null, null, null);
                                exp = 1;
                            }
                            catch { }


                            if (imp == 0 && exp == 0)
                            {
                                TempData["Warning"] = "No Data To Export";
                                return View();
                            }
                            else
                            {
                                 if (imp == 1 && exp==0)
                                    Report_2(imp_list,null , fmodel.On_Date.ToString("dd/MM/yyyy"), exchangerate.ToString(), fmodel.Rep_Type_hd, copy_file_path, Orginal_file_path);
                                 if (imp == 0 && exp == 1)
                                   Report_2(null,exp_list , fmodel.On_Date.ToString("dd/MM/yyyy"), exchangerate.ToString(), fmodel.Rep_Type_hd, copy_file_path, Orginal_file_path);
                                if(exp==1 && imp==1)
                                    Report_2(imp_list,exp_list , fmodel.On_Date.ToString("dd/MM/yyyy"), exchangerate.ToString(), fmodel.Rep_Type_hd, copy_file_path, Orginal_file_path);
                       
                                  }



                        }

                        else if (Month_Id != 0 && fmodel.FilterType == 2)
                        {
                            var month = (from d in db.Months where d.month_id == Month_Id select d.month1).FirstOrDefault();
                            try
                            {
                                imp_list = db.GetReport_Levy_Transactions_Import_Rep2(null, Month_Id, null, null, null);
                                imp = 1;

                            }
                            catch
                            { }

                            try
                            {
                                exp_list = db.GetReport_Levy_Transactions_Export_Rep2(null, Month_Id, null, null, null);
                                exp = 1;

                            }
                            catch
                            { }


                            if (imp == 0 && exp == 0)
                            {
                                TempData["Warning"] = "No Data To Export";
                                return View();
                            }
                            else
                            {
                                if (imp == 1 && exp==0)
                                    Report_2(imp_list,null ,month, exchangerate.ToString(), fmodel.Rep_Type_hd, copy_file_path, Orginal_file_path);
                                if (imp==0 && exp == 1)
                                   Report_2(null,exp_list ,month, exchangerate.ToString(), fmodel.Rep_Type_hd, copy_file_path, Orginal_file_path);
                                if(exp==1 && imp==1)
                                    Report_2(imp_list,exp_list ,month, exchangerate.ToString(), fmodel.Rep_Type_hd, copy_file_path, Orginal_file_path);
                            }
                        }

                        else if (Year_Id != 0 && fmodel.FilterType == 3)
                        {
                            //Report_2(db.GetReport_Levy_Transactions_Import_Rep2(null, null, Year_Id, null, null), db.GetReport_Levy_Transactions_Export_Rep2(null, null, Year_Id, null, null), (from d in db.Years where d.year_id == Year_Id select d.year_code).FirstOrDefault(), exchangerate.ToString(), fmodel.Rep_Type_hd);
                            try
                            {
                                imp_list = db.GetReport_Levy_Transactions_Import_Rep2(null, null, Year_Id, null, null);
                                imp = 1;

                            }
                            catch
                            { }
                            try
                            {
                                exp_list = db.GetReport_Levy_Transactions_Export_Rep2(null, null, Year_Id, null, null);
                                exp = 1;

                            }
                            catch
                            { }

                            if (imp == 0 && exp == 0)
                            {
                                TempData["Warning"] = "No Data To Export";
                                return View();
                            }
                            else
                            {

                                  if (imp == 1 && exp==0)
                                    Report_2(imp_list,null ,(from d in db.Years where d.year_id == Year_Id select d.year_code).FirstOrDefault(), exchangerate.ToString(), fmodel.Rep_Type_hd, copy_file_path, Orginal_file_path);
                                  if (imp == 0 && exp == 1)
                                   Report_2(null,exp_list , (from d in db.Years where d.year_id == Year_Id select d.year_code).FirstOrDefault(), exchangerate.ToString(), fmodel.Rep_Type_hd, copy_file_path, Orginal_file_path);
                                  if (imp == 1 && exp == 1)
                                    Report_2(imp_list,exp_list , (from d in db.Years where d.year_id == Year_Id select d.year_code).FirstOrDefault(), exchangerate.ToString(), fmodel.Rep_Type_hd, copy_file_path, Orginal_file_path);
                       

                            
                            }
                        }

                        else if (fmodel.Interval_1 != default_date && fmodel.Interval_2 != default_date && fmodel.FilterType == 4)
                        {
                            //Report_2(db.GetReport_Levy_Transactions_Import_Rep2(null, null, null, fmodel.Interval_1, fmodel.Interval_2), db.GetReport_Levy_Transactions_Export_Rep2(null, null, null, fmodel.Interval_1, fmodel.Interval_2), fmodel.Interval_1.ToString("dd/MM/yyyy") + " - " + fmodel.Interval_2.ToString("dd/MM/yyyy"), exchangerate.ToString(), fmodel.Rep_Type_hd);
                            try
                            {
                                imp_list = db.GetReport_Levy_Transactions_Import_Rep2(null, null, null, fmodel.Interval_1, fmodel.Interval_2);
                                imp = 1;

                            }
                            catch
                            { }

                            try
                            {
                                exp_list = db.GetReport_Levy_Transactions_Export_Rep2(null, null, null, fmodel.Interval_1, fmodel.Interval_2);
                                exp = 1;

                            }
                            catch
                            { }


                            if (imp == 0 && exp == 0)
                            {
                                TempData["Warning"] = "No Data To Export";
                                return View();
                            }
                            else
                            {
                                      if (imp == 1 && exp==0)
                                          Report_2(imp_list, null, fmodel.Interval_1.ToString("dd/MM/yyyy") + " - " + fmodel.Interval_2.ToString("dd/MM/yyyy"), exchangerate.ToString(), fmodel.Rep_Type_hd, copy_file_path, Orginal_file_path);
                                      if (imp == 0 && exp == 1)
                                    Report_2(null, exp_list, fmodel.Interval_1.ToString("dd/MM/yyyy") + " - " + fmodel.Interval_2.ToString("dd/MM/yyyy"), exchangerate.ToString(), fmodel.Rep_Type_hd, copy_file_path, Orginal_file_path);
                                if(exp==1 && imp==1)
                                    Report_2(imp_list, exp_list, fmodel.Interval_1.ToString("dd/MM/yyyy") + " - " + fmodel.Interval_2.ToString("dd/MM/yyyy"), exchangerate.ToString(), fmodel.Rep_Type_hd, copy_file_path, Orginal_file_path);
                       

                            }
                        }

                        else return View();


                        


                        break;
                        #endregion

                    case 3:
                        IEnumerable<GetReport3_Levy_Products_Result> impList = db.GetReport3_Levy_Products(null, null, null, null, null);
                        if (fmodel.On_Date != default_date && fmodel.FilterType == 1)
                        {

                            try
                            {
                                impList = db.GetReport3_Levy_Products(fmodel.On_Date, null, null, null, null);
                                imp = 1;
                            }
                            catch { }

                            if(imp==1)
                            Report_3(impList, fmodel.On_Date.ToString("dd/MM/yyyy"), exchangerate.ToString(), fmodel.Rep_Type_hd);

                        }

                        else if (Month_Id != 0 && fmodel.FilterType == 2)
                        {
                            try
                            {
                                impList = db.GetReport3_Levy_Products(null, Month_Id, null, null, null);
                                imp = 1;
                            }
                            catch { }

                            if (imp == 1)
                            Report_3(impList, (from d in db.Months where d.month_id == Month_Id select d.month1).FirstOrDefault(), exchangerate.ToString(), fmodel.Rep_Type_hd);
                        }

                        else if (Year_Id != 0 && fmodel.FilterType == 3)
                        {
                            try
                            {
                                impList = db.GetReport3_Levy_Products(null, null, Year_Id, null, null);
                                imp = 1;
                            }
                            catch { }

                            if (imp == 1)
                                Report_3(impList, (from d in db.Years where d.year_id == Year_Id select d.year_code).FirstOrDefault(), exchangerate.ToString(), fmodel.Rep_Type_hd);
                        }

                        else if (fmodel.Interval_1 != default_date && fmodel.Interval_2 != default_date && fmodel.FilterType == 4)
                        {
                            try
                            {
                                impList = db.GetReport3_Levy_Products(null, null, null, fmodel.Interval_1, fmodel.Interval_2);
                                imp = 1;
                            }
                            catch { }

                            if (imp == 1)
                            Report_3(impList, fmodel.Interval_1.ToString("dd/MM/yyyy") + " - " + fmodel.Interval_2.ToString("dd/MM/yyyy"), exchangerate.ToString(), fmodel.Rep_Type_hd);
                        }

                        else return View();

                        break;

                    case 4:

                             last_date = "9999/12/31";
                             exchangerate = (from d in db.Exchange_Rate where d.end_date == last_date && d.from_currency_id == 100 && d.to_currency_id == 106 select d.exchange_rate1).FirstOrDefault();//db.GetExchangeRate(dtExchangeRateDate).FirstOrDefault();

                             Orginal_file_path = string.Empty;

                             if (!fmodel.Rep_Type_hd.Equals("pdf"))
                                Orginal_file_path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Report_Templates"), "Report_4_B.xlsx");

                            else
                                Orginal_file_path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Report_Templates"), "Report_4_A.xlsx");


                            //temp file name 
                            copy_file_name = DateTime.Now.Ticks + ".xlsx";
                            //temp file path
                            copy_file_path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Images"), copy_file_name);


                           #region "Report 4"
                        if (fmodel.On_Date != default_date && fmodel.FilterType == 1)
                        {
                            //Report_2(db.GetReport_Levy_Transactions_Import_Rep2(fmodel.On_Date, null, null, null, null), db.GetReport_Levy_Transactions_Export_Rep2(fmodel.On_Date, null, null, null, null), fmodel.On_Date.ToString("dd/MM/yyyy"), exchangerate.ToString(), fmodel.Rep_Type_hd);
                            try
                            {
                                imp_list4 = db.Report_4_Import(fmodel.On_Date, null, null, null, null);
                                imp = 1;
                            }
                            catch { }

                            try
                            {
                                exp_list4 = db.Report_4_Export(fmodel.On_Date, null, null, null, null);
                                exp = 1;
                            }
                            catch { }


                            if (imp == 0 && exp == 0)
                            {
                                TempData["Warning"] = "No Data To Export";
                                return View();
                            }
                            else
                            {
                                 if (imp == 1 && exp==0)
                                    Report_4(imp_list4,null , fmodel.On_Date.ToString("dd/MM/yyyy"), exchangerate.ToString(), fmodel.Rep_Type_hd, copy_file_path, Orginal_file_path);
                                 if (imp == 0 && exp == 1)
                                   Report_4(null,exp_list4 , fmodel.On_Date.ToString("dd/MM/yyyy"), exchangerate.ToString(), fmodel.Rep_Type_hd, copy_file_path, Orginal_file_path);
                                if(exp==1 && imp==1)
                                    Report_4(imp_list4,exp_list4 , fmodel.On_Date.ToString("dd/MM/yyyy"), exchangerate.ToString(), fmodel.Rep_Type_hd, copy_file_path, Orginal_file_path);
                       
                                  }



                        }

                        else if (Month_Id != 0 && fmodel.FilterType == 2)
                        {
                            var month = (from d in db.Months where d.month_id == Month_Id select d.month1).FirstOrDefault();
                            try
                            {
                                imp_list4 = db.Report_4_Import(null, Month_Id, null, null, null);
                                imp = 1;

                            }
                            catch
                            { }

                            try
                            {
                                exp_list4 = db.Report_4_Export(null, Month_Id, null, null, null);
                                exp = 1;

                            }
                            catch
                            { }


                            if (imp == 0 && exp == 0)
                            {
                                TempData["Warning"] = "No Data To Export";
                                return View();
                            }
                            else
                            {
                                if (imp == 1 && exp==0)
                                    Report_4(imp_list4,null ,month, exchangerate.ToString(), fmodel.Rep_Type_hd, copy_file_path, Orginal_file_path);
                                if (imp==0 && exp == 1)
                                   Report_4(null,exp_list4 ,month, exchangerate.ToString(), fmodel.Rep_Type_hd, copy_file_path, Orginal_file_path);
                                if(exp==1 && imp==1)
                                    Report_4(imp_list4,exp_list4 ,month, exchangerate.ToString(), fmodel.Rep_Type_hd, copy_file_path, Orginal_file_path);
                            }
                        }

                        else if (Year_Id != 0 && fmodel.FilterType == 3)
                        {
                            //Report_2(db.GetReport_Levy_Transactions_Import_Rep2(null, null, Year_Id, null, null), db.GetReport_Levy_Transactions_Export_Rep2(null, null, Year_Id, null, null), (from d in db.Years where d.year_id == Year_Id select d.year_code).FirstOrDefault(), exchangerate.ToString(), fmodel.Rep_Type_hd);
                            try
                            {
                                imp_list4 = db.Report_4_Import(null, null, Year_Id, null, null);
                                imp = 1;

                            }
                            catch
                            { }
                            try
                            {
                                exp_list4 = db.Report_4_Export(null, null, Year_Id, null, null);
                                exp = 1;

                            }
                            catch
                            { }

                            if (imp == 0 && exp == 0)
                            {
                                TempData["Warning"] = "No Data To Export";
                                return View();
                            }
                            else
                            {

                                  if (imp == 1 && exp==0)
                                    Report_4(imp_list4,null ,(from d in db.Years where d.year_id == Year_Id select d.year_code).FirstOrDefault(), exchangerate.ToString(), fmodel.Rep_Type_hd, copy_file_path, Orginal_file_path);
                                  if (imp == 0 && exp == 1)
                                   Report_4(null,exp_list4 , (from d in db.Years where d.year_id == Year_Id select d.year_code).FirstOrDefault(), exchangerate.ToString(), fmodel.Rep_Type_hd, copy_file_path, Orginal_file_path);
                                  if (imp == 1 && exp == 1)
                                    Report_4(imp_list4,exp_list4 , (from d in db.Years where d.year_id == Year_Id select d.year_code).FirstOrDefault(), exchangerate.ToString(), fmodel.Rep_Type_hd, copy_file_path, Orginal_file_path);
                       

                            
                            }
                        }

                        else if (fmodel.Interval_1 != default_date && fmodel.Interval_2 != default_date && fmodel.FilterType == 4)
                        {
                            //Report_2(db.GetReport_Levy_Transactions_Import_Rep2(null, null, null, fmodel.Interval_1, fmodel.Interval_2), db.GetReport_Levy_Transactions_Export_Rep2(null, null, null, fmodel.Interval_1, fmodel.Interval_2), fmodel.Interval_1.ToString("dd/MM/yyyy") + " - " + fmodel.Interval_2.ToString("dd/MM/yyyy"), exchangerate.ToString(), fmodel.Rep_Type_hd);
                            try
                            {
                                imp_list4 = db.Report_4_Import(null, null, null, fmodel.Interval_1, fmodel.Interval_2);
                                imp = 1;

                            }
                            catch
                            { }

                            try
                            {
                                exp_list4 = db.Report_4_Export(null, null, null, fmodel.Interval_1, fmodel.Interval_2);
                                exp = 1;

                            }
                            catch
                            { }


                            if (imp == 0 && exp == 0)
                            {
                                TempData["Warning"] = "No Data To Export";
                                return View();
                            }
                            else
                            {
                                      if (imp == 1 && exp==0)
                                          Report_4(imp_list4, null, fmodel.Interval_1.ToString("dd/MM/yyyy") + " - " + fmodel.Interval_2.ToString("dd/MM/yyyy"), exchangerate.ToString(), fmodel.Rep_Type_hd, copy_file_path, Orginal_file_path);
                                      if (imp == 0 && exp == 1)
                                    Report_4(null, exp_list4, fmodel.Interval_1.ToString("dd/MM/yyyy") + " - " + fmodel.Interval_2.ToString("dd/MM/yyyy"), exchangerate.ToString(), fmodel.Rep_Type_hd, copy_file_path, Orginal_file_path);
                                if(exp==1 && imp==1)
                                    Report_4(imp_list4, exp_list4, fmodel.Interval_1.ToString("dd/MM/yyyy") + " - " + fmodel.Interval_2.ToString("dd/MM/yyyy"), exchangerate.ToString(), fmodel.Rep_Type_hd, copy_file_path, Orginal_file_path);
                       

                            }
                        }

                        else return View();


                        


                        break;
                        #endregion

                        
                    default:
                        break;

                }
            }
            catch (Exception ee)
            {
                TempData["Warning"] =ee.ToString();
                return View();

            }
            return View();


        }

        #endregion

        #region "Get Corresponding Data"
        public int GetYearId(int year)
        {
            try
            {
                if (year > 1000)
                {
                    string yearcode = year.ToString();
                    return (from j in db.Years where j.year_code == yearcode select j.year_id).FirstOrDefault();
                }
                else
                    return 0;
            }
            catch { return 0; }
        }

        public int GetMonthId(string month, string year)
        {
            try
            {
                //string year = dt.Year.ToString();
                //string month = dt.Month.ToString("00");
                string code = year + month;
                return (from d in db.Months where d.month_code == code select d.month_id).FirstOrDefault();
            }
            catch { return 0; }
        }

        public string GetDecimal(string str)
        {
            try
            {
                decimal dec = decimal.Parse(str);
                dec = decimal.Round(dec, 2);
                return dec.ToString();
            }
            catch
            { return ""; }
        }
        #endregion

        #region "Reports"

        public void Report_1(DateTime Report_date, string typ)
        {
            //default last date
            string last_date = "9999/12/31";

            //current exchanfge rate
            var exchangerate = (from d in db.Exchange_Rate where d.end_date == last_date && d.from_currency_id == 100 && d.to_currency_id == 106 select d.exchange_rate1).FirstOrDefault();//db.GetExchangeRate(dtExchangeRateDate).FirstOrDefault();

            //file path W:r:to type(pdf/excel)
            string Orginal_file_path = string.Empty;

            if (typ.Equals("pdf"))
                Orginal_file_path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Report_Templates"), "Report_1_A.xlsx");

            else
                Orginal_file_path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Report_Templates"), "Report_1_B.xlsx");

            //temp file name 
            string copy_file_name = DateTime.Now.Ticks + ".xlsx";
            //temp file path
            string copy_file_path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Images"), copy_file_name);

            //open file
            XlsFile report_1 = new XlsFile(Orginal_file_path);

            //sheet always 1 
            report_1.ActiveSheet = 1;

            //get import data
            var daily_trans_import = db.Report_1_Import(Report_date);

            //get import data count
            var daily_trans_import_count = db.Report_1_Import(Report_date).Count();

            //skip if no data
            if (daily_trans_import_count <= 0)
                goto skipImport;



            //print
            int i = 6;

            report_1.SetCellValue(3, 4, exchangerate);

            report_1.SetCellValue(2, 2, Report_date.ToString("dd/MM/yyyy"));




            foreach (var item in daily_trans_import)
            {

                report_1.SetCellValue(i, 2, item.BY_CASH_CHEQUE);
                report_1.SetCellValue(i, 3, item.USD);
                report_1.SetCellValue(i, 4, item.SOS);
                report_1.SetCellValue(i, 5, item.TOTAL_USD);
                report_1.SetCellValue(i, 6, item.TOTAL_SOS);
                i++;
            }

            //skiped import 
        skipImport:



            //get export dtaa
            var daily_trans_export = db.Report_1_Export(Report_date);

            //get export count
            var daily_trans_export_count = db.Report_1_Export(Report_date).Count();


            //skip export if no data
            if (daily_trans_export_count <= 0)
                goto skipExport;

            //print export
            i = 14;

            foreach (var item in daily_trans_export)
            {

                report_1.SetCellValue(i, 2, item.BY_CASH_CHEQUE);
                report_1.SetCellValue(i, 3, item.USD);
                report_1.SetCellValue(i, 4, item.SOS);
                report_1.SetCellValue(i, 5, item.TOTAL_USD);
                report_1.SetCellValue(i, 6, item.TOTAL_SOS);
                i++;
            }

            //skipped export
        skipExport:

            //if no import and export return alert msg
            if (daily_trans_import_count <= 0 && daily_trans_export_count <= 0)
            {
                TempData["Warning"] = "No Data To Export";
                return;
            }
            else
            {

                //else print

                report_1.PrintToFit = true;
                report_1.PrintPaperSize = TPaperSize.A4;

                TWorkbookProtectionOptions protoptions = new TWorkbookProtectionOptions();
                protoptions.Window = false;
                protoptions.Structure = false;
                TSharedWorkbookProtectionOptions op = new TSharedWorkbookProtectionOptions();
                op.SharingWithTrackChanges = false;

                report_1.Protection.SetSharedWorkbookProtection("XXXX", op);

                report_1.Protection.SetWorkbookProtection("XXXX", protoptions);
                report_1.Protection.SetModifyPassword("XXXX", true, "XXXXXX");
                report_1.Protection.SetWorkbookProtectionOptions(protoptions);

                string dte = Report_date.ToString("dd/MMM/yyyy");
                dte = dte.Replace("/", "_");
                report_1.Save(copy_file_path);

                if (typ.Equals("pdf"))
                    GeneratePDF(report_1, "Report_1_" + dte);

                else
                {
                    Response.AppendHeader("Content-Disposition", "attachment; filename=Report_1_" + dte + ".xlsx");
                    Response.TransmitFile(copy_file_path);
                    Response.End();
                }

            }


        }

        public void Report_2(IEnumerable<GetReport_Levy_Transactions_Import_Rep2_Result> daily_trans_import, IEnumerable<GetReport_Levy_Transactions_Export_Rep2_Result> daily_trans_Export, string filterType, string exchange_rate, string typ, string copy_file_path, string Orginal_file_path)
        {

            int nocolor = 0;
            int coloronly = 0;

            int leftborder = 0;
            int rightborder = 0;

            int nocolorleft = 0;
            int nocolorright = 0;

            int nocolorbold = 0;
            int colorbold = 0;

           
            int c = 1;

            //open File
            XlsFile report_2 = new XlsFile(Orginal_file_path);

            //Keeping Impot Sheet Active
            report_2.ActiveSheet = 1;

            //tables for Import & export
            DataTable dtImport = new DataTable();
            // DataTable dtExport = new DataTable();

            //if import data is null , then import will be skipped --using goto skipImport else get the value in dtimport
            try
            {
                if(daily_trans_import!=null)
                dtImport = LinqQueryToDataTable(daily_trans_import.ToList());

                if (dtImport.Rows.Count <= 1)
                    goto skip_Import;

            }
            catch
            { goto skip_Import; }

            //Transpose result for excel
            DataTable dtTranspose1 = GenerateTransposedTable(dtImport);

            //// integer c to handle the cell number in both sheets
            //if (!typ.Equals("pdf"))
            //    c = 1;

            //to set exchange_rate
            report_2.SetCellValue(3, 3 + c, GetDecimal(exchange_rate));

            //to set header
            report_2.SetCellValue(2, 2 + c, filterType);

             nocolor = NoColory(report_2, 7, 2);
             coloronly = ColorOnly(report_2, 7, 2);

             leftborder = ColorAndLeftBorder(report_2, 7, 2);
             rightborder = ColorAndRightBorder(report_2, 7, 2);

             nocolorleft = NoColorAndLeftBorder(report_2, 7, 2);
             nocolorright = NoColorAndRightBorder(report_2, 7, 2);

             nocolorbold = NoColorBold(report_2, 7, 2);
             colorbold = ColorBold(report_2, 7, 2);



            //to print all data
            int i = 7;

            int color1 = 0;
            int color2 = 0;
            int color3 = 0;
            int color4 = 0;

            int datarow = 7;
            foreach (DataRow dr in dtTranspose1.Rows)
            {

                if (dr[1].ToString().Length > 0)
                {
                    if (i % 2 != 0)
                    {
                        color1 = nocolorleft;
                        color2 = nocolor;
                        color3 = nocolorright;
                        color4 = nocolorbold;

                    }
                    else
                    {
                        color1 = leftborder;
                        color2 = coloronly;
                        color3 = rightborder;
                        color4 = colorbold;

                    }

                    report_2.SetCellValue(i, 1 + c, dr[1].ToString().TrimEnd(']').TrimStart('['), color1);
                    report_2.SetCellValue(i, 2 + c, GetDecimal(dr[2].ToString()), color2);
                    report_2.SetCellValue(i, 3 + c, GetDecimal(dr[3].ToString()), color2);
                    report_2.SetCellValue(i, 4 + c, GetDecimal(dr[4].ToString()), color4);
                    report_2.SetCellValue(i, 5 + c, GetDecimal(dr[5].ToString()), color3);
                    datarow++;
                }
                i++;

            }

            int laststyle = TopBorderOnly(report_2, i, 1 + c);
            //last line border
            report_2.SetCellValue(datarow, 1 + c, "", laststyle);
            report_2.SetCellValue(datarow, 2 + c, "", laststyle);
            report_2.SetCellValue(datarow, 3 + c, "", laststyle);
            report_2.SetCellValue(datarow, 4 + c, "", laststyle);
            report_2.SetCellValue(datarow, 5 + c, "", laststyle);

             

        skip_Import:

            //Keeping Impot Sheet Active
            report_2.ActiveSheet = 2;

            //tables for Import & export
            DataTable dtExport = new DataTable();

            // integer c to handle the cell number in both sheets
            //if (!typ.Equals("pdf"))
            //    c = 1;

            //to set exchange_rate
            report_2.SetCellValue(3, 3 + c, GetDecimal(exchange_rate));

            //to set header
            report_2.SetCellValue(2, 2 + c, filterType);

           
            //checking export data, if null then skipping printing export  details
            try
            {
                if (daily_trans_Export != null)
                dtExport = LinqQueryToDataTable(daily_trans_Export.ToList());

                if (dtExport.Rows.Count <= 1)
                    goto skipExport;

            }
            catch
            { goto skipExport; }

            //transposing for excel
            DataTable dtTranspose2 = GenerateTransposedTable(dtExport);

            //printing rows
             i = 7;

             color1 = 0;
             color2 = 0;
             color3 = 0;
             color4 = 0;

             datarow = 7;
            foreach (DataRow dr in dtTranspose2.Rows)
            {

                if (dr[1].ToString().Length > 0)
                {
                    if (i % 2 != 0)
                    {
                        color1 = nocolorleft;
                        color2 = nocolor;
                        color3 = nocolorright;
                        color4 = nocolorbold;

                    }
                    else
                    {
                        color1 = leftborder;
                        color2 = coloronly;
                        color3 = rightborder;
                        color4 = colorbold;

                    }

                    report_2.SetCellValue(i, 1 + c, dr[1].ToString().TrimEnd(']').TrimStart('['), color1);
                    report_2.SetCellValue(i, 2 + c, GetDecimal(dr[2].ToString()), color2);
                    report_2.SetCellValue(i, 3 + c, GetDecimal(dr[3].ToString()), color2);
                    report_2.SetCellValue(i, 4 + c, GetDecimal(dr[4].ToString()), color4);
                    report_2.SetCellValue(i, 5 + c, GetDecimal(dr[5].ToString()), color3);
                    datarow++;
                }
                i++;

            }

            laststyle = TopBorderOnly(report_2, i, 1 + c);
            //last line border
            report_2.SetCellValue(datarow, 1 + c, "", laststyle);
            report_2.SetCellValue(datarow, 2 + c, "", laststyle);
            report_2.SetCellValue(datarow, 3 + c, "", laststyle);
            report_2.SetCellValue(datarow, 4 + c, "", laststyle);
            report_2.SetCellValue(datarow, 5 + c, "", laststyle);

        //doing the rest if data is null
        skipExport:

            report_2.PrintToFit = true;
            report_2.PrintPaperSize = TPaperSize.A4;

            TWorkbookProtectionOptions protoptions = new TWorkbookProtectionOptions();
            protoptions.Window = false;
            protoptions.Structure = false;

            TSharedWorkbookProtectionOptions op = new TSharedWorkbookProtectionOptions();
            op.SharingWithTrackChanges = false;


            report_2.Protection.SetSharedWorkbookProtection("XXXX", op);

            report_2.Protection.SetWorkbookProtection("XXXX", protoptions);
            report_2.Protection.SetModifyPassword("XXXX", true, "XXXXXX");
            report_2.Protection.SetWorkbookProtectionOptions(protoptions);



            if (dtImport.Rows.Count <= 1)
                TempData["Warning"] = "No Data To Export";
            else
            {
                report_2.Save(copy_file_path);

              
                if (typ.Equals("pdf"))
                    GeneratePDF(report_2, "Report_2_"+filterType);

                else
                {
                    Response.AppendHeader("Content-Disposition", "attachment; filename=Report_2_"+filterType+".xlsx");
                    Response.TransmitFile(copy_file_path);
                    Response.End();
                }
            }
            
        }

        private int ColorOnly(XlsFile xls, int row, int col)
        {
            TFlxFormat tf4 = xls.GetCellVisibleFormatDef(row, col);

            TFlxFillPattern tfpattern3 = new TFlxFillPattern();
            tfpattern3.BgColor = Colors.LightGray;
            tf4.FillPattern = tfpattern3;

            return xls.AddFormat(tf4);
        }

        private int NoColory(XlsFile xls, int row, int col)
        {
            TFlxFormat tf4 = xls.GetCellVisibleFormatDef(row, col);

            return xls.AddFormat(tf4);
        }


        private int ColorAndLeftBorder(XlsFile xls, int row, int col)
        {
            TFlxFormat tf4 = xls.GetCellVisibleFormatDef(row, col);
            tf4.HAlignment = THFlxAlignment.left;
            TFlxBorders tfBorderF1 = new TFlxBorders();

            TFlxOneBorder oborder1 = new TFlxOneBorder();
            oborder1.Style = TFlxBorderStyle.Thin;
            tfBorderF1.Left = oborder1;

            tf4.Borders = tfBorderF1;

            TFlxFillPattern tfpattern3 = new TFlxFillPattern();
            tfpattern3.BgColor = Colors.LightGray;
            tf4.FillPattern = tfpattern3;

            return xls.AddFormat(tf4);
        }

        private int ColorAndRightBorder(XlsFile xls, int row, int col)
        {
            TFlxFormat tf4 = xls.GetCellVisibleFormatDef(row, col);

            TFlxFont tfont = new TFlxFont();
            tfont.Style = TFlxFontStyles.Bold;
            tf4.Font = tfont;

            TFlxBorders tfBorderF1 = new TFlxBorders();

            TFlxOneBorder oborder1 = new TFlxOneBorder();
            oborder1.Style = TFlxBorderStyle.Thin;

            tfBorderF1.Right = oborder1;

            tf4.Borders = tfBorderF1;

            TFlxFillPattern tfpattern3 = new TFlxFillPattern();
            tfpattern3.BgColor = Colors.LightGray;
            tf4.FillPattern = tfpattern3;

            return xls.AddFormat(tf4);
        }

        private int NoColorAndLeftBorder(XlsFile xls, int row, int col)
        {
            TFlxFormat tf3 = xls.GetCellVisibleFormatDef(row, col);
            tf3.HAlignment = THFlxAlignment.left;
            TFlxBorders tfBorderF = new TFlxBorders();
            TFlxOneBorder oborder = new TFlxOneBorder();
            oborder.Style = TFlxBorderStyle.Thin;
            tfBorderF.Left = oborder;

            tf3.Borders = tfBorderF;

            return xls.AddFormat(tf3);

        }

        private int NoColorAndRightBorder(XlsFile xls, int row, int col)
        {
            TFlxFormat tf3 = xls.GetCellVisibleFormatDef(row, col);

            TFlxFont tfont = new TFlxFont();
            tfont.Style = TFlxFontStyles.Bold;
            tf3.Font = tfont;

            TFlxBorders tfBorderF = new TFlxBorders();
            TFlxOneBorder oborder = new TFlxOneBorder();
            oborder.Style = TFlxBorderStyle.Thin;
            tfBorderF.Right = oborder;

            tf3.Borders = tfBorderF;

            return xls.AddFormat(tf3);

        }

        private int ColorBold(XlsFile xls, int row, int col)
        {
            TFlxFormat tf4 = xls.GetCellVisibleFormatDef(row, col);

            TFlxFont tfont = new TFlxFont();
            tfont.Style = TFlxFontStyles.Bold;

            tf4.Font = tfont;

            TFlxFillPattern tfpattern3 = new TFlxFillPattern();
            tfpattern3.BgColor = Colors.LightGray;
            tf4.FillPattern = tfpattern3;

            return xls.AddFormat(tf4);
        }

        private int NoColorBold(XlsFile xls, int row, int col)
        {
            TFlxFormat tf4 = xls.GetCellVisibleFormatDef(row, col);
            TFlxFont tfont = new TFlxFont();
            tfont.Style = TFlxFontStyles.Bold;

            tf4.Font = tfont;
            return xls.AddFormat(tf4);
        }

        private int ColorAndBottomBorder(XlsFile xls, int row, int col)
        {
            TFlxFormat tf4 = xls.GetCellVisibleFormatDef(row, col);

            TFlxBorders tfBorderF1 = new TFlxBorders();

            TFlxOneBorder oborder1 = new TFlxOneBorder();
            oborder1.Style = TFlxBorderStyle.Thin;

            tfBorderF1.Bottom = oborder1;

            tf4.Borders = tfBorderF1;

            TFlxFillPattern tfpattern3 = new TFlxFillPattern();
            tfpattern3.BgColor = Colors.LightGray;
            tf4.FillPattern = tfpattern3;

            return xls.AddFormat(tf4);
        }

        private int NoColorAndBottomBorder(XlsFile xls, int row, int col)
        {
            TFlxFormat tf3 = xls.GetCellVisibleFormatDef(row, col);
            tf3.HAlignment = THFlxAlignment.left;
            TFlxBorders tfBorderF = new TFlxBorders();
            TFlxOneBorder oborder = new TFlxOneBorder();
            oborder.Style = TFlxBorderStyle.Thin;
            tfBorderF.Bottom = oborder;

            tf3.Borders = tfBorderF;

            return xls.AddFormat(tf3);

        }

        private int TopBorderOnly(XlsFile xls, int row, int col)
        {
            TFlxFormat tf3 = xls.GetCellVisibleFormatDef(row, col);

            TFlxBorders tfBorderF = new TFlxBorders();
            TFlxOneBorder oborder = new TFlxOneBorder();
            oborder.Style = TFlxBorderStyle.Thin;
            tfBorderF.Top = oborder;

            tf3.Borders = tfBorderF;

            return xls.AddFormat(tf3);

        }
      
        

        public void Report_3(IEnumerable<GetReport3_Levy_Products_Result> daily_trans, string filterType, string exchange_rate, string typ)
        {

            //var bK = daily_trans;
            //if (bK.Count() <= 0)
            //{
            //    TempData["Warning"] = "No Data To Export";
            //    return;
            //}

            string Orginal_file_path = string.Empty;

            if (typ.Equals("pdf"))
                Orginal_file_path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Report_Templates"), "Report_3_B.xlsx");

            else
                Orginal_file_path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Report_Templates"), "Report_3_A.xlsx");

            //temp file name 
            string copy_file_name = DateTime.Now.Ticks + ".xlsx";

            //temp file path
            string copy_file_path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Images"), copy_file_name);
            //copy to temp destination 

            XlsFile report_3 = new XlsFile(Orginal_file_path);

            int c = 0;
            if (!typ.Equals("pdf"))
                c = 1;

            report_3.ActiveSheet = 1;

            report_3.SetCellValue(3, 3 + c, GetDecimal(exchange_rate));
            report_3.SetCellValue(2, 1 + c, filterType);

            //get color 1
            int col1 = report_3.AddFormat(GetColor1(report_3.GetCellVisibleFormatDef(6, 2)));

            //get colo1 2
            int col2 = report_3.AddFormat(GetColor2(report_3.GetCellVisibleFormatDef(6, 2)));

            int style = col1;

            int i = 6;
            string category_1 = string.Empty;

            int dataExist = 0;

            int j = 0;
            foreach (var item in daily_trans)
            {
                dataExist = 1;
                if (i != 6 && category_1.Equals(item.Category_Name))
                    report_3.MergeCells(i - 1, 1, i, 1);

                else
                {
                    if (style == col1)
                        style = col2;
                    else
                        style = col1;
                }

                //style = res_arr[j];

                category_1 = item.Category_Name;

                report_3.SetCellValue(i, 1 + c, item.Category_Name, style);


                if (item.Goods_Name.Equals("All"))
                {
                    //get current style and add bold
                    TFlxFormat tformat = GetBold(report_3.GetCellVisibleFormatDef(i, 1 + c));
                    int res2 = report_3.AddFormat(tformat);
                    //end  style modification

                    report_3.SetCellValue(i, 2 + c, item.Goods_Name, res2);
                    report_3.SetCellValue(i, 3 + c, item.Unit, res2);
                    report_3.SetCellValue(i, 4 + c, item.Total_Quantity, res2);
                    report_3.SetCellValue(i, 5 + c, item.Total_Amount_in_SOS, res2);
                    report_3.SetCellValue(i, 6 + c, item.Total_Amount_in_USD, res2);

                }
                else
                {
                    report_3.SetCellValue(i, 2 + c, item.Goods_Name, style);
                    report_3.SetCellValue(i, 3 + c, item.Unit, style);
                    report_3.SetCellValue(i, 4 + c, item.Total_Quantity, style);
                    report_3.SetCellValue(i, 5 + c, item.Total_Amount_in_SOS, style);
                    report_3.SetCellValue(i, 6 + c, item.Total_Amount_in_USD, style);

                }


                i++;

            }

            if (dataExist <= 0)
            {
                TempData["Warning"] = "No Data To Export";
                return;
            }
            else
            {


                report_3.PrintToFit = true;
                report_3.PrintPaperSize = TPaperSize.A4;

                TWorkbookProtectionOptions protoptions = new TWorkbookProtectionOptions();
                protoptions.Window = false;
                protoptions.Structure = false;

                TSharedWorkbookProtectionOptions op = new TSharedWorkbookProtectionOptions();
                op.SharingWithTrackChanges = false;


                report_3.Protection.SetSharedWorkbookProtection("XXXX", op);
                report_3.Protection.SetWorkbookProtection("XXXX", protoptions);
                report_3.Protection.SetModifyPassword("XXXX", true, "XXXXXX");
                report_3.Protection.SetWorkbookProtectionOptions(protoptions);

                report_3.Save(copy_file_path);

                if (typ.Equals("pdf"))
                    GeneratePDF(report_3, "Report_3_"+filterType);
                else
                {
                    Response.AppendHeader("Content-Disposition", "attachment; filename=Report_3_"+filterType+".xlsx");
                    Response.TransmitFile(copy_file_path);
                    Response.End();
                }

            }

        }

    
        //public void Report_4(DateTime Report_date, string typ)
        //{
        //    string last_date = "9999/12/31";
        //    var exchangerate = (from d in db.Exchange_Rate where d.end_date == last_date && d.from_currency_id == 100 && d.to_currency_id == 106 select d.exchange_rate1).FirstOrDefault();//db.GetExchangeRate(dtExchangeRateDate).FirstOrDefault();

        //    string Orginal_file_path = string.Empty;

        //    if (!typ.Equals("pdf"))
        //        Orginal_file_path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Report_Templates"), "Report_4_B.xlsx");

        //    else
        //        Orginal_file_path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Report_Templates"), "Report_4_A.xlsx");


        //    //temp file name 
        //    string copy_file_name = DateTime.Now.Ticks + ".xlsx";
        //    //temp file path
        //    string copy_file_path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Images"), copy_file_name);

        //    //open file
        //    XlsFile report_4 = new XlsFile(Orginal_file_path);

        //    //sheet always 1 
        //    report_4.ActiveSheet = 1;

        //    int tformat_head = report_4.AddFormat(report_4.GetCellVisibleFormatDef(10, 3));
        //    int tformat_col1 = report_4.AddFormat(report_4.GetCellVisibleFormatDef(12, 3));
        //    int tformat_col2 = report_4.AddFormat(report_4.GetCellVisibleFormatDef(12, 4));


        //    string Header = string.Empty;

        //    Header = "Daily Bolleto Dogonale Report     :" + Report_date.ToString("dd/MMM/yyyy");

        //    report_4.SetCellValue(10, 3, Header, tformat_head);


        //    //get import data
        //    var daily_trans_import = db.Report_4_Import(Report_date);

        //    //get import data count
        //    var daily_trans_import_count = db.Report_4_Import(Report_date).Count();

        //    //skip if no data
        //    if (daily_trans_import_count <= 0)
        //        goto skipImport;


        //    TFlxFormat tf = report_4.GetCellVisibleFormatDef(12, 3);
        //    tf.HAlignment = THFlxAlignment.center;
        //    int tformat_BldandCntr = report_4.AddFormat(tf);

        //    TFlxFormat tf2 = report_4.GetCellVisibleFormatDef(12, 4);
        //    tf2.HAlignment = THFlxAlignment.center;
        //    int tformat_BldandCntr2 = report_4.AddFormat(tf2);



        //    //  DataTable dtRows = LinqQueryToDataTable(daily_trans.ToList());
        //    int i = 12;
        //    int s = 1;



        //    foreach (var item in daily_trans_import)
        //    {


        //        report_4.SetCellValue(i, 3, s.ToString(), tformat_BldandCntr);
        //        report_4.SetCellValue(i, 4, item.Code, tformat_BldandCntr2);

        //        report_4.SetCellValue(i, 5, item.Name, tformat_col1);
        //        report_4.SetCellValue(i, 6, item.Status, tformat_BldandCntr2);

        //        report_4.SetCellValue(i, 7, item.USD, tformat_col1);
        //        report_4.SetCellValue(i, 8, item.SOS, tformat_col2);


        //        i++;
        //        s++;


        //    }



        //    //skiped import 
        //skipImport:

        //    int tformat_head2 = report_4.AddFormat(report_4.GetCellVisibleFormatDef(10, 3));
        //    int tformat_col12 = report_4.AddFormat(report_4.GetCellVisibleFormatDef(12, 3));
        //    int tformat_col22 = report_4.AddFormat(report_4.GetCellVisibleFormatDef(12, 4));

        //    string Header2 = string.Empty;

        //    Header = "Daily Bolleto Dogonale Report     :" + Report_date.ToString("dd/MMM/yyyy");

        //    report_4.SetCellValue(10, 3, Header2, tformat_head2);

        //    TFlxFormat tfa = report_4.GetCellVisibleFormatDef(12, 3);
        //    tfa.HAlignment = THFlxAlignment.center;
        //    int tformat_BldandCntra = report_4.AddFormat(tfa);

        //    TFlxFormat tf2a = report_4.GetCellVisibleFormatDef(12, 4);
        //    tf2a.HAlignment = THFlxAlignment.center;
        //    int tformat_BldandCntr2a = report_4.AddFormat(tf2a);




        //    //get export dtaa
        //    var daily_trans_export = db.Report_4_Export(Report_date);

        //    //get export count
        //    var daily_trans_export_count = db.Report_4_Export(Report_date).Count();

        //    report_4.ActiveSheet = 2;

        //    //skip export if no data
        //    if (daily_trans_export_count <= 0)
        //        goto skipExport;

        //    //print export
        //    i = 12;
        //    s = 1;


        //    foreach (var item in daily_trans_export)
        //    {


        //        report_4.SetCellValue(i, 3, s.ToString(), tformat_BldandCntra);
        //        report_4.SetCellValue(i, 4, item.Code, tformat_BldandCntr2a);

        //        report_4.SetCellValue(i, 5, item.Name, tformat_col12);
        //        report_4.SetCellValue(i, 6, item.Status, tformat_BldandCntr2a);

        //        report_4.SetCellValue(i, 7, item.USD, tformat_col12);
        //        report_4.SetCellValue(i, 8, item.SOS, tformat_col22);


        //        i++;
        //        s++;


        //    }



        //    //skipped export
        //skipExport:

        //    //if no import and export return alert msg
        //    if (daily_trans_import_count <= 0 && daily_trans_export_count <= 0)
        //    {
        //        TempData["Warning"] = "No Data To Export";
        //        return;
        //    }
        //    else
        //    {

        //        //else print

        //        report_4.PrintToFit = true;
        //        report_4.PrintPaperSize = TPaperSize.A4;

        //        TWorkbookProtectionOptions protoptions = new TWorkbookProtectionOptions();
        //        protoptions.Window = false;
        //        protoptions.Structure = false;
        //        TSharedWorkbookProtectionOptions op = new TSharedWorkbookProtectionOptions();
        //        op.SharingWithTrackChanges = false;

        //        report_4.Protection.SetSharedWorkbookProtection("XXXX", op);

        //        report_4.Protection.SetWorkbookProtection("XXXX", protoptions);
        //        report_4.Protection.SetModifyPassword("XXXX", true, "XXXXXX");
        //        report_4.Protection.SetWorkbookProtectionOptions(protoptions);


        //        report_4.Save(copy_file_path);

        //        if (typ.Equals("pdf"))
        //            GeneratePDF(report_4, "Report_4");

        //        else
        //        {
        //            Response.AppendHeader("Content-Disposition", "attachment; filename=Report_4.xlsx");
        //            Response.TransmitFile(copy_file_path);
        //            Response.End();
        //        }

        //    }

        //}

        public void Report_4(IEnumerable<Report_4_Import_Result> daily_trans_import, IEnumerable<Report_4_Export_Result> daily_trans_Export, string filterType, string exchange_rate, string typ, string copy_file_path, string Orginal_file_path)
        {
             
            int c = 1;

           
            //open File
            XlsFile report_4 = new XlsFile(Orginal_file_path);

            //sheet always 1 
            report_4.ActiveSheet = 1;

            int tformat_head = report_4.AddFormat(report_4.GetCellVisibleFormatDef(10, 3));
            int tformat_col1 = report_4.AddFormat(report_4.GetCellVisibleFormatDef(12, 3));
            int tformat_col2 = report_4.AddFormat(report_4.GetCellVisibleFormatDef(12, 4));
            int tformat_col3 = report_4.AddFormat(report_4.GetCellVisibleFormatDef(12, 5));
            int tformat_col4 = report_4.AddFormat(report_4.GetCellVisibleFormatDef(12, 6));
            int tformat_col5 = report_4.AddFormat(report_4.GetCellVisibleFormatDef(12, 7));
            int tformat_col6 = report_4.AddFormat(report_4.GetCellVisibleFormatDef(12, 8));

            string Header = string.Empty;

            Header = "Daily Bolleto Dogonale Report     :" + filterType;

            report_4.SetCellValue(10, 3, Header, tformat_head);

            DataTable dtImport = new DataTable();

            //get import data
           // var daily_trans_import = db.Report_4_Import(Report_date);

            //get import data count
            try
            {
                if (daily_trans_import != null)
                    dtImport = LinqQueryToDataTable(daily_trans_import.ToList());

                if (dtImport.Rows.Count <= 1)
                    goto skip_Import;

            }
            catch
            { goto skip_Import; }


               int i = 12;
            int s = 1;
            c = 2;
            int datarow = 12;
            foreach (DataRow dr in dtImport.Rows)
            {

                if (dr[1].ToString().Length > 0)
                {
                     
                    report_4.SetCellValue(i, 1 + c, s, tformat_col1);
                    report_4.SetCellValue(i, 2 + c, dr[3].ToString(), tformat_col2);
                    report_4.SetCellValue(i, 3 + c, dr[4].ToString(), tformat_col3);
                    report_4.SetCellValue(i, 4 + c, dr[5].ToString(), tformat_col4);
                    report_4.SetCellValue(i, 5 + c, GetDecimal(dr[1].ToString()), tformat_col5);
                    report_4.SetCellValue(i, 6 + c, GetDecimal(dr[2].ToString()), tformat_col6);
                    datarow++;
                }
                i++;
                s++;

            }

            


        skip_Import:

          
          
            Header = "Daily Bolleto Dogonale Report     :" + filterType;

            DataTable dtexport = new DataTable();

          
            

            //get export dtaa
           
            report_4.ActiveSheet = 2;

            report_4.SetCellValue(10, 3, Header, tformat_head);


            //skip export if no data
            try
            {
                if (daily_trans_Export != null)
                    dtexport = LinqQueryToDataTable(daily_trans_Export.ToList());

                if (dtexport.Rows.Count <= 1)
                    goto skipExport;

            }
            catch
            { goto skipExport; }

            //print export
            i = 12;
            s = 1;

            datarow = 12;
            foreach (DataRow dr in dtexport.Rows)
            {

                if (dr[1].ToString().Length > 0)
                {

                    report_4.SetCellValue(i, 1 + c, s, tformat_col1);
                    report_4.SetCellValue(i, 2 + c, dr[3].ToString(), tformat_col2);
                    report_4.SetCellValue(i, 3 + c, dr[4].ToString(), tformat_col3);
                    report_4.SetCellValue(i, 4 + c, dr[5].ToString(), tformat_col4);
                    report_4.SetCellValue(i, 5 + c, GetDecimal(dr[1].ToString()), tformat_col5);
                    report_4.SetCellValue(i, 6 + c, GetDecimal(dr[2].ToString()), tformat_col6);

                   
                    datarow++;
                }
                i++;
                s++;
            }

            

      //doing the rest if data is null
        skipExport:

            //if no import and export return alert msg
            if (dtImport.Rows.Count <= 1)
                TempData["Warning"] = "No Data To Export";
            else
            {
                report_4.Save(copy_file_path);


                if (typ.Equals("pdf"))
                    GeneratePDF(report_4, "Report_4_"+filterType);

                else
                {
                    Response.AppendHeader("Content-Disposition", "attachment; filename=Report_4_"+filterType+".xlsx");
                    Response.TransmitFile(copy_file_path);
                    Response.End();
                }
            }

        }

        #endregion

        #region "Cell Format"
        public TFlxFormat GetColor1(TFlxFormat obj)
        {
            TFlxFillPattern pattern = new TFlxFillPattern();
            pattern.FgColor = Colors.White;
            pattern.Pattern = TFlxPatternStyle.Solid;

            TFlxBorders border = new TFlxBorders();
            border.SetAllBorders(TFlxBorderStyle.Thin, Colors.Black);

            obj.FillPattern = pattern;
            obj.Borders = border;

            return obj;
        }

        public TFlxFormat GetColor2(TFlxFormat obj)
        {
            TFlxFillPattern pattern = new TFlxFillPattern();
            pattern.FgColor = Colors.LightGray;
            pattern.Pattern = TFlxPatternStyle.Solid;

            TFlxBorders border = new TFlxBorders();
            border.SetAllBorders(TFlxBorderStyle.Thin, Colors.Black);

            obj.FillPattern = pattern;
            obj.Borders = border;

            return obj;
        }

        public TFlxFormat GetBold(TFlxFormat obj)
        {
            TFlxFont tfont = new TFlxFont();
            tfont.Style = TFlxFontStyles.Bold;
            
            obj.Font = tfont;

            return obj;
        }
        #endregion

        #region "PDF"
        public void GeneratePDF(XlsFile xls, string Rep_name)
        {
            //create temp directory
            string merge_directory = DateTime.Now.Ticks.ToString();
            string merge_directory_path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/MergePDF"), merge_directory);
            System.IO.Directory.CreateDirectory(merge_directory_path);

            //ExportPdf(xls, filepath, Rep_name);
            //XlsFile xls = new XlsFile(path);

            xls.ActiveSheet = 1;

            string copy_file_name_import = DateTime.Now.Ticks + ".pdf";

            //temp file path
            string copy_file_path_import = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/MergePDF/" + merge_directory), copy_file_name_import);

            FlexCelPdfExport flexpdf = new FlexCelPdfExport();
            flexpdf.Workbook = xls;
            flexpdf.AllowOverwritingFiles = true;
            flexpdf.Export(copy_file_path_import);



            if (!Rep_name.ToLower().Contains("report_1_") && !Rep_name.ToLower().Contains("report_3_"))
            {
                xls.ActiveSheet = 2;

                string copy_file_name_export = DateTime.Now.Ticks + ".pdf";

                //temp file path
                string copy_file_path_export = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/MergePDF/" + merge_directory), copy_file_name_export);

                flexpdf = new FlexCelPdfExport();
                flexpdf.Workbook = xls;
                flexpdf.AllowOverwritingFiles = true;
                flexpdf.Export(copy_file_path_export);

            }
            string copy_file_name_target = DateTime.Now.Ticks + ".pdf";

            string copy_file_path_target = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Images"), copy_file_name_target);

            CreateMergedPDF(copy_file_path_target, merge_directory_path);
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Rep_name + ".pdf");
            Response.TransmitFile(copy_file_path_target);
            Response.End();
        }


        static void CreateMergedPDF(string targetPDF, string sourceDir)
        {
            using (FileStream stream = new FileStream(targetPDF, FileMode.Create))
            {
                Document pdfDoc = new Document(PageSize.A4);
                PdfCopy pdf = new PdfCopy(pdfDoc, stream);
                pdfDoc.Open();
                var files = Directory.GetFiles(sourceDir);

                int i = 1;
                foreach (string file in files)
                {
                    int cnt = file.Length;
                    pdf.AddDocument(new PdfReader(file));
                    i++;
                }

                if (pdfDoc != null)
                    pdfDoc.Close();


            }

            
        }

     



        #endregion

        #region "Transpose"
        private DataTable GenerateTransposedTable(DataTable inputTable)
        {
            DataTable outputTable = new DataTable();

            // Add columns by looping rows

            // Header row's first column is same as in inputTable
            outputTable.Columns.Add(inputTable.Columns[0].ColumnName.ToString());

            // Header row's second column onwards, 'inputTable's first column taken
            foreach (DataRow inRow in inputTable.Rows)
            {
                string newColName = inRow[0].ToString();
                outputTable.Columns.Add(newColName);
            }

            // Add rows by looping columns        
            for (int rCount = 1; rCount <= inputTable.Columns.Count - 1; rCount++)
            {
                DataRow newRow = outputTable.NewRow();

                // First column is inputTable's Header row's second column
                newRow[0] = inputTable.Columns[rCount].ColumnName.ToString();
                for (int cCount = 0; cCount <= inputTable.Rows.Count - 1; cCount++)
                {
                    string colValue = inputTable.Rows[cCount][rCount].ToString();
                    newRow[cCount + 1] = colValue;
                }
                outputTable.Rows.Add(newRow);
            }

            return outputTable;
        }

        #endregion

        #region "var to DataTable"

        public List<SelectListItem> FilterTypeDDL()
        {
            List<SelectListItem> siList = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            si.Value = "0";
            si.Text = "--select--";
            siList.Add(si);
            si = new SelectListItem();
            si.Value = "1";
            si.Text = "Daily";
            siList.Add(si);
            si = new SelectListItem();
            si.Value = "2";
            si.Text = "Monthly";
            siList.Add(si);
            si = new SelectListItem();
            si.Value = "3";
            si.Text = "Yearly";
            siList.Add(si);
            si = new SelectListItem();
            si.Value = "4";
            si.Text = "Between Intervals";
            siList.Add(si);



            return siList;
        }

        public List<SelectListItem> Month_DDL()
        {
            List<SelectListItem> siList = new List<SelectListItem>();
            SelectListItem si = new SelectListItem();
            si.Value = "01";
            si.Text = "JANUARY";
            siList.Add(si);
            si = new SelectListItem();
            si.Value = "02";
            si.Text = "FEBRUARY";
            siList.Add(si);
            si = new SelectListItem();
            si.Value = "03";
            si.Text = "MARCH";
            siList.Add(si);
            si = new SelectListItem();
            si.Value = "04";
            si.Text = "APRIL";
            siList.Add(si);
            si = new SelectListItem();
            si.Value = "05";
            si.Text = "MAY";
            siList.Add(si);

            si = new SelectListItem();
            si.Value = "06";
            si.Text = "JUNE";
            siList.Add(si);

            si = new SelectListItem();
            si.Value = "07";
            si.Text = "JULY";
            siList.Add(si);

            si = new SelectListItem();
            si.Value = "08";
            si.Text = "AUGUST";
            siList.Add(si);

            si = new SelectListItem();
            si.Value = "09";
            si.Text = "SEPTEMBER";
            siList.Add(si);

            si = new SelectListItem();
            si.Value = "10";
            si.Text = "OCTOBER";
            siList.Add(si);

            si = new SelectListItem();
            si.Value = "11";
            si.Text = "NOVEMBER";
            siList.Add(si);

            si = new SelectListItem();
            si.Value = "12";
            si.Text = "DECEMBER";
            siList.Add(si);
            return siList;
        }


        public static DataTable LinqQueryToDataTable(IEnumerable<dynamic> v)
        {
            //We really want to know if there is any data at all
            var firstRecord = v.FirstOrDefault();
            if (firstRecord == null)
                return null;

            /*Okay, we have some data. Time to work.*/

            //So dear record, what do you have?
            PropertyInfo[] infos = firstRecord.GetType().GetProperties();

            //Our table should have the columns to support the properties
            DataTable table = new DataTable();

            //Add, add, add the columns
            foreach (var info in infos)
            {

                Type propType = info.PropertyType;

                if (propType.IsGenericType
                    && propType.GetGenericTypeDefinition() == typeof(Nullable<>)) //Nullable types should be handled too
                {
                    table.Columns.Add(info.Name, Nullable.GetUnderlyingType(propType));
                }
                else
                {
                    table.Columns.Add(info.Name, info.PropertyType);
                }
            }

            //Hmm... we are done with the columns. Let's begin with rows now.
            DataRow row;

            foreach (var record in v)
            {
                row = table.NewRow();
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    row[i] = infos[i].GetValue(record) != null ? infos[i].GetValue(record) : DBNull.Value;
                }

                table.Rows.Add(row);
            }

            //Table is ready to serve.
            table.AcceptChanges();

            return table;
        }

        #endregion

        #region "ManageOlderFiles"
        public void DeleteOlderFiles()
        {

            //delete temp directories
            DirectoryInfo[] mergeDirectory = new DirectoryInfo(Server.MapPath("~/MergePDF")).GetDirectories();
            foreach (DirectoryInfo foundFile in mergeDirectory)
            {
                if (foundFile.CreationTime.AddDays(1) <= DateTime.Now)
                {
                    DirectoryInfo hdDirectory = new DirectoryInfo(Server.MapPath("~/MergePDF/" + foundFile));

                    FileInfo[] filesInDir11 = hdDirectory.GetFiles();

                    foreach (FileInfo foundFile1 in filesInDir11)
                    {
                        foundFile1.Delete();
                    }
                    foundFile.Delete();
                }
            }

            //dlete temp files
            DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(Server.MapPath("~/Images"));

            FileInfo[] filesInDir1 = hdDirectoryInWhichToSearch.GetFiles("*.pdf*");

            FileInfo[] filesInDir2 = hdDirectoryInWhichToSearch.GetFiles("*.xlsx*");

            foreach (FileInfo foundFile in filesInDir1)
            {
                if (foundFile.CreationTime.AddDays(1) <= DateTime.Now)
                    foundFile.Delete();

            }

            foreach (FileInfo foundFile in filesInDir2)
            {
                if (foundFile.CreationTime.AddDays(1) <= DateTime.Now)
                    foundFile.Delete();
            }
        }


        #endregion

   
    }




}
