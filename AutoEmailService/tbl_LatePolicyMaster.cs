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
    
    public partial class tbl_LatePolicyMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_LatePolicyMaster()
        {
            this.tbl_AllocatedLatePolicies = new HashSet<tbl_AllocatedLatePolicies>();
        }
    
        public int LatePolicyId { get; set; }
        public string LatePolicName { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedOn { get; set; }
        public string EditedBy { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_AllocatedLatePolicies> tbl_AllocatedLatePolicies { get; set; }
    }
}
