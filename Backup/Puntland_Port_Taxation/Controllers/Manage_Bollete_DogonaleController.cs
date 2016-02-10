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
    public class Manage_Bollete_DogonaleController : Controller
    {
        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

        //
        // GET: /Manage_Bollete_Dogonale/

        public ActionResult Index()
        {
            var bollete_dogonale = (from c in db.Calculated_Levi
                                    join w in db.Way_Bill on c.way_bill_id equals w.way_bill_id
                                    join i in db.Imports on w.import_id equals i.import_id
                                    join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                    select new Bolleto_DogonaleModel { way_bill_code = w.way_bill_code, import_code = i.import_code, ship_arrival_code = sa.ship_arrival_code, bolleto_dogonale_code = i.bollete_dogonale_code, way_bill_id = w.way_bill_id }).Distinct();
            return View(bollete_dogonale.ToList());
        }

        //
        // GET: /Manage_Bollete_Dogonale/Details/5

        public ActionResult Details(int way_bill_id)
        {
            var grand_total = db.Get_Grand_Total(way_bill_id);
            foreach (var v in grand_total)
            {
                ViewBag.grand_total = v.Value;
            }
            var tax_calculation = db.Display_Tax_Details(way_bill_id);
            var bollete_dogonale = (from c in db.Calculated_Levi
                                    join w in db.Way_Bill on c.way_bill_id equals w.way_bill_id
                                    join i in db.Imports on w.import_id equals i.import_id
                                    join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                    where c.way_bill_id == way_bill_id
                                    select new Bolleto_DogonaleModel { way_bill_code = w.way_bill_code, import_code = i.import_code, ship_arrival_code = sa.ship_arrival_code, bolleto_dogonale_code = i.bollete_dogonale_code }).Distinct();
            ViewBag.bolleto = bollete_dogonale;
            return View(tax_calculation.ToList());
        }

        ////
        //// GET: /Manage_Bollete_Dogonale/Create

        //public ActionResult Create()
        //{
        //    ViewBag.way_bill = new HomeController().Way_Bill();
        //    ViewBag.import = new HomeController().Import();
        //    ViewBag.goods = new HomeController().Goods();
        //    ViewBag.currency = new HomeController().Currency();
        //    return View();
        //}

        //
        // POST: /Manage_Bollete_Dogonale/Create

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(Bolleto_Dogonale bolleto_dogonale)
        //{
        //    decimal levi1 = 0;
        //    decimal levi2 = 0;
        //    decimal levi3 = 0;
        //    decimal levi4 = 0;
        //    decimal levi5 = 0;
        //    decimal levi6 = 0;
        //    var levi = from b in db.Bolleto_Dogonale
        //               join g in db.Goods on b.goods_id equals g.goods_id
        //               join l in db.Levis on g.levi_id equals l.levi_id
        //               where b.goods_id == bolleto_dogonale.goods_id
        //               select l;
        //    foreach (var item in levi)
        //    {
        //        levi1 = item.levi_1;
        //        levi2 = item.levi_2;
        //        levi3 = item.levi_3;
        //        levi4 = item.levi_4;
        //        levi5 = item.levi_5;
        //        levi6 = item.levi_6;
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        bolleto_dogonale.levi_1 = levi1;
        //        bolleto_dogonale.calculated_levi_1 = (bolleto_dogonale.price * levi1) / 100;
        //        bolleto_dogonale.levi_2 = levi2;
        //        bolleto_dogonale.calculated_levi_2 = (bolleto_dogonale.price * levi2) / 100;
        //        bolleto_dogonale.levi_3 = levi3;
        //        bolleto_dogonale.calculated_levi_3 = (bolleto_dogonale.price * levi3) / 100;
        //        bolleto_dogonale.levi_4 = levi4;
        //        bolleto_dogonale.calculated_levi_4 = (bolleto_dogonale.price * levi4) / 100;
        //        bolleto_dogonale.levi_5 = levi5;
        //        bolleto_dogonale.calculated_levi_5 = (bolleto_dogonale.price * levi5) / 100;
        //        bolleto_dogonale.levi_6 = levi6;
        //        bolleto_dogonale.calculated_levi_6 = (bolleto_dogonale.price * levi6) / 100;
        //        bolleto_dogonale.total_tax = bolleto_dogonale.calculated_levi_1 + bolleto_dogonale.calculated_levi_2 + bolleto_dogonale.calculated_levi_3 + bolleto_dogonale.calculated_levi_4 + bolleto_dogonale.calculated_levi_5 + bolleto_dogonale.calculated_levi_6;
        //        bolleto_dogonale.total_payment = (bolleto_dogonale.total_tax)*bolleto_dogonale.quantity + Convert.ToInt32(bolleto_dogonale.any_other_amount);
        //        db.Bolleto_Dogonale.Add(bolleto_dogonale);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(bolleto_dogonale);
        //}

        ////
        //// GET: /Manage_Bollete_Dogonale/Edit/5

        //public ActionResult Edit(int id = 0)
        //{
        //    Bolleto_Dogonale bolleto_dogonale = db.Bolleto_Dogonale.Find(id);
        //    if (bolleto_dogonale == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    TempData["id"] = bolleto_dogonale.bolleto_dogonale_id;
        //    ViewBag.way_bill = new HomeController().Way_Bill();
        //    ViewBag.import = new HomeController().Import();
        //    ViewBag.goods = new HomeController().Goods();
        //    ViewBag.currency = new HomeController().Currency();
        //    return View(bolleto_dogonale);
        //}

        ////
        //// POST: /Manage_Bollete_Dogonale/Edit/5

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(Bolleto_Dogonale bolleto_dogonale)
        //{
        //    int id = Convert.ToInt32(TempData["id"]);
        //    decimal levi1 = 0;
        //    decimal levi2 = 0;
        //    decimal levi3 = 0;
        //    decimal levi4 = 0;
        //    decimal levi5 = 0;
        //    decimal levi6 = 0;
        //    var levi = from b in db.Bolleto_Dogonale
        //               join g in db.Goods on b.goods_id equals g.goods_id
        //               join l in db.Levis on g.levi_id equals l.levi_id
        //               where b.goods_id == bolleto_dogonale.goods_id
        //               select l;
        //    foreach (var item in levi)
        //    {
        //        levi1 = item.levi_1;
        //        levi2 = item.levi_2;
        //        levi3 = item.levi_3;
        //        levi4 = item.levi_4;
        //        levi5 = item.levi_5;
        //        levi6 = item.levi_6;
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        bolleto_dogonale.levi_1 = levi1;
        //        bolleto_dogonale.calculated_levi_1 = (bolleto_dogonale.price * levi1) / 100;
        //        bolleto_dogonale.levi_2 = levi2;
        //        bolleto_dogonale.calculated_levi_2 = (bolleto_dogonale.price * levi2) / 100;
        //        bolleto_dogonale.levi_3 = levi3;
        //        bolleto_dogonale.calculated_levi_3 = (bolleto_dogonale.price * levi3) / 100;
        //        bolleto_dogonale.levi_4 = levi4;
        //        bolleto_dogonale.calculated_levi_4 = (bolleto_dogonale.price * levi4) / 100;
        //        bolleto_dogonale.levi_5 = levi5;
        //        bolleto_dogonale.calculated_levi_5 = (bolleto_dogonale.price * levi5) / 100;
        //        bolleto_dogonale.levi_6 = levi6;
        //        bolleto_dogonale.calculated_levi_6 = (bolleto_dogonale.price * levi6) / 100;
        //        bolleto_dogonale.total_tax = bolleto_dogonale.calculated_levi_1 + bolleto_dogonale.calculated_levi_2 + bolleto_dogonale.calculated_levi_3 + bolleto_dogonale.calculated_levi_4 + bolleto_dogonale.calculated_levi_5 + bolleto_dogonale.calculated_levi_6;
        //        bolleto_dogonale.total_payment = (bolleto_dogonale.total_tax)*bolleto_dogonale.quantity + Convert.ToInt32(bolleto_dogonale.any_other_amount);
        //        bolleto_dogonale.bolleto_dogonale_id = id;
        //        db.Entry(bolleto_dogonale).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(bolleto_dogonale);
        //}

        //
        // GET: /Manage_Bollete_Dogonale/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    Bolleto_Dogonale bolleto_dogonale = db.Bolleto_Dogonale.Find(id);
        //    if (bolleto_dogonale == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(bolleto_dogonale);
        //}

        //
        // POST: /Manage_Bollete_Dogonale/Delete/5

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Bolleto_Dogonale bolleto_dogonale = db.Bolleto_Dogonale.Find(id);
        //    db.Bolleto_Dogonale.Remove(bolleto_dogonale);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //
        // GET: /Manage_Bollete_Dogonale/DbSearch

        public ActionResult DbSearch()
        {
            return View();
        }

        //
        // POST: /Manage_Bollete_Dogonale/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Bolleto_DogonaleModel bollete_dogonale)
        {
            //Queue q = new Queue();
            if (bollete_dogonale.bolleto_dogonale_code != null && bollete_dogonale.way_bill_code != null)
            {
                var result = (from c in db.Calculated_Levi
                                    join w in db.Way_Bill on c.way_bill_id equals w.way_bill_id
                                    join i in db.Imports on w.import_id equals i.import_id
                                    join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                    where i.bollete_dogonale_code.StartsWith(bollete_dogonale.bolleto_dogonale_code) && w.way_bill_code.StartsWith(bollete_dogonale.way_bill_code)
                                    select new Bolleto_DogonaleModel { way_bill_code = w.way_bill_code, import_code = i.import_code, ship_arrival_code = sa.ship_arrival_code, bolleto_dogonale_code = i.bollete_dogonale_code, way_bill_id = w.way_bill_id }).Distinct();
                return View("Index", result.ToList());
            }
            else if (bollete_dogonale.bolleto_dogonale_code != null && bollete_dogonale.way_bill_code == null)
            {
                var result = (from c in db.Calculated_Levi
                              join w in db.Way_Bill on c.way_bill_id equals w.way_bill_id
                              join i in db.Imports on w.import_id equals i.import_id
                              join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                              where i.bollete_dogonale_code.StartsWith(bollete_dogonale.bolleto_dogonale_code)
                              select new Bolleto_DogonaleModel { way_bill_code = w.way_bill_code, import_code = i.import_code, ship_arrival_code = sa.ship_arrival_code, bolleto_dogonale_code = i.bollete_dogonale_code, way_bill_id = w.way_bill_id }).Distinct();
                return View("Index", result.ToList());
            }
            else if (bollete_dogonale.bolleto_dogonale_code == null && bollete_dogonale.way_bill_code != null)
            {
                var result = (from c in db.Calculated_Levi
                              join w in db.Way_Bill on c.way_bill_id equals w.way_bill_id
                              join i in db.Imports on w.import_id equals i.import_id
                              join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                              where w.way_bill_code.StartsWith(bollete_dogonale.way_bill_code)
                              select new Bolleto_DogonaleModel { way_bill_code = w.way_bill_code, import_code = i.import_code, ship_arrival_code = sa.ship_arrival_code, bolleto_dogonale_code = i.bollete_dogonale_code, way_bill_id = w.way_bill_id }).Distinct();
                return View("Index", result.ToList());
            }
            return RedirectToAction("Index");
        }

        ////
        //// GET: /Manage_Bollete_Dogonale/DbSearch_new

        //public ActionResult DbSearch_new()
        //{
        //    ViewBag.goods = new HomeController().Goods();
        //    return View();
        //}

        //////
        ////// POST: /Manage_Bollete_Dogonale/DbSearchresult_new
        //////In this function is for database search
        //[HttpPost]
        //public ActionResult DbSearchresult_new(Bolleto_Dogonale bolleto_dogonale)
        //{
        //    var way_bill_code = Session["way_bill_code_in_bolleto"].ToString();
        //    if (bolleto_dogonale.goods_id != 0)
        //    {
        //        var result = from b in db.Bolleto_Dogonale
        //                     join w in db.Way_Bill on b.way_bill_id equals w.way_bill_id
        //                     join g in db.Goods on w.goods_id equals g.goods_id
        //                     where b.goods_id == bolleto_dogonale.goods_id && w.way_bill_code == way_bill_code
        //                     select new Bolleto_DogonaleModel { bolleto_dogonale_id = b.bolleto_dogonale_id, goods = g.goods_name, importer_name = b.importer_name, ship = b.ship_name, arrival_date = b.ship_arrival_date, quantity = b.quantity, unit_of_measure = b.unit_of_measure, total_payment = b.total_payment, administration_duty = b.administration_duty, calculated_administration_duty = b.calculated_administration_duty, hubinta_iyo_tirokoobka = b.hubinta_iyo_tirokoobka, calculated_hubinta_iyo_tirokoobka = b.calculated_hubinta_iyo_tirokoobka, airport = b.airport, calculated_airport = b.calculated_airport, difaaca_qaranka = b.difaaca_qaranka, calculated_difaaca_qaranka = b.calculated_difaaca_qaranka, dowladda_hoose = b.dowladda_hoose, calculated_dowladda_hoose = b.calculated_dowladda_hoose, wasaaradda_ganacsiga = b.wasaaradda_ganacsiga, calculated_wasaaradda_ganacsiga = b.wasaaradda_ganacsiga, wadada_ceel_dahir = b.wadada_ceel_dahir, calculated_wadada_ceel_dahir = b.calculated_wadada_ceel_dahir, total_tax = b.total_tax, price = b.price };
        //        return View("Details", result.ToList());
        //    }
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}