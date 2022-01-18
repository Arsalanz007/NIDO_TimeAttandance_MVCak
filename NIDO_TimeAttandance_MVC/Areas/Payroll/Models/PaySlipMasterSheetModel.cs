using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class PaySlipMasterSheetModel
    {
        public IList<PaySlipAllowanceModel> Allowances { get; set; }
        public IList<PaySlipDeductionModel> Deductions { get; set; }
        public IList<PaySlipTaxModel> Taxes { get; set; }
        public IList<PaySlipEOBIModel> EOBI { get; set; }
        public IList<PaySlipEOBIModel> EOBIContribution { get; set; }
        public IList<PaySlipEOBIModel> EOBIDeduction { get; set; }
        public IList<PaySlipProvidentFundModel> ProvidentFund { get; set; }
        public IList<PaySlipProvidentFundModel> ProvidentFundContribution { get; set; }
        public IList<PaySlipProvidentFundModel> ProvidentFundDeduction { get; set; }
        public IList<PaySlipBonusModel> Bonuses { get; set; }

        public long? EmpId { get; set; }
        public string EmpCode { get; set; }
        public long EmployeeTypeId { get; set; }
        public int? DepartId { get; set; }
        public string DepartName { get; set; }
        public string EmpName { get; set; }
        public DateTime? DOJ { get; set; }
        public string Designation { get; set; }
        public int? MonthNo { get; set; }
        public int? YearNo { get; set; }
        public double? PerDaySalary { get; set; }
        public double? BasicSalary { get; set; }
        public double? PerHourSalary { get; set; }
        public string TotalOT { get; set; }
        public double? OTAmount { get; set; }
        public int? TotalAbsent { get; set; }
        public double? AbsentAmount { get; set; }
        public int? TotalLeave { get; set; }
        public double? LeaveAmount { get; set; }
        public int? TotalLate { get; set; }
        public double? LateAmount { get; set; }
        public int? TotalHalfDay { get; set; }
        public double? HalfDayAmount { get; set; }
        public double? LoanAmount { get; set; }
        public double? TotalLoanAmount { get; set; }
        public double? LoanAmountPaid { get; set; }

        public double? AdvanceAmount { get; set; }
        public string HolidayOT { get; set; }
        public double? HolidayOTAmount { get; set; }
        public int TotalDays { get; set; }
        public string EmpCNIC { get; set; }
        public string EmpBankAccNo { get; set; }
        public int? BankId { get; set; }
        public double? IncomeTaxAmount { get; set; }
        public double? SecurityDepositBalance { get; set; }
        public double? SecurityDeposit { get; set; }
        public double? SecurityDepositPaid { get; set; }
        public double? Advances { get; set; }
        public double? Absent { get; set; }
        public IList<LateDeductModel> lates { get; set; } = new List<LateDeductModel>();
        public IList<LateDeductModel> allLate { get; set; } = new List<LateDeductModel>();



        public async Task<IList<PaySlipMasterSheetModel>> GetPaySlipsMasterSheet(long[] employeeIds, string DateFrom, string DateTo)
        {
            try
            {
                //long []employeeIds = new long[empId.Length];
                //int index = 0;
                //foreach(var item in empId)
                //{
                //    employeeIds[index++] = Convert.ToInt64(item);
                //}

                DateTime dtTillDate = Convert.ToDateTime(DateTo);
                DateTime dtFromDate = Convert.ToDateTime(DateFrom);
                int TotalDays = ((dtTillDate - dtFromDate).Days + 1);
                PakOman_NedoEntities _db = new PakOman_NedoEntities();


                // var model1 = _db.tbl_SalaryPostingMaster.Where(q => q.FromDate >= dtFromDate && q.TillDate <= dtTillDate).ToList();

                IList<PaySlipMasterSheetModel> paySlips = await (from q in _db.tbl_SalaryPostingMaster
                                                      where q.MonthNo == dtFromDate.Month && q.YearNo == dtFromDate.Year && employeeIds.Contains(q.EmpId.Value)
                                                      select new PaySlipMasterSheetModel
                                                      {
                                                          EmpId = q.EmpId.Value,
                                                          EmpCode = q.EmpMaster.EmpCode,
                                                          EmpName = q.EmpMaster.EmpName,
                                                          Designation = q.EmpMaster.DesignationMaster.DesignationDesc,
                                                          EmployeeTypeId = q.EmpMaster.EmployeeTypeID,
                                                          DepartId   = q.EmpMaster.DepartmentId,
                                                          DepartName = q.EmpMaster.DepartmentMaster.DepartmentDesc,
                                                          MonthNo = q.MonthNo.Value,
                                                          YearNo = q.YearNo.Value,
                                                          PerDaySalary = q.PerDaySalary.Value,
                                                          BasicSalary = q.BasicSalary.Value,
                                                          PerHourSalary = q.PerHourSalary.Value,
                                                          TotalOT = q.TotalOT,
                                                          OTAmount = q.OTAmount.Value,
                                                          TotalAbsent = q.TotalAbsent.Value,
                                                          AbsentAmount = q.AbsentAmount.Value,
                                                          TotalLeave = q.TotalLeave.Value,
                                                          LeaveAmount = q.LeaveAmount.Value,
                                                          TotalLate = q.TotalLate.Value,
                                                          LateAmount = q.LateAmount.Value,
                                                          TotalHalfDay = q.TotalHalfDay.Value,
                                                          HalfDayAmount = q.HalfDayAmount.Value,
                                                          LoanAmount = q.LoanAmount.Value,
                                                          AdvanceAmount = q.AdvanceAmount.Value,
                                                          HolidayOT = q.HolidayOT,
                                                          HolidayOTAmount = q.HolidayOTAmount.Value,
                                                          TotalDays = TotalDays,
                                                          IncomeTaxAmount = (from p in _db.tbl_IncomeTaxPosting
                                                                             where p.EmpId == q.EmpId.Value && p.MonthNo == q.MonthNo.Value && p.YearNo == q.YearNo.Value
                                                                             select p).ToList().FirstOrDefault().IncomeTaxAmount,
                                                          Allowances = (from p in _db.tbl_AllowancePosting
                                                                        where p.EmpId == q.EmpId.Value && p.MonthNo == q.MonthNo.Value && p.YearNo == q.YearNo.Value
                                                                        select new PaySlipAllowanceModel
                                                                        {
                                                                            AllowanceName = p.AllownceMaster.AllownceName,
                                                                            AllowanceAmount = p.AllowanceAmount.Value
                                                                        }).ToList(),
                                                          Deductions = (from p in _db.tbl_DeductionPosting
                                                                        where p.EmpId == q.EmpId.Value && p.MonthNo == q.MonthNo.Value && p.YearNo == q.YearNo.Value
                                                                        select new PaySlipDeductionModel
                                                                        {
                                                                            DeductionName = p.DeductionMaster.DeductionName,
                                                                            DeductionAmount = p.DeductionAmount.Value
                                                                        }).ToList(),
                                                          Taxes = (from p in _db.tbl_TaxPosting
                                                                   where p.EmpId == q.EmpId.Value && p.MonthNo == q.MonthNo.Value && p.YearNo == q.YearNo.Value
                                                                   select new PaySlipTaxModel
                                                                   {
                                                                       TaxName = p.tbl_TaxMaster.TaxDesc,
                                                                       TaxAmount = p.TaxAmount.Value
                                                                   }).ToList(),
                                                          EOBI = (from p in _db.tbl_EOBIPosting
                                                                  where p.EmpId == q.EmpId.Value && p.MonthNo == q.MonthNo.Value && p.YearNo == q.YearNo.Value && p.CategoryId == 2
                                                                  select new PaySlipEOBIModel
                                                                  {
                                                                      EOBIName = p.tbl_EOBIMaster.EOBIName,
                                                                      EOBIAmount = p.EOBIAmount
                                                                  }).ToList(),
                                                          ProvidentFund = (from p in _db.tbl_ProvidentFundPosting
                                                                           where p.EmpId == q.EmpId.Value && p.MonthNo == q.MonthNo.Value && p.YearNo == q.YearNo.Value && p.CategoryId == 2
                                                                           select new PaySlipProvidentFundModel
                                                                           {
                                                                               ProvidentFundName = p.tbl_ProvidentFundMaster.ProvidentFundName,
                                                                               ProvidentFundAmount = p.ProvidentFundAmount
                                                                           }).ToList(),
                                                          Bonuses = (from p in _db.tbl_BonusPosting
                                                                     where p.EmpId == q.EmpId.Value && p.MonthNo == q.MonthNo.Value && p.YearNo == q.YearNo.Value
                                                                     select new PaySlipBonusModel
                                                                     {
                                                                         BonusName = p.tbl_BonusMaster.BonusName,
                                                                         BonusAmount = p.BonusAmount.Value
                                                                     }).ToList(),
                                                          SecurityDeposit = _db.tbl_SecurityDepositPosting.Where(p => p.EmpId == q.EmpId.Value
                                                                              && p.MonthNo == q.MonthNo.Value
                                                                              && p.YearNo == q.YearNo.Value).Sum(c => c.SecurityDepositAmount) ?? 0,
                                                          SecurityDepositPaid = _db.tbl_SecurityDepositPosting.Where(p => p.EmpId == q.EmpId.Value).Sum(c => c.SecurityDepositAmount),

                                                          

                                                          Advances = _db.tbl_AdvanceDetail.Where(p => p.tbl_AdvanceMaster.EmpId == q.EmpId.Value
                                                                        && !p.tbl_AdvanceMaster.IsDeleted
                                                                        && p.AdvanceInstallmentDate.Value.Month == q.MonthNo.Value
                                                                        && p.AdvanceInstallmentDate.Value.Year == q.YearNo.Value).Sum(c => c.InstallmentAmount),
                                                          Absent = _db.tbl_SalaryPostingMaster.Where(p => p.EmpId == q.EmpId.Value
                                                                      && p.MonthNo == q.MonthNo.Value
                                                                      && p.YearNo == q.YearNo.Value).Select(c => c.AbsentAmount).FirstOrDefault(),

                                                          lates = (from p in _db.AttendancePostings
                                                                   where (p.EmpId == q.EmpId.Value && p.AttDate.Value.Month == q.MonthNo.Value && p.AttDate.Value.Year == q.YearNo.Value)
                                                                   from a in _db.tbl_LateAttendnaceMaster
                                                                   where p.TimeIn >= a.Start_Time && p.TimeIn <= a.End_Time && a.Deduct_Percent > 0 && a.Deduct_Percent < 100
                                                                   select new LateDeductModel
                                                                   {
                                                                       Count = 1,
                                                                       Amount = a.Deduct_Percent
                                                                   }).OrderBy(c => c.Amount).ToList(),

                                                          allLate = (from a in _db.tbl_LateAttendnaceMaster
                                                                     where a.Deduct_Percent > 0 && a.Deduct_Percent < 100
                                                                     select new LateDeductModel
                                                                     {
                                                                         Count = 1,
                                                                         Amount = a.Deduct_Percent
                                                                     }).ToList(),
                                                          TotalLoanAmount = _db.tbl_LoanMaster.Where(c => c.EmpId == q.EmpId.Value && !c.IsDeleted).Select(c => c.LoanAmount).FirstOrDefault() ?? 0,
                                                          LoanAmountPaid = _db.tbl_LoanDetail.Where(c => c.tbl_LoanMaster.EmpId == q.EmpId.Value && !c.tbl_LoanMaster.IsDeleted && c.LoanStatusId == 2).Sum(c => c.InstallmentAmount) ?? 0





                                                      }).ToListAsync();// ToList();


                return paySlips;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "PaySlipModel", "PaySlip_GetPaySlip");

                return null;
            }
        }


        public async Task<IList<LateDeductModel>> GetLates()
        {
            var db = new PakOman_NedoEntities();
            var lates = await db.tbl_LateAttendnaceMaster.Where(c => !c.IsDeleted.Value && c.Deduct_Percent > 0 /*&& c.Deduct_Percent < 100*/)
                .Select(c=> new LateDeductModel { Count = 1, Amount = c.Deduct_Percent}).OrderBy(c=>c.Amount).ToListAsync();
            return lates;
        }




    }
}