using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class PaySlipAllowanceModel
    {
        public string AllowanceName { get; set; }
        public double? AllowanceAmount { get; set; }
    }
}