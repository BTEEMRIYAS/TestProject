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
                int id = Convert.ToInt32(Session["id"]);
                return View((db.Employees.Where(e => e.employee_id != id)).ToList());
            }
            else
            {
                return RedirectToAction("Index","Home");
            }
        }

        //
        // GET: /Manage_User/Details/5

        public ActionResult Details(int id = 0)
        {
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        //
        // GET: /Manage_User/Create

        public ActionResult Create()
        {
            ViewBag.departments = new HomeController().Departments();
            ViewBag.roles = new HomeController().Roles();
            return View();
        }

        //
        // POST: /Manage_User/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee employee, User user)
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

        //
        // GET: /Manage_User/Edit/5

        public ActionResult Edit(int id = 0)
        {
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

        //
        // POST: /Manage_User/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Employee employee)
        {
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

        //
        // GET: /Manage_User/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        //
        // POST: /Manage_User/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // GET: /manage_user/change_password/5

        public ActionResult change_password(int id = 0)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            user.password = this.Decrypt(user.password);
            return View(user);
        }


        // POST: /manage_user/change_password/5
        [HttpPost]
        public ActionResult change_password(User user)
        {
            if (ModelState.IsValid)
            {
                if (user.old_password == user.password)
                {
                    if (user.new_password == user.confirm_password)
                    {
                        var newpswd = this.Encrypt(user.new_password);
                        user.password = newpswd;
                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();
                        return Redirect(Request.UrlReferrer.ToString());
                    }
                    else
                    {
                        ViewBag.password_missmatch = "Password mismatch";
                    }
                }
                else
                {
                    ViewBag.password_error = "Incorrect password";
                }
            }
            return Redirect(Request.UrlReferrer.ToString());
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
        private string Decrypt(string cipherText)
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
            return View();
        }

        //
        // POST: /manage_user/DbSearchresult
        //In this function is for database search
        [HttpPost]
        public ActionResult DbSearchresult(Employee employee)
        {
            int id = Convert.ToInt32(Session["id"]);
            if (employee.first_name != null && employee.middle_name != null && employee.last_name != null)
            {
                var result = from e in db.Employees
                             where e.first_name.StartsWith(employee.first_name) && e.middle_name.StartsWith(employee.middle_name) && e.last_name.StartsWith(employee.last_name) && e.employee_id != id
                             select e;
                return View("Index", result.ToList());
            }
            else if (employee.first_name != null && employee.middle_name != null && employee.last_name == null)
            {
                var result = from e in db.Employees
                             where e.first_name.StartsWith(employee.first_name) && e.middle_name.StartsWith(employee.middle_name) && e.employee_id != id
                             select e;
                return View("Index", result.ToList());
            }
            else if (employee.first_name != null && employee.middle_name == null && employee.last_name != null)
            {
                var result = from e in db.Employees
                             where e.first_name.StartsWith(employee.first_name) && e.last_name.StartsWith(employee.last_name) && e.employee_id != id
                             select e;
                return View("Index", result.ToList());
            }
            else if (employee.first_name != null && employee.middle_name == null && employee.last_name == null)
            {
                var result = from e in db.Employees
                             where e.first_name.StartsWith(employee.first_name) && e.employee_id != id
                             select e;
                return View("Index", result.ToList());
            }
            else if (employee.first_name == null && employee.middle_name != null && employee.last_name != null)
            {
                var result = from e in db.Employees
                             where e.middle_name.StartsWith(employee.middle_name) && e.last_name.StartsWith(employee.last_name) && e.employee_id != id
                             select e;
                return View("Index", result.ToList());
            }
            else if (employee.first_name == null && employee.middle_name != null && employee.last_name == null)
            {
                var result = from e in db.Employees
                             where e.middle_name.StartsWith(employee.middle_name) && e.employee_id != id
                             select e;
                return View("Index", result.ToList());
            }
            else if (employee.first_name == null && employee.middle_name == null && employee.last_name != null)
            {
                var result = from e in db.Employees
                             where e.last_name.StartsWith(employee.last_name) && e.employee_id != id
                             select e;
                return View("Index", result.ToList());
            }
            return RedirectToAction("Index");
        }
    }
}