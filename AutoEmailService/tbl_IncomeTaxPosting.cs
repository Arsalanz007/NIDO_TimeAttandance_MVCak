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
    
    public partial class tbl_IncomeTaxPosting
    {
        public long IncomeTaxPostingId { get; set; }
        public Nullable<long> EmpId { get; set; }
        public Nullable<int> MonthNo { get; set; }
        public Nullable<int> YearNo { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public Nullable<int> IncomeTaxId { get; set; }
        public Nullable<double> IncomeTaxAmount { get; set; }
    
        public virtual tbl_IncomeTaxMaster tbl_IncomeTaxMaster { get; set; }
    }
}
