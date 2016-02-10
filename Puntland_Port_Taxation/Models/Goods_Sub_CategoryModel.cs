using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Puntland_Port_Taxation.Models
{
    public class Goods_Sub_CategoryModel
    {
        public int goods_subcategory_id { get; set; }
        public string goods_category { get; set; }
        public string goods_subcategory_name { get; set; }
        public string goods_subcategory_code { get; set; }
        public int status_id { get; set; }
    }
}