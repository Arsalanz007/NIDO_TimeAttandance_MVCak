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
    
    public partial class EmpEducationDetail
    {
        public long EmpId { get; set; }
        public string Degree { get; set; }
        public string Institute { get; set; }
        public string GradeOrGPA { get; set; }
        public string Year { get; set; }
        public long PKId { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string EditedBy { get; set; }
        public string EditedOn { get; set; }
    
        public virtual EmpMaster EmpMaster { get; set; }
    }
}
