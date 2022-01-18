using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data.Entity;
using NIDO_TimeAttandance_MVC.Utilities;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class LoanDetailModel
    {
        public long? LoanId { get; set; }
        public long LoanDetailId { get; set; }
        public DateTime? LoanInstallmentDate { get; set; }
        public int? LoanStatusId { get; set; }
        public string LoanStatusDesc { get; set; }
        public double? InstallmentAmount { get; set; }

        public async Task<bool> EditLoanDetail()
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                tbl_LoanDetail dm = _db.tbl_LoanDetail.Where(x => x.LoanDetailId == LoanDetailId).FirstOrDefault();
                if (dm != null)
                {
                    dm.LoanStatusId = LoanStatusId;
                    dm.InstallmentAmount = InstallmentAmount;
                    await _db.SaveChangesAsync();
                    return true;

                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelLoanDetail", "LoanDetail_Edit");

                return false;
            }
        }

        public async Task<bool> InsertLoanDetail()
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                var dm = new tbl_LoanDetail();

                var loan = _db.tbl_LoanMaster.Where(c => c.LoanId == LoanId).FirstOrDefault();
                if(loan != null)
                {
                   var loanAccept = loan.LoanAmount - loan.tbl_LoanDetail.Sum(c => c.InstallmentAmount) > 0;
                    if (!loanAccept)
                    {
                        throw new InvalidOperationException("403");
                    }
                }

                if (dm != null)
                {
                    dm.LoanId = (int)LoanId;
                    dm.LoanInstallmentDate = LoanInstallmentDate;
                    dm.LoanStatusId = LoanStatusId;
                    dm.InstallmentAmount = InstallmentAmount;
                    _db.tbl_LoanDetail.Add(dm);

                    var lm = _db.tbl_LoanMaster.Where(c => c.LoanId == LoanId).FirstOrDefault();
                    if(lm !=null && dm.LoanInstallmentDate > lm.LoanMaturityDate)
                    {
                        lm.LoanMaturityDate = dm.LoanInstallmentDate;
                    }
                    await _db.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message == "403")
                {
                    throw ex;
                }
                return false;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelLoanDetail", "LoanDetail_Insert");
               return false;
            }
        }

        
    }
}