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
    
    public partial class tbl_SalaryIncrement_Posting
    {
        public int Id { get; set; }
        public Nullable<long> EmpId { get; set; }
        public Nullable<double> Increment_Amount { get; set; }
        public Nullable<System.DateTime> Increment_Date { get; set; }
        public Nullable<int> DesignationId { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<double> BasicSalary { get; set; }
    
        public virtual DepartmentMaster DepartmentMaster { get; set; }
        public virtual DesignationMaster DesignationMaster { get; set; }
        public virtual EmpMaster EmpMaster { get; set; }
    }
}
