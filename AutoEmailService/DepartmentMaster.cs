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
    
    public partial class DepartmentMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DepartmentMaster()
        {
            this.tbl_SalaryIncrement_Posting = new HashSet<tbl_SalaryIncrement_Posting>();
            this.EmpMasters = new HashSet<EmpMaster>();
            this.tbl_Manager = new HashSet<tbl_Manager>();
        }
    
        public int DepartmentId { get; set; }
        public string DepartmentDesc { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string EditedBy { get; set; }
        public Nullable<System.DateTime> EditedOn { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_SalaryIncrement_Posting> tbl_SalaryIncrement_Posting { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmpMaster> EmpMasters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Manager> tbl_Manager { get; set; }
    }
}
