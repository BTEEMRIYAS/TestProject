using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Puntland_Port_Taxation.Models
{
    public class ImporterModel
    {
        public int importer_id { get; set; }
        public string importer_first_name { get; set; }
        public string importer_middle_name { get; set; }
        public string importer_last_name { get; set; }
        public string importer_email_id { get; set; }
        public string importer_mob_no { get; set; }
        public int status_id { get; set; }
        public string importer_type_name { get; set; }
        public string importer_country_name { get; set; }
        public bool multiple_way_bill { get; set; }
    }
}