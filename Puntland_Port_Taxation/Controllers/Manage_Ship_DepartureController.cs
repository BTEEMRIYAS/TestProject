using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using Puntland_Port_Taxation.Models;

namespace Puntland_Port_Taxation.Controllers
{
    public class Manage_Ship_DepartureController : Controller
    {
        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

        //
        // GET: /Manage_Ship_Departure/

        public ActionResult Index()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(28))
                {
                    int page;
                    int page_no = 1;
                    var count = db.Ship_Departure.Count();
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
                    var ship_departure = (from sa in db.Ship_Departure
                                        join s in db.Ships on sa.shipp_id equals s.ship_id
                                        join st in db.States on sa.state_id equals st.state_id
                                        join c in db.Countries on st.country_id equals c.country_id
                                        join g in db.Geographies on c.geography_id equals g.geography_id
                                        join d in db.Days on sa.day_id equals d.day_id
                                        select new Ship_DepartureModel { ship_departure_id = sa.ship_departure_id, shipp_name = s.ship_name, country = c.country_name, state = st.state_name, day_code = d.day_code, status_id = sa.status_id, ship_departure_code = sa.ship_departure_code }).OrderByDescending(s => s.ship_departure_id).Skip(start_from).Take(9);
                    return View(ship_departure.ToList());
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
        // GET: /Manage_Ship_Departure/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(28))
                {
                    Ship_Departure ship_departure = db.Ship_Departure.Find(id);
                    if (ship_departure == null)
                    {
                        return HttpNotFound();
                    }
                    return View(ship_departure);
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
        // GET: /Manage_Ship_Departure/Create

        public ActionResult Create()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(28))
                {
                    ViewBag.ships = new HomeController().Ship();
                    ViewBag.geography_id = new HomeController().Geography();
                    ViewBag.day = new HomeController().Day();
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
        // POST: /Manage_Ship_Departure/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Ship_DepartureModel ship_departureModel)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(28))
                {
                    Ship_Departure ship_departure = new Ship_Departure();
                    DateTime shipp_departure_date = Convert.ToDateTime(ship_departureModel.day_code);
                    var year = shipp_departure_date.Year;
                    var month = shipp_departure_date.Month;
                    var day = shipp_departure_date.Day;
                    var day_2 = "D" + day.ToString("00");
                    var month_2 = "M" + month.ToString("00");
                    var day_code = year + month_2 + day_2;
                    var ship_code = "";
                    var state_name = "";
                    var day_id = 0;
                    var ship_code1 = from s in db.Ships
                                     where s.ship_id == ship_departureModel.shipp_id
                                     select s.ship_code_1;
                    foreach (var item in ship_code1)
                    {
                        ship_code = item;
                    }
                    var geography_code1 = from s in db.States
                                          where s.state_id == ship_departureModel.state_id
                                          select s.state_name;
                    foreach (var item in geography_code1)
                    {
                        state_name = item;
                    }
                    var day_code1 = from d in db.Days
                                    where d.day_code == day_code
                                    select d.day_id;
                    foreach (var item in day_code1)
                    {
                        day_id = item;
                    }
                    if (ModelState.IsValid)
                    {
                        var is_exist = (from sa in db.Ship_Departure where sa.shipp_id == ship_departureModel.shipp_id && sa.day_id == day_id select sa).Count();
                        if (is_exist > 0)
                        {
                            TempData["errorMessage"] = "This Ship Departure Already Exist";
                        }
                        else
                        {
                            ship_departure.shipp_id = ship_departureModel.shipp_id;
                            ship_departure.state_id = ship_departureModel.state_id;
                            ship_departure.day_id = day_id;
                            ship_departure.ship_departure_code = ship_code + " " + state_name + " " + ship_departureModel.day_code;
                            ship_departure.status_id = 1;
                            ship_departure.starting_date = Convert.ToDateTime(ship_departureModel.day_code);
                            ship_departure.created_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                            ship_departure.updated_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                            db.Ship_Departure.Add(ship_departure);
                            db.SaveChanges();
                            TempData["errorMessage"] = "Ship Departure Added Successfully";
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
        // GET: /Manage_Ship_Departure/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(28))
                {
                    Ship_Departure ship_departure = db.Ship_Departure.Find(id);
                    if (ship_departure == null)
                    {
                        return HttpNotFound();
                    }
                    var country_id = 0;
                    var country_name = "";
                    var geography_id = 0;
                    var geography_name = "";
                    var state_name = "";
                    var day_code = "";
                    var geography = from sa in db.Ship_Departure
                                    join s in db.States on sa.state_id equals s.state_id
                                    join c in db.Countries on s.country_id equals c.country_id
                                    join g in db.Geographies on c.geography_id equals g.geography_id
                                    join d in db.Days on sa.day_id equals d.day_id
                                    where s.state_id == ship_departure.state_id && d.day_id == ship_departure.day_id
                                    select new { c.country_id, c.country_name, g.geography_id, g.geography_name, s.state_name, s.state_id, d.day_code };
                    foreach (var item in geography)
                    {
                        country_id = item.country_id;
                        country_name = item.country_name;
                        geography_id = item.geography_id;
                        geography_name = item.geography_name;
                        state_name = item.state_name;
                        day_code = item.day_code;
                    }
                    ViewBag.ships = new HomeController().Ship();
                    ViewBag.geography_id = new HomeController().Geography();
                    ViewBag.day = new HomeController().Day();
                    ViewBag.status = new HomeController().Status();
                    ViewBag.country_id = new SelectList(geography, "country_id", "country_name");
                    ViewBag.state_id = new SelectList(geography, "state_id", "state_name");
                    TempData["id"] = ship_departure.ship_departure_id;
                    TempData["created_date"] = ship_departure.created_date;
                    var result = CustomSplit(day_code, new string[] { "", "", "" });
                    var year_value = result[0];
                    var month_value = result[1];
                    var day_value = result[2];
                    var month_number = Regex.Match(month_value, @"\d+").Value;
                    var day_number = Regex.Match(day_value, @"\d+").Value;
                    var day = year_value + "-" + month_number + "-" + day_number;
                    Ship_DepartureModel ship_departureModel = new Ship_DepartureModel();
                    ship_departureModel.shipp_id = ship_departure.shipp_id;
                    ship_departureModel.geography_id = geography_id;
                    ship_departureModel.country_id = country_id;
                    ship_departureModel.state_id = ship_departure.state_id;
                    ship_departureModel.state_id = ship_departure.day_id;
                    ship_departureModel.status_id = ship_departure.status_id;
                    ship_departureModel.ship_departure_id = ship_departure.ship_departure_id;
                    ship_departureModel.day_code = day;
                    return View(ship_departureModel);
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
        // POST: /Manage_Ship_Departure/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Ship_DepartureModel ship_departureModel)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(28))
                {
                    int id = Convert.ToInt32(TempData["id"]);
                    var created_date = TempData["created_date"].ToString();
                    Ship_Departure ship_departure = new Ship_Departure();
                    DateTime shipp_arrival_date = Convert.ToDateTime(ship_departureModel.day_code);
                    var year = shipp_arrival_date.Year;
                    var month = shipp_arrival_date.Month;
                    var day = shipp_arrival_date.Day;
                    var day_2 = "D" + day.ToString("00");
                    var month_2 = "M" + month.ToString("00");
                    var day_code = year + month_2 + day_2;
                    var ship_code = "";
                    var state_name = "";
                    var day_id = 0;
                    var ship_code1 = from s in db.Ships
                                     where s.ship_id == ship_departureModel.shipp_id
                                     select s.ship_code_1;
                    foreach (var item in ship_code1)
                    {
                        ship_code = item;
                    }
                    var geography_code1 = from s in db.States
                                          where s.state_id == ship_departureModel.state_id
                                          select s.state_name;
                    foreach (var item in geography_code1)
                    {
                        state_name = item;
                    }
                    var day_code1 = from d in db.Days
                                    where d.day_code == day_code
                                    select d.day_id;
                    foreach (var item in day_code1)
                    {
                        day_id = item;
                    }
                    if (ModelState.IsValid)
                    {
                        var is_exist = (from sa in db.Ship_Departure where sa.shipp_id == ship_departureModel.shipp_id && sa.day_id == day_id && sa.ship_departure_id != id select sa).Count();
                        if (is_exist > 0)
                        {
                            TempData["errorMessage"] = "This Ship Departure Already Exist";
                        }
                        else
                        {
                            ship_departure.status_id = ship_departureModel.status_id;
                            ship_departure.shipp_id = ship_departureModel.shipp_id;
                            ship_departure.state_id = ship_departureModel.state_id;
                            ship_departure.day_id = day_id;
                            ship_departure.ship_departure_code = ship_code + " " + state_name + " " + ship_departureModel.day_code;
                            ship_departure.ship_departure_id = id;
                            ship_departure.starting_date = Convert.ToDateTime(ship_departureModel.day_code);
                            ship_departure.created_date = created_date;
                            ship_departure.updated_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                            db.Entry(ship_departure).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["errorMessage"] = "Edited Successfully";
                        }
                        return RedirectToAction("Index");
                    }
                    return View(ship_departure);
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
        // GET: /Manage_Ship_Departure/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(28))
                {
                    Ship_Departure ship_departure = db.Ship_Departure.Find(id);
                    if (ship_departure == null)
                    {
                        return HttpNotFound();
                    }
                    return View(ship_departure);
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
        // POST: /Manage_Ship_Departure/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(28))
                {
                    Ship_Departure ship_departure = db.Ship_Departure.Find(id);
                    db.Ship_Departure.Remove(ship_departure);
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
        // GET: /Manage_Ship_Departure/DbSearch

        public ActionResult DbSearch()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(28))
                {
                    ViewBag.ships = new HomeController().Ship();
                    ViewBag.geography = new HomeController().Geography();
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
        // POST: /Manage_Ship_Departure/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Ship_DepartureModel ship_departure)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(28))
                {
                    if (ship_departure.shipp_name != null && ship_departure.geography_id != 0)
                    {
                        var result = from sa in db.Ship_Departure
                                     join s in db.Ships on sa.shipp_id equals s.ship_id
                                     join st in db.States on sa.state_id equals st.state_id
                                     join c in db.Countries on st.country_id equals c.country_id
                                     join g in db.Geographies on c.geography_id equals g.geography_id
                                     join d in db.Days on sa.day_id equals d.day_id
                                     where s.ship_name.Contains(ship_departure.shipp_name) && g.geography_id == ship_departure.geography_id
                                     select new Ship_DepartureModel { ship_departure_id = sa.ship_departure_id, shipp_name = s.ship_name, country = c.country_name, state = st.state_name, day_code = d.day_code, status_id = sa.status_id, ship_departure_code = sa.ship_departure_code };
                        return View("Index", result.ToList());
                    }
                    else if (ship_departure.shipp_name != null && ship_departure.geography_id == 0)
                    {
                        var result = from sa in db.Ship_Departure
                                     join s in db.Ships on sa.shipp_id equals s.ship_id
                                     join st in db.States on sa.state_id equals st.state_id
                                     join c in db.Countries on st.country_id equals c.country_id
                                     join g in db.Geographies on c.geography_id equals g.geography_id
                                     join d in db.Days on sa.day_id equals d.day_id
                                     where s.ship_name.Contains(ship_departure.shipp_name)
                                     select new Ship_DepartureModel { ship_departure_id = sa.ship_departure_id, shipp_name = s.ship_name, country = c.country_name, state = st.state_name, day_code = d.day_code, status_id = sa.status_id, ship_departure_code = sa.ship_departure_code };
                        return View("Index", result.ToList());
                    }
                    else if (ship_departure.shipp_name == null && ship_departure.geography_id != 0)
                    {
                        var result = from sa in db.Ship_Departure
                                     join s in db.Ships on sa.shipp_id equals s.ship_id
                                     join st in db.States on sa.state_id equals st.state_id
                                     join c in db.Countries on st.country_id equals c.country_id
                                     join g in db.Geographies on c.geography_id equals g.geography_id
                                     join d in db.Days on sa.day_id equals d.day_id
                                     where g.geography_id == ship_departure.geography_id
                                     select new Ship_DepartureModel { ship_departure_id = sa.ship_departure_id, shipp_name = s.ship_name, country = c.country_name, state = st.state_name, day_code = d.day_code, status_id = sa.status_id, ship_departure_code = sa.ship_departure_code };
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
        public string[] CustomSplit(string s, string[] delimiters)
        {
            //Ensures there are arguments
            if (!String.IsNullOrEmpty(s) && delimiters.Any())
            {
                //Build custom Regular Expression (seperate delimiters with pipes)
                string regex = String.Format(@"({0})\d+", String.Join("|", delimiters));

                //Grabs your matches and outputs accordingly
                return Regex.Matches(s, regex).Cast<Match>().Select(m => m.Value).Where(m => !String.IsNullOrEmpty(m)).ToArray();
            }
            else
            {
                //Arguments were invalid - handle accordingly
                return null;
            }
        }
    }
}

