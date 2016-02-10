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
    public class PaymentController : Controller
    {

        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

        //
        // GET: /Payment/

        public ActionResult Index(int way_bill_id = 0)
        {
            Session["payment"] = way_bill_id;
            var payment_details = db.Display_Payment(way_bill_id);
            return View(payment_details.ToList());
        }
        //
        // GET: /Payment/Search_Way_Bill

        public ActionResult Search_Way_Bill()
        {
            ViewBag.way_bill = new HomeController().Way_Bill_payment();
            return View();
        }

        //
        // POST: /Payment/Search_Way_Bill

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search_Way_Bill(Manifest_Control_SectionModel manifest)
        {
            return RedirectToAction("Index", new { way_bill_id = manifest.way_bill_id });
        }

        //
        // GET: /Payment/Submit_Manifest/W0009

        public ActionResult Submit_Manifest(int way_bill_id)
        {
                var way_bill = (from w in db.Way_Bill
                                join i in db.Imports on w.import_id equals i.import_id
                                where w.way_bill_id == way_bill_id
                                select new { w.way_bill_id, i.import_id }).First();
                Import import = db.Imports.Find(way_bill.import_id);
                import.importing_status_id = 10;
                import.payment_id = 2;
                db.Entry(import).State = EntityState.Modified;
                db.SaveChanges();
                TempData["errorMessage"] = "Payment Submitted";
                return RedirectToAction("Index");
        }

    }
}

