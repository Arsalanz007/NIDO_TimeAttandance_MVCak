using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class LateExemptModel
    {
        public int id { get; set; }
        public string empCode { get; set; }
        public string empName { get; set; }
        public long empId { get; set; }
        public string month { get; set; }
        public int year { get; set; }
        public int monthNo { get; set; }
        public string deductAmount { get; set; }
        public int deductId { get; set; }
        public int count { get; set; }


    }
}