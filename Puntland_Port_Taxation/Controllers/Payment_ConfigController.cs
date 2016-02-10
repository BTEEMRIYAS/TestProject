using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Puntland_Port_Taxation.Controllers
{
    public class Payment_ConfigController : Controller
    {
        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

        //
        // GET: /Payment_Config/

        public ActionResult Index()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(25))
                {
                    var end_date = Convert.ToDateTime("9999-12-31");
                    return View(db.Payment_Config.Where(p => p.end_date == end_date).ToList());
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
        // GET: /Payment_Config/Details/5

        public ActionResult Details(int id = 0)
        {
            Payment_Config payment_config = db.Payment_Config.Find(id);
            if (payment_config == null)
            {
                return HttpNotFound();
            }
            return View(payment_config);
        }

        //
        // GET: /Payment_Config/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Payment_Config/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Payment_Config payment_config)
        {
            if (ModelState.IsValid)
            {
                db.Payment_Config.Add(payment_config);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(payment_config);
        }

        //
        // GET: /Payment_Config/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Payment_Config payment_config = db.Payment_Config.Find(id);
            if (payment_config == null)
            {
                return HttpNotFound();
            }
            TempData["payment_config_id"] = payment_config.payment_config_id;
            return View(payment_config);
        }

        //
        // POST: /Payment_Config/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Payment_Config payment_config)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(25))
                {
                    if (ModelState.IsValid)
                    {
                        if (payment_config.sos + payment_config.usd != 100)
                        {
                            TempData["errorMessage"] = "Failed To Update";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            var payment_config_id = Convert.ToInt32(TempData["payment_config_id"]);
                            Payment_Config payment_config_new = db.Payment_Config.Find(payment_config_id);
                            payment_config_new.end_date = DateTime.Now;
                            db.Entry(payment_config_new).State = EntityState.Modified;
                            db.SaveChanges();
                            payment_config.created_date = DateTime.Now;
                            payment_config.end_date = Convert.ToDateTime("9999-12-31");
                            db.Payment_Config.Add(payment_config);
                            db.SaveChanges();
                            TempData["errorMessage"] = "Edited Successfully";
                            return RedirectToAction("Index");
                        }
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
        // GET: /Payment_Config/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Payment_Config payment_config = db.Payment_Config.Find(id);
            if (payment_config == null)
            {
                return HttpNotFound();
            }
            return View(payment_config);
        }

        //
        // POST: /Payment_Config/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Payment_Config payment_config = db.Payment_Config.Find(id);
            db.Payment_Config.Remove(payment_config);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}