using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class PaySlipDeductionModel
    {
        public string DeductionName { get; set; }
        public double? DeductionAmount { get; set; }
    }
}