using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class PaySlipSecurityDepositModel
    {
        public string SecurityDepositName { get; set; }
        public double? SecurityDepositAmount { get; set; }
    }
}