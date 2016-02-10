using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Puntland_Port_Taxation.Models
{
    public class Way_BillModel
    {
        public int way_bill_id { get; set; }
        public string way_bill_code { get; set; }
        public string way_bill_name { get; set; }
        public int import_id { get; set; }
        public int importer_id { get; set; }
        public int goods_category_id { get; set; }
        public int goods_subcategory_id { get; set; }
        public int goods_type_id { get; set; }
        public int way_bill_details_id { get; set; }
        public int goods_id { get; set; }
        public string import_name { get; set; }
        public string importer_name { get; set; }
        public string goods { get; set; }
        public string quantity { get; set; }
        public string units { get; set; }
        public string unit_of_measure { get; set; }
        public int unit_of_measure_id { get; set; }
        public decimal total_quantity { get; set; }
        public decimal unit_price { get; set; }
        public Nullable<decimal> price { get; set; }
        public string ship_arrival_code { get; set; }
        public int ship_arrival_id { get; set; }
        public int currency_id { get; set; }
        public int import_status_id { get; set; }
    }
}