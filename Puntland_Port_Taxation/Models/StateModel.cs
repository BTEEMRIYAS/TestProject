using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Puntland_Port_Taxation.Models
{
    public class StateModel
    {
        public int state_id { get; set; }
        public string country_name { get; set; }
        public string state_name { get; set; }
        public string geography_name { get; set; }
        public int status_id { get; set; }
    }
}