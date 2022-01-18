//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NIDO_TimeAttandance_MVC
{
    using System;
    using System.Collections.Generic;
    
    public partial class LeaveMapMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LeaveMapMaster()
        {
            this.LeaveMapDetails = new HashSet<LeaveMapDetail>();
        }
    
        public long ID { get; set; }
        public long EmpID { get; set; }
        public int Year { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string LeaveCodes { get; set; }
        public Nullable<bool> isCarry { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
    
        public virtual EmpMaster EmpMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LeaveMapDetail> LeaveMapDetails { get; set; }
    }
}