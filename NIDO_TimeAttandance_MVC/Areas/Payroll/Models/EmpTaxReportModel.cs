using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services.Description;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class EmpIncomeTaxReportModel
    {
        public long EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string Designation { get; set; }
        public string Company { get; set; }
        public string SlapRange { get; set; }
        public string CNICNo { get; set; }

        public string StartSlap { get; set; }
        public double? EndSlap { get; set; }

        public double? TaxAmount { get; set; }

        public async Task<IList<EmpIncomeTaxReportModel>> GetEmpTaxData(long[] empIds, DateTime fromDate, DateTime toDate)
        {
            var _db = new PakOman_NedoEntities();

         


            var paySlips = await (from q in _db.tbl_IncomeTaxPosting
                                  join emp in _db.EmpMasters on q.EmpId equals emp.EmpId  
                                   where q.MonthNo == fromDate.Month && q.YearNo == toDate.Year && empIds.Contains(q.EmpId.Value)
                                  //where empIds.Contains(q.EmpId.Value)
                                  select new EmpIncomeTaxReportModel
                                  {
                                      EmpId = q.EmpId.Value,
                                      EmpCode = emp.EmpCode,
                                      EmpName = emp.EmpName,
                                      Designation = emp.DesignationMaster.DesignationDesc,
                                      TaxAmount = q.IncomeTaxAmount,
                                      Company =emp.CompanyMaster.CompanyDesc,
                                      SlapRange = q.tbl_IncomeTaxMaster.SalaryStartSlab.ToString() + " - " + q.tbl_IncomeTaxMaster.SalaryEndSlab.ToString(),
                                      CNICNo = emp.CNICNo

                                  }).GroupBy(c=>c.EmpId).ToListAsync();// ToList();

            IList<EmpIncomeTaxReportModel> empTaxList = new List<EmpIncomeTaxReportModel>();

            foreach (var item in paySlips)
            {
                var empDetails = item.FirstOrDefault();
                empTaxList.Add(new EmpIncomeTaxReportModel
                {
                    EmpId = empDetails.EmpId,
                    EmpCode = empDetails.EmpCode,
                    EmpName = empDetails.EmpName,
                    Designation = empDetails.Designation,
                    TaxAmount = item.Sum(c => c.TaxAmount),
                    Company = empDetails.Company,
                    SlapRange = empDetails.SlapRange,
                    CNICNo = CNICNo
                });
            }

            return empTaxList.OrderBy(c=>c.EmpCode).ToList();
        }
    }
}