using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Puntland_Port_Taxation.Models
{
    public class Tax_CalculationModel
    {
        public int tax_calculation_id { get; set; }
        public string way_bill_name { get; set; }
        public string way_bill_code { get; set; }
        public int way_bill_id { get; set; }
        public string import_code { get; set; }
        public string ship_arrival_code { get; set; }
        public string goods { get; set; }
        public int quantity { get; set; }
        public string unit_of_measure { get; set; }
        public decimal price { get; set; }
        public decimal import_duty { get; set; }
        public decimal constant_levi { get; set; }
        public decimal temporary_levi { get; set; }
        public decimal specific_levi { get; set; }
        public decimal total_levi { get; set; }
        public int importing_status_id { get; set; }
        public string importing_status { get; set; }
        public string assigned_to { get; set; }
    }
}