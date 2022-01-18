using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class EmployeeHistoryModel
    {
        public long empId { get; set; }
        public string empCode { get; set; }
        public string empName { get; set; }
        public string department { get; set; }
        public string designation { get; set; }
        public string joiningDate { get; set; }
        public DateTime? trailStartDate { get; set; }
        public DateTime? trailEndDate { get; set; }
        public DateTime? appointedStartDate { get; set; }
        public DateTime? appointedEndDate { get; set; }
        public DateTime? confirmDate { get; set; }

        public async Task<IList<EmployeeHistoryModel>> GetEmployeeHistoryReport(long[] employeeIds)
        {
            try
            {
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                IList<EmployeeHistoryModel> data = await (from emp in db.EmpMasters
                                                       where emp.ActiveInActive == false && employeeIds.Contains(emp.EmpId)
                                                       select new EmployeeHistoryModel
                                                       {
                                                           empId = emp.EmpId,
                                                           empCode = emp.EmpCode,
                                                           empName = emp.EmpName,
                                                           department = emp.DepartmentMaster.DepartmentDesc,
                                                           designation = emp.DesignationMaster.DesignationDesc,
                                                           joiningDate = emp.AppointmentDate,
                                                           appointedStartDate = emp.AppointedStartDate,
                                                           appointedEndDate = emp.AppointedEndDate,
                                                           trailStartDate = emp.TrailStartDate,
                                                           trailEndDate = emp.TrailEndDate,
                                                           confirmDate = emp.ConfirmDate
                                                       }).ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "EmployeeHistoryModal", " GetEmployeeHistoryReport");

                throw;
            }
        }
    }
}