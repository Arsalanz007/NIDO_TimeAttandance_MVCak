using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class PayrollDashModel
    {
        public long EmpId { get; set; }
        public string EmpName { get; set; }
        public string Designation { get; set; }
        public int? MonthNo { get; set; }
        public int? YearNo { get; set; }
        public double? BasicSalary { get; set; }

        public double? LoanAmount { get; set; }
        public double? GrossSalary { get; set; }
        public double? Deduction { get; set; }
        public double? Tax { get; set; }
        public double? LoanReceived { get; set; }
        public double? NetSalary { get; set; }
        public double? NetSalaryTotal { get; set; }
        public double? GrossSalaryTotal { get; set; }


        public double? UnPaidLoan { get; set; }
        public double? PaidLoan { get; set; }
        public double? LoanPercent { get; set; }
        public double? Allowance { get; set; }

        public double? AllowancesTotal { get; set; }
        public double? DeductionsTotal { get; set; }
        public double? TaxesTotal { get; set; }
        public double? EOBIDeduction { get; set; }
        public double? EOBIContribution { get; set; }

        public double? PfDeduction { get; set; }
        public double? PfContribution { get; set; }
        public double? PfAmount { get; set; }
        public double? EOBIAmount { get; set; }

        public DateTime LastSalaryPostedData { get; set; }




        public static async Task<string> LastSalaryPostedDate()
        {
            var _db = new PakOman_NedoEntities();

            var model = await _db.tbl_SalaryPostingMaster.OrderByDescending(c => c.SalaryPostingId).FirstOrDefaultAsync();

            if (model != null)
            { return model.PostingDate.Value.ToString("dd/MMM/yyyy"); }

            return "Not Posted";

        }
        public static async Task<IList<PayrollDashModel>> GetUserDashboardData(int empId)
        {
            var _db = new PakOman_NedoEntities();

            var departId = await Task.Run(() => _db.tbl_Manager.Where(c => c.ManagerID == empId).Select(c => c.DepartmentID).ToList());

            var empList = new List<long>();

            if (departId != null && departId.Count > 0)
            {
                empList = _db.EmpMasters.Where(c => departId.Contains(c.DepartmentId.Value)).Select(c => c.EmpId).ToList();
            }
            var lastSalary = _db.tbl_SalaryPostingMaster.OrderByDescending(c=>c.TillDate).FirstOrDefault();
            var lastMonth = 0;
            var lastYear = 0;
            
            if (lastSalary == null)
            {
                lastMonth = DateTime.Now.Month;
                lastYear = DateTime.Now.Year;
            }
            else
            {
                lastMonth = lastSalary.MonthNo.Value;
                lastYear = lastSalary.YearNo.Value;
            }


            empList.Add(empId);

            var paySlips = await (from q in _db.tbl_SalaryPostingMaster
                                  where q.MonthNo == lastMonth && q.YearNo == lastYear && empList.Contains(q.EmpId.Value) && q.IsPublish.Value
                                  //  where q.MonthNo == 2 && empList.Contains(q.EmpId.Value) && q.IsPublish.Value
                                  select new PaySlipModel
                                  {
                                      EmpId = q.EmpId.Value,
                                      EmpCode = q.EmpMaster.EmpCode,
                                      EmpName = q.EmpMaster.EmpName,
                                      Designation = q.EmpMaster.DesignationMaster.DesignationDesc,
                                      EmployeeTypeId = q.EmpMaster.EmployeeTypeID,
                                      MonthNo = q.MonthNo.Value,
                                      YearNo = q.YearNo.Value,
                                      PerDaySalary = q.PerDaySalary.Value,
                                      BasicSalary = q.BasicSalary.Value,
                                      PerHourSalary = q.PerHourSalary.Value,

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
                                      EOBIContribution = (from p in _db.tbl_EOBIPosting
                                                          where p.EmpId == q.EmpId.Value && p.MonthNo == q.MonthNo.Value && p.YearNo == q.YearNo.Value && p.CategoryId == 1
                                                          select new PaySlipEOBIModel
                                                          {
                                                              EOBIName = p.tbl_EOBIMaster.EOBIName,
                                                              EOBIAmount = p.EOBIAmount
                                                          }).ToList(),
                                      EOBIDeduction = (from p in _db.tbl_EOBIPosting
                                                       where p.EmpId == q.EmpId.Value && p.MonthNo == q.MonthNo.Value && p.YearNo == q.YearNo.Value && p.CategoryId == 2
                                                       select new PaySlipEOBIModel
                                                       {
                                                           EOBIName = p.tbl_EOBIMaster.EOBIName,
                                                           EOBIAmount = p.EOBIAmount
                                                       }).ToList(),
                                      ProvidentFundContribution = (from p in _db.tbl_ProvidentFundPosting
                                                                   where p.EmpId == q.EmpId.Value && p.MonthNo == q.MonthNo.Value && p.YearNo == q.YearNo.Value && p.CategoryId == 1
                                                                   select new PaySlipProvidentFundModel
                                                                   {
                                                                       ProvidentFundName = p.tbl_ProvidentFundMaster.ProvidentFundName,
                                                                       ProvidentFundAmount = p.ProvidentFundAmount
                                                                   }).ToList(),
                                      ProvidentFundDeduction = (from p in _db.tbl_ProvidentFundPosting
                                                                where p.EmpId == q.EmpId.Value && p.MonthNo == q.MonthNo.Value && p.YearNo == q.YearNo.Value && p.CategoryId == 2
                                                                select new PaySlipProvidentFundModel
                                                                {
                                                                    ProvidentFundName = p.tbl_ProvidentFundMaster.ProvidentFundName,
                                                                    ProvidentFundAmount = p.ProvidentFundAmount
                                                                }).ToList(),

                                  }).ToListAsync();// ToList();



            var dataList = new List<PayrollDashModel>();

            var a = paySlips.DistinctBy(c => c.EmpId).ToList();

            foreach (var item in paySlips.DistinctBy(c => c.EmpId).ToList())
            {
                var model = new PayrollDashModel();
                if (item != null)
                {
                    var totalAllowance = item.Allowances.Sum(c => c.AllowanceAmount);

                    var totalDeduction = item.Deductions.Sum(c => c.DeductionAmount);

                    var totalTax = item.Taxes.Sum(c => c.TaxAmount);



                    var grossSalary = (item.BasicSalary + totalAllowance) ?? 0;

                    model.EmpName = item.EmpName;
                    model.Designation = item.Designation;
                    model.EmpId = item.EmpId.Value;
                    model.GrossSalary = grossSalary;
                    model.Deduction = totalDeduction ?? 0;
                    model.Tax = totalTax ?? 0;
                    model.NetSalary = grossSalary - (totalTax + totalDeduction) ?? 0;
                    model.MonthNo = item.MonthNo;
                    model.PfContribution = item.ProvidentFundContribution.Sum(c => c.ProvidentFundAmount);
                    model.PfDeduction = item.ProvidentFundDeduction.Sum(c => c.ProvidentFundAmount);
                    model.EOBIContribution = item.EOBIContribution.Sum(c => c.EOBIAmount);
                    model.EOBIDeduction = item.EOBIDeduction.Sum(c => c.EOBIAmount);


                    model.PfAmount = _db.tbl_AllocatedProvidentFund.Where(c => c.EmpId == item.EmpId).Sum(c => c.tbl_ProvidentFundMaster.ProvidentFundValue) ?? 0;
                    model.EOBIAmount = _db.tbl_AllocatedEOBI.Where(c => c.EmpId == item.EmpId).Sum(c => c.tbl_EOBIMaster.EOBIValue) ?? 0;


                    var loanMaster = _db.tbl_LoanMaster.Where(c => c.EmpId == model.EmpId && !c.IsDeleted).FirstOrDefault();
                    if (loanMaster != null)
                    {
                        model.LoanAmount = loanMaster.LoanAmount ?? 0;
                        model.UnPaidLoan = loanMaster.tbl_LoanDetail.Where(c => c.LoanStatusId == 1).Sum(c => c.InstallmentAmount) ?? 0;
                        model.PaidLoan = loanMaster.tbl_LoanDetail.Where(c => c.LoanStatusId == 2).Sum(c => c.InstallmentAmount) ?? 0;

                    }
                    else
                    {
                        model.LoanAmount = 0;
                        model.UnPaidLoan = 0;
                        model.PaidLoan = 0;

                    }
                    model.LoanPercent = model.LoanAmount == 0 ? 0 : (model.PaidLoan / model.LoanAmount) * 100 ?? 0;

                }
                else
                {
                    var empData = _db.EmpMasters.Where(c => c.EmpId == model.EmpId).Select(c => new { c.EmpName, c.DesignationMaster.DesignationDesc }).FirstOrDefault();

                    model.EmpName = empData.EmpName;
                    model.Designation = empData.DesignationDesc;
                    model.GrossSalary = 0;
                    model.Deduction = 0;
                    model.Tax = 0;
                    model.NetSalary = 0;
                    model.MonthNo = DateTime.Now.Month;
                    model.NetSalary = 0;

                }

                dataList.Add(model);

            }

            if(dataList.Count == 0 && empList.Count > 0)
            {   
                var empDetails =  _db.EmpMasters.Where(c => departId.Contains(c.DepartmentId.Value)).ToList();
                foreach (var item in empDetails)
                {
                    var model = new PayrollDashModel
                    {
                        EmpName = item.EmpName,
                        Designation = item.DesignationMaster.DesignationDesc,
                        MonthNo = DateTime.Now.Month
                    };
                    dataList.Add(model);
                }
            }

            return dataList;
        }
        public static async Task<IList<PayrollDashModel>> GetMonthlyData(int empId)
        {
            var _db = new PakOman_NedoEntities();

            var departId = await Task.Run(() => _db.tbl_Manager.Where(c => c.ManagerID == empId).Select(c => c.DepartmentID).ToList());

            var empList = new List<long>();

            if (departId != null && departId.Count > 0)
            {
                empList = _db.EmpMasters.Where(c => departId.Contains(c.DepartmentId.Value)).Select(c => c.EmpId).ToList();
            }

            empList.Add(empId);
            var model = _db.tbl_SalaryPostingMaster.Where(c => empList.Contains(c.EmpId.Value) && c.MonthNo <= DateTime.Now.Month && c.YearNo == DateTime.Now.Year && c.IsPublish.Value)
                    .Select(c => new PayrollDashModel
                    {
                        MonthNo = c.MonthNo,
                        Allowance = _db.tbl_AllowancePosting.Where(p => p.EmpId == c.EmpId.Value && p.MonthNo == c.MonthNo.Value && p.YearNo == c.YearNo.Value).Sum(a => a.AllowanceAmount) ?? 0,
                        BasicSalary = c.BasicSalary ?? 0,
                        Tax = _db.tbl_TaxPosting.Where(p => p.EmpId == c.EmpId.Value && p.MonthNo == c.MonthNo.Value && p.YearNo == c.YearNo.Value).Sum(a => a.TaxAmount) ?? 0,
                        Deduction = _db.tbl_DeductionPosting.Where(p => p.EmpId == c.EmpId.Value && p.MonthNo == c.MonthNo.Value && p.YearNo == c.YearNo.Value).Sum(a => a.DeductionAmount) ?? 0,
                        EmpId = c.EmpId.Value,


                    }).ToList().Select(c => new PayrollDashModel
                    {
                        MonthNo = c.MonthNo,
                        Allowance = c.Allowance,
                        BasicSalary = c.BasicSalary,
                        Tax = c.Tax,
                        Deduction = c.Deduction,
                        EmpId = c.EmpId,
                        GrossSalary = c.BasicSalary + c.Allowance,
                        NetSalary = c.BasicSalary + c.Allowance - (c.Tax + c.Deduction)
                    });


            //foreach (var item in model)
            //{
            //    item.GrossSalary = (item.BasicSalary ?? 0) + (item.Allowance ?? 0 ) ;
            //    item.NetSalary = (item.GrossSalary ?? 0) + (item.Tax ?? 0 + item.Deduction ?? 0);
            //}

            var monthList = new List<PayrollDashModel>();

            foreach (var item in model.GroupBy(c => c.MonthNo).ToList())
            {

                monthList.Add(new PayrollDashModel
                {
                    MonthNo = item.Key,
                    NetSalary = item.DistinctBy(c => c.EmpId).Sum(c => c.NetSalary) ?? 0,
                    Deduction = item.DistinctBy(c => c.EmpId).Sum(c => c.Deduction) ?? 0,
                    Tax = item.DistinctBy(c => c.EmpId).Sum(c => c.Tax) ?? 0
                });

                // var a = item.Select(c => c.Tax).Sum(c => c);

            }
            return monthList.OrderBy(c => c.MonthNo).ToList();
        }

        public static async Task<IList<PayrollDashModel>> GetManagerDashboardData(int empId)
        {
            var _db = new PakOman_NedoEntities();

            var departId = _db.tbl_Manager.Where(c => c.ManagerID == empId).Select(c => c.DepartmentID).FirstOrDefault();

            var empList = new List<long>();

            if (departId != 0)
            {
                empList = _db.EmpMasters.Where(c => c.DepartmentId == departId).Select(c => c.EmpId).ToList();
            }

            empList.Add(empId);


            var payslip = await _db.tbl_SalaryPostingMaster.Where(c => c.MonthNo == 2 && empList.Contains(c.EmpId.Value) && c.IsPublish.Value)
                .Select(c => new PayrollDashModel
                {
                    EmpName = c.EmpMaster.EmpName,
                    Designation = c.EmpMaster.DesignationMaster.DesignationDesc,
                    MonthNo = c.MonthNo.Value,
                    YearNo = c.YearNo.Value,
                    Allowance = _db.tbl_AllowancePosting.Where(a => a.EmpId == c.EmpId.Value).Sum(x => x.AllowanceAmount),
                    Tax = _db.tbl_TaxPosting.Where(a => a.EmpId == c.EmpId.Value).Sum(x => x.TaxAmount),
                    Deduction = _db.tbl_DeductionPosting.Where(a => a.EmpId == c.EmpId.Value).Sum(x => x.DeductionAmount),


                }).ToListAsync();

            //   var model = new List<PayrollDashModel>();

            foreach (var model in payslip)
            {
                if (model != null)
                {
                    var grossSalary = model.BasicSalary + model.AllowancesTotal;

                    model.GrossSalary = grossSalary ?? 0;
                    //   model.Deduction = model.DeductionsTotal ?? 0;
                    //   model.Tax = model.Tax ?? 0;
                    model.NetSalary = grossSalary - (model.Tax + model.Deduction) ?? 0;
                    model.MonthNo = model.MonthNo;


                    var loanMaster = _db.tbl_LoanMaster.Where(c => c.EmpId == empId).FirstOrDefault();
                    if (loanMaster != null)
                    {
                        model.LoanAmount = loanMaster.LoanAmount ?? 0;
                        model.UnPaidLoan = loanMaster.tbl_LoanDetail.Where(c => c.LoanStatusId == 1).Sum(c => c.InstallmentAmount) ?? 0;
                        model.PaidLoan = loanMaster.tbl_LoanDetail.Where(c => c.LoanStatusId == 2).Sum(c => c.InstallmentAmount) ?? 0;

                    }
                    else
                    {
                        model.LoanAmount = 0;
                        model.UnPaidLoan = 0;
                        model.PaidLoan = 0;

                    }
                    model.LoanPercent = model.LoanAmount == 0 ? 0 : (model.PaidLoan / model.LoanAmount) * 100 ?? 0;

                }
                else
                {
                    var empData = _db.EmpMasters.Where(c => c.EmpId == empId).Select(c => new { c.EmpName, c.DesignationMaster.DesignationDesc }).FirstOrDefault();

                    model.EmpName = empData.EmpName;
                    model.Designation = empData.DesignationDesc;
                    model.GrossSalary = 0;
                    model.Deduction = 0;
                    model.Tax = 0;
                    model.NetSalary = 0;
                    model.MonthNo = DateTime.Now.Month;
                    model.NetSalary = 0;
                }



            }




            return payslip;
        }


    }


}