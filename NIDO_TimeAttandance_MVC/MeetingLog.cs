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
    
    public partial class MeetingLog
    {
        public int ID { get; set; }
        public Nullable<long> EmpId { get; set; }
        public Nullable<decimal> Latitude { get; set; }
        public Nullable<decimal> Longitude { get; set; }
        public Nullable<System.DateTime> CheckDateTime { get; set; }
        public Nullable<int> CheckType { get; set; }
        public string Reason { get; set; }
        public string Address { get; set; }
    
        public virtual EmpMaster EmpMaster { get; set; }
    }
}
