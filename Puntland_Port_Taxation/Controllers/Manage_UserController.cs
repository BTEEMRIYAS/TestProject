using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using Puntland_Port_Taxation.Models;

namespace Puntland_Port_Taxation.Controllers
{
    public class Manage_UserController : Controller
    {
        private Puntland_Port_Taxation_DBEntities db = new Puntland_Port_Taxation_DBEntities();

        //
        // GET: /Manage_User/

        public ActionResult Index()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(3))
                {
                    int id = Convert.ToInt32(Session["id"]);
                    int page;
                    int page_no = 1;
                    var count = db.Employees.Where(e => e.employee_id != id).Count();
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
                    var employee = (from e in db.Employees
                                   join d in db.Departments on e.department_id equals d.department_id
                                   join r in db.Roles on e.role_id equals r.role_id
                                   select new EmployeeModel { employee_id = e.employee_id, first_name = e.first_name, middle_name = e.middle_name, last_name = e.last_name, email_id = e.email_id, department_name = d.department_name, role_name = r.role_name, status_id = e.status_id }).Where(e => e.employee_id != id).OrderBy(e => e.employee_id).Skip(start_from).Take(9).ToList();
                    return View(employee);
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
        // GET: /Manage_User/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(3))
                {
                    Employee employee = db.Employees.Find(id);
                    if (employee == null)
                    {
                        return HttpNotFound();
                    }
                    return View(employee);
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
        // GET: /Manage_User/Create

        public ActionResult Create()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(3))
                {
                    ViewBag.departments = new HomeController().Departments();
                    ViewBag.roles = new HomeController().Roles();
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
        // POST: /Manage_User/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee employee, User user)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(3))
                {
                    if (ModelState.IsValid)
                    {
                        var is_exist = (from e in db.Employees where e.email_id==employee.email_id select e).Count();
                        if (is_exist > 0)
                        {
                            TempData["errorMessage"] = "This User Already Exist";
                            return RedirectToAction("Index");
                        }
                        if (employee.profile_pic != null)
                        {
                            //following code is used for profile pic adding and storing in images folder
                            HttpPostedFileBase file = Request.Files.Get("profile_pic");

                            if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                            {
                                employee.profile_pic = file.FileName;
                                string fileContentType = file.ContentType;
                                byte[] fileBytes = new byte[file.ContentLength];
                                file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                                var FileLocation = Path.Combine(Server.MapPath("~/Images"), employee.profile_pic);
                                file.SaveAs(FileLocation);
                            }
                        }
                        employee.employee_code = "PT-PU-";
                        employee.status_id = 1;
                        employee.first_name = employee.first_name.Trim();
                        employee.middle_name = employee.middle_name.Trim();
                        employee.last_name = employee.last_name.Trim();
                        employee.created_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                        employee.updated_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                        db.Employees.Add(employee);                                       
                        db.SaveChanges();
                        TempData["errorMessage"] = "User Added Successfully";
                        var emp_id = employee.employee_id.ToString("0000");
                        employee.employee_code = employee.employee_code + emp_id;
                        db.Entry(employee).State = EntityState.Modified;
                        db.SaveChanges();
                        user.employee_id = employee.employee_id;
                        user.user_name = employee.employee_code;
                        var password = this.GetRandomNumber();
                        user.password = this.Encrypt(password);
                        db.Users.Add(user);
                        db.SaveChanges();
                        //following code used for sending mail
                        MailMessage Msg = new MailMessage("bteemtest@gmail.com", employee.email_id);//first mail address is sender mail id second is reciever mail id
                        // Sender e-mail address.
                        Msg.Subject = "Username and Password for Puntland Port Taxation User";
                        Msg.Body = "Hai " + employee.first_name + ' ' + employee.middle_name + ' ' + employee.last_name + "\n" + "Your username = " + employee.employee_code + " and password = " + password;

                        // your remote SMTP server IP.
                        SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                        smtp.Credentials = new System.Net.NetworkCredential()
                        {
                            UserName = "bteemtest@gmail.com",//sender mail Id
                            Password = "only4bteem"//Sender mail's password
                        };
                        smtp.EnableSsl = true;
                        smtp.Send(Msg);

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
        // GET: /Manage_User/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                Employee employee = db.Employees.Find(id);
                if (employee == null)
                {
                    return HttpNotFound();
                }
                TempData["user_id"] = employee.employee_id;
                TempData["role_id"] = employee.role_id;
                TempData["status_id"] = employee.status_id;
                TempData["department_id"] = employee.department_id;
                TempData["profile_pic"] = employee.profile_pic;
                TempData["employee_code"] = employee.employee_code;
                ViewBag.status = new HomeController().Status();
                ViewBag.departments = new HomeController().Departments();
                ViewBag.roles = new HomeController().Roles();
                return View(employee);
            }
            else
            {
                return RedirectToAction("../Home");
            }
        }

        //
        // POST: /Manage_User/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Employee employee)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                var employee_id = Convert.ToInt32(TempData["user_id"]);
                int id = Convert.ToInt32(Session["id"]);
                if (ModelState.IsValid)
                {
                    var is_exist = (from e in db.Employees where e.email_id == employee.email_id && e.employee_id != employee_id select e).Count();
                    if (is_exist > 0)
                    {
                        TempData["errorMessage"] = "This User Already Exist";
                        if (employee.employee_id == id)
                        {
                            return Redirect(Request.UrlReferrer.ToString());
                        }
                        else
                        {
                            return RedirectToAction("Index");
                        }
                    }
                    employee.updated_date = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                    if (employee.profile_pic != null)
                    {
                        //following code is used for profile pic adding and storing in images folder
                        HttpPostedFileBase file = Request.Files.Get("profile_pic");

                        if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                        {
                            employee.profile_pic = file.FileName;
                            string fileContentType = file.ContentType;
                            byte[] fileBytes = new byte[file.ContentLength];
                            file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                            var FileLocation = Path.Combine(Server.MapPath("~/Images"), employee.profile_pic);
                            file.SaveAs(FileLocation);
                        }
                    }
                    else if(TempData["profile_pic"] != null)
                    {
                        employee.profile_pic = TempData["profile_pic"].ToString();
                    }
                    employee.employee_id = Convert.ToInt32(TempData["user_id"]);
                    employee.first_name = employee.first_name.Trim();
                    employee.middle_name = employee.middle_name.Trim();
                    employee.last_name = employee.last_name.Trim();
                    if (employee.employee_id == id)
                    {
                        employee.role_id = Convert.ToInt32(TempData["role_id"]);
                        employee.status_id = Convert.ToInt32(TempData["status_id"]);
                        employee.department_id = Convert.ToInt32(TempData["department_id"]);
                        this.edit_session(employee);
                        employee.employee_code = TempData["employee_code"].ToString();
                        db.Entry(employee).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["errorMessage"] = "Edited Successfully";
                        return Redirect(Request.UrlReferrer.ToString());
                    }
                    employee.employee_code = TempData["employee_code"].ToString();
                    db.Entry(employee).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["errorMessage"] = "Edited Successfully";
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("../Home");
            }
        }

        //
        // GET: /Manage_User/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(3))
                {
                    Employee employee = db.Employees.Find(id);
                    if (employee == null)
                    {
                        return HttpNotFound();
                    }
                    return View(employee);
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
        // POST: /Manage_User/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(3))
                {
                    Employee employee = db.Employees.Find(id);
                    db.Employees.Remove(employee);
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
        // GET: /manage_user/check_user_name/abc

        public int check_user_name(string old)
        {
            var user_id = Convert.ToInt32(Session["user_id"]);
            User user = db.Users.Find(user_id);
            if (old == user.user_name)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        //
        // GET: /manage_user/change_user_name/5

        public ActionResult change_user_name(int id = 0)
        {
            if (Session["login_status"] != null)
            {            
                User user = db.Users.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                TempData["user_name"] = user.user_name;
                TempData["user_id"] = user.user_id;
                return View();
            }
            else
            {
                return RedirectToAction("../Home");
            }
        }


        // POST: /manage_user/change_user_name/5
        [HttpPost]
        public ActionResult change_user_name(Change_User_Password user)
        {
            if (Session["login_status"] != null)
            {
                var is_exist = db.Users.Where(u => u.user_name == user.new_user_name).Count();
                if(is_exist > 0)
                {
                    TempData["errorMessage"] = "This User Name Already Exists, Please Enter Another One";
                    return Redirect(Request.UrlReferrer.ToString());
                }
                var user_id = Convert.ToInt32(TempData["user_id"]);
                User user_new = db.Users.Find(user_id);
                var user_name = TempData["user_name"].ToString();
                if (ModelState.IsValid)
                {
                    if (user.old_user_name == user_name)
                    {
                        if (user.new_user_name == user.confirm_user_name)
                        {
                            user_new.user_name = user.new_user_name;
                            db.Entry(user_new).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["errorMessage"] = "User Name Changed Successfully";
                            return Redirect(Request.UrlReferrer.ToString());
                        }
                        else
                        {
                            TempData["errorMessage"] = "User Name Mismatch";
                            return Redirect(Request.UrlReferrer.ToString());
                        }
                    }
                    else
                    {
                        TempData["errorMessage"] = "Incorrect User Name";
                        return Redirect(Request.UrlReferrer.ToString());
                    }
                }
                return Redirect(Request.UrlReferrer.ToString());
            }
            else
            {
                return RedirectToAction("../Home");
            }
        }

        //
        // GET: /manage_user/check_user_name/abc

        public int check_password(string old)
        {
            var user_id = Convert.ToInt32(Session["user_id"]);
            User user = db.Users.Find(user_id);
            var old_encrypt = this.Encrypt(old);
            if (old_encrypt == user.password)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        //
        // GET: /manage_user/change_password/5

        public ActionResult change_password(int id = 0)
        {
            if (Session["login_status"] != null)
            {   
                User user = db.Users.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                TempData["password"] = user.password;
                TempData["user_id"] = user.user_id;
                return View();
            }
            else
            {
                return RedirectToAction("../Home");
            }
        }


        // POST: /manage_user/change_password/5
        [HttpPost]
        public ActionResult change_password(Change_User_Password user)
        {
            if (Session["login_status"] != null)
            {   
                var password = TempData["password"].ToString();
                var user_id = Convert.ToInt32(TempData["user_id"]);
                var old_password_encrypt = this.Encrypt(user.old_password);
                User user_new = db.Users.Find(user_id);
                if (ModelState.IsValid)
                {
                    if (old_password_encrypt == password)
                    {
                        if (user.new_password == user.confirm_password)
                        {
                            var newpswd = this.Encrypt(user.new_password);
                            user_new.password = newpswd;
                            db.Entry(user_new).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["errorMessage"] = "Password Changed Successfully";
                            return Redirect(Request.UrlReferrer.ToString());
                        }
                        else
                        {
                            TempData["errorMessage"] = "Password Mismatch";
                        }
                    }
                    else
                    {
                        TempData["errorMessage"] = "Incorrect Password";
                    }
                }
                return Redirect(Request.UrlReferrer.ToString());
            }
            else
            {
                return RedirectToAction("../Home");
            }
        }

        //Used for encrypting password
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

        //used for decryptying password
        public string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        //It is used for generating random password
        private string GetRandomNumber()
        {
            var chars = "0123456789";
            var strPwdchar = "abcdefghijklmnopqrstuvwxyz";
            var random = new Random();
            var result = new string(Enumerable.Repeat(chars, 3)
                                        .Select(s => s[random.Next(s.Length)])
                                        .ToArray());
            //return result;
            var strPwd = "";
            Random rnd = new Random();
            for (int i = 0; i <= 2; i++)
            {
                int iRandom = rnd.Next(0, strPwdchar.Length - 1);
                strPwd += strPwdchar.Substring(iRandom, 1);
            }
            result += strPwd;
            return result;
        }

        /*This function is for editing session*/
        private void edit_session(Employee employee)
        {
            Session["user_name"] = employee.first_name + ' ' + employee.middle_name + ' ' + employee.last_name;
            Session["profile_pic"] = employee.profile_pic;
        }
 
        //
        // GET: /manage_user/DbSearch

        public ActionResult DbSearch()
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(3))
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
        // POST: /manage_user/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Employee employee)
        {
            if (Session["login_status"] != null)
            {
                int[] z = (int[])Session["function_id"];
                if (z.Contains(3))
                {
                    int id = Convert.ToInt32(Session["id"]);
                    if (employee.first_name != null && employee.middle_name != null && employee.last_name != null)
                    {
                        var result = from e in db.Employees
                                     join d in db.Departments on e.department_id equals d.department_id
                                     join r in db.Roles on e.role_id equals r.role_id
                                     where e.first_name.StartsWith(employee.first_name) && e.middle_name.StartsWith(employee.middle_name) && e.last_name.StartsWith(employee.last_name) && e.employee_id != id
                                     select new EmployeeModel { employee_id = e.employee_id, first_name = e.first_name, middle_name = e.middle_name, last_name = e.last_name, email_id = e.email_id, department_name = d.department_name, role_name = r.role_name, status_id = e.status_id };
                        return View("Index", result.ToList());
                    }
                    else if (employee.first_name != null && employee.middle_name != null && employee.last_name == null)
                    {
                        var result = from e in db.Employees
                                     join d in db.Departments on e.department_id equals d.department_id
                                     join r in db.Roles on e.role_id equals r.role_id
                                     where e.first_name.StartsWith(employee.first_name) && e.middle_name.StartsWith(employee.middle_name) && e.employee_id != id
                                     select new EmployeeModel { employee_id = e.employee_id, first_name = e.first_name, middle_name = e.middle_name, last_name = e.last_name, email_id = e.email_id, department_name = d.department_name, role_name = r.role_name, status_id = e.status_id };
                        return View("Index", result.ToList());
                    }
                    else if (employee.first_name != null && employee.middle_name == null && employee.last_name != null)
                    {
                        var result = from e in db.Employees
                                     join d in db.Departments on e.department_id equals d.department_id
                                     join r in db.Roles on e.role_id equals r.role_id
                                     where e.first_name.StartsWith(employee.first_name) && e.last_name.StartsWith(employee.last_name) && e.employee_id != id
                                     select new EmployeeModel { employee_id = e.employee_id, first_name = e.first_name, middle_name = e.middle_name, last_name = e.last_name, email_id = e.email_id, department_name = d.department_name, role_name = r.role_name, status_id = e.status_id };
                        return View("Index", result.ToList());
                    }
                    else if (employee.first_name != null && employee.middle_name == null && employee.last_name == null)
                    {
                        var result = from e in db.Employees
                                     join d in db.Departments on e.department_id equals d.department_id
                                     join r in db.Roles on e.role_id equals r.role_id
                                     where e.first_name.StartsWith(employee.first_name) && e.employee_id != id
                                     select new EmployeeModel { employee_id = e.employee_id, first_name = e.first_name, middle_name = e.middle_name, last_name = e.last_name, email_id = e.email_id, department_name = d.department_name, role_name = r.role_name, status_id = e.status_id };
                        return View("Index", result.ToList());
                    }
                    else if (employee.first_name == null && employee.middle_name != null && employee.last_name != null)
                    {
                        var result = from e in db.Employees
                                     join d in db.Departments on e.department_id equals d.department_id
                                     join r in db.Roles on e.role_id equals r.role_id
                                     where e.middle_name.StartsWith(employee.middle_name) && e.last_name.StartsWith(employee.last_name) && e.employee_id != id
                                     select new EmployeeModel { employee_id = e.employee_id, first_name = e.first_name, middle_name = e.middle_name, last_name = e.last_name, email_id = e.email_id, department_name = d.department_name, role_name = r.role_name, status_id = e.status_id };
                        return View("Index", result.ToList());
                    }
                    else if (employee.first_name == null && employee.middle_name != null && employee.last_name == null)
                    {
                        var result = from e in db.Employees
                                     join d in db.Departments on e.department_id equals d.department_id
                                     join r in db.Roles on e.role_id equals r.role_id
                                     where e.middle_name.StartsWith(employee.middle_name) && e.employee_id != id
                                     select new EmployeeModel { employee_id = e.employee_id, first_name = e.first_name, middle_name = e.middle_name, last_name = e.last_name, email_id = e.email_id, department_name = d.department_name, role_name = r.role_name, status_id = e.status_id };
                        return View("Index", result.ToList());
                    }
                    else if (employee.first_name == null && employee.middle_name == null && employee.last_name != null)
                    {
                        var result = from e in db.Employees
                                     join d in db.Departments on e.department_id equals d.department_id
                                     join r in db.Roles on e.role_id equals r.role_id
                                     where e.last_name.StartsWith(employee.last_name) && e.employee_id != id
                                     select new EmployeeModel { employee_id = e.employee_id, first_name = e.first_name, middle_name = e.middle_name, last_name = e.last_name, email_id = e.email_id, department_name = d.department_name, role_name = r.role_name, status_id = e.status_id };
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