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
    public class Manage_ExportController : Controller
    {
        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

        //
        // GET: /Manage_Export/

        public ActionResult Index()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(30))
                {
                    int page;
                    int page_no = 1;
                    var count = db.Exports.Count();
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
                    var export = (from e in db.Exports
                                  join it in db.Importers on e.exporter_id equals it.importer_id
                                  join s in db.Ship_Departure on e.ship_departure_id equals s.ship_departure_id
                                  join si in db.Ships on s.shipp_id equals si.ship_id
                                  join ity in db.Importer_Type on it.importer_type_id equals ity.importer_type_id
                                  join imps in db.Importing_Status on e.exporting_status_id equals imps.importing_status_id
                                  join w in db.E_Way_Bill on e.e_way_bill_id equals w.e_way_bill_id into wa
                                  //join b in db.Bolleto_Dogonale  on i.bollete_dogonale_id equals b.bolleto_dogonale_id into bo
                                  from k in wa.DefaultIfEmpty()
                                  join cp in db.E_Calculated_Penalty_Config on e.e_way_bill_id equals cp.e_way_bill_id into za
                                  from l in za.DefaultIfEmpty()
                                  //from l in bo.DefaultIfEmpty()
                                  select new ExportModel { export_id = e.export_id, export_code = e.export_code, exporter_name = it.importer_first_name + " " + it.importer_middle_name + " " + it.importer_last_name, ship_departure_code = s.ship_departure_code, exporter_type_name = ity.importer_type_name, exporting_status_name = imps.importing_status, e_payment_id = e.e_payment_id, e_way_bill_code = k.e_way_bill_code, e_bolleto_dogonale_code = e.e_bollete_dogonale_code, e_way_bill_id = e.e_way_bill_id, e_calculated_Penalty_way_bill_id = l.e_way_bill_id }).OrderByDescending(i => i.export_id).Skip(start_from).Take(9);
                    return View(export.ToList());
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
        // GET: /Manage_Export/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(30))
                {
                    Export export = db.Exports.Find(id);
                    if (export == null)
                    {
                        return HttpNotFound();
                    }
                    return View(export);
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
        // GET: /Manage_Export/View_Way_Bill/19

        public ActionResult View_Way_Bill(int id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(30))
                {
                    var way_bill = (from w in db.E_Way_Bill
                                    join wd in db.E_Way_Bill_Details on w.e_way_bill_id equals wd.e_way_bill_id
                                    join i in db.Importers on w.exporter_id equals i.importer_id
                                    join g in db.Goods on wd.goods_id equals g.goods_id
                                    join u in db.Unit_Of_Measure on wd.unit_of_measure_id equals u.unit_id
                                    where w.e_way_bill_id == id
                                    select new Way_BillModel { way_bill_code = w.e_way_bill_code, goods = g.goods_name, unit_of_measure = u.unit_code, total_quantity = wd.total_quantity, importer_name = i.importer_first_name + " " + i.importer_middle_name + " " + i.importer_last_name, is_damaged = wd.is_damaged });
                    var penaltized_goods = (from pgd in db.E_Penaltized_Goods_Details
                                            join g in db.Goods on pgd.goods_id equals g.goods_id
                                            join u in db.Unit_Of_Measure on pgd.unit_of_measure_id equals u.unit_id
                                            where pgd.e_way_bill_id == id
                                            select new Way_BillModel { goods = g.goods_name, quantity = pgd.goods_id, unit_of_measure = u.unit_code, is_damaged = pgd.is_damaged }).ToList();
                    ViewBag.penalty_goods = penaltized_goods;
                    return View(way_bill.ToList());
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
        // GET: /Manage_Export/View_Bolleto/19

        //public ActionResult View_Bolleto(int id)
        //{
        //    if (Session["login_status"] != null)
        //    {
        //        int[] z = (int[])Session["function_id"];
        //        if (z.Contains(30))
        //        {
        //            var grand_total = db.Get_Grand_Total(id, 106);
        //            foreach (var v in grand_total)
        //            {
        //                ViewBag.grand_total = v;
        //            }
        //            var tax_calculation = db.Display_Tax_Details(id, 106);
        //            var bollete_dogonale = (from c in db.Calculated_Levi
        //                                    join w in db.Way_Bill on c.way_bill_id equals w.way_bill_id
        //                                    join i in db.Imports on w.import_id equals i.import_id
        //                                    join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
        //                                    where c.way_bill_id == id
        //                                    select new Bolleto_DogonaleModel { way_bill_code = w.way_bill_code, import_code = i.import_code, ship_arrival_code = sa.ship_arrival_code, bolleto_dogonale_code = i.bollete_dogonale_code }).Distinct();
        //            ViewBag.bolleto = bollete_dogonale;
        //            return View();
        //        }
        //        else
        //        {
        //            return RedirectToAction("../Home/Dashboard");
        //        }
        //    }
        //    else
        //    {
        //        return RedirectToAction("../Home");
        //    }
        //}


        //
        // GET: /Manage_Export/Create

        public ActionResult Create()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(30))
                {
                    ViewBag.way_bill = new HomeController().Way_Bill_Export();
                    ViewBag.exporter = new HomeController().Importer();
                    ViewBag.ship_departure = new HomeController().Ship_Departure();
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
        // POST: /Manage_Export/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Export export)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(30))
                {
                    var exporter_name = "";
                    var exporter_name1 = from i in db.Importers
                                         where i.importer_id == export.exporter_id
                                         select i;
                    foreach (var item in exporter_name1)
                    {
                        exporter_name = item.importer_first_name + " " + item.importer_middle_name + " " + item.importer_last_name;
                    }
                    if (ModelState.IsValid)
                    {
                        var is_exist = (from i in db.Exports
                                        join ir in db.Importers on i.exporter_id equals ir.importer_id
                                        join it in db.Importer_Type on ir.importer_type_id equals it.importer_type_id
                                        where i.exporter_id == export.exporter_id && i.ship_departure_id == export.ship_departure_id && it.is_multiple_allowed != true
                                        select i).Count();
                        if (is_exist > 0)
                        {
                            TempData["errorMessage"] = "This Export Already Exist";
                        }
                        else
                        {
                            export.e_payment_id = 1;
                            export.export_code = exporter_name;
                            export.exporting_status_id = 24;
                            export.created_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                            export.updated_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                            db.Exports.Add(export);
                            db.SaveChanges();
                            if (export.export_id < 1000)
                            {
                                export.export_code = export.export_code + export.export_id.ToString("0000");
                            }
                            else
                            {
                                export.export_code = export.export_code + export.export_id;
                            }
                            db.Entry(export).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["errorMessage"] = "Export Added Successfully";
                        }
                        return RedirectToAction("Index");
                    }

                    return View(export);

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
        // GET: /Manage_Export/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(30))
                {
                    Export export = db.Exports.Find(id);
                    if (export == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.way_bill = new HomeController().Way_Bill_Export();
                    ViewBag.exporter = new HomeController().Importer();
                    ViewBag.ship_departure = new HomeController().Ship_Departure();
                    ViewBag.status = new HomeController().Importing_Status();
                    TempData["id"] = export.export_id;
                    TempData["created_date"] = export.created_date;
                    TempData["way_bill_id"] = export.e_way_bill_id;
                    TempData["bollete_dogonale_code"] = export.e_bollete_dogonale_code;
                    TempData["payment_id"] = export.e_payment_id;
                    TempData["import_status"] = export.exporting_status_id;
                    return View(export);

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
        // POST: /Manage_Export/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Export export)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(30))
                {
                    var exporter_name = "";
                    var exporter_name1 = from i in db.Importers
                                         where i.importer_id == export.exporter_id
                                         select i;
                    foreach (var item in exporter_name1)
                    {
                        exporter_name = item.importer_first_name + " " + item.importer_middle_name + " " + item.importer_last_name;
                    }
                    int id = Convert.ToInt32(TempData["id"]);
                    var created_date = TempData["created_date"].ToString();
                    if (ModelState.IsValid)
                    {
                        if (TempData["way_bill_id"] != null)
                        {
                            export.e_way_bill_id = Convert.ToInt32(TempData["way_bill_id"]);
                        }
                        if (TempData["bollete_dogonale_code"] != null)
                        {
                            export.e_bollete_dogonale_code = TempData["bollete_dogonale_code"].ToString();
                        }
                        var is_exist = (from e in db.Exports where e.exporter_id == export.exporter_id && e.ship_departure_id == export.ship_departure_id && e.export_id != id select e).Count();
                        if (is_exist > 0)
                        {
                            TempData["errorMessage"] = "This Export Already Exist";
                        }
                        else
                        {
                            if (id < 1000)
                            {
                                export.export_code = exporter_name + id.ToString("0000");
                            }
                            else
                            {
                                export.export_code = exporter_name + id;
                            }
                            export.export_id = id;
                            export.exporting_status_id = Convert.ToInt32(TempData["import_status"]);
                            export.e_payment_id = Convert.ToInt32(TempData["payment_id"]);
                            export.created_date = created_date;
                            export.updated_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                            db.Entry(export).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["errorMessage"] = "Edited Successfully";
                        }
                        return RedirectToAction("Index");
                    }
                    return View(export);

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
        // GET: /Manage_Export/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(30))
                {
                    Export export = db.Exports.Find(id);
                    if (export == null)
                    {
                        return HttpNotFound();
                    }
                    return View(export);

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
        // POST: /Manage_Export/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(30))
                {
                    Export export = db.Exports.Find(id);
                    db.Exports.Remove(export);
                    db.SaveChanges();
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
        // GET: /Manage_Export/DbSearch

        public ActionResult DbSearch()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(30))
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
        // POST: /Manage_Export/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(ExportModel export)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(30))
                {
                    //Queue q = new Queue();
                    if (export.exporter_name != null && export.ship_departure_code != null && export.e_payment_id != 0)
                    {
                        var result = from e in db.Exports
                                     join it in db.Importers on e.exporter_id equals it.importer_id
                                     join s in db.Ship_Departure on e.ship_departure_id equals s.ship_departure_id
                                     join si in db.Ships on s.shipp_id equals si.ship_id
                                     join ity in db.Importer_Type on it.importer_type_id equals ity.importer_type_id
                                     join imps in db.Importing_Status on e.exporting_status_id equals imps.importing_status_id
                                     join w in db.E_Way_Bill on e.e_way_bill_id equals w.e_way_bill_id into wa
                                     from k in wa.DefaultIfEmpty()
                                     where (it.importer_first_name + " " + it.importer_middle_name + " " + it.importer_last_name).Contains(export.exporter_name) && s.ship_departure_code.Contains(export.ship_departure_code) && e.e_payment_id == export.e_payment_id
                                     select new ExportModel { export_id = e.export_id, export_code = e.export_code, exporter_name = it.importer_first_name + " " + it.importer_middle_name + " " + it.importer_last_name, ship_departure_code = s.ship_departure_code, exporter_type_name = ity.importer_type_name, exporting_status_name = imps.importing_status, e_payment_id = e.e_payment_id, e_way_bill_code = k.e_way_bill_code, e_bolleto_dogonale_code = e.e_bollete_dogonale_code, e_way_bill_id = e.e_way_bill_id };
                        return View("Index", result.ToList());
                    }
                    if (export.exporter_name != null && export.ship_departure_code != null && export.e_payment_id == 0)
                    {
                        var result = from e in db.Exports
                                     join it in db.Importers on e.exporter_id equals it.importer_id
                                     join s in db.Ship_Departure on e.ship_departure_id equals s.ship_departure_id
                                     join si in db.Ships on s.shipp_id equals si.ship_id
                                     join ity in db.Importer_Type on it.importer_type_id equals ity.importer_type_id
                                     join imps in db.Importing_Status on e.exporting_status_id equals imps.importing_status_id
                                     join w in db.E_Way_Bill on e.e_way_bill_id equals w.e_way_bill_id into wa
                                     from k in wa.DefaultIfEmpty()
                                     where (it.importer_first_name + " " + it.importer_middle_name + " " + it.importer_last_name).Contains(export.exporter_name) && s.ship_departure_code.Contains(export.ship_departure_code)
                                     select new ExportModel { export_id = e.export_id, export_code = e.export_code, exporter_name = it.importer_first_name + " " + it.importer_middle_name + " " + it.importer_last_name, ship_departure_code = s.ship_departure_code, exporter_type_name = ity.importer_type_name, exporting_status_name = imps.importing_status, e_payment_id = e.e_payment_id, e_way_bill_code = k.e_way_bill_code, e_bolleto_dogonale_code = e.e_bollete_dogonale_code, e_way_bill_id = e.e_way_bill_id };
                        return View("Index", result.ToList());
                    }
                    else if (export.exporter_name != null && export.ship_departure_code == null && export.e_payment_id != 0)
                    {
                        var result = from e in db.Exports
                                     join it in db.Importers on e.exporter_id equals it.importer_id
                                     join s in db.Ship_Departure on e.ship_departure_id equals s.ship_departure_id
                                     join si in db.Ships on s.shipp_id equals si.ship_id
                                     join ity in db.Importer_Type on it.importer_type_id equals ity.importer_type_id
                                     join imps in db.Importing_Status on e.exporting_status_id equals imps.importing_status_id
                                     join w in db.E_Way_Bill on e.e_way_bill_id equals w.e_way_bill_id into wa
                                     from k in wa.DefaultIfEmpty()
                                     where (it.importer_first_name + " " + it.importer_middle_name + " " + it.importer_last_name).Contains(export.exporter_name) && e.e_payment_id == export.e_payment_id
                                     select new ExportModel { export_id = e.export_id, export_code = e.export_code, exporter_name = it.importer_first_name + " " + it.importer_middle_name + " " + it.importer_last_name, ship_departure_code = s.ship_departure_code, exporter_type_name = ity.importer_type_name, exporting_status_name = imps.importing_status, e_payment_id = e.e_payment_id, e_way_bill_code = k.e_way_bill_code, e_bolleto_dogonale_code = e.e_bollete_dogonale_code, e_way_bill_id = e.e_way_bill_id };
                        return View("Index", result.ToList());
                    }
                    else if (export.exporter_name != null && export.ship_departure_code == null && export.e_payment_id == 0)
                    {
                        var result = from e in db.Exports
                                     join it in db.Importers on e.exporter_id equals it.importer_id
                                     join s in db.Ship_Departure on e.ship_departure_id equals s.ship_departure_id
                                     join si in db.Ships on s.shipp_id equals si.ship_id
                                     join ity in db.Importer_Type on it.importer_type_id equals ity.importer_type_id
                                     join imps in db.Importing_Status on e.exporting_status_id equals imps.importing_status_id
                                     join w in db.E_Way_Bill on e.e_way_bill_id equals w.e_way_bill_id into wa
                                     from k in wa.DefaultIfEmpty()
                                     where (it.importer_first_name + " " + it.importer_middle_name + " " + it.importer_last_name).Contains(export.exporter_name)
                                     select new ExportModel { export_id = e.export_id, export_code = e.export_code, exporter_name = it.importer_first_name + " " + it.importer_middle_name + " " + it.importer_last_name, ship_departure_code = s.ship_departure_code, exporter_type_name = ity.importer_type_name, exporting_status_name = imps.importing_status, e_payment_id = e.e_payment_id, e_way_bill_code = k.e_way_bill_code, e_bolleto_dogonale_code = e.e_bollete_dogonale_code, e_way_bill_id = e.e_way_bill_id };
                        return View("Index", result.ToList());
                    }
                    else if (export.exporter_name == null && export.ship_departure_code != null && export.e_payment_id != 0)
                    {
                        var result = from e in db.Exports
                                     join it in db.Importers on e.exporter_id equals it.importer_id
                                     join s in db.Ship_Departure on e.ship_departure_id equals s.ship_departure_id
                                     join si in db.Ships on s.shipp_id equals si.ship_id
                                     join ity in db.Importer_Type on it.importer_type_id equals ity.importer_type_id
                                     join imps in db.Importing_Status on e.exporting_status_id equals imps.importing_status_id
                                     join w in db.E_Way_Bill on e.e_way_bill_id equals w.e_way_bill_id into wa
                                     from k in wa.DefaultIfEmpty()
                                     where s.ship_departure_code.Contains(export.ship_departure_code) && e.e_payment_id == export.e_payment_id
                                     select new ExportModel { export_id = e.export_id, export_code = e.export_code, exporter_name = it.importer_first_name + " " + it.importer_middle_name + " " + it.importer_last_name, ship_departure_code = s.ship_departure_code, exporter_type_name = ity.importer_type_name, exporting_status_name = imps.importing_status, e_payment_id = e.e_payment_id, e_way_bill_code = k.e_way_bill_code, e_bolleto_dogonale_code = e.e_bollete_dogonale_code, e_way_bill_id = e.e_way_bill_id };
                        return View("Index", result.ToList());
                    }
                    else if (export.exporter_name == null && export.ship_departure_code != null && export.e_payment_id == 0)
                    {
                        var result = from e in db.Exports
                                     join it in db.Importers on e.exporter_id equals it.importer_id
                                     join s in db.Ship_Departure on e.ship_departure_id equals s.ship_departure_id
                                     join si in db.Ships on s.shipp_id equals si.ship_id
                                     join ity in db.Importer_Type on it.importer_type_id equals ity.importer_type_id
                                     join imps in db.Importing_Status on e.exporting_status_id equals imps.importing_status_id
                                     join w in db.E_Way_Bill on e.e_way_bill_id equals w.e_way_bill_id into wa
                                     from k in wa.DefaultIfEmpty()
                                     where s.ship_departure_code.Contains(export.ship_departure_code)
                                     select new ExportModel { export_id = e.export_id, export_code = e.export_code, exporter_name = it.importer_first_name + " " + it.importer_middle_name + " " + it.importer_last_name, ship_departure_code = s.ship_departure_code, exporter_type_name = ity.importer_type_name, exporting_status_name = imps.importing_status, e_payment_id = e.e_payment_id, e_way_bill_code = k.e_way_bill_code, e_bolleto_dogonale_code = e.e_bollete_dogonale_code, e_way_bill_id = e.e_way_bill_id };
                        return View("Index", result.ToList());
                    }
                    else if (export.exporter_name == null && export.ship_departure_code == null && export.e_payment_id != 0)
                    {
                        var result = from e in db.Exports
                                     join it in db.Importers on e.exporter_id equals it.importer_id
                                     join s in db.Ship_Departure on e.ship_departure_id equals s.ship_departure_id
                                     join si in db.Ships on s.shipp_id equals si.ship_id
                                     join ity in db.Importer_Type on it.importer_type_id equals ity.importer_type_id
                                     join imps in db.Importing_Status on e.exporting_status_id equals imps.importing_status_id
                                     join w in db.E_Way_Bill on e.e_way_bill_id equals w.e_way_bill_id into wa
                                     from k in wa.DefaultIfEmpty()
                                     where e.e_payment_id == export.e_payment_id
                                     select new ExportModel { export_id = e.export_id, export_code = e.export_code, exporter_name = it.importer_first_name + " " + it.importer_middle_name + " " + it.importer_last_name, ship_departure_code = s.ship_departure_code, exporter_type_name = ity.importer_type_name, exporting_status_name = imps.importing_status, e_payment_id = e.e_payment_id, e_way_bill_code = k.e_way_bill_code, e_bolleto_dogonale_code = e.e_bollete_dogonale_code, e_way_bill_id = e.e_way_bill_id };
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
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace Puntland_Port_Taxation.Controllers
//{
//    public class Manage_ExportController : Controller
//    {
//        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

//        //
//        // GET: /Manage_Export/

//        public ActionResult Index()
//        {
//            return View(db.Exports.ToList());
//        }

//        //
//        // GET: /Manage_Export/Details/5

//        public ActionResult Details(int id = 0)
//        {
//            Export export = db.Exports.Find(id);
//            if (export == null)
//            {
//                return HttpNotFound();
//            }
//            return View(export);
//        }

//        //
//        // GET: /Manage_Export/Create

//        public ActionResult Create()
//        {
//            return View();
//        }

//        //
//        // POST: /Manage_Export/Create

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create(Export export)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Exports.Add(export);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            return View(export);
//        }

//        //
//        // GET: /Manage_Export/Edit/5

//        public ActionResult Edit(int id = 0)
//        {
//            Export export = db.Exports.Find(id);
//            if (export == null)
//            {
//                return HttpNotFound();
//            }
//            return View(export);
//        }

//        //
//        // POST: /Manage_Export/Edit/5

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit(Export export)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(export).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(export);
//        }

//        //
//        // GET: /Manage_Export/Delete/5

//        public ActionResult Delete(int id = 0)
//        {
//            Export export = db.Exports.Find(id);
//            if (export == null)
//            {
//                return HttpNotFound();
//            }
//            return View(export);
//        }

//        //
//        // POST: /Manage_Export/Delete/5

//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            Export export = db.Exports.Find(id);
//            db.Exports.Remove(export);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            db.Dispose();
//            base.Dispose(disposing);
//        }
//    }
//}