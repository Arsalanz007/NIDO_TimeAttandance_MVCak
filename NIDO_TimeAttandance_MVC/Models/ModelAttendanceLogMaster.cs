using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;

namespace NIDO_TimeAttandance_MVC.Models
{
    public class ModelAttendanceLogMaster
    {
        #region Properties
        public long EmpId { get; set; }
        public string EmpCode { get; set; }//by moiez
        public string EmpName { get; set; }//by moiez
        public string EmpImg { get; set; }
        public decimal ID { get; set; }
        public string DepartmentDesc { get; set; }
        public string Designation { get; set; }//by moiez
        public DateTime AttDate { get; set; }
        public TimeSpan? TimeIn { get; set; }
        public TimeSpan? TimeOut { get; set; }
        public int? TotalWorking { get; set; }
        public int? LateMin { get; set; }
        public int? EarlyMin { get; set; }
        public int? OT { get; set; }
        public string Result { get; set; }
        public string MachineName { get; set; }
        public string AttDay { get; set; }
        public string Remarks { get; set; }
        public int? Total_Days { get; set; }
        public int? Weekends { get; set; }
        public int? Presents { get; set; }
        public int? late { get; set; }
        public int? Overtime { get; set; }
        public int? HalfDay { get; set; }
        public int? WorkingDays { get; set; }
        public int? Gazetted { get; set; }
        public int? Absent { get; set; }
        public int? TotalLates { get; set; }
        public int? WorkingHours { get; set; }
        public int? late_Deduction { get; set; }
        public int ReportID { get; set; }
        public DateTime checkTime { get; set; }//by moiez
        public string InOutType { get; set; }//by moiez
        public int InOutTypeID { get; set; }//by moiez

        public string PostingType { get; set; }//by moiez
        public string PostedBy { get; set; }//by moiez

        #endregion
        public async Task<IList<ModelAttendanceLogMaster>> GetAttendanceLogMaster(DateTime fromdate, DateTime todate, long[] empids)
        {
            try
            {
                todate = todate.AddDays(1).AddMilliseconds(-1);


                PakOman_NedoEntities db = new PakOman_NedoEntities();
                IList<ModelAttendanceLogMaster> data = await (from AT in db.AttendanceLogMasters
                                                              join EP in db.EmpMasters on AT.AcNo equals EP.EmpCode
                                                              where EP.ActiveInActive == false && AT.CheckTime >= fromdate && AT.CheckTime <= todate && empids.Contains(EP.EmpId)
                                                              orderby EP.EmpName, AT.CheckTime
                                                              select new ModelAttendanceLogMaster
                                                              {
                                                                 
                                                                  EmpName = EP.EmpName,
                                                                  Designation=EP.DesignationMaster.DesignationDesc,
                                                                  EmpCode = EP.EmpCode,
                                                                  ID=AT.ID,
                                                                  InOutType = AT.InOutTypeId ==1 ? "IN": "OUT",
                                                                 InOutTypeID=AT.InOutTypeId,
                                                                  checkTime = AT.CheckTime,
                                                                 PostingType=AT.IsManual==true ? "Manual Posting": "System Posting",
                                                                 PostedBy=AT.CreatedBy
                                                              }).ToListAsync();
                return data;

            }
            catch (Exception)
            {

                return null;
            }


        }
        public async Task<bool> UpdateLogMaster(string[] data)
        {
            try
            {
                string[] val;
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                foreach (var item in data)
                {


                    val = item.Split('-');
                    int? ID = int.Parse(val[0]);
                    val = val[1].Split('?');
                    int? InOutID= int.Parse(val[0]);
                    val = val[1].Split('&');
                    string date = val[0] +" "+ val[1];
                    DateTime time = Convert.ToDateTime(date);
                    var result =await (from p in db.AttendanceLogMasters  where p.ID == ID  select p).FirstOrDefaultAsync();
                    result.InOutTypeId = InOutID.Value;
                    result.CheckTime = time;
                   await db.SaveChangesAsync();                 
                    
                   
                }
                return true;

            }
            catch (Exception)
            {
                return false;
                throw;
            }


        }
    }
}