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
    
    public partial class tbl_AdvanceMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_AdvanceMaster()
        {
            this.tbl_AdvanceDetail = new HashSet<tbl_AdvanceDetail>();
        }
    
        public int AdvanceId { get; set; }
        public Nullable<long> EmpId { get; set; }
        public Nullable<System.DateTime> AdvanceStartDate { get; set; }
        public Nullable<System.DateTime> AdvanceMaturityDate { get; set; }
        public Nullable<double> AdvanceAmount { get; set; }
        public bool IsDeleted { get; set; }
        public string AdvanceDesc { get; set; }
        public Nullable<System.DateTime> ApplyDate { get; set; }
    
        public virtual EmpMaster EmpMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_AdvanceDetail> tbl_AdvanceDetail { get; set; }
    }
}
