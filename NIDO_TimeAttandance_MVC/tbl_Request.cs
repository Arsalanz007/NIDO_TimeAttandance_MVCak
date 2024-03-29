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
    
    public partial class tbl_Request
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_Request()
        {
            this.tbl_PendingRequestMaster = new HashSet<tbl_PendingRequestMaster>();
        }
    
        public long ID { get; set; }
        public Nullable<long> EmpID { get; set; }
        public Nullable<int> LeaveID { get; set; }
        public Nullable<System.DateTime> LeaveStartDate { get; set; }
        public Nullable<System.DateTime> LeaveEndDate { get; set; }
        public string Reason { get; set; }
        public Nullable<int> InOutTypeID { get; set; }
        public Nullable<System.DateTime> AttendanceDate { get; set; }
        public Nullable<System.TimeSpan> AttendanceTime { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ReqTrackingID { get; set; }
        public bool IsApproved { get; set; }
        public string ReqStatus { get; set; }
        public string Code { get; set; }
        public bool IsRejected { get; set; }
        public string RejectedReason { get; set; }
        public bool IsCancel { get; set; }
    
        public virtual EmpMaster EmpMaster { get; set; }
        public virtual LeaveSetup LeaveSetup { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_PendingRequestMaster> tbl_PendingRequestMaster { get; set; }
    }
}
