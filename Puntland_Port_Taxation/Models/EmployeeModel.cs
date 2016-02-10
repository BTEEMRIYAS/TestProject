using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Puntland_Port_Taxation.Models
{
    public class EmployeeModel
    {
        public int employee_id { get; set; }
        public string first_name { get; set; }
        public string middle_name { get; set; }
        public string last_name { get; set; }
        public string email_id { get; set; }
        public string profile_pic { get; set; }
        public int status_id { get; set; }
        public string role_name { get; set; }
        public string department_name { get; set; }
        public string created_date { get; set; }
        public string updated_date { get; set; }
        public string employee_code { get; set; }
    }
}