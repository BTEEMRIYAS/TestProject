using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace Puntland_Port_Taxation.Models
{

    public class Filter_Models
    {
        public int Report { get; set; }
        public DateTime On_Date { get; set; }
        public int Month_Id { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
        public DateTime Interval_1 { get; set; }
        public DateTime Interval_2 { get; set; }
        public int FilterType { get; set; }
        public int FilterType_hd { get; set; }
        public string Rep_Type_hd { get; set; }
    }
}
