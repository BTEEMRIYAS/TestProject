using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Puntland_Port_Taxation.Models
{
    public class Add_Goods_Price
    {
        public string goods_name { get; set; }
        public decimal []goods_price { get; set; }
        public Nullable <decimal> goods_price_value { get; set; }
        public int []currency_id { get; set; }
        public Nullable <int> currency_id_value { get; set; }
        public int []goods_id { get; set; }
        public int goods_id_value { get; set; }
        public decimal price { get; set; }
        public string quantity { get; set; }
        public int total_quantity { get; set; }
        public string unit_of_measure { get; set; }
        public decimal total_price { get; set; }
        public decimal goods_tariff { get; set; }
        public int penalty { get; set; }
        public bool []ispercentage { get; set; }
        public bool ispercentage_value { get; set; }
        public int count { get; set; }
        public bool is_damaged { get; set; }
    }
}