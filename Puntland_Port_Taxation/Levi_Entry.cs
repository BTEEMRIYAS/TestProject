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
    
    public partial class Levi_Entry
    {
        public int Levi_entry_id { get; set; }
        public string levi_name { get; set; }
        public string description { get; set; }
        public int levi_type_id { get; set; }
        public int goods_heirarchy_id { get; set; }
        public Nullable<int> goods_item { get; set; }
        public bool ispercentage { get; set; }
        public decimal levi { get; set; }
        public int status_id { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public bool isprocessed { get; set; }
        public int currency_id { get; set; }
        public string document_name { get; set; }
        public Nullable<int> is_on_subtotal { get; set; }
    }
}