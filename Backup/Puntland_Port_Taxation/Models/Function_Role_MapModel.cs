using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Puntland_Port_Taxation.Models
{
    public class Function_Role_MapModel
    {
        public int function_role_map_id { get; set; }
        public int role_id { get; set; }
        public int function_id { get; set; }
        public int status_id { get; set; }
        public string role_name { get; set; }
        public string function_name { get; set; }
        public string created_date { get; set; }
        public string updated_date { get; set; }
    }
}