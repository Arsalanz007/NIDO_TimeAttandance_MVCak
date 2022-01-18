using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class LeaveModel
    {
        public long EmpId { get; set; }
        public int LeaveId { get; set; }
        public string LeaveDesc { get; set; }
        public double LeaveAllowed { get; set; }
        public double ApproveLeave { get; set; }

    }
}