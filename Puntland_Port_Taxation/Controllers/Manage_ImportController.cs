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
    public class Manage_ImportController : Controller
    {
        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

        //
        // GET: /Manage_Import/

        public ActionResult Index()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(11))
                {
                    int page;
                    int page_no = 1;
                    var count = db.Imports.Count();
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
                    var import = (from i in db.Imports
                                 join it in db.Importers on i.importer_id equals it.importer_id
                                 join s in db.Ship_Arrival on i.ship_arrival_id equals s.ship_arrival_id
                                 join si in db.Ships on s.shipp_id equals si.ship_id
                                 join ity in db.Importer_Type on it.importer_type_id equals ity.importer_type_id
                                 join imps in db.Importing_Status on i.importing_status_id equals imps.importing_status_id
                                 join w in db.Way_Bill on i.way_bill_id equals w.way_bill_id into wa
                                 //join b in db.Bolleto_Dogonale  on i.bollete_dogonale_id equals b.bolleto_dogonale_id into bo
                                 from k in wa.DefaultIfEmpty()
                                 join cp in db.Calculated_Penalty_Config on i.way_bill_id equals cp.way_bill_id into za
                                 from l in za.DefaultIfEmpty()
                                 //from l in bo.DefaultIfEmpty()
                                  select new ImportModel { import_id = i.import_id, import_code = i.import_code, importer_name = it.importer_first_name + " " + it.importer_middle_name + " " + it.importer_last_name, ship_arrival_code = s.ship_arrival_code, importer_type_name = ity.importer_type_name, importing_status_name = imps.importing_status, payment_id = i.payment_id, way_bill_code = k.way_bill_code, bolleto_dogonale_code = i.bollete_dogonale_code, way_bill_id = i.way_bill_id, calculated_Penalty_way_bill_id = l.way_bill_id }).OrderByDescending(i => i.import_id).Skip(start_from).Take(9);
                    return View(import.ToList());
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
        // GET: /Manage_Import/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(11))
                {
                    Import import = db.Imports.Find(id);
                    if (import == null)
                    {
                        return HttpNotFound();
                    }
                    return View(import);
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
        // GET: /Manage_Import/View_Way_Bill/19

        public ActionResult View_Way_Bill(int id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(11))
                {
                    var way_bill = (from w in db.Way_Bill
                                    join wd in db.Way_Bill_Details on w.way_bill_id equals wd.way_bill_id
                                    join i in db.Importers on w.importer_id equals i.importer_id
                                    join g in db.Goods on wd.goods_id equals g.goods_id
                                    join u in db.Unit_Of_Measure on wd.unit_of_measure_id equals u.unit_id
                                    where w.way_bill_id == id
                                    select new Way_BillModel { way_bill_code = w.way_bill_code, goods = g.goods_name, unit_of_measure = u.unit_code, total_quantity = wd.total_quantity, importer_name = i.importer_first_name + " " + i.importer_middle_name + " " + i.importer_last_name, is_damaged = wd.is_damaged });
                    var penaltized_goods = (from pgd in db.Penaltized_Goods_Details
                                            join g in db.Goods on pgd.goods_id equals g.goods_id
                                            join u in db.Unit_Of_Measure on pgd.unit_of_measure_id equals u.unit_id
                                            where pgd.way_bill_id == id
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
        // GET: /Manage_Import/View_Bolleto/19

        public ActionResult View_Bolleto(int id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(11))
                {
                    var grand_total = db.Get_Grand_Total(id,106);
                    foreach (var v in grand_total)
                    {
                        ViewBag.grand_total = v;
                    }
                    var tax_calculation = db.Display_Tax_Details(id,106);
                    var bollete_dogonale = (from c in db.Calculated_Levi
                                            join w in db.Way_Bill on c.way_bill_id equals w.way_bill_id
                                            join i in db.Imports on w.import_id equals i.import_id
                                            join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                            where c.way_bill_id == id
                                            select new Bolleto_DogonaleModel { way_bill_code = w.way_bill_code, import_code = i.import_code, ship_arrival_code = sa.ship_arrival_code, bolleto_dogonale_code = i.bollete_dogonale_code }).Distinct();
                    ViewBag.bolleto = bollete_dogonale;
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
        // GET: /Manage_Import/Create

        public ActionResult Create()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(11))
                {
                    ViewBag.way_bill = new HomeController().Way_Bill_Import();
                    ViewBag.importer = new HomeController().Importer();
                    ViewBag.ship_arrival = new HomeController().Ship_Arrival();
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
        // POST: /Manage_Import/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Import import)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(11))
                {
                    var importer_name = "";
                    var importer_name1 = from i in db.Importers
                                         where i.importer_id == import.importer_id
                                         select i;
                    foreach (var item in importer_name1)
                    {
                        importer_name = item.importer_first_name + " " + item.importer_middle_name + " " + item.importer_last_name;
                    }
                    if (ModelState.IsValid)
                    {
                        var is_exist = (from i in db.Imports
                                        join ir in db.Importers on i.importer_id equals ir.importer_id
                                        join it in db.Importer_Type on ir.importer_type_id equals it.importer_type_id
                                        where i.importer_id == import.importer_id && i.ship_arrival_id == import.ship_arrival_id && it.is_multiple_allowed != true select i).Count();
                        if (is_exist > 0)
                        {
                            TempData["errorMessage"] = "This Import Already Exist";
                        }
                        else
                        {
                            import.payment_id = 1;
                            import.import_code = importer_name;
                            import.importing_status_id = 1;
                            import.created_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                            import.updated_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                            db.Imports.Add(import);
                            db.SaveChanges();
                            import.import_code = import.import_code + "000" + import.import_id;
                            db.Entry(import).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["errorMessage"] = "Import Added Successfully";
                        }
                        return RedirectToAction("Index");
                    }

                    return View(import);

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
        // GET: /Manage_Import/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(11))
                {
                    Import import = db.Imports.Find(id);
                    if (import == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.way_bill = new HomeController().Way_Bill_Import();
                    ViewBag.importer = new HomeController().Importer();
                    ViewBag.ship_arrival = new HomeController().Ship_Arrival();
                    ViewBag.status = new HomeController().Importing_Status();
                    TempData["id"] = import.import_id;
                    TempData["created_date"] = import.created_date;
                    TempData["way_bill_id"] = import.way_bill_id;
                    TempData["bollete_dogonale_code"] = import.bollete_dogonale_code;
                    TempData["payment_id"] = import.payment_id;
                    TempData["import_status"] = import.importing_status_id;
                    return View(import);

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
        // POST: /Manage_Import/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Import import)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(11))
                {
                    var importer_name = "";
                    var importer_name1 = from i in db.Importers
                                         where i.importer_id == import.importer_id
                                         select i;
                    foreach (var item in importer_name1)
                    {
                        importer_name = item.importer_first_name + " " + item.importer_middle_name + " " + item.importer_last_name;
                    }
                    int id = Convert.ToInt32(TempData["id"]);
                    var created_date = TempData["created_date"].ToString();
                    if (ModelState.IsValid)
                    {
                        if (TempData["way_bill_id"] != null)
                        {
                            import.way_bill_id = Convert.ToInt32(TempData["way_bill_id"]);
                        }
                        if (TempData["bollete_dogonale_code"] != null)
                        {
                            import.bollete_dogonale_code = TempData["bollete_dogonale_code"].ToString();
                        }
                        var is_exist = (from i in db.Imports where i.importer_id == import.importer_id && i.ship_arrival_id == import.ship_arrival_id && i.import_id != id select i).Count();
                        if (is_exist > 0)
                        {
                            TempData["errorMessage"] = "This Import Already Exist";
                        }
                        else
                        {
                            import.import_code = importer_name + "000" + id;
                            import.import_id = id;
                            import.importing_status_id = Convert.ToInt32(TempData["import_status"]);
                            import.payment_id = Convert.ToInt32(TempData["payment_id"]);
                            import.created_date = created_date;
                            import.updated_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                            db.Entry(import).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["errorMessage"] = "Edited Successfully";
                        }
                        return RedirectToAction("Index");
                    }
                    return View(import);

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
        // GET: /Manage_Import/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(11))
                {
                    Import import = db.Imports.Find(id);
                    if (import == null)
                    {
                        return HttpNotFound();
                    }
                    return View(import);

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
        // POST: /Manage_Import/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(11))
                {
                    Import import = db.Imports.Find(id);
                    db.Imports.Remove(import);
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
        // GET: /Manage_Import/DbSearch

        public ActionResult DbSearch()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(11))
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
        // POST: /Manage_Import/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(ImportModel import)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(11))
                {
                    //Queue q = new Queue();
                    if (import.importer_name != null && import.ship_arrival_code != null && import.payment_id != 0)
                    {
                        var result = from i in db.Imports
                                     join it in db.Importers on i.importer_id equals it.importer_id
                                     join s in db.Ship_Arrival on i.ship_arrival_id equals s.ship_arrival_id
                                     join si in db.Ships on s.shipp_id equals si.ship_id
                                     join ity in db.Importer_Type on it.importer_type_id equals ity.importer_type_id
                                     join imps in db.Importing_Status on i.importing_status_id equals imps.importing_status_id
                                     join w in db.Way_Bill on i.way_bill_id equals w.way_bill_id into wa
                                     from k in wa.DefaultIfEmpty()
                                     where (it.importer_first_name + " " + it.importer_middle_name + " " + it.importer_last_name).Contains(import.importer_name) && s.ship_arrival_code.Contains(import.ship_arrival_code) && i.payment_id == import.payment_id
                                     select new ImportModel { import_id = i.import_id, import_code = i.import_code, importer_name = it.importer_first_name + " " + it.importer_middle_name + " " + it.importer_last_name, ship_arrival_code = s.ship_arrival_code, importer_type_name = ity.importer_type_name, importing_status_name = imps.importing_status, payment_id = i.payment_id, way_bill_code = k.way_bill_code, bolleto_dogonale_code = i.bollete_dogonale_code, way_bill_id = i.way_bill_id };
                        return View("Index", result.ToList());
                    }
                    if (import.importer_name != null && import.ship_arrival_code != null && import.payment_id == 0)
                    {
                        var result = from i in db.Imports
                                     join it in db.Importers on i.importer_id equals it.importer_id
                                     join s in db.Ship_Arrival on i.ship_arrival_id equals s.ship_arrival_id
                                     join si in db.Ships on s.shipp_id equals si.ship_id
                                     join ity in db.Importer_Type on it.importer_type_id equals ity.importer_type_id
                                     join imps in db.Importing_Status on i.importing_status_id equals imps.importing_status_id
                                     join w in db.Way_Bill on i.way_bill_id equals w.way_bill_id into wa
                                     from k in wa.DefaultIfEmpty()
                                     where (it.importer_first_name + " " + it.importer_middle_name + " " + it.importer_last_name).Contains(import.importer_name) && s.ship_arrival_code.Contains(import.ship_arrival_code)
                                     select new ImportModel { import_id = i.import_id, import_code = i.import_code, importer_name = it.importer_first_name + " " + it.importer_middle_name + " " + it.importer_last_name, ship_arrival_code = s.ship_arrival_code, importer_type_name = ity.importer_type_name, importing_status_name = imps.importing_status, payment_id = i.payment_id, way_bill_code = k.way_bill_code, bolleto_dogonale_code = i.bollete_dogonale_code, way_bill_id = i.way_bill_id };
                        return View("Index", result.ToList());
                    }
                    else if (import.importer_name != null && import.ship_arrival_code == null && import.payment_id != 0)
                    {
                        var result = from i in db.Imports
                                     join it in db.Importers on i.importer_id equals it.importer_id
                                     join s in db.Ship_Arrival on i.ship_arrival_id equals s.ship_arrival_id
                                     join si in db.Ships on s.shipp_id equals si.ship_id
                                     join ity in db.Importer_Type on it.importer_type_id equals ity.importer_type_id
                                     join imps in db.Importing_Status on i.importing_status_id equals imps.importing_status_id
                                     join w in db.Way_Bill on i.way_bill_id equals w.way_bill_id into wa
                                     from k in wa.DefaultIfEmpty()
                                     where (it.importer_first_name + " " + it.importer_middle_name + " " + it.importer_last_name).Contains(import.importer_name) && i.payment_id == import.payment_id
                                     select new ImportModel { import_id = i.import_id, import_code = i.import_code, importer_name = it.importer_first_name + " " + it.importer_middle_name + " " + it.importer_last_name, ship_arrival_code = s.ship_arrival_code, importer_type_name = ity.importer_type_name, importing_status_name = imps.importing_status, payment_id = i.payment_id, way_bill_code = k.way_bill_code, bolleto_dogonale_code = i.bollete_dogonale_code, way_bill_id = i.way_bill_id };
                        return View("Index", result.ToList());
                    }
                    else if (import.importer_name != null && import.ship_arrival_code == null && import.payment_id == 0)
                    {
                        var result = from i in db.Imports
                                     join it in db.Importers on i.importer_id equals it.importer_id
                                     join s in db.Ship_Arrival on i.ship_arrival_id equals s.ship_arrival_id
                                     join si in db.Ships on s.shipp_id equals si.ship_id
                                     join ity in db.Importer_Type on it.importer_type_id equals ity.importer_type_id
                                     join imps in db.Importing_Status on i.importing_status_id equals imps.importing_status_id
                                     join w in db.Way_Bill on i.way_bill_id equals w.way_bill_id into wa
                                     from k in wa.DefaultIfEmpty()
                                     where (it.importer_first_name + " " + it.importer_middle_name + " " + it.importer_last_name).Contains(import.importer_name)
                                     select new ImportModel { import_id = i.import_id, import_code = i.import_code, importer_name = it.importer_first_name + " " + it.importer_middle_name + " " + it.importer_last_name, ship_arrival_code = s.ship_arrival_code, importer_type_name = ity.importer_type_name, importing_status_name = imps.importing_status, payment_id = i.payment_id, way_bill_code = k.way_bill_code, bolleto_dogonale_code = i.bollete_dogonale_code, way_bill_id = i.way_bill_id };
                        return View("Index", result.ToList());
                    }
                    else if (import.importer_name == null && import.ship_arrival_code != null && import.payment_id != 0)
                    {
                        var result = from i in db.Imports
                                     join it in db.Importers on i.importer_id equals it.importer_id
                                     join s in db.Ship_Arrival on i.ship_arrival_id equals s.ship_arrival_id
                                     join si in db.Ships on s.shipp_id equals si.ship_id
                                     join ity in db.Importer_Type on it.importer_type_id equals ity.importer_type_id
                                     join imps in db.Importing_Status on i.importing_status_id equals imps.importing_status_id
                                     join w in db.Way_Bill on i.way_bill_id equals w.way_bill_id into wa
                                     from k in wa.DefaultIfEmpty()
                                     where s.ship_arrival_code.Contains(import.ship_arrival_code) && i.payment_id == import.payment_id
                                     select new ImportModel { import_id = i.import_id, import_code = i.import_code, importer_name = it.importer_first_name + " " + it.importer_middle_name + " " + it.importer_last_name, ship_arrival_code = s.ship_arrival_code, importer_type_name = ity.importer_type_name, importing_status_name = imps.importing_status, payment_id = i.payment_id, way_bill_code = k.way_bill_code, bolleto_dogonale_code = i.bollete_dogonale_code, way_bill_id = i.way_bill_id };
                        return View("Index", result.ToList());
                    }
                    else if (import.importer_name == null && import.ship_arrival_code != null && import.payment_id == 0)
                    {
                        var result = from i in db.Imports
                                     join it in db.Importers on i.importer_id equals it.importer_id
                                     join s in db.Ship_Arrival on i.ship_arrival_id equals s.ship_arrival_id
                                     join si in db.Ships on s.shipp_id equals si.ship_id
                                     join ity in db.Importer_Type on it.importer_type_id equals ity.importer_type_id
                                     join imps in db.Importing_Status on i.importing_status_id equals imps.importing_status_id
                                     join w in db.Way_Bill on i.way_bill_id equals w.way_bill_id into wa
                                     from k in wa.DefaultIfEmpty()
                                     where s.ship_arrival_code.Contains(import.ship_arrival_code)
                                     select new ImportModel { import_id = i.import_id, import_code = i.import_code, importer_name = it.importer_first_name + " " + it.importer_middle_name + " " + it.importer_last_name, ship_arrival_code = s.ship_arrival_code, importer_type_name = ity.importer_type_name, importing_status_name = imps.importing_status, payment_id = i.payment_id, way_bill_code = k.way_bill_code, bolleto_dogonale_code = i.bollete_dogonale_code, way_bill_id = i.way_bill_id };
                        return View("Index", result.ToList());
                    }
                    else if (import.importer_name == null && import.ship_arrival_code == null && import.payment_id != 0)
                    {
                        var result = from i in db.Imports
                                     join it in db.Importers on i.importer_id equals it.importer_id
                                     join s in db.Ship_Arrival on i.ship_arrival_id equals s.ship_arrival_id
                                     join si in db.Ships on s.shipp_id equals si.ship_id
                                     join ity in db.Importer_Type on it.importer_type_id equals ity.importer_type_id
                                     join imps in db.Importing_Status on i.importing_status_id equals imps.importing_status_id
                                     join w in db.Way_Bill on i.way_bill_id equals w.way_bill_id into wa
                                     from k in wa.DefaultIfEmpty()
                                     where i.payment_id == import.payment_id
                                     select new ImportModel { import_id = i.import_id, import_code = i.import_code, importer_name = it.importer_first_name + " " + it.importer_middle_name + " " + it.importer_last_name, ship_arrival_code = s.ship_arrival_code, importer_type_name = ity.importer_type_name, importing_status_name = imps.importing_status, payment_id = i.payment_id, way_bill_code = k.way_bill_code, bolleto_dogonale_code = i.bollete_dogonale_code, way_bill_id = i.way_bill_id };
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