using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Puntland_Port_Taxation.Models
{
    public class ExportModel
    {
        public int export_id { get; set; }
        public string export_code { get; set; }
        public int e_payment_id { get; set; }
        public string exporter_name { get; set; }
        public string exporter_type_name { get; set; }
        public string exporting_status_name { get; set; }
        public string ship_name { get; set; }
        public string geography_name { get; set; }
        public string e_way_bill_code { get; set; }
        public Nullable<int> e_way_bill_id { get; set; }
        public string e_bolleto_dogonale_code { get; set; }
        public string ship_departure_code { get; set; }
        public Nullable<int> e_calculated_Penalty_way_bill_id { get; set; }
    }
}