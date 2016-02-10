using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Puntland_Port_Taxation.Models
{
    public class Reject_ReasonModel
    {
        public int reject_reason_id { get; set; }
        public int way_bill_id { get; set; }
        public string way_bill_code { get; set; }
        public string reason { get; set; }
        public string rejected_from { get; set; }
        public DateTime rejected_date { get; set; }
        public string rechecked_by { get; set; }
        public Nullable <int> reject_number { get; set; }
        public int import_status { get; set; }
    }
}