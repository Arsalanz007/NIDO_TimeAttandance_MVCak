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
    
    public partial class tbl_AllocatedEOBI
    {
        public int AllocatedEOBIId { get; set; }
        public Nullable<long> EmpId { get; set; }
        public Nullable<int> EOBIId { get; set; }
        public Nullable<System.DateTime> EobiDate { get; set; }
    
        public virtual EmpMaster EmpMaster { get; set; }
        public virtual tbl_EOBIMaster tbl_EOBIMaster { get; set; }
    }
}
