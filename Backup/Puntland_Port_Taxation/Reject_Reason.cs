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
    
    public partial class Reject_Reason
    {
        public int reject_reason_id { get; set; }
        public int way_bill_id { get; set; }
        public string reject_reason { get; set; }
        public string reject_from { get; set; }
        public System.DateTime rejected_date { get; set; }
    }
}
