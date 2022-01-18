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
    
    public partial class AttendancePosting
    {
        public long ID { get; set; }
        public long EmpId { get; set; }
        public Nullable<System.DateTime> AttDate { get; set; }
        public string AttDay { get; set; }
        public Nullable<bool> IsNight { get; set; }
        public Nullable<System.TimeSpan> TimeIn { get; set; }
        public Nullable<int> LateMin { get; set; }
        public Nullable<System.TimeSpan> TimeOut { get; set; }
        public Nullable<int> EarlyMin { get; set; }
        public Nullable<bool> IsHoliday { get; set; }
        public string HolidayType { get; set; }
        public Nullable<int> OT { get; set; }
        public Nullable<int> TotalWorking { get; set; }
        public Nullable<int> NoOfLates { get; set; }
        public Nullable<bool> NoOfAbsent { get; set; }
        public Nullable<bool> IsOnLeave { get; set; }
        public Nullable<System.DateTime> AttDate2 { get; set; }
        public Nullable<System.DateTime> AttDate1 { get; set; }
        public Nullable<int> NoOfHalfDay { get; set; }
        public Nullable<System.DateTime> PostingDate { get; set; }
        public string PostingBy { get; set; }
        public string PC_Name { get; set; }
        public Nullable<long> RoasterID { get; set; }
    
        public virtual EmpMaster EmpMaster { get; set; }
    }
}
