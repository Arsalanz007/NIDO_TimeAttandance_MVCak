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
    
    public partial class tbl_Organization_Heirarchy
    {
        public int ID { get; set; }
        public Nullable<int> empid { get; set; }
        public string EmpName { get; set; }
        public string DepartmentDesc { get; set; }
        public string DesignationDesc { get; set; }
        public Nullable<int> levelID { get; set; }
        public Nullable<int> ParentID { get; set; }
        public string EmpImg { get; set; }
    }
}