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
    public class ReleaseController : Controller
    {

        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

        //
        // GET: /Release/

        public ActionResult Index(int way_bill_id = 0)
        {
            Session["release"] = way_bill_id;
            if (way_bill_id != 0)
            {
                var way_bill = (from w in db.Way_Bill
                                join i in db.Imports on w.import_id equals i.import_id
                                join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                join ist in db.Importing_Status on i.importing_status_id equals ist.importing_status_id
                                where w.way_bill_id == way_bill_id
                                select new Manifest_Control_SectionModel { way_bill_code = w.way_bill_code, import_name = i.import_code, ship_arrival_code = sa.ship_arrival_code, bolleto_code = i.bollete_dogonale_code, way_bill_id = w.way_bill_id }).Distinct();
                return View(way_bill.ToList());
            }
            else
            {
                var way_bill = (from w in db.Way_Bill
                                join i in db.Imports on w.import_id equals i.import_id
                                join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                join ist in db.Importing_Status on i.importing_status_id equals ist.importing_status_id
                                where i.importing_status_id == 10
                                select new Manifest_Control_SectionModel { way_bill_code = w.way_bill_code, import_name = i.import_code, ship_arrival_code = sa.ship_arrival_code, bolleto_code = i.bollete_dogonale_code, way_bill_id = w.way_bill_id }).Distinct();
                return View(way_bill.ToList());
            }
        }

        //
        // GET: /Release/Details/W0001

        public ActionResult Details(int way_bill_id)
        { 
          
            var way_bill_code = (from w in db.Way_Bill where w.way_bill_id == way_bill_id select w.way_bill_code).First();
            TempData["way_bill_code"] = way_bill_code;
            var release = db.Display_Release(way_bill_id);
            return View(release.ToList());
        }

        //
        // GET: /Release/Search_Way_Bill

        public ActionResult Search_Way_Bill()
        {
            ViewBag.way_bill = new HomeController().Way_Bill_Release();
            return View();
        }

        //
        // POST: /Release/Search_Way_Bill

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search_Way_Bill(Manifest_Control_SectionModel manifest)
        {
            return RedirectToAction("Index", new { way_bill_id = manifest.way_bill_id });
        }

        //
        // GET: /Release/Print_Release/13

        public ActionResult Print_Release(int id)
        {
            var grand_total = db.Get_Grand_Total(id);
            foreach (var v in grand_total)
            {
                ViewBag.grand_total = v.Value;
            }
            var tax_calculation = db.Display_Tax_Details(id);
            var bollete_dogonale = (from c in db.Calculated_Levi
                                    join w in db.Way_Bill on c.way_bill_id equals w.way_bill_id
                                    join i in db.Imports on w.import_id equals i.import_id
                                    join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                    where c.way_bill_id == id
                                    select new Bolleto_DogonaleModel { way_bill_code = w.way_bill_code, import_code = i.import_code, ship_arrival_code = sa.ship_arrival_code, bolleto_dogonale_code = i.bollete_dogonale_code }).Distinct();
            ViewBag.bolleto = bollete_dogonale;
            return View(tax_calculation.ToList());
        }

        //
        // GET: /Release/Submit_Manifest/W0009

        public ActionResult Submit_Manifest(int way_bill_id)
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

    }
}
