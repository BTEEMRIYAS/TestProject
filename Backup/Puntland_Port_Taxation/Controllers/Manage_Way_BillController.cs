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
    public class Manage_Way_BillController : Controller
    {
        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

        //
        // GET: /Manage_Way_Bill/

        public ActionResult Index()
        {
            var way_bill = (from w in db.Way_Bill
                            join i in db.Imports on w.import_id equals i.import_id
                            join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                            select new Way_BillModel { way_bill_code = w.way_bill_code, import_name = i.import_code, ship_arrival_code = sa.ship_arrival_code, import_status_id = i.importing_status_id, way_bill_id = w.way_bill_id });
            return View(way_bill.ToList());
        }

        //
        // GET: /Manage_Way_Bill/Details/W0001

        public ActionResult Details(int way_bill_id)
        {
            var way_bill = (from w in db.Way_Bill
                            join wd in db.Way_Bill_Details on w.way_bill_id equals wd.way_bill_id
                            join imps in db.Imports on w.import_id equals imps.import_id
                            join i in db.Importers on w.importer_id equals i.importer_id
                            join g in db.Goods on wd.goods_id equals g.goods_id
                            join u in db.Unit_Of_Measure on wd.unit_of_measure_id equals u.unit_id
                            where w.way_bill_id == way_bill_id
                            select new Way_BillModel { way_bill_id = w.way_bill_id, importer_name = i.importer_first_name, way_bill_code = w.way_bill_code, way_bill_name = w.way_bill_name, goods = g.goods_name, unit_of_measure = u.unit_code, total_quantity = wd.total_quantity, import_status_id = imps.importing_status_id, goods_id = wd.goods_id, way_bill_details_id = wd.way_bill_details_id, units = wd.units, quantity = wd.quantity });
            var way_bill_code = (from w in db.Way_Bill where w.way_bill_id == way_bill_id select w.way_bill_code).First();
            Session["way_bill_code"] = way_bill_code;
            Session["way_bill_id"] = way_bill_id;
            return View(way_bill.ToList());           
        }

        //
        // GET: /Manage_Way_Bill/View_Way_Bill/W0001

        public ActionResult View_Way_Bill(int id)
        {
            var way_bill = (from w in db.Way_Bill
                            join wd in db.Way_Bill_Details on w.way_bill_id equals wd.way_bill_id
                            join i in db.Importers on w.importer_id equals i.importer_id
                            join g in db.Goods on wd.goods_id equals g.goods_id
                            join u in db.Unit_Of_Measure on wd.unit_of_measure_id equals u.unit_id
                            where wd.way_bill_id == id
                            select new Way_BillModel { goods = g.goods_name, units = wd.units, quantity = wd.quantity, unit_of_measure = u.unit_code, total_quantity = wd.total_quantity });            
            var way_biil_code = (from w in db.Way_Bill where w.way_bill_id == id select w.way_bill_code).First();
            ViewBag.way_bill_code = way_biil_code;
            return View(way_bill.ToList());
        }

        //
        // GET: /Manage_Way_Bill/Create

        public ActionResult Create()
        {
            ViewBag.import = new HomeController().Import();
            return View();
        }

        //
        // POST: /Manage_Way_Bill/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Way_Bill way_bill)
        {
            var importer_id = 0;
            var importer = from i in db.Imports
                           join ir in db.Importers on i.importer_id equals ir.importer_id
                           where i.import_id == way_bill.import_id
                           select ir.importer_id;
            var way_bill_exist = (from w in db.Way_Bill
                                  where w.way_bill_code == way_bill.way_bill_code
                                  select new { w.way_bill_code});
            var import_exist = (from w in db.Way_Bill
                                where w.import_id == way_bill.import_id
                                select new { w.import_id });
            if (import_exist.Count() > 0)
            {
                TempData["errorMessage"] = "Only One Way Bill Allowed For an Import, Please Choose Another Import";
                return RedirectToAction("Index");
            }
            else if (way_bill_exist.Count() > 0)
            {
                TempData["errorMessage"] = "Way Bill Already Exists,Please Enter Another One";
                return RedirectToAction("Index");
            }
            foreach (var item in importer)
            {
                importer_id = item;
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

        //
        // GET: /Manage_Way_Bill/Create_Way_Bill

        public ActionResult Create_Way_Bill()
        {
            ViewBag.categories = new HomeController().Category();
            ViewBag.currency = new HomeController().Currency();
            ViewBag.unif_of_measures = new HomeController().Unit_Of_Measures();
            return View();
        }

        //Manage_Way_Bill/way_bill_already

        public int way_bill_already()
        {
            return 0;
        }

        //
        // POST: /Manage_Way_Bill/Create_Way_Bill

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create_Way_Bill(Way_BillModel way_billModel)
        {
            Way_Bill_Details way_bill_details = new Way_Bill_Details();
            var way_bill_id = Convert.ToInt32(Session["way_bill_id"]);
            if (ModelState.IsValid)
            {
                way_bill_details.way_bill_id = way_bill_id;
                way_bill_details.goods_id = way_billModel.goods_id;
                way_bill_details.units = way_billModel.units;
                way_bill_details.quantity = way_billModel.quantity;
                way_bill_details.unit_of_measure_id = way_billModel.unit_of_measure_id;
                way_bill_details.total_quantity = way_billModel.total_quantity;
                db.Way_Bill_Details.Add(way_bill_details);
                db.SaveChanges();
                return RedirectToAction("Details", new { way_bill_id = way_bill_id });
            }
            return RedirectToAction("Details", new { way_bill_id = way_bill_id });
        }

        //
        // GET: /Manage_Way_Bill/Edit/5

        public ActionResult Edit(int id = 0)
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
                                           select new { w.way_bill_name, w.import_id, w.importer_id, w.is_calculated, w.way_bill_code, w.way_bill_id }).First();
            var goods = from g in db.Goods
                        join gt in db.Goods_Type on g.goods_type_id equals gt.goods_type_id
                        join gs in db.Goods_Subcategory on gt.goods_subcategory_id equals gs.goods_subcategory_id
                        join gc in db.Goods_Category on gs.goods_category_id equals gc.goods_category_id
                        where g.goods_id == way_bill_details.goods_id
                        select new { g.goods_id, g.goods_name, gt.goods_type_id, gt.goods_type_name, gs.goods_subcategory_id, gs.goods_subcategory_name, gc.goods_category_id };
            ViewBag.subcategories = new SelectList(goods, "goods_subcategory_id", "goods_subcategory_name");
            ViewBag.goods_type = new SelectList(goods, "goods_type_id", "goods_type_name");
            ViewBag.goods = new SelectList(goods, "goods_id", "goods_name");
            TempData["id"] = way_bill_details.way_bill_details_id;
            TempData["way_bill_id"] = way_bill_details.way_bill_id;
            ViewBag.categories = new HomeController().Category();
            ViewBag.currency = new HomeController().Currency();
            ViewBag.unif_of_measures = new HomeController().Unit_Of_Measures();
            way_billModel.way_bill_code = way_bill_master_details.way_bill_code;
            way_billModel.import_id = way_bill_master_details.import_id;
            way_billModel.goods_category_id = goods.First().goods_category_id;
            way_billModel.goods_subcategory_id = goods.First().goods_subcategory_id;
            way_billModel.goods_type_id = goods.First().goods_type_id;
            way_billModel.goods_id = way_bill_details.goods_id;
            way_billModel.units = way_bill_details.units;
            way_billModel.quantity = way_bill_details.quantity;
            way_billModel.unit_of_measure_id = way_bill_details.unit_of_measure_id;
            way_billModel.total_quantity = way_bill_details.total_quantity;
            return View(way_billModel);
        }

        //
        // POST: /Manage_Way_Bill/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Way_BillModel way_billModel)
        {
            Way_Bill_Details way_bill_details = new Way_Bill_Details();
            int id = Convert.ToInt32(TempData["id"]);
            if (ModelState.IsValid)
            {
                way_bill_details.way_bill_details_id = id;
                way_bill_details.way_bill_id = Convert.ToInt32(TempData["way_bill_id"]);
                way_bill_details.goods_id = way_billModel.goods_id;
                way_bill_details.units = way_billModel.units;
                way_bill_details.quantity = way_billModel.quantity;
                way_bill_details.unit_of_measure_id = way_billModel.unit_of_measure_id;
                way_bill_details.total_quantity = way_billModel.total_quantity;
                way_bill_details.currency_id = way_billModel.currency_id;
                db.Entry(way_bill_details).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { way_bill_id = way_bill_details.way_bill_id });
            }
            return RedirectToAction("Details", new { way_bill_id = way_bill_details.way_bill_id });
        }

         //
        // GET: /Manage_Way_Bill/Submit_Way_Bill/W0009

        public ActionResult Submit_Way_Bill()
        {
            var way_bill_code = Session["way_bill_code"].ToString();
            var way_bill_id = Convert.ToInt32(Session["way_bill_id"]);
            var way_bill = (from w in db.Way_Bill
                            join i in db.Imports on w.import_id equals i.import_id
                             where w.way_bill_id == way_bill_id
                            select new { w.way_bill_id, i.import_id, i.importing_status_id }).First();
            Import import = db.Imports.Find(way_bill.import_id);
            import.way_bill_id = way_bill_id;
            if (way_bill.importing_status_id == 1)
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
            return RedirectToAction("Index");
        }

        //
        // GET: /Manage_Way_Bill/Reject_Reason_View/5

        public ActionResult Reject_Reason_View(int way_bill_id)
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

        //
        // GET: /Manage_Way_Bill/Delete/5

        public ActionResult Delete(int id = 0)
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

        //
        // POST: /Manage_Way_Bill/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var way_bill_id = Convert.ToInt32(Session["way_bill_id"]);
            Way_Bill_Details way_bill_details = db.Way_Bill_Details.Find(id);
            db.Way_Bill_Details.Remove(way_bill_details);
            db.SaveChanges();
            return RedirectToAction("Details", new { way_bill_id = way_bill_id });
        }

        //
        // GET: /Manage_Way_Bill/DbSearch

        public ActionResult DbSearch()
        {
            ViewBag.import = new HomeController().Import();
            ViewBag.ship_arrivals = new HomeController().Ship_Arrival();
            return View();
        }

        ////
        //// POST: /Manage_Way_Bill/DbSearchresult
        ////In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Way_BillModel way_bill)
        {
            //Queue q = new Queue();
            if ( way_bill.way_bill_code != null && way_bill.import_id != 0 && way_bill.ship_arrival_id != 0)
            {
                var result = (from w in db.Way_Bill
                              join i in db.Imports on w.import_id equals i.import_id
                              join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                              where w.way_bill_code == way_bill.way_bill_code && w.import_id == way_bill.import_id && i.ship_arrival_id == way_bill.ship_arrival_id
                              select new Way_BillModel { way_bill_code = w.way_bill_code, import_name = i.import_code, ship_arrival_code = sa.ship_arrival_code, import_status_id = i.importing_status_id, way_bill_id = w.way_bill_id });
                return View("Index", result.ToList());
            }
            else if (way_bill.way_bill_code != null && way_bill.import_id != 0 && way_bill.ship_arrival_id == 0)
            {
                var result = (from w in db.Way_Bill
                              join i in db.Imports on w.import_id equals i.import_id
                              join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                              where w.way_bill_code == way_bill.way_bill_code && w.import_id == way_bill.import_id
                              select new Way_BillModel { way_bill_code = w.way_bill_code, import_name = i.import_code, ship_arrival_code = sa.ship_arrival_code, import_status_id = i.importing_status_id, way_bill_id = w.way_bill_id });
                return View("Index", result.ToList());
            }
            else if (way_bill.way_bill_code != null && way_bill.import_id == 0 && way_bill.ship_arrival_id != 0)
            {
                var result = (from w in db.Way_Bill
                              join i in db.Imports on w.import_id equals i.import_id
                              join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                              where w.way_bill_code == way_bill.way_bill_code && i.ship_arrival_id == way_bill.ship_arrival_id
                              select new Way_BillModel { way_bill_code = w.way_bill_code, import_name = i.import_code, ship_arrival_code = sa.ship_arrival_code, import_status_id = i.importing_status_id, way_bill_id = w.way_bill_id });
                return View("Index", result.ToList());
            }
            else if (way_bill.way_bill_code != null && way_bill.import_id == 0 && way_bill.ship_arrival_id == 0)
            {
                var result = (from w in db.Way_Bill
                              join i in db.Imports on w.import_id equals i.import_id
                              join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                              where w.way_bill_code == way_bill.way_bill_code
                              select new Way_BillModel { way_bill_code = w.way_bill_code, import_name = i.import_code, ship_arrival_code = sa.ship_arrival_code, import_status_id = i.importing_status_id, way_bill_id = w.way_bill_id });
                return View("Index", result.ToList());
            }
            else if (way_bill.way_bill_code == null && way_bill.import_id != 0 && way_bill.ship_arrival_id != 0)
            {
                var result = (from w in db.Way_Bill
                              join i in db.Imports on w.import_id equals i.import_id
                              join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                              where w.import_id == way_bill.import_id && i.ship_arrival_id == way_bill.ship_arrival_id
                              select new Way_BillModel { way_bill_code = w.way_bill_code, import_name = i.import_code, ship_arrival_code = sa.ship_arrival_code, import_status_id = i.importing_status_id, way_bill_id = w.way_bill_id });
                return View("Index", result.ToList());
            }
            else if (way_bill.way_bill_code == null && way_bill.import_id != 0 && way_bill.ship_arrival_id == 0)
            {
                var result = (from w in db.Way_Bill
                              join i in db.Imports on w.import_id equals i.import_id
                              join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                              where w.import_id == way_bill.import_id
                              select new Way_BillModel { way_bill_code = w.way_bill_code, import_name = i.import_code, ship_arrival_code = sa.ship_arrival_code, import_status_id = i.importing_status_id, way_bill_id = w.way_bill_id });
                return View("Index", result.ToList());
            }
            else if (way_bill.way_bill_code == null && way_bill.import_id == 0 && way_bill.ship_arrival_id != 0)
            {
                var result = (from w in db.Way_Bill
                              join i in db.Imports on w.import_id equals i.import_id
                              join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                              where i.ship_arrival_id == way_bill.ship_arrival_id
                              select new Way_BillModel { way_bill_code = w.way_bill_code, import_name = i.import_code, ship_arrival_code = sa.ship_arrival_code, import_status_id = i.importing_status_id, way_bill_id = w.way_bill_id });
                return View("Index", result.ToList());
            }
            return RedirectToAction("Index");
            
        }

        //
        // GET: /Manage_Way_Bill/DbSearch_new

        public ActionResult DbSearch_new()
        {
            ViewBag.goods = new HomeController().Goods();
            return View();
        }

        ////
        //// POST: /Manage_Way_Bill/DbSearchresult_new
        ////In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult_new(Way_Bill_Details way_bill)
        {
            var way_bill_id = Convert.ToInt32(Session["way_bill_id"]);
            if (way_bill.goods_id != 0)
            {
                var result = (from w in db.Way_Bill
                              join wd in db.Way_Bill_Details on w.way_bill_id equals wd.way_bill_id
                              join imps in db.Imports on w.import_id equals imps.import_id
                              join i in db.Importers on w.importer_id equals i.importer_id
                              join g in db.Goods on wd.goods_id equals g.goods_id
                              join u in db.Unit_Of_Measure on wd.unit_of_measure_id equals u.unit_id
                              where wd.goods_id == way_bill.goods_id && wd.way_bill_id == way_bill_id
                              select new Way_BillModel { way_bill_id = w.way_bill_id, importer_name = i.importer_first_name, way_bill_code = w.way_bill_code, way_bill_name = w.way_bill_name, goods = g.goods_name, unit_of_measure = u.unit_code, total_quantity = wd.total_quantity, import_status_id = imps.importing_status_id, goods_id = wd.goods_id, way_bill_details_id = wd.way_bill_details_id, units = wd.units, quantity = wd.quantity });
                return View("Details", result.ToList());              
            }
            return RedirectToAction("Details", new { way_bill_id = way_bill_id });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}