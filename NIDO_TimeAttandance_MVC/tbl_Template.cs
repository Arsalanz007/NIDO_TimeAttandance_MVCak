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
    
    public partial class tbl_Template
    {
        public int ID { get; set; }
        public int TemplateName { get; set; }
        public string Discription { get; set; }
        public bool IsActive { get; set; }
    
        public virtual tbl_TemplateName tbl_TemplateName { get; set; }
    }
}