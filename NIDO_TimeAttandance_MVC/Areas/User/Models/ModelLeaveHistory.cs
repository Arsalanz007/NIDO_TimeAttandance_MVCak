using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;

namespace NIDO_TimeAttandance_MVC.Areas.User.Models
{
    public class ModelLeaveHistory
    {
        public string name { get; set; }
        public string location { get; set; }
        public string color { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string LeaveName { get; set; }
        public double? DeductDay { get; set; }
        public string remarks { get; set; }
        public int TotalDays { get; set; }
        public int Month { get; set; }

        public async Task<IList<ModelLeaveHistory>> getData()
        {
            int id = int.Parse(HttpContext.Current.Session["UserId"].ToString());
            PakOman_NedoEntities db = new PakOman_NedoEntities();
            var data = await (from P in db.AttendancePostings
                              join LA in db.LeaveApprovals on P.EmpId equals LA.EmpId
                              join LS in db.LeaveSetups on LA.LeaveID equals LS.ID
                              where P.EmpId == id && P.IsOnLeave == true && P.AttDate == LA.LeaveDate
                              select new ModelLeaveHistory
                              {
                                  name = LS.LeaveDsc,
                                  location = LA.Remarks,
                                  startDate = LA.LeaveDate.Value,
                                  endDate = LA.LeaveDate.Value,
                                  color = LS.Color,
                              }).ToListAsync();
            return data;

        }
        public async Task<IList<ModelLeaveHistory>> getDeduction()
        {
            int id = int.Parse(HttpContext.Current.Session["UserId"].ToString());
            PakOman_NedoEntities db = new PakOman_NedoEntities();
            var data = await (from q in db.tbl_LeaveDeduction
                              where q.Empid == id && q.Year == DateTime.Now.Year
                              select new ModelLeaveHistory
                              {
                                  LeaveName= q.LeaveSetup.LeaveDsc,
                                  DeductDay=q.DeductDays,
                                  remarks=q.Remarks,
                                  TotalDays=q.TotalDays.Value,
                                  Month=q.Month
                              }).ToListAsync();
            return data;

        }

    }
}