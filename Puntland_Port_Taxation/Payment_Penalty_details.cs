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
    
    public partial class Payment_Penalty_details
    {
        public int Payment_Penalty_details_Id { get; set; }
        public int import_id { get; set; }
        public string amount_tobe_paid { get; set; }
        public int currency_id_tobe_paid { get; set; }
        public decimal sos_part_total { get; set; }
        public decimal sos_by_cash { get; set; }
        public decimal sos_by_cheque { get; set; }
        public decimal usd_part_total { get; set; }
        public decimal usd_by_cash { get; set; }
        public decimal usd_by_cheque { get; set; }
        public System.DateTime paid_date { get; set; }
    }
}
