using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dial4Jobz.Models
{
    public class ReportSummary
    {
        public int? Id { get; set; }
        public String Name { get; set; }
        public String Gender { get; set; }
        public int TotalCount { get; set; }
        public long? NameLng { get; set; }
        public DateTime? CreatedDate { get; set; }
        public String  GroupName { get; set; }

    }

    public class GroupReport
    {
        public int? Id { get; set; }
        public String Name { get; set; }
        public int? Id1 { get; set; }
        public int? Id2 { get; set; }
        public String Name1 { get; set; }
        public int TotalCount { get; set; }

    }


}