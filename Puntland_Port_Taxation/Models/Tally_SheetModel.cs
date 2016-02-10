using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Puntland_Port_Taxation.Models
{
    public class Tally_SheetModel
    {
        public int tally_sheet_id { get; set; }
        public int employee_id { get; set; }
        public int ship_arrival_id { get; set; }
        public int goods_id { get; set; }
        public int currency_id { get; set; }
        public string employee_name { get; set; }
        public string tally_sheet_code { get; set; }
        public string ship_arrival_code { get; set; }
        public int tally_sheet_details_id { get; set; }
        public string way_bill_code { get; set; }
        public string goods { get; set; }
        public int quantity { get; set; }
        public decimal units { get; set; }
        public string unit_of_measure { get; set; }
        public int unit_of_measure_id { get; set; }
        public int total_quantity { get; set; }
        public decimal price { get; set; }
        public int goods_category_id { get; set; }
        public int goods_subcategory_id { get; set; }
        public int goods_type_id { get; set; }
        public int total_quantity_ts { get; set; }
        public string importer_name { get; set; }
        public bool is_damaged { get; set; }
    }
}