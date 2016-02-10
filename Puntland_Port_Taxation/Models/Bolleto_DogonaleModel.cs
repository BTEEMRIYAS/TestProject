using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Puntland_Port_Taxation.Models
{
    public class Bolleto_DogonaleModel
    {
        public int bolleto_dogonale_id { get; set; }
        public string bolleto_dogonale_code { get; set; }
        public string way_bill_name { get; set; }
        public string way_bill_code { get; set; }
        public int way_bill_id { get; set; }
        public string import_code { get; set; }
        public string ship_arrival_code { get; set; }
        public string goods { get; set; }
        public string importer_name { get; set; }
        public string ship { get; set; }
        public string arrival_date { get; set; }
        public int quantity { get; set; }
        public string unit_of_measure { get; set; }
        public decimal price { get; set; }
        public decimal total_tax { get; set; }
        public decimal total_payment { get; set; }
        public int currency_id { get; set; }
        public DateTime date { get; set; }
        public string way_bill_date { get; set; }
        public string mark { get; set; }
    }
}