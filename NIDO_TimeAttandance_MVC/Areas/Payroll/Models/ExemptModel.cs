using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.Office.Interop.Excel;
using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class ExemptModel
    {
        public int ExemptTypeId { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public long empId { get; set; }
        public int monthNo { get; set; }
        public int yearNo { get; set; }

        public DateTime ExemptDate { get; set; }
        public string EmpName { get; set; }
        public string oldExemptValue { get; set; }
        public string newExemptValue { get; set; }

        public async Task<IList<ExemptModel>> GetExemptData()
        {
            var queryString = DataHelper.QuerystringData;

            try
            {
                var emplist = queryString[0].Split(',').Select(long.Parse).ToList();
                var exempttypeId = int.Parse(queryString[1]);
                var dateStr = queryString[2];


                var date = Convert.ToDateTime(dateStr);
                //DateTime dtTillDate = Convert.ToDateTime(toDate);
                //DateTime dtFromDate = Convert.ToDateTime(fromDate);
                //int TotalDays = ((dtTillDate - dtFromDate).Days + 1);
                PakOman_NedoEntities _db = new PakOman_NedoEntities();

                double result = 0;
                var exemptTypeList = await _db.tbl_ExemptTypes.Where(c => c.IsActive.Value).
                    Select(a => new
                    {
                        Id = a.Id,
                        Name = a.ExemptType.ToLower()
                    }).ToListAsync();



                IList<ExemptModel> exemptList = new List<ExemptModel>();
                foreach (var item in emplist)
                {
                    var exemptModel = exemptTypeList.Where(c => c.Id == exempttypeId).FirstOrDefault();

                    if (exemptModel.Name.Contains("late"))
                    {
                        //var model = await _db.AttendancePostings
                        //    .Where(q => q.AttDate.Value.Month == date.Month
                        //    && q.AttDate.Value.Year == date.Year
                        //    && q.EmpId == item)
                        //    .Select(c => c.NoOfLates).ToListAsync();

                        var model = (from p in _db.AttendancePostings
                                   from a in _db.tbl_LateAttendnaceMaster
                                   where p.TimeIn >= a.Start_Time && p.TimeIn <= a.End_Time && a.Deduct_Percent == 0 
                                   && (p.EmpId == item && p.AttDate.Value.Month == date.Month && p.AttDate.Value.Year == date.Year) 
                                   

                                   select new
                                   {
                                       p.NoOfLates
                                   }).ToList();
                        var latesCnt = _db.AttendancePostings.Where(c => c.EmpId == item && ((c.TimeIn != null && c.TimeOut == null) || (c.TimeIn == null && c.TimeOut != null))).Sum(c=>c.NoOfLates) ?? 0;





                        result = model.Sum(c => c.NoOfLates.Value) + latesCnt;
                    }
                    else if (exemptModel.Name.Contains("halfday"))
                    {
                        var model = await _db.AttendancePostings
                            .Where(q => q.AttDate.Value.Month == date.Month
                            && q.AttDate.Value.Year == date.Year
                            && q.EmpId == item)
                            .Select(c => c.NoOfHalfDay).ToListAsync();

                        result = model.Sum(c => c.Value);
                    }

                    else if (exemptModel.Name.Contains("absent"))
                    {
                        var model = _db.AttendancePostings
                            .Where(q => q.AttDate.Value.Month == date.Month
                            && q.AttDate.Value.Year == date.Year
                            && q.EmpId == item && q.HolidayType == "H").Count();
                        result = model;
                    }
                    else if (exemptModel.Name.Contains("loan"))
                    {
                        var model = await _db.tbl_LoanDetail
                            .Where(q => q.tbl_LoanMaster.EmpId == item
                            && q.LoanStatusId == 1
                            && q.LoanInstallmentDate.Value.Month == date.Month
                            && q.LoanInstallmentDate.Value.Year == date.Year)
                            .Select(c => c.InstallmentAmount).FirstOrDefaultAsync();
                        result = model ?? 0;
                    }

                    else if (exemptModel.Name.Contains("advance"))
                    {
                        var model = await _db.tbl_AdvanceDetail
                            .Where(q => q.tbl_AdvanceMaster.EmpId == item
                            && q.PaymentStatusId == 1
                            && q.AdvanceInstallmentDate.Value.Month == date.Month
                            && q.AdvanceInstallmentDate.Value.Year == date.Year)
                            .Select(c => c.InstallmentAmount).FirstOrDefaultAsync();
                        result = model ?? 0;
                    }
                    else if (exemptModel.Name.Contains("securitydeposit"))
                    {
                        var empDetails = await _db.EmpMasters.Where(c => c.EmpId == item).FirstOrDefaultAsync();

                        var dailyWages = await _db.AttendancePostings.Where(c => c.EmpId == item
                                 && c.AttDate.Value.Month == date.Month
                                 && c.AttDate.Value.Year == date.Year
                                 && (c.HolidayType == "Missing" || c.HolidayType == "P")).ToListAsync();

                        double? basicSalary = 0;
                        if(empDetails.EmployeeTypeID == 6)
                        {
                           basicSalary = empDetails.BasicSallary;
                        }
                        else if (empDetails.EmployeeTypeID == 8)
                        {
                            basicSalary = empDetails.DailyWage * dailyWages.Count();
                        }

                        var value = await _db.tbl_AllocatedSecurityDeposit
                            .Where(q => q.EmpId == item && q.tbl_SecurityDepositMaster.ValueTypeId == 1)
                            .Select(c => c.tbl_SecurityDepositMaster.DepositValue ).FirstOrDefaultAsync();

                        if (value.HasValue)
                        {
                            value = (basicSalary * (value / 100));
                        }
                        else
                        {
                            value = await _db.tbl_AllocatedSecurityDeposit
                            .Where(q => q.EmpId == item && q.tbl_SecurityDepositMaster.ValueTypeId == 2)
                            .Select(c => c.tbl_SecurityDepositMaster.DepositValue).FirstOrDefaultAsync();
                        }

                  
                          

                        result = value ?? 0;
                    }

                    var data = new ExemptModel
                    {
                        ExemptTypeId = exempttypeId,
                        monthNo = date.Month,
                        ExemptDate = date,
                        oldExemptValue = result.ToString(),
                        empId = item,
                        yearNo = date.Year,
                        EmpName = await _db.EmpMasters.Where(c => c.EmpId == item).Select(c => c.EmpName).FirstOrDefaultAsync(),

                    };
                    exemptList.Add(data);

                }

                return exemptList;
            }
            catch (Exception ex)
            {

                throw;
            }

            // var model1 = _db.tbl_SalaryPostingMaster.Where(q => q.FromDate >= dtFromDate && q.TillDate <= dtTillDate).ToList();



        }
        public async Task<bool> PostExemptData(List<ExemptModel> data)
        {
            PakOman_NedoEntities _db = new PakOman_NedoEntities();

            try
            {
                foreach (var item in data)
                {
                    var model = new tbl_ExemptMaster();

                    model = await _db.tbl_ExemptMaster
                        .Where(c => c.EmpId == item.empId
                        && c.ExemptTypeId == item.ExemptTypeId
                        && c.Exempt_Date.Value.Month == item.ExemptDate.Month
                        && c.Exempt_Date.Value.Year == item.ExemptDate.Year)
                        .FirstOrDefaultAsync();

                    if (model == null)
                        model = new tbl_ExemptMaster();

                    model.ExemptTypeId = item.ExemptTypeId;
                    model.PreviousValue = item.oldExemptValue;
                    model.NewValue = item.newExemptValue;
                    model.EmpId = item.empId;
                    model.CreatedOn = DateTime.Now;
                    model.Exempt_Date = item.ExemptDate;
                    model.MonthNo = item.monthNo;
                    model.YearNo = item.yearNo;
                    model.EditedOn = DateTime.Now;
                    model.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                    model.EditedBy = HttpContext.Current.Session["UserName"].ToString();

                    if (model.Id == 0)
                        _db.tbl_ExemptMaster.Add(model);


                }
                await _db.SaveChangesAsync();
                return true;
            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ExemptModel", "PostExemptData");
                return false;

            }


        }
    }
    public enum ExemptTypes
    {
        Lates = 1,
        HalfDays = 2,
        Absent = 3,
        Loan = 4,
        Advance = 5,
        SecurityDeposite = 6

    }


}