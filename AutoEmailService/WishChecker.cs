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
    
    public partial class WishChecker
    {
        public int ID { get; set; }
        public Nullable<System.DateTime> ActionDate { get; set; }
        public Nullable<int> EmpID { get; set; }
        public string EmpName { get; set; }
        public string Gender { get; set; }
        public string EmpImg { get; set; }
        public Nullable<int> Age { get; set; }
        public Nullable<int> OfficeAge { get; set; }
        public Nullable<bool> IsEmailBirthday { get; set; }
        public Nullable<bool> IsNotiBirthday { get; set; }
        public Nullable<bool> IsEmailJob { get; set; }
        public Nullable<bool> isNotiJob { get; set; }
        public Nullable<bool> isBirthday { get; set; }
        public Nullable<bool> isJob { get; set; }
        public string EmailAdd { get; set; }
    }
}
