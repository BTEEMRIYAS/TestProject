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
    
    public partial class Goods_Tariff
    {
        public int goods_tariff_id { get; set; }
        public int goods_id { get; set; }
        public decimal goods_tariff { get; set; }
        public bool ispercentage { get; set; }
        public int unit_of_measure_id { get; set; }
        public System.DateTime created_date { get; set; }
        public System.DateTime end_date { get; set; }
        public int currency_id { get; set; }
        public string dcument_name { get; set; }
    }
}
