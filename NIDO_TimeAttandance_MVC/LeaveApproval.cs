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
    
    public partial class LeaveApproval
    {
        public long TranNo { get; set; }
        public Nullable<System.DateTime> TranDate { get; set; }
        public Nullable<long> EmpId { get; set; }
        public Nullable<int> LeaveID { get; set; }
        public Nullable<System.DateTime> LeaveDate { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string EditedBy { get; set; }
        public Nullable<System.DateTime> EditedOn { get; set; }
        public string PCName { get; set; }
        public string IP { get; set; }
    
        public virtual EmpMaster EmpMaster { get; set; }
        public virtual LeaveSetup LeaveSetup { get; set; }
    }
}
