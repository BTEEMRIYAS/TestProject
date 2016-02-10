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
    public class Manage_Goods_TypeController : Controller
    {
        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

        //
        // GET: /Manage_Goods_Type/

        public ActionResult Index()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(2))
                {
                    int page;
                    int page_no = 1;
                    var count = db.Goods_Type.Count();
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
                    var goods_type = (from gp in db.Goods_Type
                                join gs in db.Goods_Subcategory on gp.goods_subcategory_id equals gs.goods_subcategory_id
                                join gc in db.Goods_Category on gs.goods_category_id equals gc.goods_category_id
                                select new Goods_TypeModel { goods_type_id = gp.goods_type_id, goods_type_name = gp.goods_type_name, goods_subcategory = gs.goods_subcategory_name, goods_category = gc.goods_category_name, status_id = gp.status_id, goods_type_code = gp.goods_type_code }).OrderBy( gt => gt.goods_type_id).Skip(start_from).Take(9);
                    return View(goods_type.ToList());
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
        // GET: /Manage_Goods_Type/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(2))
                {
                    Goods_Type goods_type = db.Goods_Type.Find(id);
                    if (goods_type == null)
                    {
                        return HttpNotFound();
                    }
                    return View(goods_type);
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
        // GET: /Manage_Goods_Type/Create

        public ActionResult Create()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(2))
                {
                    ViewBag.categories = new HomeController().Category();
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
        // POST: /Manage_Goods_Type/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Goods_TypeModel goods_typeModel)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(2))
                {
                    Goods_Type goods_type = new Goods_Type();
                    if (ModelState.IsValid)
                    {
                        var is_exist = (from gt in db.Goods_Type where gt.goods_type_name == goods_typeModel.goods_type_name select gt).Count();
                        if (is_exist >0)
                        {
                            TempData["errorMessage"] = "This Goods Type Already Exist";
                        }
                        else
                        {
                            goods_type.goods_subcategory_id = goods_typeModel.goods_subcategory_id;
                            goods_type.goods_type_name = goods_typeModel.goods_type_name;
                            goods_type.goods_type_code = goods_typeModel.goods_type_code;
                            goods_type.status_id = 1;
                            goods_type.created_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                            goods_type.updated_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                            db.Goods_Type.Add(goods_type);                 
                            db.SaveChanges();
                            TempData["errorMessage"] = "Goods Type Added Successfully";
                        }
                        return RedirectToAction("Index");
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
        //
        // GET: /Manage_Goods_Type/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(2))
                {
                    Goods_Type goods_type = db.Goods_Type.Find(id);
                    Goods_TypeModel goods_typeModel = new Goods_TypeModel();
                    if (goods_type == null)
                    {
                        return HttpNotFound();
                    }
                    TempData["goods_type_id"] = goods_type.goods_type_id;
                    var type = from gt in db.Goods_Type
                               join gs in db.Goods_Subcategory on gt.goods_subcategory_id equals gs.goods_subcategory_id
                               join gc in db.Goods_Category on gs.goods_category_id equals gc.goods_category_id
                               where gt.goods_type_id == goods_type.goods_type_id
                               select new { gs.goods_subcategory_id, gs.goods_subcategory_name, gc.goods_category_id, gc.goods_category_name }; 
                    ViewBag.status = new HomeController().Status();
                    ViewBag.categories = new SelectList(type, "goods_category_id", "goods_category_name");
                    ViewBag.subcategories = new SelectList(type, "goods_subcategory_id", "goods_subcategory_name");
                    goods_typeModel.goods_type_name = goods_type.goods_type_name;
                    goods_typeModel.goods_type_code = goods_type.goods_type_code;
                    goods_typeModel.goods_type_id = goods_type.goods_type_id;
                    goods_typeModel.goods_category_id = type.First().goods_category_id;
                    goods_typeModel.goods_subcategory_id = goods_type.goods_subcategory_id;
                    goods_typeModel.status_id = goods_type.status_id;
                    return View(goods_typeModel);
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
        // POST: /Manage_Goods_Type/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Goods_TypeModel goods_typeModel)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(2))
                {
                    var type_id = Convert.ToInt32(TempData["goods_type_id"]);
                    Goods_Type goods_type_new = db.Goods_Type.Find(type_id);
                    if (ModelState.IsValid)
                    {
                        var is_exist = (from gt in db.Goods_Type where gt.goods_type_name == goods_typeModel.goods_type_name && gt.goods_type_id != type_id select gt).Count();
                        if (is_exist > 0)
                        {
                            TempData["errorMessage"] = "This Goods Type Already Exist";
                        }
                        else
                        {
                            goods_type_new.goods_subcategory_id = goods_typeModel.goods_subcategory_id;
                            goods_type_new.goods_type_code = goods_typeModel.goods_type_code;
                            goods_type_new.goods_type_name = goods_typeModel.goods_type_name;
                            goods_type_new.status_id = goods_typeModel.status_id;
                            goods_type_new.updated_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                            db.Entry(goods_type_new).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["errorMessage"] = "Edited Successfully";
                        }
                        return RedirectToAction("Index");
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

        //
        // GET: /Manage_Goods_Type/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(2))
                {
                    Goods_Type goods_type = db.Goods_Type.Find(id);
                    if (goods_type == null)
                    {
                        return HttpNotFound();
                    }
                    return View(goods_type);
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
        // POST: /Manage_Goods_Type/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(2))
                {
                    Goods_Type goods_type = db.Goods_Type.Find(id);
                    db.Goods_Type.Remove(goods_type);
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

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        //
        // GET: /Manage_Goods_Type/DbSearch

        public ActionResult DbSearch()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(2))
                {
                    ViewBag.categories = new HomeController().Category();
                    ViewBag.subcategories = new HomeController().Subcategory();
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
        // POST: /Manage_Goods_Type/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Goods_TypeModel goods_type)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(2))
                {
                    //Queue q = new Queue();
                    if (goods_type.goods_type_name != null && goods_type.goods_subcategory_id != 0 && goods_type.goods_category_id !=0)
                    {
                        var result = from gp in db.Goods_Type
                                     join gs in db.Goods_Subcategory on gp.goods_subcategory_id equals gs.goods_subcategory_id
                                     join gc in db.Goods_Category on gs.goods_category_id equals gc.goods_category_id
                                     where gp.goods_type_name.StartsWith(goods_type.goods_type_name) && gp.goods_subcategory_id == goods_type.goods_subcategory_id && gs.goods_category_id ==1
                                     select new Goods_TypeModel { goods_type_id = gp.goods_type_id, goods_type_name = gp.goods_type_name, goods_subcategory = gs.goods_subcategory_name, goods_category = gc.goods_category_name, status_id = gp.status_id, goods_type_code = gp.goods_type_code };
                        return View("Index", result.ToList());
                    }
                    else if (goods_type.goods_type_name == null && goods_type.goods_subcategory_id != 0 && goods_type.goods_category_id == 0)
                    {
                        var result = from gp in db.Goods_Type
                                     join gs in db.Goods_Subcategory on gp.goods_subcategory_id equals gs.goods_subcategory_id
                                     join gc in db.Goods_Category on gs.goods_category_id equals gc.goods_category_id
                                     where gp.goods_subcategory_id == goods_type.goods_subcategory_id
                                     select new Goods_TypeModel { goods_type_id = gp.goods_type_id, goods_type_name = gp.goods_type_name, goods_subcategory = gs.goods_subcategory_name, goods_category = gc.goods_category_name, status_id = gp.status_id, goods_type_code = gp.goods_type_code };
                        return View("Index", result.ToList());
                    }
                    else if (goods_type.goods_type_name != null && goods_type.goods_subcategory_id == 0 && goods_type.goods_category_id == 0)
                    {
                        var result = from gp in db.Goods_Type
                                     join gs in db.Goods_Subcategory on gp.goods_subcategory_id equals gs.goods_subcategory_id
                                     join gc in db.Goods_Category on gs.goods_category_id equals gc.goods_category_id
                                     where gp.goods_type_name.StartsWith(goods_type.goods_type_name)
                                     select new Goods_TypeModel { goods_type_id = gp.goods_type_id, goods_type_name = gp.goods_type_name, goods_subcategory = gs.goods_subcategory_name, goods_category = gc.goods_category_name, status_id = gp.status_id, goods_type_code = gp.goods_type_code };
                        return View("Index", result.ToList());
                    }
                    else if (goods_type.goods_type_name != null && goods_type.goods_subcategory_id != 0 && goods_type.goods_category_id == 0)
                    {
                        var result = from gp in db.Goods_Type
                                     join gs in db.Goods_Subcategory on gp.goods_subcategory_id equals gs.goods_subcategory_id
                                     join gc in db.Goods_Category on gs.goods_category_id equals gc.goods_category_id
                                     where gp.goods_type_name.StartsWith(goods_type.goods_type_name) && gp.goods_subcategory_id == goods_type.goods_subcategory_id
                                     select new Goods_TypeModel { goods_type_id = gp.goods_type_id, goods_type_name = gp.goods_type_name, goods_subcategory = gs.goods_subcategory_name, goods_category = gc.goods_category_name, status_id = gp.status_id, goods_type_code = gp.goods_type_code };
                        return View("Index", result.ToList());
                    }
                    else if (goods_type.goods_type_name != null && goods_type.goods_subcategory_id == 0 && goods_type.goods_category_id != 0)
                    {
                        var result = from gp in db.Goods_Type
                                     join gs in db.Goods_Subcategory on gp.goods_subcategory_id equals gs.goods_subcategory_id
                                     join gc in db.Goods_Category on gs.goods_category_id equals gc.goods_category_id
                                     where gp.goods_type_name.StartsWith(goods_type.goods_type_name) && gs.goods_category_id == goods_type.goods_category_id
                                     select new Goods_TypeModel { goods_type_id = gp.goods_type_id, goods_type_name = gp.goods_type_name, goods_subcategory = gs.goods_subcategory_name, goods_category = gc.goods_category_name, status_id = gp.status_id, goods_type_code = gp.goods_type_code };
                        return View("Index", result.ToList());
                    }
                    else if (goods_type.goods_type_name == null && goods_type.goods_subcategory_id != 0 && goods_type.goods_category_id != 0)
                    {
                        var result = from gp in db.Goods_Type
                                     join gs in db.Goods_Subcategory on gp.goods_subcategory_id equals gs.goods_subcategory_id
                                     join gc in db.Goods_Category on gs.goods_category_id equals gc.goods_category_id
                                     where gp.goods_subcategory_id == goods_type.goods_subcategory_id && gs.goods_category_id == goods_type.goods_category_id
                                     select new Goods_TypeModel { goods_type_id = gp.goods_type_id, goods_type_name = gp.goods_type_name, goods_subcategory = gs.goods_subcategory_name, goods_category = gc.goods_category_name, status_id = gp.status_id, goods_type_code = gp.goods_type_code };
                        return View("Index", result.ToList());
                    }
                    else if (goods_type.goods_type_name == null && goods_type.goods_subcategory_id == 0 && goods_type.goods_category_id != 0)
                    {
                        var result = from gp in db.Goods_Type
                                     join gs in db.Goods_Subcategory on gp.goods_subcategory_id equals gs.goods_subcategory_id
                                     join gc in db.Goods_Category on gs.goods_category_id equals gc.goods_category_id
                                     where gs.goods_category_id == goods_type.goods_category_id
                                     select new Goods_TypeModel { goods_type_id = gp.goods_type_id, goods_type_name = gp.goods_type_name, goods_subcategory = gs.goods_subcategory_name, goods_category = gc.goods_category_name, status_id = gp.status_id, goods_type_code = gp.goods_type_code };
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
    }
}