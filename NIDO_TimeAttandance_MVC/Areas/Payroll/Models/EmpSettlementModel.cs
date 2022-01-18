using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class EmpSettlementModel
    {
        public int EmpSettlementId { get; set; }
        public long? EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string Designation { get; set; }
        public double? BasicSalary { get; set; }
        public double? EOBI { get; set; }
        public double? ProvidentFund { get; set; }
        public double? PFWithdrawl { get; set; }

        public double? Advance { get; set; }
        public double? Loan { get; set; }
        public double? LastMonthSalary { get; set; }
        public double? NetAmount { get; set; }

        public string Company { get; set; }
        public string AppointmentDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CNICNo { get; set; }
        public string Department { get; set; }
        public string Grade { get; set; }
        public string EmployeeType { get; set; }


        public IList<PaySlipAllowanceModel> PaySlip_Allowances { get; set; }
        public IList<PaySlipDeductionModel> PaySlip_Deductions { get; set; }
        public IList<PaySlipTaxModel> PaySlip_Taxes { get; set; }
        public IList<PaySlipEOBIModel> PaySlip_EOBI { get; set; }
        public IList<PaySlipEOBIModel> PaySlip_EOBIContribution { get; set; }
        public IList<PaySlipEOBIModel> PaySlip_EOBIDeduction { get; set; }
        public IList<PaySlipProvidentFundModel> PaySlip_ProvidentFund { get; set; }
        public IList<PaySlipProvidentFundModel> PaySlip_ProvidentFundContribution { get; set; }
        public IList<PaySlipProvidentFundModel> PaySlip_ProvidentFundDeduction { get; set; }



        public long EmployeeTypeId { get; set; }
        public int? MonthNo { get; set; }
        public int? YearNo { get; set; }
        public double? PerDaySalary { get; set; }
        public double? PerHourSalary { get; set; }
        public string PaySlip_TotalOT { get; set; }
        public double? PaySlip_OTAmount { get; set; }
        public int? PaySlip_TotalAbsent { get; set; }
        public double? PaySlip_AbsentAmount { get; set; }
        public int? PaySlip_TotalLeave { get; set; }
        public double? PaySlip_LeaveAmount { get; set; }
        public int? PaySlip_TotalLate { get; set; }
        public double? PaySlip_LateAmount { get; set; }
        public int? PaySlip_TotalHalfDay { get; set; }
        public double? PaySlip_HalfDayAmount { get; set; }
        public double? PaySlip_LoanAmount { get; set; }
        public double? PaySlip_AdvanceAmount { get; set; }
        public string PaySlip_HolidayOT { get; set; }
        public double? PaySlip_HolidayOTAmount { get; set; }
        public int PaySlip_TotalDays { get; set; }

        public List<PaySlipModel> PaySlip { get; set; }


        public async Task<bool> SaveEmpFinalSettlement(string empIds)
        {
            try
            {

                PakOman_NedoEntities _db = new PakOman_NedoEntities();


                string[] employeeIds = empIds.Split(new char[] { ',' });



                string abc = "," + empIds + ",";

                _db.Database.CommandTimeout = 10000;
                var today = DateTime.Now;
                var DateFrom = new DateTime(today.Year, 2, 1).ToShortDateString();
                var DateTo = today.ToShortDateString();


                //int res = await Task.Run(() => _db.stp_SalaryPosting(abc, DateFrom, today.ToShortDateString(), DataHelper.SessionUserName));


                foreach (var item in employeeIds)
                {

                    var empId = Convert.ToInt64(item);
                    var Advance = _db.tbl_AdvanceDetail.Where(a => a.tbl_AdvanceMaster.EmpId.Value == empId && a.PaymentStatusId == 1).Sum(x => x.InstallmentAmount) ?? 0;

                    var Loan = _db.tbl_LoanDetail.Where(a => a.tbl_LoanMaster.EmpId.Value == empId && a.LoanStatusId == 1).Sum(x => x.InstallmentAmount) ?? 0;

                    var ProvidentFund = _db.tbl_ProvidentFundPosting.Where(a => a.EmpId.Value == empId).Sum(x => x.ProvidentFundAmount) ?? 0;

                    var EOBI = _db.tbl_EOBIPosting.Where(a => a.EmpId.Value == empId).Sum(x => x.EOBIAmount) ?? 0;

                    var PFWithdrawl = (double?)_db.tbl_PFWithdrawlDetail.Where(a => a.EmpId.Value == empId).Sum(x => x.WithdrawedAmount) ?? 0;

                    var SalaryModel = _db.tbl_SalaryPostingMaster.Where(c => c.MonthNo == DateTime.Now.Month && c.EmpId == empId ).FirstOrDefault();

                    double? Salary = 0;

                    Salary = SalaryModel != null ? SalaryModel.BasicSalary : 0;

                    var NetAmount = Salary + EOBI + ProvidentFund - (Advance + Loan + PFWithdrawl);
                    var model = new Emp_FullAndFinalSettlement
                    {
                        Advance = Advance,
                        Loan = Loan,
                        ProvidentFund = ProvidentFund,
                        PFWithdrawl = PFWithdrawl,
                        LastMonthSalary = Salary,
                        NetAmount = NetAmount,
                        EOBI = EOBI,
                        EmpId = empId,
                    };


                    _db.Emp_FullAndFinalSettlement.Add(model);



                }

                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "EmpSettlementModel", "EmpSettlement_SaveFinalSettlement");

                throw;
            }
        }

        public async Task<IList<EmpSettlementModel>> GetFullAndFinalReport(long[] employeeIds)
        {
            try
            {
                //long []employeeIds = new long[empId.Length];
                //int index = 0;
                //foreach(var item in empId)
                //{
                //    employeeIds[index++] = Convert.ToInt64(item);
                //}

                var today = DateTime.Now;
                var DateFrom = new DateTime(today.Year, 2, 1).ToShortDateString();
                var DateTo = today.ToShortDateString();

                var psm = new PaySlipModel();

                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                IList<EmpSettlementModel> report = await (from q in _db.Emp_FullAndFinalSettlement
                                                            where employeeIds.Contains(q.EmpId.Value)
                                                            select new EmpSettlementModel
                                                            {
                                                                EmpId = q.EmpId.Value,
                                                                EmpCode = q.EmpMaster.EmpCode,
                                                                EmpName = q.EmpMaster.EmpName,
                                                                Designation = q.EmpMaster.DesignationMaster.DesignationDesc,
                                                                AppointmentDate = q.EmpMaster.AppointmentDate,
                                                                CNICNo = q.EmpMaster.CNICNo,
                                                                Company = q.EmpMaster.CompanyMaster.CompanyDesc,
                                                                EndDate = q.CreateOn.Value,
                                                                Advance = q.Advance ?? 0,
                                                                Loan = q.Loan ?? 0,
                                                                LastMonthSalary = q.LastMonthSalary ?? 0,
                                                                EOBI = q.EOBI ?? 0,
                                                                ProvidentFund = q.ProvidentFund ?? 0,
                                                                PFWithdrawl = q.PFWithdrawl ?? 0,
                                                                NetAmount = q.NetAmount ?? 0,
                                                                Department = q.EmpMaster.DepartmentMaster.DepartmentDesc,
                                                                EmployeeType = q.EmpMaster.EmployeeType.EmployeeTypeDsc,
                                                                Grade = q.EmpMaster.GradeMaster.GradeDesc,
                                                                


                                                            }).ToListAsync();// ToList();

                foreach (var item in report)
                {
                    long[] empId = { item.EmpId.Value };
                    var paySlipList = await psm.GetPaySlips(empId, DateFrom, DateTo);
                    var paySlip = paySlipList.FirstOrDefault();

                    item.PaySlip_Allowances = paySlip.Allowances;
                    item.PaySlip_Deductions = paySlip.Deductions;
                    item.PaySlip_EOBI = paySlip.EOBI;     
                    item.PaySlip_EOBIDeduction = paySlip.EOBIDeduction;
                    item.PaySlip_ProvidentFund = paySlip.ProvidentFund;
                    item.PaySlip_ProvidentFund = paySlip.ProvidentFund;
                    
                    
                    
                    item.PaySlip_AbsentAmount = paySlip.AbsentAmount;
                    item.PaySlip_AdvanceAmount = paySlip.AdvanceAmount;
                    item.PaySlip_HalfDayAmount = paySlip.HalfDayAmount;
                    item.PaySlip_HolidayOT = paySlip.HolidayOT;
                    item.PaySlip_HolidayOTAmount = paySlip.HolidayOTAmount;
                    item.PaySlip_LateAmount = paySlip.LateAmount;
                    item.PaySlip_LeaveAmount = paySlip.LeaveAmount;
                    item.PaySlip_LoanAmount = paySlip.LoanAmount;
                    item.PaySlip_OTAmount = paySlip.OTAmount;
                    item.PaySlip_TotalAbsent = paySlip.TotalAbsent;
                    item.PaySlip_TotalDays = paySlip.TotalDays;
                    item.PaySlip_TotalHalfDay = paySlip.TotalHalfDay;
                    item.PaySlip_TotalLate = paySlip.TotalLate;
                    item.PaySlip_TotalLeave = paySlip.TotalLeave;
                    item.PaySlip_TotalOT = paySlip.TotalOT;
                    item.BasicSalary = paySlip.BasicSalary;
                    item.PerDaySalary = paySlip.PerDaySalary;
                    item.PerHourSalary = paySlip.PerHourSalary;
                }

                return report;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "EmpSettlementModel", "EmpSettlement_GetFullAndFinalReport");

                return null;
            }
        }
    }
}