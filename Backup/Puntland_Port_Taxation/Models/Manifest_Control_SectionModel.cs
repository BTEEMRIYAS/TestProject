using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Puntland_Port_Taxation.Models
{
    public class Manifest_Control_SectionModel
    {
        public string way_bill_code { get; set; }
        public int way_bill_id { get; set; }
        public string bolleto_code { get; set; }
        public string goods { get; set; }
        public string quantity { get; set; }
        public string units { get; set; }
        public decimal quantity_ts { get; set; }
        public decimal total_quantity { get; set; }
        public string unit_of_measure { get; set; }
        public Nullable<decimal> price { get; set; }
        public string Status { get; set; }
        public int importing_status { get; set; }
        public decimal total_payment { get; set; }
        public string importer_fname { get; set; }
        public string importer_mname { get; set; }
        public string importer_lname { get; set; }
        public string ship_arrival_code { get; set; }
        public string date_of_arrival { get; set; }
        public string import_name { get; set; }
        public string tally_sheet_code { get; set; }
        public string total_quantity_ts { get; set; }
    }
}