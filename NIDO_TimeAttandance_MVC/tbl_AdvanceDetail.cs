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
    
    public partial class tbl_AdvanceDetail
    {
        public long AdvanceDetailId { get; set; }
        public Nullable<System.DateTime> AdvanceInstallmentDate { get; set; }
        public Nullable<int> AdvanceId { get; set; }
        public Nullable<int> PaymentStatusId { get; set; }
        public Nullable<double> InstallmentAmount { get; set; }
    
        public virtual tbl_PaymentStatusMaster tbl_PaymentStatusMaster { get; set; }
        public virtual tbl_AdvanceMaster tbl_AdvanceMaster { get; set; }
    }
}
