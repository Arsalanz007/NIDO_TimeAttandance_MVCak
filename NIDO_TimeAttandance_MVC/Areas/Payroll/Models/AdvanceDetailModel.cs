using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data.Entity;
using NIDO_TimeAttandance_MVC.Utilities;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class AdvanceDetailModel
    {
        public long? AdvanceId { get; set; }
        public long AdvanceDetailId { get; set; }
        public DateTime? AdvanceInstallmentDate { get; set; }
        public int? PaymentStatusId { get; set; }
        public string PaymentStatusDesc { get; set; }
        public double? InstallmentAmount { get; set; }

        public async Task<bool> EditAdvanceDetail()
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                tbl_AdvanceDetail dm = _db.tbl_AdvanceDetail.Where(x => x.AdvanceDetailId == AdvanceDetailId).FirstOrDefault();
                if (dm != null)
                {
                    dm.PaymentStatusId = PaymentStatusId;
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
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAdvanceDetail", "AdvanceDetail_Edit");

                return false;
            }
        }

        public async Task<bool> InsertAdvanceDetail()
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                var dm = new tbl_AdvanceDetail();
                if (dm != null)
                {
                    dm.AdvanceId = (int)AdvanceId;
                    dm.AdvanceInstallmentDate = AdvanceInstallmentDate;
                    dm.PaymentStatusId = PaymentStatusId;
                    dm.InstallmentAmount = InstallmentAmount;
                    _db.tbl_AdvanceDetail.Add(dm);
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
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAdvanceDetail", "AdvanceDetail_Insert");

                return false;
            }
        }

        public async Task<bool> UpdateAdvanceMaster(string description, int? id) {



            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();

                var model = await _db.tbl_AdvanceMaster.Where(q => q.AdvanceId == id).SingleOrDefaultAsync();
                if (model !=null)
                {
                    model.AdvanceDesc = description;

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

                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "UpdateAdvanceMaster", "AdvanceMaster_Update");

                return false;
            }
        


        }
    }
}