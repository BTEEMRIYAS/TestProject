using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Puntland_Port_Taxation.Models
{
    public class Levi_EntryModel
    {
        public int Levi_entry_id { get; set; }
        public string levi_name { get; set; }
        public string description { get; set; }
        public int levi_type_id { get; set; }
        public string levi_type { get; set; }
        public int goods_heirarchy_id { get; set; }
        public Nullable<int> goods_item { get; set; }
        public bool ispercentage { get; set; }
        public bool is_on_subtotal { get; set; }
        public decimal levi { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public bool isprocessed { get; set; }
        public int status_id { get; set; }
        public Nullable<int> goods_category_id { get; set; }
        public Nullable<int> goods_subcategory_id { get; set; }
        public Nullable<int> goods_type_id { get; set; }
        public int currency_id { get; set; }
        public string currency { get; set; }
        public string upload_document { get; set; }
    }
}