using DocumentFormat.OpenXml.Drawing;
using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebGrease.Css.Extensions;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class OverTimeModel
    {
        public long EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string Test { get; set; }
        public double? OTPerHourSalary { get; set; }
        public double? OTHours { get; set; }
        public string OTHoursStr { get; set; }
        public string HolidayHourStr { get; set; }
        public string WorkingHourStr { get; set; }
        public double? HolidayAmount { get; set; }
        public double? HolidayHour { get; set; }
        public double? OTAmount { get; set; }
        public double? BasicSalary{ get; set; }
        public double? WorkingDayRate { get; set; }
        public double? HolidayRate { get; set; }


        public async Task<IList<OverTimeModel>> GetOTReport(long[] employeeIds, DateTime DateFrom, DateTime DateTo)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();

                IList<OverTimeModel> model = await _db.tbl_SalaryPostingMaster.Where(c => employeeIds.Contains(c.EmpId.Value) &&
                c.MonthNo == DateFrom.Month && c.YearNo == DateTo.Year)
                    .Select(c => new OverTimeModel
                    {
                        EmpId = c.EmpId.Value,
                        EmpCode = c.EmpMaster.EmpCode,
                        EmpName = c.EmpMaster.EmpName,
                        OTPerHourSalary = c.PerHourSalary ?? 0,
                        
                        HolidayHourStr = c.HolidayOT ,
                        OTHoursStr = c.TotalOT,
                        OTAmount = c.OTAmount,
                        HolidayAmount = c.HolidayOTAmount ?? 0,
                        WorkingDayRate = c.PerDaySalary ?? 0,
                        HolidayRate = (c.PerHourSalary.Value * 2 ),
                        BasicSalary = c.BasicSalary ?? 0
                    }).ToListAsync();


                model.ForEach(c => {
                   // c.HolidayHour = String.IsNullOrEmpty(c.HolidayHourStr) ? 0 : (Convert.ToDouble(c.HolidayHourStr) / 60);
                   //  c.OTHours = String.IsNullOrEmpty(c.OTHoursStr) ? 0 : (Convert.ToDouble(c.OTHoursStr) / 60);
                  //  c.HolidayHour = c.HolidayHourStr;
                 //   c.OTHours = c.OTHoursStr;

                });


                return model;



            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "GetOTReport", "GetOTReport");
                return null;
            }

        }

    }
}