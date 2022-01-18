using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data.Entity;
using NIDO_TimeAttandance_MVC.Utilities;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class AdvanceModel
    {
        public int AdvanceId { get;set; }
        public long? EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public DateTime? ApplyDate { get; set; }
        public DateTime? AdvanceStartDate { get; set;  }
    
        public DateTime? AdvanceMaturityDate { get; set; }
        public double? AdvanceAmount { get; set; }
        public string AdvanceDesc { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string EditedBy { get; set; }
        public DateTime EditedOn { get; set; }
        public IList<AdvanceDetailModel> AdvanceInstallments { get; set; }

        
        public async Task<IList<AdvanceModel>> GetAll()
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                IList<AdvanceModel> aam = await (from q in _db.tbl_AdvanceMaster
                                              where q.IsDeleted == false
                                                            select new AdvanceModel
                                                            {
                                                                EmpId = q.EmpMaster.EmpId,
                                                                EmpCode = q.EmpMaster.EmpCode,
                                                                EmpName = q.EmpMaster.EmpName,
                                                                AdvanceId = q.AdvanceId,
                                                                ApplyDate = q.ApplyDate,
                                                                AdvanceMaturityDate = q.AdvanceMaturityDate,
                                                                AdvanceStartDate = q.AdvanceStartDate,
                                                                AdvanceAmount = q.AdvanceAmount,
                                                                IsDeleted = q.IsDeleted,
                                                                AdvanceDesc=q.AdvanceDesc

                                                                
                                                            }).ToListAsync();
                return aam;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAdvance", "Advance_GetAll");
                return null;
            }
        }


        public async Task<bool> SaveAdvance()
        {
            try
            {
               
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                tbl_AdvanceMaster lm = new tbl_AdvanceMaster();
                lm.EmpId = EmpId;
                lm.ApplyDate = ApplyDate;
                lm.AdvanceStartDate = AdvanceStartDate;
                lm.AdvanceMaturityDate = AdvanceMaturityDate;
                lm.AdvanceAmount = AdvanceAmount;
                lm.AdvanceDesc = AdvanceDesc;
                _db.tbl_AdvanceMaster.Add(lm);
                await _db.SaveChangesAsync();
                int iTotalMonths = ((AdvanceMaturityDate.Value.Year - AdvanceStartDate.Value.Year) * 12) + AdvanceMaturityDate.Value.Month - AdvanceStartDate.Value.Month  +1  ;
                double dInstallmentAmount = Math.Round(AdvanceAmount.Value / iTotalMonths);
                
                    DateTime dtCurrent = AdvanceStartDate.Value;
                    
                    while (dtCurrent <= AdvanceMaturityDate.Value)
                    {
                    tbl_AdvanceDetail ld = new tbl_AdvanceDetail();
                    ld.AdvanceId = lm.AdvanceId;
                    ld.AdvanceInstallmentDate = dtCurrent;
                    ld.PaymentStatusId = 1;
                    ld.InstallmentAmount = dInstallmentAmount;
                    _db.tbl_AdvanceDetail.Add(ld);
                   // lm.tbl_AdvanceDetail.Add(ld);
                    await _db.SaveChangesAsync();
                    dtCurrent = dtCurrent.AddMonths(1);

                    }



                this.AdvanceId = lm.AdvanceId;
                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAdvance", "Adavnce_Save");
                return false; ;

            }
        }
        public async Task<bool> DeleteAdvance(int id)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                var Advance = _db.tbl_AdvanceMaster.Where(x => x.AdvanceId == id).FirstOrDefault();
                if (Advance != null)
                {
                    Advance.IsDeleted = true;
                   
                    await _db.SaveChangesAsync();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAdvance", "DeleteAdvance");
                return false;
            }
        }

        public async Task<AdvanceModel> Get_Advance_By_Id(int id)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                tbl_AdvanceMaster tlm = await (from q in _db.tbl_AdvanceMaster
                                           where q.AdvanceId == id
                                           select q).SingleOrDefaultAsync();

                AdvanceModel lm = new AdvanceModel();
                lm.AdvanceId = tlm.AdvanceId;
                lm.EmpId = tlm.EmpId;
                lm.EmpCode = tlm.EmpMaster.EmpCode;
                lm.EmpName = tlm.EmpMaster.EmpName;
                lm.AdvanceStartDate = tlm.AdvanceStartDate;
                lm.AdvanceMaturityDate = tlm.AdvanceMaturityDate;
                lm.AdvanceAmount = tlm.AdvanceAmount;
                lm.AdvanceDesc = tlm.AdvanceDesc;
                lm.ApplyDate = tlm.ApplyDate;
                lm.AdvanceInstallments = new List<AdvanceDetailModel>();
                foreach(tbl_AdvanceDetail tld in tlm.tbl_AdvanceDetail)
                {
                    AdvanceDetailModel ldm = new AdvanceDetailModel();
                    ldm.AdvanceDetailId = tld.AdvanceDetailId;
                    ldm.AdvanceInstallmentDate = tld.AdvanceInstallmentDate;
                    ldm.PaymentStatusId = tld.PaymentStatusId;
                    ldm.PaymentStatusDesc = tld.tbl_PaymentStatusMaster.PaymentStatusDesc;
                    ldm.InstallmentAmount = tld.InstallmentAmount;
                    lm.AdvanceInstallments.Add(ldm);
                }


                return lm;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAllowances", "Allowance_GetAllowanceById");

                return null;
            }
        }
       
        public async Task<AdvanceDetailModel> Get_Advance_Detail_By_Id(int id)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                tbl_AdvanceDetail tld = await (from q in _db.tbl_AdvanceDetail
                                            where q.AdvanceDetailId == id
                                            select q).SingleOrDefaultAsync();

                
                    AdvanceDetailModel ldm = new AdvanceDetailModel();
                    ldm.AdvanceDetailId = tld.AdvanceDetailId;
                    ldm.AdvanceInstallmentDate = tld.AdvanceInstallmentDate;
                    ldm.PaymentStatusId = tld.PaymentStatusId;
                    ldm.PaymentStatusDesc = tld.tbl_PaymentStatusMaster.PaymentStatusDesc;
                    ldm.InstallmentAmount = tld.InstallmentAmount;
                    ldm.AdvanceId = tld.AdvanceId;
                


                return ldm;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAdvance", "Advance_GetAdvanceDetailById");

                return null;
            }
        }

        public async Task<IList<AdvanceReportModel>> GetAdvanceReport(long[] empIds, DateTime fromDate, DateTime toDate)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                var advances = await (from q in _db.tbl_AdvanceDetail
                                   where empIds.Contains(q.tbl_AdvanceMaster.EmpId.Value) && q.AdvanceInstallmentDate.Value <= toDate && q.AdvanceInstallmentDate.Value >= fromDate && !q.tbl_AdvanceMaster.IsDeleted
                                   select new AdvanceReportModel
                                   {
                                       EmpCode = q.tbl_AdvanceMaster.EmpMaster.EmpCode,
                                       EmpName = q.tbl_AdvanceMaster.EmpMaster.EmpName,
                                       DepartmentDesc = q.tbl_AdvanceMaster.EmpMaster.DepartmentMaster.DepartmentDesc,
                                       InstallmentMonth = q.AdvanceInstallmentDate.Value,
                                       InstallmentAmount = q.InstallmentAmount.Value,
                                        AdvanceApplyDate = q.tbl_AdvanceMaster.ApplyDate.Value,
                                       AdvanceId = q.AdvanceId.Value,
                                       PaymentStatusDesc = q.tbl_PaymentStatusMaster.PaymentStatusDesc,
                                       TotalAdvance = q.tbl_AdvanceMaster.AdvanceAmount.Value,
                                       TotalAdvancePaid = _db.tbl_AdvanceDetail.Where(x => x.AdvanceId == q.AdvanceId && x.AdvanceInstallmentDate <= fromDate && x.PaymentStatusId == 2).Sum(a => a.InstallmentAmount.Value)

                                   }).ToListAsync();
                return advances;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAdvance", "GetAdvanceReport");

                return null;
            }

        }


    }
}