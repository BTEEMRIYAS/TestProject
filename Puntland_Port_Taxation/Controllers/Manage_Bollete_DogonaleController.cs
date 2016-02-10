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
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(15))
                {
                    int page;
                    int page_no = 1;
                    var count = db.Imports.Where(i => i.bollete_dogonale_code != null).Count();
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
                    var bollete_dogonale = ((from c in db.Calculated_Levi
                                            join w in db.Way_Bill on c.way_bill_id equals w.way_bill_id
                                            join i in db.Imports on w.import_id equals i.import_id
                                            join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                            join cpc in db.Calculated_Payment_Config on w.way_bill_id equals cpc.way_bill_id
                                            select new Bolleto_DogonaleModel { way_bill_code = w.way_bill_code, import_code = i.import_code, ship_arrival_code = sa.ship_arrival_code, bolleto_dogonale_code = i.bollete_dogonale_code, way_bill_id = w.way_bill_id, date = cpc.calculated_date }).Distinct()).OrderByDescending( a => a.date).Skip(start_from).Take(9);
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
        // GET: /Manage_Bollete_Dogonale/Details/5

        public ActionResult Details(int way_bill_id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(15))
                {
                    var grand_total = db.Get_Grand_Total(way_bill_id,106);
                    foreach (var v in grand_total)
                    {
                        ViewBag.grand_total = v;
                    }
                    db.Display_Tax_Details(way_bill_id,106);
                    var display1 = from d1 in db.TempDisplay1
                                   select d1;
                    var display2 = (from d2 in db.TempDisplay2
                                   select d2).ToList();
                    ViewBag.display2 = display2;
                    var bollete_dogonale = (from c in db.Calculated_Levi
                                            join w in db.Way_Bill on c.way_bill_id equals w.way_bill_id
                                            join i in db.Imports on w.import_id equals i.import_id
                                            join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                            where c.way_bill_id == way_bill_id
                                            select new Bolleto_DogonaleModel { way_bill_code = w.way_bill_code, import_code = i.import_code, ship_arrival_code = sa.ship_arrival_code, bolleto_dogonale_code = i.bollete_dogonale_code }).Distinct();
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
        // GET: /Manage_Bollete_Dogonale/DbSearch

        public ActionResult DbSearch()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(15))
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
        // POST: /Manage_Bollete_Dogonale/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Bolleto_DogonaleModel bollete_dogonale)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(15))
                {
                    //Queue q = new Queue();
                    if (bollete_dogonale.importer_name != null && bollete_dogonale.way_bill_code != null)
                    {
                        var result = (from c in db.Calculated_Levi
                                            join w in db.Way_Bill on c.way_bill_id equals w.way_bill_id
                                            join i in db.Imports on w.import_id equals i.import_id
                                            join ir in db.Importers on i.importer_id equals ir.importer_id
                                            join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                            where (ir.importer_first_name + " " + ir.importer_middle_name + " " + ir.importer_last_name).Contains(bollete_dogonale.importer_name) && w.way_bill_code.StartsWith(bollete_dogonale.way_bill_code)
                                            select new Bolleto_DogonaleModel { way_bill_code = w.way_bill_code, import_code = i.import_code, ship_arrival_code = sa.ship_arrival_code, bolleto_dogonale_code = i.bollete_dogonale_code, way_bill_id = w.way_bill_id }).Distinct();
                        return View("Index", result.ToList());
                    }
                    else if (bollete_dogonale.importer_name != null && bollete_dogonale.way_bill_code == null)
                    {
                        var result = (from c in db.Calculated_Levi
                                      join w in db.Way_Bill on c.way_bill_id equals w.way_bill_id
                                      join i in db.Imports on w.import_id equals i.import_id
                                      join ir in db.Importers on i.importer_id equals ir.importer_id
                                      join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                      where (ir.importer_first_name + " " + ir.importer_middle_name + " " + ir.importer_last_name).Contains(bollete_dogonale.importer_name)
                                      select new Bolleto_DogonaleModel { way_bill_code = w.way_bill_code, import_code = i.import_code, ship_arrival_code = sa.ship_arrival_code, bolleto_dogonale_code = i.bollete_dogonale_code, way_bill_id = w.way_bill_id }).Distinct();
                        return View("Index", result.ToList());
                    }
                    else if (bollete_dogonale.importer_name == null && bollete_dogonale.way_bill_code != null)
                    {
                        var result = (from c in db.Calculated_Levi
                                      join w in db.Way_Bill on c.way_bill_id equals w.way_bill_id
                                      join i in db.Imports on w.import_id equals i.import_id
                                      join ir in db.Importers on i.importer_id equals ir.importer_id
                                      join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                      where w.way_bill_code.StartsWith(bollete_dogonale.way_bill_code)
                                      select new Bolleto_DogonaleModel { way_bill_code = w.way_bill_code, import_code = i.import_code, ship_arrival_code = sa.ship_arrival_code, bolleto_dogonale_code = i.bollete_dogonale_code, way_bill_id = w.way_bill_id }).Distinct();
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