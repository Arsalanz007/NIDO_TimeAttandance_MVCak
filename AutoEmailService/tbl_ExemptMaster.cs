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
    
    public partial class tbl_ExemptMaster
    {
        public int Id { get; set; }
        public Nullable<int> ExemptTypeId { get; set; }
        public string PreviousValue { get; set; }
        public string NewValue { get; set; }
        public Nullable<long> EmpId { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedOn { get; set; }
        public string EditedBy { get; set; }
        public Nullable<int> MonthNo { get; set; }
        public Nullable<System.DateTime> Exempt_Date { get; set; }
        public Nullable<int> YearNo { get; set; }
    
        public virtual EmpMaster EmpMaster { get; set; }
    }
}
