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
    
    public partial class tbl_LateAttendnaceMaster
    {
        public int Id { get; set; }
        public Nullable<System.TimeSpan> Start_Time { get; set; }
        public Nullable<System.TimeSpan> End_Time { get; set; }
        public Nullable<int> Deduct_Percent { get; set; }
        public Nullable<int> Days { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedOn { get; set; }
        public string EditedBy { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> LatePolicyId { get; set; }
    }
}
