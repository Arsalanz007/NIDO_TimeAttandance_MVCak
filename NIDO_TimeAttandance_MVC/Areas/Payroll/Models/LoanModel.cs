using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data.Entity;
using NIDO_TimeAttandance_MVC.Utilities;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class LoanModel
    {
        public int LoanId { get;set; }
        public long? EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public DateTime? LoanStartDate { get; set;  }
        public DateTime? LoanMaturityDate { get; set; }
        public double? LoanAmount { get; set; }
        public double? UnAdjustLoan { get; set; }
        public double? PaidLoan { get; set; }
        public bool IsDeleted { get; set; }
        public string LoanDesc { get; set; }

        public DateTime? LoanApplyDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string EditedBy { get; set; }
        public DateTime EditedOn { get; set; }
        public IList<LoanDetailModel> LoanInstallments { get; set; }
        public async Task<IList<LoanModel>> GetAll()
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                IList<LoanModel> aam = await (from q in _db.tbl_LoanMaster
                                              where q.IsDeleted == false
                                                            select new LoanModel
                                                            {
                                                                EmpId = q.EmpMaster.EmpId,
                                                                EmpCode = q.EmpMaster.EmpCode,
                                                                EmpName = q.EmpMaster.EmpName,
                                                                LoanId = q.LoanId,
                                                                LoanApplyDate=q.ApplyDate,
                                                                LoanMaturityDate = q.LoanMaturityDate,
                                                                LoanStartDate = q.LoanStartDate,
                                                                LoanAmount = q.LoanAmount,
                                                                IsDeleted = q.IsDeleted,
                                                                LoanDesc=q.LoanDesc
                                                                
                                                            }).ToListAsync();
                return aam;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelLoan", "Loan_GetAll");
                return null;
            }
        }


        public async Task<bool> SaveDescription()
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                var loan = _db.tbl_LoanMaster.Where(x => x.LoanId == this.LoanId).FirstOrDefault();
                if (loan != null)
                {
                    loan.LoanDesc = this.LoanDesc;

                    await _db.SaveChangesAsync();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelLoan", "SaveDescription");
                return false;
            }
        }
        public async Task<bool> SaveLoan()
        {
            try
            {
               
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                tbl_LoanMaster lm = new tbl_LoanMaster();
                lm.EmpId = EmpId;
                lm.LoanStartDate = LoanStartDate;
                lm.LoanMaturityDate = LoanMaturityDate;
                lm.LoanAmount = LoanAmount;
                lm.LoanDesc = LoanDesc;
                lm.ApplyDate = LoanApplyDate;
                _db.tbl_LoanMaster.Add(lm);
                await _db.SaveChangesAsync();
                int iTotalMonths = ((LoanMaturityDate.Value.Year - LoanStartDate.Value.Year) * 12) + LoanMaturityDate.Value.Month - LoanStartDate.Value.Month  +1  ;
                double dInstallmentAmount = LoanAmount.Value / iTotalMonths;
                
                    DateTime dtCurrent = LoanStartDate.Value;
                    
                    while (dtCurrent <= LoanMaturityDate.Value)
                    {
                    tbl_LoanDetail ld = new tbl_LoanDetail();
                    ld.LoanId = lm.LoanId;
                    ld.LoanInstallmentDate = dtCurrent;
                    ld.LoanStatusId = 1;
                    ld.InstallmentAmount = dInstallmentAmount;
                    _db.tbl_LoanDetail.Add(ld);
                   // lm.tbl_LoanDetail.Add(ld);
                    await _db.SaveChangesAsync();
                    dtCurrent = dtCurrent.AddMonths(1);

                    }



                this.LoanId = lm.LoanId;
                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAllocatedDeduction", "AllocatedDeduction_Allocate");
                return false; ;

            }
        }
        public async Task<bool> DeleteLoan(int id)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                var loan = _db.tbl_LoanMaster.Where(x => x.LoanId == id).FirstOrDefault();
                if (loan != null)
                {
                    loan.IsDeleted = true;
                   
                    await _db.SaveChangesAsync();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelLoan", "DeleteLoan");
                return false;
            }
        }

        public async Task<LoanModel> Get_Loan_By_Id(int id)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                tbl_LoanMaster tlm = await (from q in _db.tbl_LoanMaster
                                           where q.LoanId == id
                                           select q).SingleOrDefaultAsync();

                LoanModel lm = new LoanModel();
                lm.LoanId = tlm.LoanId;
                lm.EmpId = tlm.EmpId;
                lm.EmpCode = tlm.EmpMaster.EmpCode;
                lm.EmpName = tlm.EmpMaster.EmpName;
                lm.LoanStartDate = tlm.LoanStartDate;
                lm.LoanMaturityDate = tlm.LoanMaturityDate;
                lm.LoanAmount = tlm.LoanAmount;
                lm.LoanDesc = tlm.LoanDesc;
                lm.LoanApplyDate = tlm.ApplyDate;
                
                lm.LoanInstallments = new List<LoanDetailModel>();
                foreach(tbl_LoanDetail tld in tlm.tbl_LoanDetail)
                {
                    LoanDetailModel ldm = new LoanDetailModel();
                    ldm.LoanDetailId = tld.LoanDetailId;
                    ldm.LoanInstallmentDate = tld.LoanInstallmentDate;
                    ldm.LoanStatusId = tld.LoanStatusId;
                    ldm.LoanStatusDesc = tld.tbl_LoanStatusMaster.LoanStatusDesc;
                    ldm.InstallmentAmount = tld.InstallmentAmount;
                    lm.LoanInstallments.Add(ldm);
                }
                lm.UnAdjustLoan = lm.LoanAmount - lm.LoanInstallments.Sum(c => c.InstallmentAmount);
                lm.PaidLoan = lm.LoanInstallments.Where(c=>c.LoanStatusId == 2).Sum(c => c.InstallmentAmount);


                return lm;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAllowances", "Allowance_GetAllowanceById");

                return null;
            }
        }

        public async Task<LoanDetailModel> Get_Loan_Detail_By_Id(int id)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                tbl_LoanDetail tld = await (from q in _db.tbl_LoanDetail
                                            where q.LoanDetailId == id
                                            select q).SingleOrDefaultAsync();

                
                    LoanDetailModel ldm = new LoanDetailModel();
                    ldm.LoanDetailId = tld.LoanDetailId;
                    ldm.LoanInstallmentDate = tld.LoanInstallmentDate;
                    ldm.LoanStatusId = tld.LoanStatusId;
                    ldm.LoanStatusDesc = tld.tbl_LoanStatusMaster.LoanStatusDesc;
                    ldm.InstallmentAmount = tld.InstallmentAmount;
                    ldm.LoanId = tld.LoanId;
                


                return ldm;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelLoan", "Loan_GetLoanDetailById");

                return null;
            }
        }

        public async Task<IList<LoanLedgerModel>> GetLoanReport(long[] empIds, DateTime fromDate, DateTime toDate)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                //var loans = await (from q in _db.tbl_LoanDetail
                //                   where empIds.Contains(q.tbl_LoanMaster.EmpId.Value) && q.LoanInstallmentDate.Value <= toDate && q.LoanInstallmentDate.Value >= fromDate
                //                   select new LoanReportModel
                //                   {
                //                       EmpCode = q.tbl_LoanMaster.EmpMaster.EmpCode,
                //                       EmpName = q.tbl_LoanMaster.EmpMaster.EmpName,
                //                       DepartmentDesc = q.tbl_LoanMaster.EmpMaster.DepartmentMaster.DepartmentDesc,
                //                       InstallmentMonth = q.LoanInstallmentDate.Value,
                //                       InstallmentAmount = q.InstallmentAmount.Value,
                //                       LoanId = q.LoanId.Value,
                //                       PaymentStatusDesc = q.tbl_LoanStatusMaster.LoanStatusDesc,
                //                       TotalLoan = q.tbl_LoanMaster.LoanAmount.Value,
                //                       TotalLoanPaid = _db.tbl_LoanDetail.Where(x => x.LoanId == q.LoanId && x.LoanInstallmentDate <= fromDate && x.LoanStatusId == 2).Sum(a => a.InstallmentAmount.Value),


                //                   }).ToListAsync();
                var loans = await (from q in _db.tbl_LoanMaster
                                   where empIds.Contains(q.EmpId.Value) && q.LoanStartDate.Value <= toDate && q.LoanMaturityDate.Value >= fromDate && !q.IsDeleted
                                   select new LoanLedgerModel
                                   {
                                       EmpId = q.EmpId.Value,
                                       EmpCode = q.EmpMaster.EmpCode,
                                       EmpName = q.EmpMaster.EmpName,
                                       DepartmentDesc = q.EmpMaster.DepartmentMaster.DepartmentDesc,
                                       LoanId = q.LoanId,
                                       TotalLoan = q.LoanAmount.Value,
                                       
                                       LoanStartDate = q.LoanStartDate.Value,
                                       LoanMaturityDate = q.LoanMaturityDate.Value,
                                       LoanApplyDate = q.ApplyDate.Value,
                                       TotalLoanPaid = q.tbl_LoanDetail.Where(c=>c.LoanInstallmentDate == fromDate && c.LoanStatusId == 2).Sum(c=>c.InstallmentAmount.Value),
                                       LoanDetails = q.tbl_LoanDetail.Select(c=> new LoanLedgerDetailsModel {
                                       
                                       Id = c.LoanDetailId,
                                       Amount = c.InstallmentAmount,
                                       InstallmentDate = c.LoanInstallmentDate,
                                       InstallmentMonth = c.LoanInstallmentDate.Value,
                                       PaymentStatusDesc = c.LoanStatusId.Value == 1  ? "UnPaid" : "Paid"
                                       }).OrderBy(c=>c.Id).ToList(),

                                   }).ToListAsync();
                return loans;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelLoan", "GetLoanReport");

                return null;
            }
           
        }

        public async Task<IList<LoanLedgerModel>> GetLoanSummaryReport(long[] empIds, DateTime fromDate, DateTime toDate)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();

                var loans = await (from q in _db.tbl_LoanMaster
                                   where empIds.Contains(q.EmpId.Value) && q.LoanStartDate.Value <= toDate && q.LoanMaturityDate.Value >= fromDate && !q.IsDeleted
                                   select new LoanLedgerModel
                                   {
                                       EmpId = q.EmpId.Value,
                                       EmpCode = q.EmpMaster.EmpCode,
                                       EmpName = q.EmpMaster.EmpName,
                                       DepartmentDesc = q.EmpMaster.DepartmentMaster.DepartmentDesc,
                                       LoanId = q.LoanId,
                                       TotalLoan = q.LoanAmount.Value,
                                       LoanStartDate = q.LoanStartDate.Value,
                                       LoanMaturityDate = q.LoanMaturityDate.Value,
                                       TotalLoanPaid = _db.tbl_LoanDetail.Where(x => x.LoanId == q.LoanId && x.LoanInstallmentDate <= fromDate && x.LoanStatusId == 2).Sum(a => a.InstallmentAmount.Value),
                                        

                                   }).ToListAsync();
                return loans;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelLoan", "GetLoanReport");

              
                return null;
            }

        }
       

    }
}