/*
 * This Controller is used for login page
 * first checking the session, if already log in then directly going to dashboard
                               else going to login authentication
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using Puntland_Port_Taxation.Models;
using System.Data.Objects.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Collections;
using System.Xml.Linq;
//using FlexCel.Core;
//using FlexCel.XlsAdapter;

using Excel = Microsoft.Office.Interop.Excel;
using ExcelAutoFormat = Microsoft.Office.Interop.Excel.XlRangeAutoFormat;
using FlexCel.Core;
using FlexCel.XlsAdapter;


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
            if (Session["login_status"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("../Home");
            }
        }

        public int Session_Check()
        {
            if (Session["login_status"] == null)
            {
                return 0;
            }
            else
            {
                return 1;
            }
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
            Session["edit_examination"] = 0;
            return View();
        }

        // GET : /Export_Management_Dashboard
        public ActionResult Export_Management_Dashboard()
        {
            return View();
        }

        // GET : /Evaluation_Management_Dashboard
        public ActionResult Evaluation_Management_Dashboard()
        {
            return View();
        }

        // GET : /Bank_Management_Dashboard
        public ActionResult Bank_Management_Dashboard()
        {
            return View();
        }

        // GET : /Levy_Management_Dashboard
        public ActionResult Levy_Management_Dashboard()
        {
            return View();
        }

        // GET : /Bolleto_Management_Dashboard
        public ActionResult Bolleto_Management_Dashboard()
        {
            return View();
        }

        // GET : /Cargo_Management_Dashboard
        public ActionResult Cargo_Management_Dashboard()
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
            Session["edit_examination"] = 0;
            return View();
        }

        // GET : /Inner_Export_Management_Dashboard
        public ActionResult Inner_Export_Management_Dashboard()
        {
            return View();
        }

        // GET : /Inner_Evaluation_Management_Dashboard
        public ActionResult Inner_Evaluation_Management_Dashboard()
        {
            return View();
        }

        // GET : /Inner_Bank_Management_Dashboard
        public ActionResult Inner_Bank_Management_Dashboard()
        {
            return View();
        }

        // GET : /Inner_Levy_Management_Dashboard
        public ActionResult Inner_Levy_Management_Dashboard()
        {
            return View();
        }

        // GET : /Inner_Bolleto_Management_Dashboard
        public ActionResult Inner_Bolleto_Management_Dashboard()
        {
            return View();
        }

        // GET : /Inner_Cargo_Management_Dashboard
        public ActionResult Inner_Cargo_Management_Dashboard()
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
                             select new { e.employee_id, e.first_name, e.middle_name, e.last_name, e.status_id, e.profile_pic, r.role_id,r.role_name, u.user_id };
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
                Session["user_id"] = item.user_id;
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
                       orderby s.ship_id descending
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
                                    orderby s.ship_arrival_id descending
                                    select s;
            return (new SelectList(ship_arrival_code, "ship_arrival_id", "ship_arrival_code"));
        }
        public object Ship_Departure()
        {
            var ship_departure_code = from s in db.Ship_Departure
                                    where s.status_id == 1
                                      orderby s.ship_departure_id descending
                                    select s;
            return (new SelectList(ship_departure_code, "ship_departure_id", "ship_departure_code"));
        }
        public object Importer()
        {
            var importer = from i in db.Importers
                           where i.status_id == 1
                           orderby i.importer_id descending
                           select new { importer_id = i.importer_id, importer_name = i.importer_first_name + " " + i.importer_middle_name + " " + i.importer_last_name };
            return (new SelectList(importer, "importer_id", "importer_name"));
        }
        public object Importer_Name()//to use in cargo manifest
        {
            var importer = from i in db.Importers
                           where i.status_id == 1
                           orderby i.importer_id descending
                           select new { importer_id = i.importer_id, importer_name = i.importer_first_name + " " + i.importer_middle_name + " " + i.importer_last_name };
            return (new SelectList(importer, "importer_name", "importer_name"));
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
                            orderby w.way_bill_id descending
                            select new { w.way_bill_code }).Distinct();
            return (new SelectList(way_bills, "way_bill_code", "way_bill_code"));
        }
        public object Way_Bill_Import()
        {
            var way_bills = from w in db.Way_Bill
                            orderby w.way_bill_id descending
                            select w;
            return (new SelectList(way_bills, "way_bill_id", "way_bill_code"));
        }
        public object Way_Bill_Export()
        {
            var way_bills = from w in db.E_Way_Bill
                            orderby w.e_way_bill_id descending
                            select w;
            return (new SelectList(way_bills, "e_way_bill_id", "e_way_bill_code"));
        }
        public object Way_Bill_Examination_Unit()
        {
            var way_bills = (from w in db.Way_Bill
                             join i in db.Imports on w.import_id equals i.import_id
                             where i.importing_status_id == 2 || i.importing_status_id == 14 || i.importing_status_id == 15 || i.importing_status_id == 13
                             orderby w.way_bill_id descending
                             select new { w.way_bill_id, w.way_bill_code }).Distinct();
            return (new SelectList(way_bills, "way_bill_id", "way_bill_code"));
        }
        public object Way_Bill_Manifest()
        {
            var way_bills = (from w in db.Way_Bill
                             join i in db.Imports on w.import_id equals i.import_id
                             where i.importing_status_id == 16 || i.importing_status_id == 20
                             orderby w.way_bill_id descending
                             select new { w.way_bill_id, w.way_bill_code }).Distinct();
            return (new SelectList(way_bills, "way_bill_id", "way_bill_code"));
        }
        public object Way_Bill_Accounting()
        {
            var way_bills = (from w in db.Way_Bill
                             join i in db.Imports on w.import_id equals i.import_id
                             where i.importing_status_id == 6
                             orderby w.way_bill_id descending
                             select new { w.way_bill_id, w.way_bill_code }).Distinct();
            return (new SelectList(way_bills, "way_bill_id", "way_bill_code"));
        }
        public object Way_Bill_Compliance()
        {
            var way_bills = (from w in db.Way_Bill
                             join i in db.Imports on w.import_id equals i.import_id
                             where i.importing_status_id == 7
                             orderby w.way_bill_id descending
                             select new { w.way_bill_id, w.way_bill_code }).Distinct();
            return (new SelectList(way_bills, "way_bill_id", "way_bill_code"));
        }
        public object Way_Bill_payment()
        {
            var way_bills = (from w in db.Way_Bill
                             join i in db.Imports on w.import_id equals i.import_id
                             where i.importing_status_id == 9 || i.importing_status_id == 10 || i.importing_status_id == 22 || i.importing_status_id == 23
                             orderby w.way_bill_id descending
                             select new { w.way_bill_id, w.way_bill_code }).Distinct();
            return (new SelectList(way_bills, "way_bill_id", "way_bill_code"));
        }
        public object E_Way_Bill_payment()
        {
            var way_bills = (from w in db.E_Way_Bill
                             join e in db.Exports on w.export_id equals e.export_id
                             where e.exporting_status_id == 27 || e.exporting_status_id == 28 || e.exporting_status_id == 32 || e.exporting_status_id == 33
                             orderby w.e_way_bill_id descending
                             select new { w.e_way_bill_id, w.e_way_bill_code }).Distinct();
            return (new SelectList(way_bills, "e_way_bill_id", "e_way_bill_code"));
        }
        public object Way_Bill_Release()
        {
            var way_bills = (from w in db.Way_Bill
                             join i in db.Imports on w.import_id equals i.import_id
                             where i.importing_status_id == 10 || i.importing_status_id == 23
                             orderby w.way_bill_id descending
                             select new { w.way_bill_id, w.way_bill_code }).Distinct();
            return (new SelectList(way_bills, "way_bill_id", "way_bill_code"));
        }
        public object E_Way_Bill_Release()
        {
            var way_bills = (from w in db.E_Way_Bill
                             join e in db.Exports on w.export_id equals e.export_id
                             where e.exporting_status_id == 28 || e.exporting_status_id == 33
                             orderby w.e_way_bill_id descending
                             select new { w.e_way_bill_id, w.e_way_bill_code }).Distinct();
            return (new SelectList(way_bills, "e_way_bill_id", "e_way_bill_code"));
        }
        public object Way_Bill_Assign_User()
        {
            var user = db.Get_Way_Bill_Assign_User();
            return (new SelectList(user, "user_id", "user_nanme"));
        }
        public object E_Way_Bill_Assign_User()
        {
            var user = db.E_Get_Way_Bill_Assign_User();
            return (new SelectList(user, "user_id", "user_nanme"));
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
                          where i.way_bill_id == null
                           orderby i.import_id descending
                           select i;
            return (new SelectList(imports, "import_id", "import_code"));
        }
        public object Export()
        {
            var exports = from e in db.Exports
                          where e.e_way_bill_id == null
                          orderby e.export_id descending
                          select e;
            return (new SelectList(exports, "export_id", "export_code"));
        }
        //Taking values from unit_of_measure table
        public object Unit_Of_Measures()
        {
            var unit_of_measures = from u in db.Unit_Of_Measure
                                   orderby u.unit_code ascending
                                   select u;
            return (new SelectList(unit_of_measures, "unit_id", "unit_code"));
        }
        public object Levi()
        {
            var levi = from l in db.Levis
                       orderby l.levi_id descending
                       select l;
            return (new SelectList(levi, "levi_id", "levi_name"));
        }
        public object Levi_Type()
        {
            var levi = from lt in db.Levi_Type
                       orderby lt.levi_type_id ascending
                       where lt.levi_type_id != 1 && lt.levi_type_id != 0
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

        public object Payment_Mode()
        {
            var payment_mode = from p in db.Payment_mode
                               orderby p.payment_mode_id ascending
                               select p;
            return (new SelectList(payment_mode, "payment_mode_id", "payment_mode"));
        }

        public object Sos_Usd_Currency()
        {
            var path = Path.Combine(HttpRuntime.AppDomainAppPath, "App_Data/For_Config.xml");
            XDocument doc = XDocument.Load(path);
            var sos_id = Convert.ToInt32(doc.Descendants("SOS_Currency_Id").First().Value);
            var usd_id = Convert.ToInt32(doc.Descendants("USD_Currency_Id").First().Value);
            var currency_id = from c in db.Currencies
                              where c.currency_id == sos_id || c.currency_id == usd_id
                              orderby c.currency_id descending
                              select c;
            return (new SelectList(currency_id, "currency_id", "currency_name"));
        }

        public ActionResult Error_Message(string error_message)
        {
            TempData["error_message"] = error_message;
            return View();
        }

        public ActionResult Forgot_Password()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Forgot_Password( Change_User_Password user)
        {
            var is_exist = (from e in db.Employees
                            join u in db.Users on e.employee_id equals u.employee_id
                            where e.email_id == user.email_id && u.user_name == user.user_name
                            select new { e.first_name, e.middle_name, e.last_name, u.password });
            if (is_exist.Count() > 0)
            {
                var password = new Manage_UserController().Decrypt(is_exist.First().password);
                //following code used for sending mail
                MailMessage Msg = new MailMessage("bteemtest@gmail.com", user.email_id);//first mail address is sender mail id second is reciever mail id
                // Sender e-mail address.
                Msg.Subject = "Username and Password for Puntland Port Taxation User";
                Msg.Body = "Hai " + is_exist.First().first_name + ' ' + is_exist.First().middle_name + ' ' + is_exist.First().last_name + "\n" + "Your username = " + user.user_name + " and password = " + password;

                // your remote SMTP server IP.
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new System.Net.NetworkCredential()
                {
                    UserName = "bteemtest@gmail.com",//sender mail Id
                    Password = "only4bteem"//Sender mail's password
                };
                smtp.EnableSsl = true;
                smtp.Send(Msg);
                TempData["errorMessage"] = "Password Sent To Registered Email";
            }
            else
            {
                TempData["errorMessage"] = "You Entered Incorrect Details";
            }
            return RedirectToAction("Index");
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

        [HttpPost]
        public ActionResult bind_unit_of_measure_dropdown_list(int id)
        {
            var values = (from gt in db.Goods_Tariff
                          join u in db.Unit_Of_Measure on gt.unit_of_measure_id equals u.unit_id
                          where gt.goods_id == id
                          select new DropdownListModel { Id = u.unit_id, name = u.unit_code });

            if (values == null)
                return Json(null);

            List<DropdownListModel> items = (List<DropdownListModel>)values.ToList();
            return Json(items);
        }

        [HttpPost]
        public ActionResult bind_unit_of_measure_dropdown_list_tally_sheet(string goods)
        {
            var values = (from gt in db.Goods_Tariff
                          join g in db.Goods on gt.goods_id equals g.goods_id
                          join u in db.Unit_Of_Measure on gt.unit_of_measure_id equals u.unit_id
                          where g.goods_name == goods
                          select new DropdownListModel { Id = u.unit_id, name = u.unit_code });

            if (values == null)
                return Json(null);

            List<DropdownListModel> items = (List<DropdownListModel>)values.ToList();
            return Json(items);
        }

        //Save Tax Details 
        public ActionResult Save_Excel(int way_bill_id)
        {
            if (Session["login_status"] != null)
            {
                TempData["way_bill_id"] = way_bill_id;
                ViewBag.currency = new HomeController().Sos_Usd_Currency();
                return View();
            }
            else
            {
                return RedirectToAction("../Home");
            }
        }

        //
        // GET: /Home/ExportData_ToExcel/W0009
        public ActionResult ExportData_ToExcel(int way_bill_id)
        {
            // Report_date = new DateTime(2015,12,19);

            //temp file name 
            string copy_file_name = DateTime.Now.Ticks + ".xlsx";
            //template file name 
            string Orginal_file_path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Report_Templates"), "Bolleto_Dogonale_Import.xlsx");
            //temp file path
            string copy_file_path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Images"), copy_file_name);

            XlsFile bolleto = new XlsFile(Orginal_file_path);



            bolleto.ActiveSheet = 1;

            var bolleto_code = (from i in db.Imports
                                where i.way_bill_id == way_bill_id
                                select i.bollete_dogonale_code).First();
            var j = 0;
            var bollete_dogonale = (from im in db.Imports
                                    join w in db.Way_Bill on im.way_bill_id equals w.way_bill_id
                                    join sa in db.Ship_Arrival on im.ship_arrival_id equals sa.ship_arrival_id
                                    join cpc in db.Calculated_Payment_Config on im.way_bill_id equals cpc.way_bill_id
                                    where im.way_bill_id == way_bill_id
                                    select new Bolleto_DogonaleModel { way_bill_code = w.way_bill_code, import_code = im.import_code, ship_arrival_code = sa.ship_arrival_code, bolleto_dogonale_code = im.bollete_dogonale_code, date = cpc.calculated_date });
            foreach (var item in bollete_dogonale)
            {

                bolleto.SetCellValue(2, 3, item.bolleto_dogonale_code);
                bolleto.SetCellValue(3, 3, item.way_bill_code);
                bolleto.SetCellValue(4, 3, item.import_code);
                bolleto.SetCellValue(5, 3, item.ship_arrival_code);
                bolleto.SetCellValue(6, 3, item.date.ToString("yyyy-MM-dd"));
                //bolleto.SetCellFormat(1, 1, 8, 7, 1);


            }

            db.Display_Tax_Details(way_bill_id, 100);
            var display1 = from d1 in db.TempDisplay1
                           where d1.way_bill_id == way_bill_id
                           select d1;
            var display2 = (from d2 in db.TempDisplay2
                            where d2.way_bill_id == way_bill_id
                            select d2).ToList();
            var iRowCnt = 9;



            TFlxFormat tformatTitle = bolleto.GetCellVisibleFormatDef(9, 1);
            int titleFormat = bolleto.AddFormat(tformatTitle);

            TFlxFont tfont = new TFlxFont();
            tfont.Style = TFlxFontStyles.Bold;

            TFlxFormat tformatright = bolleto.GetCellVisibleFormatDef(10, 6);
            tformatright.Font = tfont;
            int Formatright = bolleto.AddFormat(tformatright);


            foreach (var item in display1)
            {



                // SHOW COLUMNS ON THE TOP. 

                bolleto.SetCellValue(iRowCnt, 1, "Goods", titleFormat);
                bolleto.SetCellValue(iRowCnt, 2, "Unit Price", titleFormat);
                bolleto.SetCellValue(iRowCnt, 3, "Calculated Tariff", titleFormat);
                bolleto.SetCellValue(iRowCnt, 4, "Total Quantity", titleFormat);
                bolleto.SetCellValue(iRowCnt, 5, "Unit Of Measure", titleFormat);
                bolleto.SetCellValue(iRowCnt, 6, "Base Taxation", titleFormat);
                bolleto.SetCellValue(iRowCnt, 7, "Total", titleFormat);

                //bolleto.SetCellFormat(iRowCnt, 1, iRowCnt, 7, 1);
                var x = "A" + iRowCnt;
                var y = "G" + iRowCnt;

                //xlWorkSheetToExport.Range[x, y].Font.Size = 11;
                //xlWorkSheetToExport.Range[x, y].Font.Bold = true;

                iRowCnt = iRowCnt + 1;
                bolleto.SetCellValue(iRowCnt, 1, item.goods_name);
                bolleto.SetCellValue(iRowCnt, 2, item.toal_Quantity);
                bolleto.SetCellValue(iRowCnt, 3, item.Unit_Code);
                bolleto.SetCellValue(iRowCnt, 4, item.Unit_Price);
                bolleto.SetCellValue(iRowCnt, 5, item.calculated_Tariff);
                bolleto.SetCellValue(iRowCnt, 6, item.Base_Taxation);
                bolleto.SetCellValue(iRowCnt, 7, item.Total);
                iRowCnt = iRowCnt + 2;
                x = "A" + iRowCnt;
                y = "G" + iRowCnt;
                //xlWorkSheetToExport.Range[x, y].Font.Size = 11;
                //xlWorkSheetToExport.Range[x, y].Font.Bold = true;

                bolleto.SetCellValue(iRowCnt, 2, "Levy Name", titleFormat);
                bolleto.SetCellValue(iRowCnt, 3, "Levy Type", titleFormat);
                bolleto.SetCellValue(iRowCnt, 4, "Levy", titleFormat);
                bolleto.SetCellValue(iRowCnt, 5, "Actual Levy", titleFormat);
                bolleto.SetCellValue(iRowCnt, 6, "Total Levy", titleFormat);
                bolleto.SetCellValue(iRowCnt, 7, "");
                //bolleto.SetCellFormat(iRowCnt, 1, iRowCnt, 7, 1);
                iRowCnt = iRowCnt + 1;
                for (var l = 0; l < display2.Count(); l++)
                {
                    if (l == 0 && j != 0)
                    {
                        l = j;
                    }
                    if (item.Goods_Id != display2[l].Goods_Id)
                    {
                        break;
                    }
                    j++;
                    if (display2[l].Levy_Name == "Total")
                    {
                        bolleto.SetCellValue(iRowCnt, 2, display2[l].Levy_Name, titleFormat);
                        bolleto.SetCellValue(iRowCnt, 3, display2[l].Levy_Type, titleFormat);
                        bolleto.SetCellValue(iRowCnt, 4, display2[l].Levy, titleFormat);
                        //bolleto.SetCellValue(iRowCnt, 4).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                        bolleto.SetCellValue(iRowCnt, 5, display2[l].Actua_Levy, Formatright);
                        bolleto.SetCellValue(iRowCnt, 6, display2[l].Total_Levi, Formatright);
                        bolleto.SetCellValue(iRowCnt, 7, display2[l].Sum_Levies, Formatright);
                        //bolleto.SetCellFormat(iRowCnt, 1, iRowCnt, 7, 1);
                        iRowCnt = iRowCnt + 1;
                    }
                    else
                    {
                        bolleto.SetCellValue(iRowCnt, 2, display2[l].Levy_Name);
                        bolleto.SetCellValue(iRowCnt, 3, display2[l].Levy_Type);
                        bolleto.SetCellValue(iRowCnt, 4, display2[l].Levy);
                        //bolleto.SetCellValue(iRowCnt, 4).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                        bolleto.SetCellValue(iRowCnt, 5, display2[l].Actua_Levy);
                        bolleto.SetCellValue(iRowCnt, 6, display2[l].Total_Levi);
                        bolleto.SetCellValue(iRowCnt, 7, display2[l].Sum_Levies);
                        iRowCnt = iRowCnt + 1;
                    }
                }
                iRowCnt = iRowCnt + 1;
            }


            var save_name = "Bolleto_Dogonale_" + bolleto_code + ".xlsx";

            // SAVE THE FILE IN A FOLDER.
            //bolleto.Save(Path.Combine(Server.MapPath("~/App_Data"), copy_file_name));
            bolleto.Save(copy_file_path);

            this.DownLoadFile_Export(save_name, copy_file_path);

            return RedirectToAction("Index");
        }

        // DOWNLOAD THE FILE.
        protected void DownLoadFile_Export(string save_name, string copy_file_name)
        {
            string sPath = Server.MapPath("~/Images\\");
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + save_name);
            Response.TransmitFile(copy_file_name);
            Response.End();
        }

        // DOWNLOAD THE FILE.
        protected void DownLoadFile(string save_name)
        {
            string sPath = Server.MapPath("~/App_Data\\");
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + save_name);
            Response.TransmitFile(sPath + save_name);
            Response.End();
        }

        // DELETE THE FILE.
        protected void DeleteFile(string save_name)
        {
            string fullPath = Request.MapPath("~/App_Data/" + save_name);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }

        //
        // GET: /Home/ExportData_ToExcel/W0009
        public ActionResult E_ExportData_ToExcel(int way_bill_id)
        {
            if (Session["login_status"] != null)
            {

                //temp file name 
                string copy_file_name = DateTime.Now.Ticks + ".xlsx";
                //template file name 
                string Orginal_file_path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Report_Templates"), "Bolleto_Dogonale_Export.xlsx");
                //temp file path
                string copy_file_path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Images"), copy_file_name);

                XlsFile bolleto = new XlsFile(Orginal_file_path);

                bolleto.ActiveSheet = 1;
 

                var bolleto_code = (from i in db.Exports
                                    where i.e_way_bill_id == way_bill_id
                                    select i.e_bollete_dogonale_code).First();
                var j = 0;

                var bollete_dogonale = (from e in db.Exports
                                        join w in db.E_Way_Bill on e.e_way_bill_id equals w.e_way_bill_id
                                        join sd in db.Ship_Departure on e.ship_departure_id equals sd.ship_departure_id
                                        join cpc in db.E_Calculated_Payment_Config on w.e_way_bill_id equals cpc.way_bill_id
                                        where e.e_way_bill_id == way_bill_id
                                        select new Bolleto_DogonaleModel { way_bill_code = w.e_way_bill_code, import_code = e.export_code, ship_arrival_code = sd.ship_departure_code, bolleto_dogonale_code = e.e_bollete_dogonale_code, date = cpc.calculated_date });
               
                foreach (var item in bollete_dogonale)
                {

                    bolleto.SetCellValue(2, 3, item.bolleto_dogonale_code);
                    bolleto.SetCellValue(3, 3, item.way_bill_code);
                    bolleto.SetCellValue(4, 3, item.import_code);
                    bolleto.SetCellValue(5, 3, item.ship_arrival_code);
                    bolleto.SetCellValue(6, 3, item.date.ToString("yyyy-MM-dd"));
                    //bolleto.SetCellFormat(1, 1, 8, 7, 1);


                }
                var grand_total = db.E_Get_Grand_Total(way_bill_id, 100);
                string grand_total_value = ""; 
                foreach (var v in grand_total)
                {
                    grand_total_value = v;
                }
                db.E_Display_Tax_Details(way_bill_id, 100);
                var display1 = from d1 in db.E_TempDisplay1
                               where d1.way_bill_id == way_bill_id
                               select d1;
                var display2 = (from d2 in db.E_TempDisplay2
                                where d2.way_bill_id == way_bill_id
                                select d2).ToList();



                TFlxFormat tformatTitle = bolleto.GetCellVisibleFormatDef(9, 1);
                int titleFormat = bolleto.AddFormat(tformatTitle);

                TFlxFont tfont = new TFlxFont();
                tfont.Style = TFlxFontStyles.Bold;

                TFlxFormat tformatright = bolleto.GetCellVisibleFormatDef(10, 6);
                tformatright.Font = tfont;
                int Formatright = bolleto.AddFormat(tformatright);

                int iRowCnt = 9;

                foreach (var item in display1)
                {

                    // SHOW COLUMNS ON THE TOP. 

                    bolleto.SetCellValue(iRowCnt, 1, "Goods", titleFormat);
                    bolleto.SetCellValue(iRowCnt, 2, "Unit Price", titleFormat);
                    bolleto.SetCellValue(iRowCnt, 3, "Calculated Tariff", titleFormat);
                    bolleto.SetCellValue(iRowCnt, 4, "Total Quantity", titleFormat);
                    bolleto.SetCellValue(iRowCnt, 5, "Unit Of Measure", titleFormat);
                    bolleto.SetCellValue(iRowCnt, 6, "Base Taxation", titleFormat);
                    bolleto.SetCellValue(iRowCnt, 7, "Total", titleFormat);

                    //bolleto.SetCellFormat(iRowCnt, 1, iRowCnt, 7, 1);
                    var x = "A" + iRowCnt;
                    var y = "G" + iRowCnt;

                    //xlWorkSheetToExport.Range[x, y].Font.Size = 11;
                    //xlWorkSheetToExport.Range[x, y].Font.Bold = true;

                    iRowCnt = iRowCnt + 1;
                    bolleto.SetCellValue(iRowCnt, 1, item.goods_name);
                    bolleto.SetCellValue(iRowCnt, 2, item.toal_Quantity);
                    bolleto.SetCellValue(iRowCnt, 3, item.Unit_Code);
                    bolleto.SetCellValue(iRowCnt, 4, item.Unit_Price);
                    bolleto.SetCellValue(iRowCnt, 5, item.calculated_Tariff);
                    bolleto.SetCellValue(iRowCnt, 6, item.Base_Taxation);
                    bolleto.SetCellValue(iRowCnt, 7, item.Total);
                    iRowCnt = iRowCnt + 2;
                    x = "A" + iRowCnt;
                    y = "G" + iRowCnt;
                    //xlWorkSheetToExport.Range[x, y].Font.Size = 11;
                    //xlWorkSheetToExport.Range[x, y].Font.Bold = true;

                    bolleto.SetCellValue(iRowCnt, 2, "Levy Name", titleFormat);
                    bolleto.SetCellValue(iRowCnt, 3, "Levy Type", titleFormat);
                    bolleto.SetCellValue(iRowCnt, 4, "Levy", titleFormat);
                    bolleto.SetCellValue(iRowCnt, 5, "Actual Levy", titleFormat);
                    bolleto.SetCellValue(iRowCnt, 6, "Total Levy", titleFormat);
                    bolleto.SetCellValue(iRowCnt, 7, "");
                    //bolleto.SetCellFormat(iRowCnt, 1, iRowCnt, 7, 1);
                    iRowCnt = iRowCnt + 1;
                    for (var l = 0; l < display2.Count(); l++)
                    {
                        if (l == 0 && j != 0)
                        {
                            l = j;
                        }
                        if (item.Goods_Id != display2[l].Goods_Id)
                        {
                            break;
                        }
                        j++;
                        if (display2[l].Levy_Name == "Total")
                        {
                            bolleto.SetCellValue(iRowCnt, 2, display2[l].Levy_Name, titleFormat);
                            bolleto.SetCellValue(iRowCnt, 3, display2[l].Levy_Type, titleFormat);
                            bolleto.SetCellValue(iRowCnt, 4, display2[l].Levy, titleFormat);
                            //bolleto.SetCellValue(iRowCnt, 4).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                            bolleto.SetCellValue(iRowCnt, 5, display2[l].Actua_Levy, Formatright);
                            bolleto.SetCellValue(iRowCnt, 6, display2[l].Total_Levi, Formatright);
                            bolleto.SetCellValue(iRowCnt, 7, display2[l].Sum_Levies, Formatright);
                            //bolleto.SetCellFormat(iRowCnt, 1, iRowCnt, 7, 1);
                            iRowCnt = iRowCnt + 1;
                        }
                        else
                        {
                            bolleto.SetCellValue(iRowCnt, 2, display2[l].Levy_Name);
                            bolleto.SetCellValue(iRowCnt, 3, display2[l].Levy_Type);
                            bolleto.SetCellValue(iRowCnt, 4, display2[l].Levy);
                            //bolleto.SetCellValue(iRowCnt, 4).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                            bolleto.SetCellValue(iRowCnt, 5, display2[l].Actua_Levy);
                            bolleto.SetCellValue(iRowCnt, 6, display2[l].Total_Levi);
                            bolleto.SetCellValue(iRowCnt, 7, display2[l].Sum_Levies);
                            iRowCnt = iRowCnt + 1;
                        }
                    }
                    iRowCnt = iRowCnt + 1;
                }

                var get_payment_config = db.E_Calculated_Payment_Config.Where(cpc => cpc.way_bill_id == way_bill_id).ToList();
                foreach (var item in get_payment_config)
                {
                    TempData["sos_amount"] = item.calculated_sos_amount;
                    TempData["usd_amount"] = item.calculated_usd_amount;
                    TempData["sos"] = item.calculated_sos_part;
                    TempData["usd"] = item.calculated_usd_part;
                }


                iRowCnt = iRowCnt + 3;

                bolleto.SetCellValue(iRowCnt, 4, "USD Amount (" + TempData["usd"] + "%)", titleFormat);
                bolleto.SetCellValue(iRowCnt, 5, TempData["usd_amount"], titleFormat);

                iRowCnt++;

                bolleto.SetCellValue(iRowCnt, 4, "SOS Amount (" + TempData["sos"] + "%)", titleFormat);
                bolleto.SetCellValue(iRowCnt, 5, TempData["sos_amount"], titleFormat);


                iRowCnt++;
              

                bolleto.SetCellValue(iRowCnt,7,"Date : " + DateTime.Now.ToString("yyyy-MM-dd"));

                iRowCnt++;

                bolleto.SetCellValue(iRowCnt, 7, "User : " + Session["user_name"].ToString());

                var save_name = "Bolleto_Dogonale_Export_" + bolleto_code + ".xlsx";

                // SAVE THE FILE IN A FOLDER.
                //bolleto.Save(Path.Combine(Server.MapPath("~/App_Data"), copy_file_name));
                bolleto.Save(copy_file_path);

                this.DownLoadFile_Export(save_name, copy_file_path);

                return RedirectToAction("Index");


           

            

           
            }
            else
            {
                return RedirectToAction("../Home");
            }
        }

       
        public ActionResult View_Tariff_Document(string document_name)
        {
            if (Session["login_status"] != null)
            {
                string sPath = Server.MapPath("~/App_Data\\");
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + document_name);
                Response.TransmitFile(sPath + document_name);
                Response.End();
                return Redirect(Request.UrlReferrer.ToString());
            }
            else
            {
                return RedirectToAction("../Home");
            }
        }

        //View Tax Details 
        public ActionResult View_Tax_Details(int way_bill_id, string controllerName)
        {
            if (Session["login_status"] != null)
            {
                TempData["way_bill_id"] = way_bill_id;
                Session["controllerName"] = controllerName;
                ViewBag.currency = new HomeController().Sos_Usd_Currency();
                return View();
            }
            else
            {
                return RedirectToAction("../Home");
            }
        }

        public ActionResult Display_Tax_Details(int currency_id, int way_bill_id, string controllerName)
        {
            if (Session["login_status"] != null)
            {
                ViewBag.currency = new HomeController().Sos_Usd_Currency();
                TempData["way_bill_id"] = way_bill_id;
                TempData["currency_id"] = currency_id;
                var grand_total = db.Get_Grand_Total(way_bill_id, currency_id);
                foreach (var v in grand_total)
                {
                    ViewBag.grand_total = v;
                }
                db.Display_Tax_Details(way_bill_id, currency_id);
                var display1 = from d1 in db.TempDisplay1
                               where d1.way_bill_id == way_bill_id
                               select d1;
                var display2 = (from d2 in db.TempDisplay2
                                where d2.way_bill_id == way_bill_id
                                select d2).ToList();
                ViewBag.display2 = display2;
                var bollete_dogonale = (from c in db.Calculated_Levi
                                        join w in db.Way_Bill on c.way_bill_id equals w.way_bill_id
                                        join i in db.Imports on w.import_id equals i.import_id
                                        join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                        join cpc in db.Calculated_Payment_Config on w.way_bill_id equals cpc.way_bill_id
                                        where c.way_bill_id == way_bill_id
                                        select new Bolleto_DogonaleModel { way_bill_code = w.way_bill_code, import_code = i.import_code, ship_arrival_code = sa.ship_arrival_code, bolleto_dogonale_code = i.bollete_dogonale_code, date = cpc.calculated_date, mark = w.mark }).Distinct();
                ViewBag.bolleto = bollete_dogonale;
                var get_payment_config = db.Calculated_Payment_Config.Where(cpc => cpc.way_bill_id == way_bill_id).ToList();
                var employee = from cpc in db.Calculated_Payment_Config
                               join e in db.Employees on cpc.employee_id equals e.employee_id
                               where cpc.way_bill_id == way_bill_id
                               select new { employee = e.first_name + " " + e.middle_name + " " + e.last_name };
                foreach (var item in employee)
                {
                    TempData["employee"] = item.employee;
                }
                foreach (var item in get_payment_config)
                {
                    TempData["sos_amount"] = item.calculated_sos_amount;
                    TempData["usd_amount"] = item.calculated_usd_amount;
                    TempData["sos"] = item.calculated_sos_part;
                    TempData["usd"] = item.calculated_usd_part;
                }
                var importing_status_id = db.Imports.Where(i => i.way_bill_id == way_bill_id).Select( i=> i.importing_status_id).FirstOrDefault();
                TempData["importing_status_id"] = importing_status_id;
                TempData["controller"] = controllerName;
                return View(display1.ToList());
            }
            else
            {
                return RedirectToAction("../Home");
            }
        }

        public ActionResult E_Display_Tax_Details(int currency_id, int way_bill_id, string controllerName)
        {
            if (Session["login_status"] != null)
            {
                ViewBag.currency = new HomeController().Sos_Usd_Currency();
                TempData["way_bill_id"] = way_bill_id;
                TempData["currency_id"] = currency_id;
                var grand_total = db.E_Get_Grand_Total(way_bill_id, currency_id);
                foreach (var v in grand_total)
                {
                    ViewBag.grand_total = v;
                }
                db.E_Display_Tax_Details(way_bill_id, currency_id);
                var display1 = from d1 in db.E_TempDisplay1
                               where d1.way_bill_id == way_bill_id
                               select d1;
                var display2 = (from d2 in db.E_TempDisplay2
                                where d2.way_bill_id == way_bill_id
                                select d2).ToList();
                ViewBag.display2 = display2;
                var bollete_dogonale = (from c in db.E_Calculated_Levi
                                        join w in db.E_Way_Bill on c.way_bill_id equals w.e_way_bill_id
                                        join e in db.Exports on w.export_id equals e.export_id
                                        join sd in db.Ship_Departure on e.ship_departure_id equals sd.ship_departure_id
                                        join cpc in db.E_Calculated_Payment_Config on w.e_way_bill_id equals cpc.way_bill_id
                                        where c.way_bill_id == way_bill_id
                                        select new Bolleto_DogonaleModel { way_bill_code = w.e_way_bill_code, import_code = e.export_code, ship_arrival_code = sd.ship_departure_code, bolleto_dogonale_code = e.e_bollete_dogonale_code, date = cpc.calculated_date, mark = w.mark }).Distinct();
                ViewBag.bolleto = bollete_dogonale;
                var get_payment_config = db.E_Calculated_Payment_Config.Where(cpc => cpc.way_bill_id == way_bill_id).ToList();
                var employee = from cpc in db.E_Calculated_Payment_Config
                               join e in db.Employees on cpc.employee_id equals e.employee_id
                               where cpc.way_bill_id == way_bill_id
                               select new { employee = e.first_name + " " + e.middle_name + " " + e.last_name };
                foreach (var item in employee)
                {
                    TempData["employee"] = item.employee;
                }
                foreach (var item in get_payment_config)
                {
                    TempData["sos_amount"] = item.calculated_sos_amount;
                    TempData["usd_amount"] = item.calculated_usd_amount;
                    TempData["sos"] = item.calculated_sos_part;
                    TempData["usd"] = item.calculated_usd_part;
                }
                var importing_status_id = db.Exports.Where(i => i.e_way_bill_id == way_bill_id).Select(i => i.exporting_status_id).FirstOrDefault();
                TempData["importing_status_id"] = importing_status_id;
                TempData["controller"] = controllerName;
                return View(display1.ToList());
            }
            else
            {
                return RedirectToAction("../Home");
            }
        }

        //View Penalty Details 
        public ActionResult Display_Penalty_Details(int currency_id, int way_bill_id, string controllerName)
        {
            if (Session["login_status"] != null)
            {
                ViewBag.currency = new HomeController().Sos_Usd_Currency();
                TempData["way_bill_id"] = way_bill_id;
                TempData["currency_id"] = currency_id;
                var grand_total = db.Get_Penalty_Grand_Total(way_bill_id, currency_id);
                foreach (var v in grand_total)
                {
                    ViewBag.grand_total = v.Grand_total;
                    ViewBag.Penalty = v.Penalty;
                    ViewBag.total = v.total;
                    ViewBag.Total_Penalty = v.Total_Penalty;
                }
                db.Display_Penalty_Details(way_bill_id, currency_id);
                var display1 = from d1 in db.Temp_Penalty_Display1
                               where d1.way_bill_id == way_bill_id
                               select d1;
                var display2 = (from d2 in db.Temp_penalty_Display2
                                where d2.way_bill_id == way_bill_id
                                select d2).ToList();
                ViewBag.display2 = display2;
                var bollete_dogonale = (from c in db.Calculated_Penalty
                                        join w in db.Way_Bill on c.way_bill_id equals w.way_bill_id
                                        join i in db.Imports on w.import_id equals i.import_id
                                        join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                        join cpc in db.Calculated_Penalty_Config on w.way_bill_id equals cpc.way_bill_id
                                        where c.way_bill_id == way_bill_id
                                        select new Bolleto_DogonaleModel { way_bill_code = w.way_bill_code, import_code = i.import_code, ship_arrival_code = sa.ship_arrival_code, bolleto_dogonale_code = i.bollete_dogonale_code, date = cpc.calculated_date, mark = w.mark }).Distinct();
                ViewBag.bolleto = bollete_dogonale;
                var get_payment_config = db.Calculated_Penalty_Config.Where(cpc => cpc.way_bill_id == way_bill_id).ToList();
                var employee = from cpc in db.Calculated_Penalty_Config
                               join e in db.Employees on cpc.employee_id equals e.employee_id
                               where cpc.way_bill_id == way_bill_id
                               select new { employee = e.first_name + " " + e.middle_name + " " + e.last_name };
                foreach (var item in employee)
                {
                    TempData["employee"] = item.employee;
                }                
                foreach (var item in get_payment_config)
                {
                    TempData["sos_amount"] = item.calculated_sos_amount;
                    TempData["usd_amount"] = item.calculated_usd_amount;
                    TempData["sos"] = item.calculated_sos_part;
                    TempData["usd"] = item.calculated_usd_part;
                }
                var importing_status_id = db.Imports.Where(i => i.way_bill_id == way_bill_id).Select(i => i.importing_status_id).FirstOrDefault();
                TempData["importing_status_id"] = importing_status_id;
                TempData["controller"] = controllerName;
                return View(display1.ToList());
            }
            else
            {
                return RedirectToAction("../Home");
            }
        }

        //View Export Penalty Details 
        public ActionResult E_Display_Penalty_Details(int currency_id, int way_bill_id, string controllerName)
        {
            if (Session["login_status"] != null)
            {
                ViewBag.currency = new HomeController().Sos_Usd_Currency();
                TempData["way_bill_id"] = way_bill_id;
                TempData["currency_id"] = currency_id;
                var grand_total = db.E_Get_Penalty_Grand_Total(way_bill_id, currency_id);
                foreach (var v in grand_total)
                {
                    ViewBag.grand_total = v.Grand_total;
                    ViewBag.Penalty = v.Penalty;
                    ViewBag.total = v.total;
                    ViewBag.Total_Penalty = v.Total_Penalty;
                }
                db.E_Display_Penalty_Details(way_bill_id, currency_id);
                var display1 = from d1 in db.E_Temp_Penalty_Display1
                               where d1.way_bill_id == way_bill_id
                               select d1;
                var display2 = (from d2 in db.E_Temp_penalty_Display2
                                where d2.way_bill_id == way_bill_id
                                select d2).ToList();
                ViewBag.display2 = display2;
                var bollete_dogonale = (from c in db.E_Calculated_Penalty
                                        join w in db.E_Way_Bill on c.way_bill_id equals w.e_way_bill_id
                                        join e in db.Exports on w.export_id equals e.export_id
                                        join sd in db.Ship_Departure on e.ship_departure_id equals sd.ship_departure_id
                                        join cpc in db.E_Calculated_Penalty_Config on w.e_way_bill_id equals cpc.e_way_bill_id
                                        where c.way_bill_id == way_bill_id
                                        select new Bolleto_DogonaleModel { way_bill_code = w.e_way_bill_code, import_code = e.export_code, ship_arrival_code = sd.ship_departure_code, bolleto_dogonale_code = e.e_bollete_dogonale_code, date = cpc.calculated_date, mark = w.mark }).Distinct();
                ViewBag.bolleto = bollete_dogonale;
                var get_payment_config = db.E_Calculated_Penalty_Config.Where(cpc => cpc.e_way_bill_id == way_bill_id).ToList();
                var employee = from cpc in db.E_Calculated_Penalty_Config
                               join e in db.Employees on cpc.employee_id equals e.employee_id
                               where cpc.e_way_bill_id == way_bill_id
                               select new { employee = e.first_name + " " + e.middle_name + " " + e.last_name };
                foreach (var item in employee)
                {
                    TempData["employee"] = item.employee;
                }                  
                foreach (var item in get_payment_config)
                {
                    TempData["sos_amount"] = item.calculated_sos_amount;
                    TempData["usd_amount"] = item.calculated_usd_amount;
                    TempData["sos"] = item.calculated_sos_part;
                    TempData["usd"] = item.calculated_usd_part;
                }
                var importing_status_id = db.Exports.Where(i => i.e_way_bill_id == way_bill_id).Select(i => i.exporting_status_id).FirstOrDefault();
                TempData["importing_status_id"] = importing_status_id;
                TempData["controller"] = controllerName;
                return View(display1.ToList());
            }
            else
            {
                return RedirectToAction("../Home");
            }
        }

        //Print Bolleto Tax Details 
        public ActionResult print_bolleto(int way_bill_id)
        {
            if (Session["login_status"] != null)
            {
                TempData["way_bill_id"] = way_bill_id;
                ViewBag.currency = new HomeController().Sos_Usd_Currency();
                return View();
            }
            else
            {
                return RedirectToAction("../Home");
            }
        }

        public ActionResult Home_Print_Bolleto(int currency_id, int way_bill_id)
        {
            if (Session["login_status"] != null)
            {
                var prev_url = Request.UrlReferrer.ToString();
                if (prev_url.Contains("Payment"))
                {
                    TempData["payment"] = "true";
                }
                var grand_total = db.Get_Grand_Total(way_bill_id, currency_id);
                foreach (var v in grand_total)
                {
                    ViewBag.grand_total = v;
                }
                db.Display_Tax_Details(way_bill_id, currency_id);
                var display1 = from d1 in db.TempDisplay1
                               where d1.way_bill_id == way_bill_id
                               select d1;
                var display2 = (from d2 in db.TempDisplay2
                                where d2.way_bill_id == way_bill_id
                                select d2).ToList();
                ViewBag.display2 = display2;
                var bollete_dogonale = (from c in db.Calculated_Levi
                                        join w in db.Way_Bill on c.way_bill_id equals w.way_bill_id
                                        join i in db.Imports on w.import_id equals i.import_id
                                        join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                        join cpc in db.Calculated_Payment_Config on w.way_bill_id equals cpc.way_bill_id
                                        where c.way_bill_id == way_bill_id
                                        select new Bolleto_DogonaleModel { way_bill_code = w.way_bill_code, import_code = i.import_code, ship_arrival_code = sa.ship_arrival_code, bolleto_dogonale_code = i.bollete_dogonale_code, date = cpc.calculated_date , mark = w.mark }).Distinct();
                ViewBag.bolleto = bollete_dogonale;
                var get_payment_config = db.Calculated_Payment_Config.Where(cpc => cpc.way_bill_id == way_bill_id).ToList();
                var employee = from cpc in db.Calculated_Payment_Config
                               join e in db.Employees on cpc.employee_id equals e.employee_id
                               where cpc.way_bill_id == way_bill_id
                               select new { employee = e.first_name + " " + e.middle_name + " " + e.last_name };
                foreach (var item in employee)
                {
                    TempData["employee"] = item.employee;
                }                     
                foreach (var item in get_payment_config)
                {
                    TempData["sos_amount"] = item.calculated_sos_amount;
                    TempData["usd_amount"] = item.calculated_usd_amount;
                    TempData["sos"] = item.calculated_sos_part;
                    TempData["usd"] = item.calculated_usd_part;
                }
                return View(display1.ToList());
            }
            else
            {
                return RedirectToAction("../Home");
            }
        }

        public ActionResult Home_Print_E_Bolleto(int currency_id, int way_bill_id)
        {
            if (Session["login_status"] != null)
            {
                var prev_url = Request.UrlReferrer.ToString();
                if (prev_url.Contains("Payment"))
                {
                    TempData["payment"] = "true";
                }
                var grand_total = db.E_Get_Grand_Total(way_bill_id, currency_id);
                foreach (var v in grand_total)
                {
                    ViewBag.grand_total = v;
                }
                db.E_Display_Tax_Details(way_bill_id, currency_id);
                var display1 = from d1 in db.E_TempDisplay1
                               where d1.way_bill_id == way_bill_id
                               select d1;
                var display2 = (from d2 in db.E_TempDisplay2
                                where d2.way_bill_id == way_bill_id
                                select d2).ToList();
                ViewBag.display2 = display2;
                var bollete_dogonale = (from c in db.E_Calculated_Levi
                                        join w in db.E_Way_Bill on c.way_bill_id equals w.e_way_bill_id
                                        join e in db.Exports on w.export_id equals e.export_id
                                        join sd in db.Ship_Departure on e.ship_departure_id equals sd.ship_departure_id
                                        join cpc in db.E_Calculated_Payment_Config on w.e_way_bill_id equals cpc.way_bill_id
                                        where c.way_bill_id == way_bill_id
                                        select new Bolleto_DogonaleModel { way_bill_code = w.e_way_bill_code, import_code = e.export_code, ship_arrival_code = sd.ship_departure_code, bolleto_dogonale_code = e.e_bollete_dogonale_code, date = cpc.calculated_date, mark = w.mark }).Distinct();
                ViewBag.bolleto = bollete_dogonale;
                var get_payment_config = db.E_Calculated_Payment_Config.Where(cpc => cpc.way_bill_id == way_bill_id).ToList();
                var employee = from cpc in db.E_Calculated_Payment_Config
                               join e in db.Employees on cpc.employee_id equals e.employee_id
                               where cpc.way_bill_id == way_bill_id
                               select new { employee = e.first_name + " " + e.middle_name + " " + e.last_name };
                foreach (var item in employee)
                {
                    TempData["employee"] = item.employee;
                }                     
                foreach (var item in get_payment_config)
                {
                    TempData["sos_amount"] = item.calculated_sos_amount;
                    TempData["usd_amount"] = item.calculated_usd_amount;
                    TempData["sos"] = item.calculated_sos_part;
                    TempData["usd"] = item.calculated_usd_part;
                }
                return View(display1.ToList());
            }
            else
            {
                return RedirectToAction("../Home");
            }
        }

        //Print Penalty Details 
        public ActionResult Home_Print_Penalty_Details(int currency_id, int way_bill_id)
        {
            if (Session["login_status"] != null)
            {
                var prev_url = Request.UrlReferrer.ToString();
                if (prev_url.Contains("Payment"))
                {
                    TempData["payment"] = "true";
                }
                var grand_total = db.Get_Penalty_Grand_Total(way_bill_id, currency_id);
                foreach (var v in grand_total)
                {
                    ViewBag.grand_total = v.Grand_total;
                    ViewBag.Penalty = v.Penalty;
                    ViewBag.total = v.total;
                    ViewBag.Total_Penalty = v.Total_Penalty;
                }
                db.Display_Penalty_Details(way_bill_id, currency_id);
                var display1 = from d1 in db.Temp_Penalty_Display1
                               where d1.way_bill_id == way_bill_id
                               select d1;
                var display2 = (from d2 in db.Temp_penalty_Display2
                                where d2.way_bill_id == way_bill_id
                                select d2).ToList();
                ViewBag.display2 = display2;
                var bollete_dogonale = (from c in db.Calculated_Penalty
                                        join w in db.Way_Bill on c.way_bill_id equals w.way_bill_id
                                        join i in db.Imports on w.import_id equals i.import_id
                                        join sa in db.Ship_Arrival on i.ship_arrival_id equals sa.ship_arrival_id
                                        join cpc in db.Calculated_Penalty_Config on w.way_bill_id equals cpc.way_bill_id
                                        where c.way_bill_id == way_bill_id
                                        select new Bolleto_DogonaleModel { way_bill_code = w.way_bill_code, import_code = i.import_code, ship_arrival_code = sa.ship_arrival_code, bolleto_dogonale_code = i.bollete_dogonale_code, date = cpc.calculated_date, mark = w.mark }).Distinct();
                ViewBag.bolleto = bollete_dogonale;
                var get_payment_config = db.Calculated_Penalty_Config.Where(cpc => cpc.way_bill_id == way_bill_id).ToList();
                var employee = from cpc in db.Calculated_Penalty_Config
                               join e in db.Employees on cpc.employee_id equals e.employee_id
                               where cpc.way_bill_id == way_bill_id
                               select new { employee = e.first_name + " " + e.middle_name + " " + e.last_name };
                foreach (var item in employee)
                {
                    TempData["employee"] = item.employee;
                }                
                foreach (var item in get_payment_config)
                {
                    TempData["sos_amount"] = item.calculated_sos_amount;
                    TempData["usd_amount"] = item.calculated_usd_amount;
                    TempData["sos"] = item.calculated_sos_part;
                    TempData["usd"] = item.calculated_usd_part;
                }
                return View(display1.ToList());
            }
            else
            {
                return RedirectToAction("../Home");
            }
        }

        //Print Export Penalty Details 
        public ActionResult E_Home_Print_Penalty_Details(int currency_id, int way_bill_id)
        {
            if (Session["login_status"] != null)
            {
                var prev_url = Request.UrlReferrer.ToString();
                if (prev_url.Contains("Payment"))
                {
                    TempData["payment"] = "true";
                }
                var grand_total = db.E_Get_Penalty_Grand_Total(way_bill_id, currency_id);
                foreach (var v in grand_total)
                {
                    ViewBag.grand_total = v.Grand_total;
                    ViewBag.Penalty = v.Penalty;
                    ViewBag.total = v.total;
                    ViewBag.Total_Penalty = v.Total_Penalty;
                }
                db.E_Display_Penalty_Details(way_bill_id, currency_id);
                var display1 = from d1 in db.E_Temp_Penalty_Display1
                               where d1.way_bill_id == way_bill_id
                               select d1;
                var display2 = (from d2 in db.E_Temp_penalty_Display2
                                where d2.way_bill_id == way_bill_id
                                select d2).ToList();
                ViewBag.display2 = display2;
                var bollete_dogonale = (from c in db.E_Calculated_Penalty
                                        join w in db.E_Way_Bill on c.way_bill_id equals w.e_way_bill_id
                                        join e in db.Exports on w.export_id equals e.export_id
                                        join sd in db.Ship_Departure on e.ship_departure_id equals sd.ship_departure_id
                                        join cpc in db.E_Calculated_Penalty_Config on w.e_way_bill_id equals cpc.e_way_bill_id
                                        where c.way_bill_id == way_bill_id
                                        select new Bolleto_DogonaleModel { way_bill_code = w.e_way_bill_code, import_code = e.export_code, ship_arrival_code = sd.ship_departure_code, bolleto_dogonale_code = e.e_bollete_dogonale_code, date = cpc.calculated_date, mark = w.mark }).Distinct();
                ViewBag.bolleto = bollete_dogonale;
                var get_payment_config = db.E_Calculated_Penalty_Config.Where(cpc => cpc.e_way_bill_id == way_bill_id).ToList();
                var employee = from cpc in db.E_Calculated_Penalty_Config
                               join e in db.Employees on cpc.employee_id equals e.employee_id
                               where cpc.e_way_bill_id == way_bill_id
                               select new { employee = e.first_name + " " + e.middle_name + " " + e.last_name };
                foreach (var item in employee)
                {
                    TempData["employee"] = item.employee;
                }                  
                foreach (var item in get_payment_config)
                {
                    TempData["sos_amount"] = item.calculated_sos_amount;
                    TempData["usd_amount"] = item.calculated_usd_amount;
                    TempData["sos"] = item.calculated_sos_part;
                    TempData["usd"] = item.calculated_usd_part;
                }
                return View(display1.ToList());
            }
            else
            {
                return RedirectToAction("../Home");
            }
        }
    }
}