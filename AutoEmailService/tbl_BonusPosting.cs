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
    
    public partial class tbl_BonusPosting
    {
        public long BonusPostingId { get; set; }
        public Nullable<long> EmpId { get; set; }
        public Nullable<int> MonthNo { get; set; }
        public Nullable<int> YearNo { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public Nullable<int> BonusId { get; set; }
        public Nullable<double> BonusAmount { get; set; }
    
        public virtual tbl_BonusMaster tbl_BonusMaster { get; set; }
    }
}