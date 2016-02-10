using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Puntland_Port_Taxation.Models
{

    public class Payment_detailsModel
    {
        public int payment_detail_id { get; set; }
        public int import_id { get; set; }
        public decimal amount_tobe_paid { get; set; }
        public int currency_id_tobe_paid { get; set; }
        public decimal amount_paid { get; set; }
        public int currency_id_paid { get; set; }
        public string paid_date { get; set; }
        public int payment_mode_id { get; set; }
        public string day_code { get; set; }
        public int day_id { get; set; }
        public int way_bill_id { get; set; }
      
    }
}
