using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class AdvanceReportModel
    {
        public long AdvanceId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string DepartmentDesc { get; set; }
        public DateTime? InstallmentMonth { get; set; }

        public DateTime? AdvanceApplyDate { get; set; }
        public double InstallmentAmount { get; set; }
        public string PaymentStatusDesc { get; set; }
        public double? TotalAdvancePaid { get; set; }
        public double? AdvanceRemaining { get; set; }
        public double TotalAdvance { get; set; }
    }
}