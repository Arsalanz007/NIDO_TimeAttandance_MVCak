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
    
    public partial class tbl_AllocatedProvidentFund
    {
        public int AllocatedProvidentFundId { get; set; }
        public Nullable<long> EmpId { get; set; }
        public Nullable<int> ProvidentFundId { get; set; }
    
        public virtual EmpMaster EmpMaster { get; set; }
        public virtual tbl_ProvidentFundMaster tbl_ProvidentFundMaster { get; set; }
    }
}
