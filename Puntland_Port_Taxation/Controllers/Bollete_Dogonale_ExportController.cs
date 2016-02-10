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
    public class Bollete_Dogonale_ExportController : Controller
    {

        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

        //
        // GET: /Bollete_Dogonale_Export/

        public ActionResult Index()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(37))
                {
                    int page;
                    int page_no = 1;
                    var count = db.Exports.Where(e => e.e_bollete_dogonale_code != null).Count();
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
                    var bollete_dogonale = ((from c in db.E_Calculated_Levi
                                             join w in db.E_Way_Bill on c.way_bill_id equals w.e_way_bill_id
                                             join e in db.Exports on w.export_id equals e.export_id
                                             join sd in db.Ship_Departure on e.ship_departure_id equals sd.ship_departure_id
                                             join cpc in db.E_Calculated_Payment_Config on w.e_way_bill_id equals cpc.way_bill_id
                                             select new Bolleto_DogonaleModel { way_bill_code = w.e_way_bill_code, import_code = e.export_code, ship_arrival_code = sd.ship_departure_code, bolleto_dogonale_code = e.e_bollete_dogonale_code, way_bill_id = w.e_way_bill_id, date = cpc.calculated_date }).Distinct()).OrderByDescending(a => a.date).Skip(start_from).Take(9);
                    //var bollete_dogonale_dtetails = bollete_dogonale.OrderBy(a => a.way_bill_id).Skip(start_from).Take(9);
                    return View(bollete_dogonale.ToList());
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
        // GET: /Bollete_Dogonale_Export/Details/5

        public ActionResult Details(int way_bill_id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(37))
                {
                    var grand_total = db.E_Get_Grand_Total(way_bill_id, 106);
                    foreach (var v in grand_total)
                    {
                        ViewBag.grand_total = v;
                    }
                    db.E_Display_Tax_Details(way_bill_id, 106);
                    var display1 = from d1 in db.E_TempDisplay1
                                   select d1;
                    var display2 = (from d2 in db.E_TempDisplay2
                                    select d2).ToList();
                    ViewBag.display2 = display2;
                    var bollete_dogonale = (from c in db.E_Calculated_Levi
                                            join w in db.E_Way_Bill on c.way_bill_id equals w.e_way_bill_id
                                            join e in db.Exports on w.export_id equals e.export_id
                                            join sd in db.Ship_Departure on e.ship_departure_id equals sd.ship_departure_id
                                            where c.way_bill_id == way_bill_id
                                            select new Bolleto_DogonaleModel { way_bill_code = w.e_way_bill_code, import_code = e.export_code, ship_arrival_code = sd.ship_departure_code, bolleto_dogonale_code = e.e_bollete_dogonale_code }).Distinct();
                    ViewBag.bolleto = bollete_dogonale;
                    return View(display1.ToList());
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
        // GET: /Bollete_Dogonale_Export/DbSearch

        public ActionResult DbSearch()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(37))
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
        // POST: /Bollete_Dogonale_Export/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Bolleto_DogonaleModel bollete_dogonale)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(37))
                {
                    //Queue q = new Queue();
                    if (bollete_dogonale.importer_name != null && bollete_dogonale.way_bill_code != null)
                    {
                        var result = (from c in db.E_Calculated_Levi
                                      join w in db.E_Way_Bill on c.way_bill_id equals w.e_way_bill_id
                                      join e in db.Exports on w.export_id equals e.export_id
                                      join ir in db.Importers on e.exporter_id equals ir.importer_id
                                      join sd in db.Ship_Departure on e.ship_departure_id equals sd.ship_departure_id
                                      where (ir.importer_first_name + " " + ir.importer_middle_name + " " + ir.importer_last_name).Contains(bollete_dogonale.importer_name) && w.e_way_bill_code.StartsWith(bollete_dogonale.way_bill_code)
                                      select new Bolleto_DogonaleModel { way_bill_code = w.e_way_bill_code, import_code = e.export_code, ship_arrival_code = sd.ship_departure_code, bolleto_dogonale_code = e.e_bollete_dogonale_code, way_bill_id = w.e_way_bill_id }).Distinct();
                        return View("Index", result.ToList());
                    }
                    else if (bollete_dogonale.importer_name != null && bollete_dogonale.way_bill_code == null)
                    {
                        var result = (from c in db.E_Calculated_Levi
                                      join w in db.E_Way_Bill on c.way_bill_id equals w.e_way_bill_id
                                      join e in db.Exports on w.export_id equals e.export_id
                                      join ir in db.Importers on e.exporter_id equals ir.importer_id
                                      join sd in db.Ship_Departure on e.ship_departure_id equals sd.ship_departure_id
                                      where (ir.importer_first_name + " " + ir.importer_middle_name + " " + ir.importer_last_name).Contains(bollete_dogonale.importer_name)
                                      select new Bolleto_DogonaleModel { way_bill_code = w.e_way_bill_code, import_code = e.export_code, ship_arrival_code = sd.ship_departure_code, bolleto_dogonale_code = e.e_bollete_dogonale_code, way_bill_id = w.e_way_bill_id }).Distinct();
                        return View("Index", result.ToList());
                    }
                    else if (bollete_dogonale.importer_name == null && bollete_dogonale.way_bill_code != null)
                    {
                        var result = (from c in db.E_Calculated_Levi
                                      join w in db.E_Way_Bill on c.way_bill_id equals w.e_way_bill_id
                                      join e in db.Exports on w.export_id equals e.export_id
                                      join ir in db.Importers on e.exporter_id equals ir.importer_id
                                      join sd in db.Ship_Departure on e.ship_departure_id equals sd.ship_departure_id
                                      where w.e_way_bill_code.StartsWith(bollete_dogonale.way_bill_code)
                                      select new Bolleto_DogonaleModel { way_bill_code = w.e_way_bill_code, import_code = e.export_code, ship_arrival_code = sd.ship_departure_code, bolleto_dogonale_code = e.e_bollete_dogonale_code, way_bill_id = w.e_way_bill_id }).Distinct();
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