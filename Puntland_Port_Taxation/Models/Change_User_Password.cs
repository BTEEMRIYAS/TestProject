using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Puntland_Port_Taxation.Models
{
    public class Change_User_Password
    {
        public string old_password { get; set; }
        public string new_password { get; set; }
        public string confirm_password { get; set; }
        public string old_user_name { get; set; }
        public string new_user_name { get; set; }
        public string confirm_user_name { get; set; }
        public string user_name { get; set; }
        public string email_id { get; set; }
    }
}