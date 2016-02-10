using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Puntland_Port_Taxation.Models
{
    public class GoodsModel
    {
        public int goods_id { get; set; }
        public string goods_type { get; set; }
        public string goods_sub_category { get; set; }
        public string goods_category { get; set; }
        public string goods_name { get; set; }
        public string goods_code { get; set; }
        public string levi_name { get; set; }
        public int status_id { get; set; }
        public decimal goods_tariff { get; set; }
        public bool ispercentage { get; set; }
        public int unit_of_measure_id { get; set; }
        public string unit_of_measure { get; set; }
        public string currency { get; set; }
        public string tariff_document { get; set; }
    }
}