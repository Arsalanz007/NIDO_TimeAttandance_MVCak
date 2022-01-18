using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class LoanLedgerModel
    {
        public long EmpId { get; set; }
        public long LoanId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string DepartmentDesc { get; set; }
        public double? TotalLoanPaid { get; set; }
        public double? LoanRemaining { get; set; }
        public double TotalLoan { get; set; }
        public DateTime? LoanApplyDate { get; set; }
        public DateTime? LoanStartDate { get; set; }
        public DateTime? LoanMaturityDate { get; set; }
        public List<LoanLedgerDetailsModel> LoanDetails { get; set; }
    }
    public class LoanLedgerDetailsModel
    {
        public long Id { get; set; }
        public DateTime? InstallmentDate { get; set; }
        public double? Amount { get; set; }
        public DateTime InstallmentMonth { get; set; }
        public string PaymentStatusDesc { get; set; }

    }

}