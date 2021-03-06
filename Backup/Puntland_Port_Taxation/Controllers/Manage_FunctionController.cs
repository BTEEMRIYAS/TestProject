﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Puntland_Port_Taxation.Controllers
{
    public class Manage_FunctionController : Controller
    {
        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

        //
        // GET: /Manage_Function/

        public ActionResult Index()
        {
            return View(db.Functions.ToList());
        }

        //
        // GET: /Manage_Function/Details/5

        public ActionResult Details(int id = 0)
        {
            Function function = db.Functions.Find(id);
            if (function == null)
            {
                return HttpNotFound();
            }
            return View(function);
        }

        //
        // GET: /Manage_Function/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Manage_Function/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Function function)
        {
            if (ModelState.IsValid)
            {
                function.status_id = 1;
                function.created_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                function.updated_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                db.Functions.Add(function);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        //
        // GET: /Manage_Function/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Function function = db.Functions.Find(id);
            if (function == null)
            {
                return HttpNotFound();
            }
            ViewBag.status = new HomeController().Status();
            TempData["function_id"] = function.function_id;
            return View(function);
        }

        //
        // POST: /Manage_Function/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Function function)
        {
            int function_id = Convert.ToInt32(TempData["function_id"]);
            Function function_new = db.Functions.Find(function_id);
            if (ModelState.IsValid)
            {
                function_new.function_name = function.function_name;
                function_new.status_id = function.status_id;
                function_new.updated_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                db.Entry(function_new).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Manage_Function/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Function function = db.Functions.Find(id);
            if (function == null)
            {
                return HttpNotFound();
            }
            return View(function);
        }

        //
        // POST: /Manage_Function/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Function function = db.Functions.Find(id);
            db.Functions.Remove(function);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        //
        // GET: /Manage_Function/DbSearch

        public ActionResult DbSearch()
        {
            return View();
        }

        //
        // POST: /Manage_Function/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Function function)
        {
            if (function.function_name != null)
            {
                var result = from f in db.Functions
                             where f.function_name.StartsWith(function.function_name)
                             select f;
                return View("Index", result.ToList());
            }
            return RedirectToAction("Index");
        }
    }
}