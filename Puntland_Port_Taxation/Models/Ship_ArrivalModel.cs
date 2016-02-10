using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Puntland_Port_Taxation.Models
{
    public class Ship_ArrivalModel
    {
        public int ship_arrival_id { get; set; }
        public string shipp_name { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string day_code { get; set; }
        public string ship_arrival_code { get; set; }
        public int status_id { get; set; }
        public int shipp_id { get; set; }
        public int day_id { get; set; }
        public string starting_date { get; set; }
        public int geography_id { get; set; }
        public int country_id { get; set; }
        public int state_id { get; set; }
    }
}