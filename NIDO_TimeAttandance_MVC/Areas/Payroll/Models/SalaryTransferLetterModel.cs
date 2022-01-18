using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class SalaryTransferLetterModel
    {
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string CNIC { get; set; }
        public string Acc_No { get; set; }
        public double? Amount { get; set; }
        public int? BankId { get; set; }


        public async Task<IList<SalaryTransferLetterModel>> GetSalaryTransferLetterData(long[] empIds, DateTime fromDate, DateTime toDate,int? BankId)
        {
            var _db = new PakOman_NedoEntities();


            var paySlips = await (from q in _db.tbl_SalaryPostingMaster
                                      // where q.MonthNo == dtFromDate.Month && q.YearNo == dtFromDate.Year && employeeIds.Contains(q.EmpId.Value)
                                  where q.MonthNo == fromDate.Month && q.YearNo == toDate.Year && empIds.Contains(q.EmpId.Value) && (BankId.HasValue ? q.EmpMaster.BankId == BankId : true)
                                 
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
                                      EmpCNIC = q.EmpMaster.CNICNo,
                                      EmpBankAccNo = q.EmpMaster.Bank_AccountNo,
                                      BankId = q.EmpMaster.BankId,
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
                                    
                                      EOBIDeduction = (from p in _db.tbl_EOBIPosting
                                                       where p.EmpId == q.EmpId.Value && p.MonthNo == q.MonthNo.Value && p.YearNo == q.YearNo.Value && p.CategoryId == 2
                                                       select new PaySlipEOBIModel
                                                       {
                                                           EOBIName = p.tbl_EOBIMaster.EOBIName,
                                                           EOBIAmount = p.EOBIAmount
                                                       }).ToList(),
                                    
                                      ProvidentFundDeduction = (from p in _db.tbl_ProvidentFundPosting
                                                                where p.EmpId == q.EmpId.Value && p.MonthNo == q.MonthNo.Value && p.YearNo == q.YearNo.Value && p.CategoryId == 2
                                                                select new PaySlipProvidentFundModel
                                                                {
                                                                    ProvidentFundName = p.tbl_ProvidentFundMaster.ProvidentFundName,
                                                                    ProvidentFundAmount = p.ProvidentFundAmount
                                                                }).ToList(),

                                  }).ToListAsync();// ToList();



            var dataList = new List<SalaryTransferLetterModel>();



            foreach (var item in paySlips.DistinctBy(c => c).ToList())
            {
                var model = new SalaryTransferLetterModel();
                if (item != null)
                {
                    var totalAllowance = item.Allowances.Sum(c => c.AllowanceAmount);
                    var totalDeduction = item.Deductions.Sum(c => c.DeductionAmount) + item.EOBIDeduction.Sum(c => c.EOBIAmount) + item.ProvidentFundDeduction.Sum(c => c.ProvidentFundAmount);

                    var totalTax = item.Taxes.Sum(c => c.TaxAmount);



                    var grossSalary = (item.BasicSalary + totalAllowance) ?? 0;

                    var netSalary = grossSalary - totalDeduction;

                    model.EmpName = item.EmpName;
                    model.EmpCode = item.EmpCode;
                    model.Acc_No = item.EmpBankAccNo;
                    model.CNIC = item.EmpCNIC;
                    model.Amount = netSalary;
                    model.BankId = item.BankId;

                }
                dataList.Add(model);
            }

           

            return dataList;
        }

        public  async Task<IList<BankModel>> GetBankModels()
        {
            var _db = new PakOman_NedoEntities();

            IList<BankModel> model = await _db.tbl_BankMaster.Select(c=> new BankModel {
                
            BankId = c.Id,
            Bank_Name = c.Bank_Name,
            Address = c.Address,
            Branch_Code = c.Branch_Code,
            CityName = c.CityMaster.CityDesc
            
            }).ToListAsync();

            return model;
        }


    }
}