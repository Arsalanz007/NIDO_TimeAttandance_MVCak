using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class PFWithdrawlModel
    {
        public int PFWithdrawalId { get; set; }
        public long? EmpId { get; set; }
        public string EmpName { get; set; }
        public string EmpCode { get; set; }
        public decimal? WithdrawlAmount { get; set; }
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? WithdrawlDate { get; set; }
        public double? EmployeeContributedAmount { get; set; }
        public double? EmployerContributedAmount { get; set; }

        public async Task<IList<PFWithdrawlModel>> GetAll()
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                
                IList<PFWithdrawlModel> list = await (from q in _db.tbl_PFWithdrawlDetail
                                                      orderby q.PFWithdrawId ascending
                                                      select new PFWithdrawlModel
                                                      {
                                                          PFWithdrawalId = q.PFWithdrawId,
                                                          EmpId = q.EmpId,
                                                          EmpCode = q.EmpMaster.EmpCode,
                                                        EmpName = q.EmpMaster.EmpName,
                                                        WithdrawlAmount = q.WithdrawedAmount.Value,
                                                        WithdrawlDate= q.WithdrawDate.Value,
                                                        EmployeeContributedAmount = _db.tbl_ProvidentFundPosting.Where(x=>x.EmpId==q.EmpId && x.CategoryId==1).Sum(y=>y.ProvidentFundAmount.Value),
                                                        EmployerContributedAmount = _db.tbl_ProvidentFundPosting.Where(x => x.EmpId == q.EmpId && x.CategoryId == 2).Sum(y => y.ProvidentFundAmount.Value)



                                                      }).ToListAsync();
                return list;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelPFWithdrawl", "PFWithdrawl_GetAll");

                return null;
            }
        }

        public async Task<PFWithdrawlModel> Get_PFWithdrawl_By_Id(int id)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                PFWithdrawlModel am = await (from q in _db.tbl_PFWithdrawlDetail
                                           where q.PFWithdrawId == id
                                           select new PFWithdrawlModel
                                           {
                                               EmpId = q.EmpId,
                                               WithdrawlAmount = q.WithdrawedAmount,
                                               WithdrawlDate= q.WithdrawDate,
                                               EmployeeContributedAmount= _db.tbl_ProvidentFundPosting.Where(x => x.EmpId == q.EmpId && x.CategoryId == 1).Sum(y => y.ProvidentFundAmount),
                                               PFWithdrawalId = q.PFWithdrawId,
                                               EmployerContributedAmount = _db.tbl_ProvidentFundPosting.Where(x => x.EmpId == q.EmpId && x.CategoryId == 2).Sum(y => y.ProvidentFundAmount)
                                           }).SingleOrDefaultAsync();
                return am;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelPFWithdrawl", "Get_PFWithdrawl_By_Id");

                return null;
            }
        }
        public async Task<bool> SavePFWithdrawel()
        {

            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                tbl_PFWithdrawlDetail pm = new tbl_PFWithdrawlDetail();
                pm.EmpId=EmpId;
                pm.WithdrawDate = WithdrawlDate;
                pm.WithdrawedAmount = WithdrawlAmount;
                pm.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                pm.CreatedOn = DateTime.Now;
                pm.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                pm.EditedOn = DateTime.Now;
                _db.tbl_PFWithdrawlDetail.Add(pm);
                await _db.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAllowances", "Allowance_Save");
                return false;
            }

        }
        public PFWithdrawlModel GetPF(int empID)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                PFWithdrawlModel list = new PFWithdrawlModel();
                list.EmpId = empID;
                list.EmployeeContributedAmount = _db.tbl_ProvidentFundPosting.Where(x => x.EmpId == empID && x.CategoryId == 1).Sum(y => y.ProvidentFundAmount);
                list.EmployerContributedAmount = _db.tbl_ProvidentFundPosting.Where(x => x.EmpId == empID && x.CategoryId == 2).Sum(y => y.ProvidentFundAmount);

                return list;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelPFWithdrawl", "PFWithdrawl_GetPF");

                return null;
            }
        }

        public async Task<bool> EditPFWithdrawl()
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                var am = await _db.tbl_PFWithdrawlDetail.Where(x => x.PFWithdrawId == this.PFWithdrawalId).FirstOrDefaultAsync();
                if (am != null)
                {
                    am.WithdrawDate = this.WithdrawlDate;
                    am.EmpId = this.EmpId;
                    am.WithdrawedAmount = this.WithdrawlAmount;
                    am.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                    am.EditedOn = DateTime.Now;
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
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelPFWithdrawl", "ModelPFWithdrawl_Edit");

                return false;

            }
        }
        public async Task<bool> DeletePFWithdrawel(int id)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                var mPF = _db.tbl_PFWithdrawlDetail.Where(q => q.PFWithdrawId == id).FirstOrDefault();
                if (mPF != null)
                {
                    _db.tbl_PFWithdrawlDetail.Remove(mPF);
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
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelPFWithdrawl", "DeletePFWithdrawel");

                return false;
            }
        }

    }
}