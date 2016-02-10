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
    public class Examination_UnitController : Controller
    {

        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

        //
        // GET: /Examination_Unit/

        public ActionResult Index(int way_bill_id = 0)
        {
            Session["examination"] = way_bill_id;
            if (way_bill_id != 0)
            {
                var way_bill = (from w in db.Way_Bill
                                join i in db.Imports on w.import_id equals i.import_id
                                join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                join ist in db.Importing_Status on i.importing_status_id equals ist.importing_status_id
                                where w.way_bill_id == way_bill_id && (i.importing_status_id == 3 || i.importing_status_id == 15)
                                select new Manifest_Control_SectionModel { way_bill_code = w.way_bill_code, import_name = i.import_code, ship_arrival_code = sa.ship_arrival_code, way_bill_id = w.way_bill_id, importing_status = i.importing_status_id }).Distinct();
                return View(way_bill.ToList());
            }
            else
            {
                var way_bill = (from w in db.Way_Bill
                                join i in db.Imports on w.import_id equals i.import_id
                                join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                join ist in db.Importing_Status on i.importing_status_id equals ist.importing_status_id
                                where i.importing_status_id == 3 || i.importing_status_id == 15
                                select new Manifest_Control_SectionModel { way_bill_code = w.way_bill_code, import_name = i.import_code, ship_arrival_code = sa.ship_arrival_code, way_bill_id = w.way_bill_id, importing_status = i.importing_status_id }).Distinct();
                return View(way_bill.ToList());
            }
        }
        //
        // GET: /Examination_Unit/Search_Way_Bill

        public ActionResult Search_Way_Bill()
        {
            ViewBag.way_bill = new HomeController().Way_Bill_Examination_Unit();
            return View();
        }

        //
        // POST: /Examination_Unit/Search_Way_Bill

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search_Way_Bill(Manifest_Control_SectionModel manifest)
        {
            return RedirectToAction("Index", new { way_bill_id = manifest.way_bill_id });
        }

        //
        // GET: /Examination_Unit/View_Way_Bill/10

        public ActionResult Details(int way_bill_id)
        {
            var way_bill_code = (from w in db.Way_Bill where w.way_bill_id == way_bill_id select w.way_bill_code).First();
            TempData["way_bill_code"] = way_bill_code;
            var way_bill = (from w in db.Way_Bill
                            join wd in db.Way_Bill_Details on w.way_bill_id equals wd.way_bill_id
                            join i in db.Importers on w.importer_id equals i.importer_id
                            join g in db.Goods on wd.goods_id equals g.goods_id
                            join u in db.Unit_Of_Measure on wd.unit_of_measure_id equals u.unit_id
                            where wd.way_bill_id == way_bill_id
                            select new Way_BillModel { goods = g.goods_name, units = wd.units, quantity = wd.quantity, unit_of_measure = u.unit_code, total_quantity = wd.total_quantity });
            return View(way_bill.ToList());
        }

        //
        // GET: /Examination_Unit/Reject_Reason_View/5

        public ActionResult Reject_Reason_View(int way_bill_id)
        {
            var reason = (from r in db.Reject_Reason
                          join w in db.Way_Bill on r.way_bill_id equals w.way_bill_id
                          join eu in db.EU_Check on r.way_bill_id equals eu.way_bill_id
                          where r.way_bill_id == way_bill_id
                          orderby r.reject_reason_id descending
                          select new Reject_ReasonModel { reject_reason_id = r.reject_reason_id, way_bill_id = r.way_bill_id, way_bill_code = w.way_bill_code, reason = r.reject_reason, rejected_from = r.reject_from, rejected_date = r.rejected_date, rechecked_by = eu.eu_checked_by });
            return View(reason.ToList());
        }

        //
        // GET: /Examination_Unit/Submit_Manifest/W0009

        public ActionResult Submit_Manifest(int way_bill_id)
        {
            var way_bill = (from w in db.Way_Bill
                            join i in db.Imports on w.import_id equals i.import_id
                            where w.way_bill_id == way_bill_id
                            select new { w.way_bill_id, i.import_id }).First();
            Import import = db.Imports.Find(way_bill.import_id);
            import.importing_status_id = 16;
            db.Entry(import).State = EntityState.Modified;
            db.SaveChanges();
            TempData["errorMessage"] = "Examination Unit Submitted";
            return RedirectToAction("Index");
        }

        //
        // GET: /Examination_Unit/Reject_Waybill

        public ActionResult Recheck_Waybill(int way_bill_id)
        {
            TempData["way_bill_id"] = way_bill_id;
            return View();
        }

        //
        // GET: /Examination_Unit/Recheck_Manifest/W0009

        public ActionResult Recheck_Manifest(Reject_ReasonModel reject_reason_model)
        {
            var way_bill_id = Convert.ToInt32(TempData["way_bill_id"]);
            db.Reject_Calculated_Tax(way_bill_id, 15, reject_reason_model.reason, "Examination_Unit_Recheck");
            EU_Check eu_check = new EU_Check();
            eu_check.way_bill_id = way_bill_id;
            eu_check.eu_checked_by = reject_reason_model.rechecked_by;
            eu_check.reject_number = 1; //1 represents recheck
            db.EU_Check.Add(eu_check);
            db.SaveChanges();
            TempData["errorMessage"] = "Examination Unit For Recheck";
            return RedirectToAction("Index");
        }

        //
        // GET: /Examination_Unit/Reject_Waybill

        public ActionResult Reject_Waybill(int way_bill_id)
        {
            TempData["way_bill_id"] = way_bill_id;
            return View();
        }

        //
        // GET: /Examination_Unit/Reject_Manifest/W0009

        public ActionResult Reject_Manifest(Reject_ReasonModel reject_reason_model)
        {           
            var way_bill_id = Convert.ToInt32(TempData["way_bill_id"]);
            var import_status = (from i in db.Imports
                                 where i.way_bill_id == way_bill_id
                                 select i.importing_status_id).First();
            db.Reject_Calculated_Tax(way_bill_id, 17, reject_reason_model.reason, "Examination_Unit_Reject");
            EU_Check eu_check = new EU_Check();
            eu_check.way_bill_id = way_bill_id;
            eu_check.eu_checked_by = reject_reason_model.rechecked_by;
            if (import_status == 15)
            {
                eu_check.reject_number = 2; //2 represents reject after recheck
            }
            else if (import_status == 3)
            {
                eu_check.reject_number = 3; //3 represents direct reject
            }
            db.EU_Check.Add(eu_check);
            db.SaveChanges();
            TempData["errorMessage"] = "Examination Unit Rejected";
            return RedirectToAction("Index");
        }

    }
}