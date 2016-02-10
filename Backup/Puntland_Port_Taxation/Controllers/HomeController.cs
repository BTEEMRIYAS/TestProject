/*
 * This Controller is used for login page
 * first checking the session, if already log in then directly going to dashboard
                               else going to login authentication
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using Puntland_Port_Taxation.Models;
using System.Data.Objects.SqlClient;

namespace Puntland_Port_Taxation.Controllers
{
    public class HomeController : Controller
    {
        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();
        //
        // GET: /Home/

        public ActionResult Index()
        {
            if (Session["login_status"] == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("../Home/Dashboard");
            }
        }
        
        // GET : /Dashboard

        public ActionResult Dashboard()
        {
            return View();
        }

        // GET : /Master_Data_Dashboard

        public ActionResult Master_Data_Dashboard()
        {
            return View();
        }

        // GET : /User_Management_Dashboard
        public ActionResult User_Management_Dashboard()
        {
            return View();
        }

        // GET : /Ship_Arrival_Dashboard
        public ActionResult Ship_Arrival_Dashboard()
        {
            return View();
        }

        // GET : /Import_Management_Dashboard
        public ActionResult Import_Management_Dashboard()
        {
            return View();
        }

        // GET : /Inner_Master_Data_Dashboard
        public ActionResult Inner_Master_Data_Dashboard()
        {
            return View();
        }

        // GET : /Inner_User_Management_Dashboard
        public ActionResult Inner_User_Management_Dashboard()
        {
            return View();
        }

        // GET : /Inner_Ship_Arrival_Dashboard
        public ActionResult Inner_Ship_Arrival_Dashboard()
        {
            return View();
        }

        // GET : /Inner_Import_Management_Dashboard
        public ActionResult Inner_Import_Management_Dashboard()
        {
            return View();
        }

        /*This function is for login authentication
         based user typed user name and password selecting data from database and then checking authentication
         */
        public ActionResult check_username_password()
        {
            if (ModelState.IsValid)
            {
                var status = 0;
                var pswd = Request["password"];
                var usrname = Request["user_name"];
                var encptd_psw = this.Encrypt(pswd);
                var result = from e in db.Employees
                             join s in db.Status on e.status_id equals s.status_id
                             join u in db.Users on e.employee_id equals u.employee_id
                             join r in db.Roles on e.role_id equals r.role_id
                             where u.user_name == usrname && u.password == encptd_psw
                             select new { e.employee_id, e.first_name, e.middle_name, e.last_name, e.status_id, e.profile_pic, r.role_id,r.role_name };
                foreach (var item in result)
                {
                    status = item.status_id;
                }
                if (result.Count() > 0)
                {
                    if (status == 1)
                    {
                        this.set_login_session(result);
                        return RedirectToAction("Dashboard");
                    }
                    else
                    {
                        TempData["login_authorization"] = "Unauthorized Access";
                        return View("Index");
                    }
                }
                TempData["login_details"] = "Incorrect username or password";
                return View("Index");
            }
            return View("Index");
        }

        /*This function is for seting session if user is autherized user*/
        private void set_login_session(IEnumerable<dynamic> result)
        {
            foreach (var item in result)
            {
                Session["login_status"] = "Yes:Puntland_Port_Tax";
                Session["id"] = item.employee_id;
                Session["user_name"] = item.first_name + ' ' + item.middle_name + ' ' + item.last_name;
                Session["status"] = item.status_id;
                Session["role"] = item.role_name;
                Session["role_id"] = item.role_id;
                Session["profile_pic"] = item.profile_pic;
            }
            var role_id = Convert.ToInt32(Session["role_id"]);
            var functions = from f in db.Functions
                            join rm in db.Function_Role_Map on f.function_id equals rm.function_id
                            join r in db.Roles on rm.role_id equals r.role_id
                            where rm.role_id == role_id
                            orderby f.function_id
                            select f.function_id;
            var j = functions.Count();
            Session["count"] = j;
            int[] a = new int[j];
            int i = 0;
            foreach (var item in functions)
            {
                a[i] = item;
                i++;
            }
            Session["function_id"] = a;
        }

        /*This function is used for encrypting password*/
        private string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        
        //For taking values from Status table
        public object Status()
        {
            var status = from s in db.Status
                          orderby s.employee_status ascending
                          select s;
            return(new SelectList(status, "status_id", "employee_status"));
        }
        
        //For taking values from Department table
        public object Departments()
        {
            var departments = from d in db.Departments
                              where d.status_id == 1
                              orderby d.department_name ascending
                         select d;
            return (new SelectList(departments, "department_id", "department_name"));
        }
        
        //For taking values from Role table
        public object Roles()
        {
            var roles = from r in db.Roles
                        where r.status_id == 1
                        orderby r.role_name ascending
                              select r;
            return (new SelectList(roles, "role_id", "role_name"));
        }

        //For taking values from Function table
        public object Function()
        {
            var function = from f in db.Functions
                           orderby f.function_name ascending
                           select f;
            return (new SelectList(function, "function_id", "function_name"));
        }

        //For taking values from Function table by RoleId
        public object Function_Roles(int role_id)
        {
            var function = from f in db.Functions
                           join d in db.Function_Role_Map on f.function_id equals d.function_id
                           orderby f.function_name ascending
                           where d.role_id == role_id
                           select new { f.function_id, f.function_name };
            return (new SelectList(function, "function_id", "function_name"));
        }

        //For taking values from Country table
        public object Country()
        {
            var countries = from c in db.Countries
                           orderby c.country_name ascending
                           select c;
            return (new SelectList(countries, "country_id", "country_name"));
        }

        //For taking values from State table
        public object State()
        {
            var state = from s in db.States
                        orderby s.state_name ascending
                           select s;
            return (new SelectList(state, "state_id", "state_name"));
        }

        //For taking values from Ship table
        public object Ship()
        {
            var ship = from s in db.Ships
                       where s.status_id == 1
                       orderby s.ship_name ascending
                       select s;
            return (new SelectList(ship, "ship_id", "ship_name"));
        }

        //For taking values from Geography table
        public object Geography()
        {
            var geography = from g in db.Geographies
                            orderby g.geography_name ascending
                            select g;
            return (new SelectList(geography, "geography_id", "geography_name"));
        }

        //For taking values from Day table
        public object Day()
        {
            var day = from d in db.Days
                      orderby d.day_id ascending
                      select d;
            return (new SelectList(day, "day_id", "day_code"));
        }
        public object Importer_Type()
        {
            var importer_type = from i in db.Importer_Type
                                where i.status_id == 1
                      orderby i.importer_type_name ascending
                      select i;
            return (new SelectList(importer_type, "importer_type_id", "importer_type_name"));
        }
        public object Ship_Arrival()
        {
            var ship_arrival_code = from s in db.Ship_Arrival
                                    where s.status_id == 1
                                    select s;
            return (new SelectList(ship_arrival_code, "ship_arrival_id", "ship_arrival_code"));
        }
        public object Importer()
        {
            var importer = from i in db.Importers
                           orderby i.importer_first_name ascending
                           select new { importer_id = i.importer_id, importer_name = i.importer_first_name + " " + i.importer_middle_name + " " + i.importer_last_name };
            return (new SelectList(importer, "importer_id", "importer_name"));
        }
        public object Importing_Status()
        {
            var importing_status = from i in db.Importing_Status
                                   orderby i.importing_status_id ascending
                                    select i;
            return (new SelectList(importing_status, "importing_status_id", "importing_status"));
        }
        public object Way_Bill()
        {
            var way_bills = (from w in db.Way_Bill
                            orderby w.way_bill_name ascending
                            select new { w.way_bill_code }).Distinct();
            return (new SelectList(way_bills, "way_bill_code", "way_bill_code"));
        }
        public object Way_Bill_Import()
        {
            var way_bills = from w in db.Way_Bill
                            orderby w.way_bill_code ascending
                             select w;
            return (new SelectList(way_bills, "way_bill_id", "way_bill_code"));
        }
        public object Way_Bill_Manifest()
        {
            var way_bills = (from w in db.Way_Bill
                             join i in db.Imports on w.import_id equals i.import_id
                             where i.importing_status_id == 2 || i.importing_status_id == 14
                             orderby w.way_bill_code ascending
                             select new { w.way_bill_id, w.way_bill_code }).Distinct();
            return (new SelectList(way_bills, "way_bill_id", "way_bill_code"));
        }
        public object Way_Bill_Examination_Unit()
        {
            var way_bills = (from w in db.Way_Bill
                             join i in db.Imports on w.import_id equals i.import_id
                             where i.importing_status_id == 3 || i.importing_status_id == 15
                             orderby w.way_bill_code ascending
                             select new { w.way_bill_id, w.way_bill_code }).Distinct();
            return (new SelectList(way_bills, "way_bill_id", "way_bill_code"));
        }
        public object Way_Bill_Accounting()
        {
            var way_bills = (from w in db.Way_Bill
                             join i in db.Imports on w.import_id equals i.import_id
                             where i.importing_status_id == 6
                             orderby w.way_bill_name ascending
                             select new { w.way_bill_id, w.way_bill_code }).Distinct();
            return (new SelectList(way_bills, "way_bill_id", "way_bill_code"));
        }
        public object Way_Bill_Compliance()
        {
            var way_bills = (from w in db.Way_Bill
                             join i in db.Imports on w.import_id equals i.import_id
                             where i.importing_status_id == 7
                             orderby w.way_bill_name ascending
                             select new { w.way_bill_id, w.way_bill_code }).Distinct();
            return (new SelectList(way_bills, "way_bill_id", "way_bill_code"));
        }
        public object Way_Bill_payment()
        {
            var way_bills = (from w in db.Way_Bill
                             join i in db.Imports on w.import_id equals i.import_id
                             where i.importing_status_id == 9
                             orderby w.way_bill_name ascending
                             select new { w.way_bill_id, w.way_bill_code }).Distinct();
            return (new SelectList(way_bills, "way_bill_id", "way_bill_code"));
        }
        public object Way_Bill_Release()
        {
            var way_bills = (from w in db.Way_Bill
                             join i in db.Imports on w.import_id equals i.import_id
                             where i.importing_status_id == 10
                             orderby w.way_bill_name ascending
                             select new { w.way_bill_id, w.way_bill_code }).Distinct();
            return (new SelectList(way_bills, "way_bill_id", "way_bill_code"));
        }
        public object Goods()
        {
            var goods = from g in db.Goods
                        where g.status_id == 1
                            orderby g.goods_name ascending
                            select g;
            return (new SelectList(goods, "goods_id", "goods_name"));
        }
        public object Currency()
        {
            var currency = from c in db.Currencies
                        orderby c.currency_id ascending
                        select c;
            return (new SelectList(currency, "currency_id", "currency_name"));
        }
        public object Import()
        {
            var imports = from i in db.Imports
                           orderby i.import_code ascending
                           select i;
            return (new SelectList(imports, "import_id", "import_code"));
        }
        //Taking values from unit_of_measure table
        public object Unit_Of_Measures()
        {
            var unit_of_measures = from u in db.Unit_Of_Measure
                                   orderby u.unit_id ascending
                                   select u;
            return (new SelectList(unit_of_measures, "unit_id", "unit_code"));
        }
        public object Levi()
        {
            var levi = from l in db.Levis
                       orderby l.levi_id ascending
                       select l;
            return (new SelectList(levi, "levi_id", "levi_name"));
        }
        public object Levi_Type()
        {
            var levi = from lt in db.Levi_Type
                       orderby lt.levi_type_name ascending
                       where lt.levi_type_id != 1
                       select lt;
            return (new SelectList(levi, "levi_type_id", "levi_type_name"));
        }
        public object Category()
        {
            var categories = from c in db.Goods_Category
                             where c.status_id == 1
                             orderby c.goods_category_name ascending
                             select c;
            return (new SelectList(categories, "goods_category_id", "goods_category_name"));
        }
        public object Subcategory()
        {
            var subcategories = from s in db.Goods_Subcategory
                                where s.status_id == 1
                                orderby s.goods_subcategory_name ascending
                                select s;
            return (new SelectList(subcategories, "goods_subcategory_id", "goods_subcategory_name"));
        }

        public object Goods_Type()
        {
            var goods_type = from t in db.Goods_Type
                             where t.status_id == 1
                             orderby t.goods_type_name ascending
                             select t;
            return (new SelectList(goods_type, "goods_type_id", "goods_type_name"));
        }

        public ActionResult Error_Message(string error_message)
        {
            TempData["error_message"] = error_message;
            return View();
        }

        //
        //POST: /Home/bind_data_to_dropdown_list

        [HttpPost]
        public ActionResult bind_country_to_dropdown_list(int id)
        {
            var values = (from c in db.Countries
                        join g in db.Geographies on c.geography_id equals g.geography_id
                        where c.geography_id == id
                        select new DropdownListModel { Id = c.country_id, name = c.country_name }).OrderBy(m => m.name);

            if (values == null)
                return Json(null);

            List<DropdownListModel> items = (List<DropdownListModel>)values.ToList();
            return Json(items);
        }

        //
        //POST: /Home/bind_data_to_dropdown_list

        [HttpPost]
        public ActionResult bind_state_to_dropdown_list(int id)
        {
            var values = (from s in db.States
                          join c in db.Countries on s.country_id equals c.country_id
                          where s.country_id == id
                          select new DropdownListModel { Id = s.state_id, name = s.state_name }).OrderBy(m => m.name);

            if (values == null)
                return Json(null);

            List<DropdownListModel> items = (List<DropdownListModel>)values.ToList();
            return Json(items);
        }

        //
        //POST: /Home/bind_category_to_dropdown_list

        [HttpPost]
        public ActionResult bind_category_to_dropdown_list()
        {
            var values = (from gc in db.Goods_Category
                          select new DropdownListModel { Id = gc.goods_category_id, name = gc.goods_category_name }).OrderBy(m => m.name);

            if (values == null)
                return Json(null);

            List<DropdownListModel> items = (List<DropdownListModel>)values.ToList();
            return Json(items);
        }

        //
        //POST: /Home/bind_data_to_dropdown_list

        [HttpPost]
        public ActionResult bind_subcategory_to_dropdown_list(int id)
        {
            var values = (from su in db.Goods_Subcategory
                          where su.goods_category_id == id
                          select new DropdownListModel { Id = su.goods_subcategory_id, name = su.goods_subcategory_name }).OrderBy(m => m.name);

            if (values == null)
                return Json(null);

            List<DropdownListModel> items = (List<DropdownListModel>)values.ToList();
            return Json(items);
        }

        //
        //POST: /Home/bind_data_to_dropdown_list

        [HttpPost]
        public ActionResult bind_type_to_dropdown_list(int id)
        {
            var values = (from st in db.Goods_Type
                          where st.goods_subcategory_id == id
                          select new DropdownListModel { Id = st.goods_type_id, name = st.goods_type_name }).OrderBy(m => m.name);

            if (values == null)
                return Json(null);

            List<DropdownListModel> items = (List<DropdownListModel>)values.ToList();
            return Json(items);
        }

        //
        //POST: /Home/bind_data_to_dropdown_list

        [HttpPost]
        public ActionResult bind_goods_to_dropdown_list(int id)
        {
            var values = (from g in db.Goods
                          where g.goods_type_id == id
                          select new DropdownListModel { Id = g.goods_id, name = g.goods_name }).OrderBy(m => m.name);

            if (values == null)
                return Json(null);

            List<DropdownListModel> items = (List<DropdownListModel>)values.ToList();
            return Json(items);
        }

        [HttpPost]
        public ActionResult get_existing_roles_function(int id)
        {
            var values = (from fr in db.Function_Role_Map
                          where fr.role_id == id
                          select new DropdownListModel { Id = fr.function_id, name = SqlFunctions.StringConvert((double)fr.status_id) });

            if (values == null)
                return Json(null);

            List<DropdownListModel> items = (List<DropdownListModel>)values.ToList();
            return Json(items);
        }
    }
}