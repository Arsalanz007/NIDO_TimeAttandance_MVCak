using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace NIDO_TimeAttandance_MVC.Areas.Reports.Models
{
    public class ModelMonthlyRepot
    {

        #region Properties
        public long EmpId { get; set; }
        public string EmpCode { get; set; }
        public string AbrovedBy { get; set; }
        public string EmpName { get; set; }
        public string EmpImg { get; set; }
        public string DepartmentDesc { get; set; }
        public string Designation { get; set; }
        public DateTime AttDate { get; set; }
        public TimeSpan? TimeIn { get; set; }
        public TimeSpan? TimeOut { get; set; }
        public int? TotalWorking { get; set; }
        public int? LateMin { get; set; }
        public int? EarlyMin { get; set; }
        public int? OT { get; set; }
        public string Reason { get; set; }
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
        public bool isOnLeave { get; set; }

        public bool NoOfAbsent { get; set; }
        public int? WorkingDays { get; set; }
        public int? Gazetted { get; set; }
        public int? Absent { get; set; }
        public int? TotalLates { get; set; }
        public int? WorkingHours { get; set; }
        public int? late_Deduction { get; set; }
        public int ReportID { get; set; }
        public DateTime checkTime { get; set; }
        public int InOutType { get; set; }
        public decimal WorkingMin { get; set; }
        public decimal ShortTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal TotalHours { get; set; }
        public int? bankId { get; set; }
        public string location{ get; set; }
        public string address { get; set; }

        #endregion

        public async Task<IList<ModelMonthlyRepot>> ManualReport(string from, string to, string empids)
        {
            try
            {
                DateTime fromDate = Convert.ToDateTime(from);
                DateTime todate = Convert.ToDateTime(to);
                todate = todate.AddDays(1).AddMilliseconds(-1);


                PakOman_NedoEntities db = new PakOman_NedoEntities();
                IList<ModelMonthlyRepot> data = await Task.Run(() => db.Nstp_Get_Manual_Attendance(empids, from, to).Select(x => new ModelMonthlyRepot
                {

                    EmpName = x.empname,
                    EmpId = x.empid,

                    AbrovedBy = x.tbl,
                    checkTime = x.checktime,

                    Reason = x.Reason,
                    EmpCode = x.EmpCode,
                    CreatedDate = x.CreatedDate


                }).ToList());



                return data;

            }
            catch (Exception)
            {

                return null;
            }



        }
        public async Task<IList<ModelMonthlyRepot>> getLateReport(string from, string to, long[] empids)
        {
            try
            {
                DateTime fromDate = Convert.ToDateTime(from);
                DateTime todate = Convert.ToDateTime(to);
                long[] Empids = empids;
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                IList<ModelMonthlyRepot> data = await (from AP in db.AttendancePostings
                                                       join EP in db.EmpMasters on AP.EmpId equals EP.EmpId
                                                       where AP.LateMin > 0 && AP.AttDate >= fromDate && AP.AttDate <= todate && AP.NoOfHalfDay != 1 && Empids.Contains(EP.EmpId)
                                                       select new ModelMonthlyRepot
                                                       {
                                                           EmpName = EP.EmpName,
                                                           EmpImg = EP.EmpImg,
                                                           AttDate = AP.AttDate.Value,
                                                           TimeIn = AP.TimeIn.Value,
                                                           EmpCode = EP.EmpCode,
                                                           LateMin = AP.LateMin,
                                                           Designation = EP.DesignationMaster.DesignationDesc,

                                                       }).ToListAsync();
                return (data);
            }
            catch (Exception)
            {

                throw;
            }



        }
        //created by moiez
        public async Task<IList<ModelMonthlyRepot>> GetAbsentReport(string from, string to, long[] empids)
        {
            try
            {
                DateTime fromDate = Convert.ToDateTime(from);
                DateTime todate = Convert.ToDateTime(to);
                long[] Empids = empids;
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                IList<ModelMonthlyRepot> data = await (from AP in db.AttendancePostings
                                                       join EP in db.EmpMasters on AP.EmpId equals EP.EmpId
                                                       where AP.HolidayType == "A" && AP.AttDate >= fromDate && AP.AttDate <= todate && Empids.Contains(EP.EmpId)
                                                       select new ModelMonthlyRepot
                                                       {
                                                           EmpName = EP.EmpName,
                                                           EmpImg = EP.EmpImg,
                                                           AttDate = AP.AttDate.Value,
                                                           // TimeIn = AP.TimeIn.Value,
                                                           EmpCode = EP.EmpCode,
                                                           Designation = EP.DesignationMaster.DesignationDesc,
                                                       }).ToListAsync();
                return (data);
            }
            catch (Exception)
            {

                return null;
            }

        }

        //created by moiez
        public async Task<IList<ModelMonthlyRepot>> getHalfReport(string from, string to, long[] empids)
        {
            try
            {
                DateTime fromDate = Convert.ToDateTime(from);
                DateTime todate = Convert.ToDateTime(to);
                long[] Empids = empids;
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                IList<ModelMonthlyRepot> data = await (from AP in db.AttendancePostings
                                                       join EP in db.EmpMasters on AP.EmpId equals EP.EmpId
                                                       where AP.LateMin > 0 && AP.AttDate >= fromDate && AP.AttDate <= todate && AP.NoOfHalfDay == 1 && Empids.Contains(EP.EmpId)
                                                       select new ModelMonthlyRepot
                                                       {
                                                           EmpName = EP.EmpName,
                                                           EmpImg = EP.EmpImg,
                                                           AttDate = AP.AttDate.Value,
                                                           TimeIn = AP.TimeIn.Value,
                                                           EmpCode = EP.EmpCode,
                                                           Designation = EP.DesignationMaster.DesignationDesc,

                                                       }).ToListAsync();
                return (data);
            }
            catch (Exception)
            {

                throw;
            }



        }


        //created by moiez
        public async Task<IList<ModelMonthlyRepot>> GetEarlyMinReport(string from, string to, long[] empids)
        {
            try
            {
                DateTime fromDate = Convert.ToDateTime(from);
                DateTime todate = Convert.ToDateTime(to);
                long[] Empids = empids;
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                IList<ModelMonthlyRepot> data = await (from AP in db.AttendancePostings
                                                       join EP in db.EmpMasters on AP.EmpId equals EP.EmpId
                                                       where AP.EarlyMin > 0 && AP.AttDate >= fromDate && AP.AttDate <= todate && Empids.Contains(EP.EmpId)
                                                       select new ModelMonthlyRepot
                                                       {
                                                           EmpName = EP.EmpName,
                                                           EmpImg = EP.EmpImg,
                                                           AttDate = AP.AttDate.Value,
                                                           TimeIn = AP.TimeIn.Value,
                                                           TimeOut = AP.TimeOut.Value,
                                                           EmpCode = EP.EmpCode,
                                                           EarlyMin = AP.EarlyMin,
                                                           Designation = EP.DesignationMaster.DesignationDesc,

                                                       }).ToListAsync();
                return (data);
            }
            catch (Exception)
            {

                return null;
            }



        }
        //created by moiez
        public async Task<IList<ModelMonthlyRepot>> GetTimeOutReport(string from, string to, long[] empids)
        {
            try
            {
                DateTime fromDate = Convert.ToDateTime(from);
                DateTime todate = Convert.ToDateTime(to);
                long[] Empids = empids;
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                IList<ModelMonthlyRepot> data = await (from AP in db.AttendancePostings
                                                       join EP in db.EmpMasters on AP.EmpId equals EP.EmpId
                                                       where AP.TimeOut == null && AP.HolidayType.Contains("Missing")
                                                       && AP.AttDate >= fromDate && AP.AttDate <= todate && Empids.Contains(EP.EmpId)

                                                       select new ModelMonthlyRepot
                                                       {
                                                           EmpName = EP.EmpName,
                                                           EmpImg = EP.EmpImg,
                                                           AttDate = AP.AttDate.Value,
                                                           TimeIn = AP.TimeIn.Value,
                                                           AttDay = AP.AttDay,
                                                           EmpCode = EP.EmpCode,
                                                           Designation = EP.DesignationMaster.DesignationDesc,

                                                       }).ToListAsync();
                return (data);
            }
            catch (Exception)
            {

                return null;
            }



        }
        public async Task<IList<ModelMonthlyRepot>> GetTransactionReport(DateTime fromdate, DateTime todate, long[] empids)
        {
            try
            {
                todate = todate.AddDays(1).AddMilliseconds(-1);


                PakOman_NedoEntities db = new PakOman_NedoEntities();
                IList<ModelMonthlyRepot> data = await (from AT in db.AttendanceLogMasters
                                                       join EP in db.EmpMasters on AT.AcNo equals EP.EmpCode
                                                       where EP.ActiveInActive == false && AT.CheckTime >= fromdate && AT.CheckTime <= todate && empids.Contains(EP.EmpId)
                                                       orderby EP.EmpName, AT.CheckTime
                                                       select new ModelMonthlyRepot
                                                       {
                                                           EmpId = EP.EmpId,
                                                           EmpName = EP.EmpName,
                                                           EmpCode = EP.EmpCode,
                                                           Designation = EP.DesignationMaster.DesignationDesc,
                                                           MachineName = "machine_name",
                                                           checkTime = AT.CheckTime,
                                                       }).ToListAsync();
                return data;

            }
            catch (Exception)
            {

                return null;
            }


        }

        public async Task<IList<ModelMonthlyRepot>> GetTransactionWithLocationReport(DateTime fromdate, DateTime todate, long[] empids)
        {
            try
            {
                todate = todate.AddDays(1).AddMilliseconds(-1);


                PakOman_NedoEntities db = new PakOman_NedoEntities();
                IList<ModelMonthlyRepot> data = await (from AT in db.AttendanceLogMasters
                                                       join EP in db.EmpMasters on AT.AcNo equals EP.EmpId.ToString()
                                                       where EP.ActiveInActive == false && AT.CheckTime >= fromdate && AT.CheckTime <= todate && empids.Contains(EP.EmpId) && AT.isApp.Value
                                                       orderby EP.EmpName, AT.CheckTime
                                                       select new ModelMonthlyRepot
                                                       {
                                                           EmpId = EP.EmpId,
                                                           EmpName = EP.EmpName,
                                                           EmpCode = EP.EmpCode,
                                                           Designation = EP.DesignationMaster.DesignationDesc,
                                                           MachineName = "machine_name",
                                                           checkTime = AT.CheckTime,
                                                           location = AT.Location,
                                                           address = AT.Address,
                                                           AttDate = AT.CheckTime
                                                       }).ToListAsync();
                return data;

            }
            catch (Exception)
            {

                return null;
            }


        }
        public async Task<IList<ModelMonthlyRepot>> GetTransactionReport(DateTime fromdate, DateTime todate, int id)
        {
            try
            {
                todate = todate.AddDays(1).AddMilliseconds(-1);
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                IList<ModelMonthlyRepot> data = await (from AT in db.AttendanceLogMasters
                                                       join EP in db.EmpMasters on AT.AcNo equals EP.EmpCode
                                                       where EP.ActiveInActive == false && AT.CheckTime >= fromdate && AT.CheckTime <= todate && EP.EmpId == id
                                                       orderby EP.EmpName, AT.CheckTime
                                                       select new ModelMonthlyRepot
                                                       {
                                                           EmpId = EP.EmpId,
                                                           EmpName = EP.EmpName,
                                                           EmpCode = EP.EmpCode,
                                                           Designation = EP.DesignationMaster.DesignationDesc,
                                                           MachineName = "machine_name",
                                                           checkTime = AT.CheckTime,
                                                           InOutType = AT.InOutTypeId,

                                                       }).ToListAsync();
                return data;

            }
            catch (Exception)
            {

                return null;
            }


        }
        public async Task<IList<ModelMonthlyRepot>> GetWorkingHours(string EmpIDS, DateTime fromdate, DateTime todate)
        {
            try
            {
                todate = todate.AddDays(1).AddMilliseconds(-1);


                PakOman_NedoEntities db = new PakOman_NedoEntities();
                IList<ModelMonthlyRepot> data = await Task.Run(() => db.Nstp_Working("," + EmpIDS + ",", Convert.ToDateTime(fromdate), Convert.ToDateTime(todate)).Select(x => new ModelMonthlyRepot
                {
                    EmpCode = x.EmpCode,
                    EmpName = x.EmpName,
                    AttDate = x.AttDate.GetValueOrDefault(),
                    TimeIn = x.TimeIn.GetValueOrDefault(),
                    TimeOut = x.TimeOut.GetValueOrDefault(),
                    WorkingMin = x.WorkingMin.GetValueOrDefault(),
                    ShortTime = x.ShortTime.GetValueOrDefault(),
                    TotalHours = x.TotalHours


                }).ToList());



                return data;

            }
            catch (Exception)
            {

                return null;
            }


        }
        public async Task<IList<ModelMonthlyRepot>> OverTime(string EmpIDS, DateTime fromdate, DateTime todate)
        {
            try
            {
                todate = todate.AddDays(1).AddMilliseconds(-1);



                string[] empIDs = EmpIDS.Split(',');
                var empIDS = Array.ConvertAll(empIDs, s => long.Parse(s));

                PakOman_NedoEntities db = new PakOman_NedoEntities();
                IList<ModelMonthlyRepot> data = await (from AP in db.AttendancePostings
                                                       join EP in db.EmpMasters on AP.EmpId equals EP.EmpId
                                                       where AP.OT > 0
                                                       && AP.AttDate >= fromdate && AP.AttDate <= todate && empIDS.Contains(EP.EmpId)

                                                       select new ModelMonthlyRepot
                                                       {
                                                           EmpName = EP.EmpName,
                                                           EmpImg = EP.EmpImg,
                                                           AttDate = AP.AttDate.Value,
                                                           TimeIn = AP.TimeIn.Value,
                                                           TimeOut = AP.TimeOut,
                                                           AttDay = AP.AttDay,
                                                           EmpCode = EP.EmpCode,
                                                           OT = AP.OT,
                                                           Designation = EP.DesignationMaster.DesignationDesc


                                                       }).ToListAsync();









                //PakOman_NedoEntities db = new PakOman_NedoEntities();


                //IList<ModelMonthlyRepot> data = await Task.Run(() => db.Nstp_Working(EmpIDS, Convert.ToDateTime(fromdate), Convert.ToDateTime(todate)).Select(x => new ModelMonthlyRepot
                //{
                //    EmpCode = x.EmpCode,
                //    EmpName = x.EmpName,
                //    AttDate = x.AttDate.GetValueOrDefault(),
                //    TimeIn = x.TimeIn.GetValueOrDefault(),
                //    TimeOut = x.TimeOut.GetValueOrDefault(),
                //    WorkingMin = x.WorkingMin.GetValueOrDefault(),
                //    ShortTime = x.ShortTime.GetValueOrDefault(),
                //    TotalHours = x.TotalHours


                //}).ToList());



                return data;

            }
            catch (Exception)
            {

                return null;
            }


        }
        public async Task<IList<ModelMonthlyRepot>> DailyAttendance(string datetime, long[] empids)
        {
            try
            {
                DateTime date = Convert.ToDateTime(datetime);

                long[] Empids = empids;
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                IList<ModelMonthlyRepot> data = await (from AP in db.AttendancePostings
                                                       join EP in db.EmpMasters on AP.EmpId equals EP.EmpId
                                                       where AP.AttDate == date && Empids.Contains(EP.EmpId)
                                                       select new ModelMonthlyRepot
                                                       {
                                                           EmpCode = EP.EmpCode,
                                                           EmpId=EP.EmpId,
                                                           EmpName = EP.EmpName,
                                                           Designation = EP.DesignationMaster.DesignationDesc,
                                                           DepartmentDesc=EP.DepartmentMaster.DepartmentDesc,                                        
                                                           TimeIn = AP.TimeIn.Value,
                                                           LateMin=AP.LateMin,
                                                           TimeOut = AP.TimeOut.Value,
                                                           EarlyMin = AP.EarlyMin,
                                                           Overtime=AP.OT,
                                                           HalfDay=AP.NoOfHalfDay,
                                                         isOnLeave=AP.IsOnLeave.Value,
                                                         NoOfAbsent=AP.NoOfAbsent.Value

                                                       }).ToListAsync();
                return (data);
            }
            catch (Exception)
            {

                return null;
            }




        }

    }
}