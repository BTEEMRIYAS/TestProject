using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Puntland_Port_Taxation.Models
{
    public class Goods_TypeModel
    {
        public int goods_type_id { get; set; }
        public string goods_subcategory { get; set; }
        public string goods_category { get; set; }
        public string goods_type_name { get; set; }
        public string goods_type_code { get; set; }
        public int goods_subcategory_id { get; set; }
        public int goods_category_id { get; set; }
        public string goods_name { get; set; }
        public string goods_code { get; set; }
        public int levi_id { get; set; }
        public int status_id { get; set; }
        public decimal goods_tariff { get; set; }
        public bool ispercentage { get; set; }
        public int unit_of_measure_id { get; set; }
        public string unit_of_measure { get; set; }
        public string upload_document { get; set; }
        public int currency_id { get; set; }
    }
}