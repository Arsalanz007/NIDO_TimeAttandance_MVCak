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
    
    public partial class tbl_EOBIMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_EOBIMaster()
        {
            this.tbl_AllocatedEOBI = new HashSet<tbl_AllocatedEOBI>();
            this.tbl_EOBIPosting = new HashSet<tbl_EOBIPosting>();
        }
    
        public int Id { get; set; }
        public string EOBIName { get; set; }
        public Nullable<int> ValueTypeId { get; set; }
        public Nullable<double> EOBIValue { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string EditedBy { get; set; }
        public Nullable<System.DateTime> EditedOn { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_AllocatedEOBI> tbl_AllocatedEOBI { get; set; }
        public virtual tbl_DecConCategory tbl_DecConCategory { get; set; }
        public virtual ValueType ValueType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_EOBIPosting> tbl_EOBIPosting { get; set; }
    }
}
