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
    public class Manifest_Control_SectionController : Controller
    {

        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

        //
        // GET: /Manifest_Control_Section/

        public ActionResult Index(int way_bill_id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(17))
                {
                    Session["manifest"] = way_bill_id;
                    if (way_bill_id != 0)
                    {
                        var way_bill = (from w in db.Way_Bill
                                        join i in db.Imports on w.import_id equals i.import_id
                                        join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                        join ist in db.Importing_Status on i.importing_status_id equals ist.importing_status_id
                                        where w.way_bill_id == way_bill_id && (i.importing_status_id == 16 || i.importing_status_id == 20)
                                        orderby w.way_bill_id descending
                                        select new Manifest_Control_SectionModel { way_bill_code = w.way_bill_code, import_name = i.import_code, ship_arrival_code = sa.ship_arrival_code, way_bill_id = w.way_bill_id, importing_status_text = ist.importing_status, importing_status = i.importing_status_id }).Distinct();
                        return View(way_bill.ToList());
                    }
                    else
                    {
                        var way_bill = (from w in db.Way_Bill
                                        join i in db.Imports on w.import_id equals i.import_id
                                        join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                        join ist in db.Importing_Status on i.importing_status_id equals ist.importing_status_id
                                        where i.importing_status_id == 16 || i.importing_status_id == 20
                                        orderby w.way_bill_id descending
                                        select new Manifest_Control_SectionModel { way_bill_code = w.way_bill_code, import_name = i.import_code, ship_arrival_code = sa.ship_arrival_code, way_bill_id = w.way_bill_id, importing_status_text = ist.importing_status, importing_status = i.importing_status_id }).Distinct();
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
        // GET: /Manifest_Control_Section/Details

        public ActionResult Details(int way_bill_id)
        {
            Session["manifest"] = way_bill_id;
            var way_bill_code = (from w in db.Way_Bill
                             join i in db.Imports on w.import_id equals i.import_id
                             join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                             where w.way_bill_id == way_bill_id
                             select new { way_bill_code = w.way_bill_code, ship_arrival_id = sa.ship_arrival_id }).First();
            var way_bill = from w in db.Way_Bill
                           join wd in db.Way_Bill_Details on w.way_bill_id equals wd.way_bill_id
                           join g in db.Goods on wd.goods_id equals g.goods_id
                           join i in db.Imports on w.import_id equals i.import_id
                           join u in db.Unit_Of_Measure on wd.unit_of_measure_id equals u.unit_id
                           where wd.way_bill_id == way_bill_id && (i.importing_status_id == 16 || i.importing_status_id == 20)
                           select new Manifest_Control_SectionModel { way_bill_code = w.way_bill_code, goods = g.goods_name, unit_of_measure = u.unit_code, importing_status = i.importing_status_id, way_bill_id = w.way_bill_id, total_quantity = wd.total_quantity, is_damaged = wd.is_damaged };
            var tally_sheet = from t in db.Tally_Sheet
                              join td in db.Tally_Sheet_Details on t.tally_sheet_id equals td.tally_sheet_id
                              where td.way_bill_code == way_bill_code.way_bill_code && t.ship_arrival_id == way_bill_code.ship_arrival_id
                              select new Tally_SheetModel { way_bill_code = td.way_bill_code, goods = td.goods_name, unit_of_measure = td.unit_of_measure, total_quantity_ts = td.total_quantity, tally_sheet_code = t.tally_sheet_code };
            ViewBag.tally_sheet = tally_sheet;
            return View(way_bill.ToList());
        }

        //
        // GET: /Manifest_Control_Section/Search_Way_Bill

        public ActionResult Search_Way_Bill()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(17))
                {
                    ViewBag.way_bill = new HomeController().Way_Bill_Manifest();
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
        // POST: /Manifest_Control_Section/Search_Way_Bill
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search_Way_Bill( Manifest_Control_SectionModel manifest)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(17))
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
        // GET: /Manifest_Control_Section/Print_Waybill/W0001

        public ActionResult Print_Waybill(int id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(17))
                {
                    var way_bill_goods = (from w in db.Way_Bill
                                           join wd in db.Way_Bill_Details on w.way_bill_id equals wd.way_bill_id
                                           join i in db.Importers on w.importer_id equals i.importer_id
                                           join g in db.Goods on wd.goods_id equals g.goods_id
                                           join u in db.Unit_Of_Measure on wd.unit_of_measure_id equals u.unit_id
                                           where w.way_bill_id == id
                                           select new Way_BillModel { goods = g.goods_name, unit_of_measure = u.unit_code, total_quantity = wd.total_quantity, is_damaged = wd.is_damaged });
                    var waybill = (from w in db.Way_Bill 
                                   join i in db.Imports on w.import_id equals i.import_id
                                   join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                   where w.way_bill_id == id
                                   select new Bolleto_DogonaleModel { way_bill_code = w.way_bill_code, import_code = i.import_code, ship_arrival_code = sa.ship_arrival_code, way_bill_date = i.created_date }).Distinct();
                    ViewBag.waybill = waybill;
                    return View(way_bill_goods.ToList());
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
        // GET: /Manifest_Control_Section/Submit_Manifest/W0009

        public ActionResult Submit_Manifest(int way_bill_id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(17))
                {
                    var way_bill = (from w in db.Way_Bill
                                    join i in db.Imports on w.import_id equals i.import_id
                                    where w.way_bill_id == way_bill_id
                                    select new { w.way_bill_id, i.import_id }).First();
                    Import import = db.Imports.Find(way_bill.import_id);
                    import.importing_status_id = 3;
                    db.Entry(import).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["errorMessage"] = "Manifest Control Section Submitted";
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
        // GET: /Manifest_Control_Section/Reject_Reason_View/5

        public ActionResult Reject_Reason_View(int way_bill_id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(17))
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
        // GET: /Manifest_Control_Section/Reject_Waybill

        public ActionResult Reject_Waybill(int way_bill_id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(17))
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
        // GET: /Manifest_Control_Section/Reject_Manifest/W0009

        public ActionResult Reject_Manifest(Reject_ReasonModel reject_reason_model)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(17))
                {
                    var way_bill_id = Convert.ToInt32(TempData["way_bill_id"]);
                    var way_bill = (from w in db.Way_Bill
                                    join i in db.Imports on w.import_id equals i.import_id
                                    where w.way_bill_id == way_bill_id
                                    select new { w.way_bill_id, i.import_id, w.way_bill_code }).First();
                    Import import = db.Imports.Find(way_bill.import_id);
                    import.importing_status_id = 13;
                    db.Entry(import).State = EntityState.Modified;
                    db.SaveChanges();
                    Reject_Reason reject_reason = new Reject_Reason();
                    reject_reason.way_bill_id = way_bill_id;
                    reject_reason.reject_reason = reject_reason_model.reason;
                    reject_reason.reject_from = "Manifest Control Section";
                    reject_reason.rejected_date = DateTime.Now;
                    db.Reject_Reason.Add(reject_reason);
                    db.SaveChanges();
                    TempData["errorMessage"] = "Manifest Control Section Rejected";
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
