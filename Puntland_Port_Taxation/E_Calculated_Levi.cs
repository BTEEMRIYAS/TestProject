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
    
    public partial class E_Calculated_Levi
    {
        public int calculated_levi_id { get; set; }
        public int way_bill_id { get; set; }
        public string goods_name { get; set; }
        public int Goods_Id { get; set; }
        public string levi_type_name { get; set; }
        public Nullable<int> Levi_Entry_Id { get; set; }
        public Nullable<int> LEVI_TYPE_ID { get; set; }
        public string levi_name { get; set; }
        public decimal unit_price { get; set; }
        public Nullable<decimal> levi { get; set; }
        public Nullable<bool> ispercentage { get; set; }
        public decimal Tariff { get; set; }
        public int quantity { get; set; }
        public decimal actual_levi { get; set; }
        public decimal total_levi { get; set; }
        public Nullable<int> Display_Order { get; set; }
        public bool is_damaged { get; set; }
    }
}
