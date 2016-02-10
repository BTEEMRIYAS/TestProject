using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Puntland_Port_Taxation.Models
{
    public class ImportModel
    {
        public int import_id { get; set; }
        public string import_code { get; set; }
        public int payment_id { get; set; }
        public string importer_name { get; set; }
        public string importer_type_name { get; set; }
        public string importing_status_name { get; set; }
        public string ship_name { get; set; }
        public string geography_name { get; set; }
        public string way_bill_code { get; set; }
        public Nullable <int> way_bill_id { get; set; }
        public string bolleto_dogonale_code { get; set; }
        public string ship_arrival_code { get; set; }
        public Nullable<int> calculated_Penalty_way_bill_id { get; set; }
    }
}