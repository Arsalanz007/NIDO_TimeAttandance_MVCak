//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AutoEmailService
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_Payroll_Options
    {
        public int Id { get; set; }
        public Nullable<int> Pay_ScheduleId { get; set; }
        public int Salary_CaluclationId { get; set; }
        public Nullable<bool> PaySlip_Signature { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string EditedBy { get; set; }
        public Nullable<System.DateTime> EditedOn { get; set; }
        public string Curreny { get; set; }
        public Nullable<int> Month_Start { get; set; }
        public Nullable<int> Month_End { get; set; }
    
        public virtual tbl_EmpSalary_CalculationMethod tbl_EmpSalary_CalculationMethod { get; set; }
        public virtual tbl_Pay_Schedule tbl_Pay_Schedule { get; set; }
    }
}
