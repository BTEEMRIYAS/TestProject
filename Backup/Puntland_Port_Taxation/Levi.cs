//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Puntland_Port_Taxation
{
    using System;
    using System.Collections.Generic;
    
    public partial class Levi
    {
        public int levi_id { get; set; }
        public int levi_entry_id { get; set; }
        public string levi_name { get; set; }
        public string description { get; set; }
        public int levi_type_id { get; set; }
        public Nullable<int> goods_category_id { get; set; }
        public Nullable<int> goods_subcategory_id { get; set; }
        public Nullable<int> goods_type_id { get; set; }
        public Nullable<int> goods_id { get; set; }
        public bool ispercentage { get; set; }
        public decimal levi1 { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
    }
}