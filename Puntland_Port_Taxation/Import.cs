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
    
    public partial class Import
    {
        public int import_id { get; set; }
        public int importer_id { get; set; }
        public int importing_status_id { get; set; }
        public int ship_arrival_id { get; set; }
        public string import_code { get; set; }
        public string bollete_dogonale_code { get; set; }
        public Nullable<int> way_bill_id { get; set; }
        public int payment_id { get; set; }
        public string created_date { get; set; }
        public string updated_date { get; set; }
    }
}
