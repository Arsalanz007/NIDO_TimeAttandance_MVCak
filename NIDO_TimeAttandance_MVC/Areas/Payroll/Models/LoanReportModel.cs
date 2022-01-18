using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class LoanReportModel
    {
        public long LoanId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string DepartmentDesc { get; set; }
        public DateTime InstallmentMonth { get; set; }
        public double InstallmentAmount { get; set; }
        public string PaymentStatusDesc { get; set; }
        public double? TotalLoanPaid { get; set; }
        public double? LoanRemaining { get; set; }
        public double TotalLoan { get; set; }
    }
}