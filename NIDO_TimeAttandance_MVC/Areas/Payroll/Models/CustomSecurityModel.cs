using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class CustomSecurityModel
    {
        public int[] EmpId { get; set; }

        public int MonthNo {get;set;}
        public int YearNo { get; set; }
        public decimal Amount { get; set; }
    }
}