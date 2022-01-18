using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class PaySlipTaxModel
    {
        public string TaxName { get; set; }
        public double? TaxAmount { get; set; }
    }
}