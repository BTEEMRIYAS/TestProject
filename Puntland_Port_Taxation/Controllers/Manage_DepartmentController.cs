using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Puntland_Port_Taxation.Controllers
{
    public class Manage_DepartmentController : Controller
    {
        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

        //
        // GET: /Manage_Department/

        public ActionResult Index()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(1))
                {
                    int page;
                    int page_no = 1;
                    var count = db.Departments.Count();
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
                    return View(db.Departments.OrderBy(d => d.department_id).Skip(start_from).Take(9).ToList());
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
        // GET: /Manage_Department/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(1))
                {
                    Department department = db.Departments.Find(id);
                    if (department == null)
                    {
                        return HttpNotFound();
                    }
                    return View(department);
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
        // GET: /Manage_Department/Create

        public ActionResult Create()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(1))
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
        // POST: /Manage_Department/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Department department)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(1))
                {
                    if (ModelState.IsValid)
                    {
                        var is_exist = (from d in db.Departments where d.department_name == department.department_name select d).Count();
                        if (is_exist > 0)
                        {
                            TempData["errorMessage"] = "This Department Already Exist";
                        }
                        else
                        {
                            department.status_id = 1;
                            department.created_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                            department.updated_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                            db.Departments.Add(department);                    
                            db.SaveChanges();
                            TempData["errorMessage"] = "Department Added Successfully";
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
        // GET: /Manage_Department/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(1))
                {
                    Department department = db.Departments.Find(id);
                    if (department == null)
                    {
                        return HttpNotFound();
                    }
                    TempData["department_id"] = department.department_id;
                    ViewBag.status = new HomeController().Status();
                    return View(department);
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
        // POST: /Manage_Department/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Department department)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(1))
                {
                    int department_id = Convert.ToInt32(TempData["department_id"]);         
                    Department department_new = db.Departments.Find(department_id);
                    if (ModelState.IsValid)
                    {
                        var is_exist = (from d in db.Departments where d.department_name == department.department_name && d.department_id != department_id select d).Count();
                        if (is_exist > 0)
                        {
                            TempData["errorMessage"] = "This Department Already Exist";
                        }
                        else
                        {
                            department_new.department_name = department.department_name;
                            department_new.status_id = department.status_id;
                            department_new.updated_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                            db.Entry(department_new).State = EntityState.Modified;
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
        // GET: /Manage_Department/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(1))
                {
                    Department department = db.Departments.Find(id);
                    if (department == null)
                    {
                        return HttpNotFound();
                    }
                    return View(department);
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
        // POST: /Manage_Department/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(1))
                {
                    Department department = db.Departments.Find(id);
                    db.Departments.Remove(department);
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
        // GET: /Manage_Department/DbSearch

        public ActionResult DbSearch()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(1))
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
        // POST: /Manage_Department/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Department department)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(1))
                {
                    if (department.department_name != null)
                    {
                        var result =  from d in db.Departments
                                      where d.department_name.StartsWith(department.department_name)
                                      select d;
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