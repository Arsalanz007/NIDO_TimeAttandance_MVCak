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
    
    public partial class tbl_TaxMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_TaxMaster()
        {
            this.tbl_TaxPosting = new HashSet<tbl_TaxPosting>();
        }
    
        public long TaxId { get; set; }
        public string TaxDesc { get; set; }
        public int ValueTypeId { get; set; }
        public double TaxValue { get; set; }
        public double SalaryStartSlab { get; set; }
        public double SalaryEndSlab { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string EditedBy { get; set; }
        public Nullable<System.DateTime> EditedOn { get; set; }
    
        public virtual ValueType ValueType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_TaxPosting> tbl_TaxPosting { get; set; }
    }
}
