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
    
    public partial class tbl_PendingRequestMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_PendingRequestMaster()
        {
            this.tbl_PendingRequestDetail = new HashSet<tbl_PendingRequestDetail>();
        }
    
        public long ID { get; set; }
        public long RequestID { get; set; }
        public bool IsApproved { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public bool IsRejected { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_PendingRequestDetail> tbl_PendingRequestDetail { get; set; }
        public virtual tbl_Request tbl_Request { get; set; }
    }
}