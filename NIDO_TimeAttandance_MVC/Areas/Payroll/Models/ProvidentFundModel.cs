using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data.Entity;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class ProvidentFundModel
    {
        public int ProvidentFundId { get; set; }
        public string ProvidentFundName { get; set; }
        public int? ValueTypeId { get; set; }
        public double? ProvidentFundValue { get; set; }
        public int? CategoryId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string EditedBy { get; set; }
        public DateTime EditedOn { get; set; }

        public async Task<bool> SaveProvidentFund()
        {

            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                tbl_ProvidentFundMaster pm = new tbl_ProvidentFundMaster();
                pm.ProvidentFundName = ProvidentFundName;
                pm.ProvidentFundValue = ProvidentFundValue;
                pm.ValueTypeId = ValueTypeId;
                pm.CategoryId = CategoryId;
                pm.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                pm.CreatedOn = DateTime.Now;
                pm.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                pm.EditedOn = DateTime.Now;
                _db.tbl_ProvidentFundMaster.Add(pm);
                await _db.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelProvidentFund", "ProvidentFund_Save");
                return false;
            }

        }
        public async Task<IList<ProvidentFundModel>> GetAll()
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                IList<ProvidentFundModel> list = await (from q in _db.tbl_ProvidentFundMaster
                                                    orderby q.Id ascending
                                                    select new ProvidentFundModel
                                                    {
                                                        ProvidentFundId = q.Id,
                                                        ProvidentFundName = q.ProvidentFundName,
                                                        ProvidentFundValue = q.ProvidentFundValue,
                                                        CategoryId = q.CategoryId,
                                                        ValueTypeId = q.ValueTypeId

                                                    }).ToListAsync();
                return list;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelProvidentFund", "Provident_GetAll");

                return null;
            }
        }
        public async Task<bool> DeleteProvidentFund(int id)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                var mProvidnetFundModel = _db.tbl_ProvidentFundMaster.Where(q => q.Id == id).FirstOrDefault();
                if (mProvidnetFundModel != null)
                {
                    _db.tbl_ProvidentFundMaster.Remove(mProvidnetFundModel);
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
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelProvidentFund", "ProvidentFund_Delete");

                return false;
            }
        }
        public async Task<ProvidentFundModel> Get_ProvidentFund_By_Id(int id)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                ProvidentFundModel pm = await (from q in _db.tbl_ProvidentFundMaster
                                           where q.Id == id
                                           select new ProvidentFundModel
                                           {
                                               ProvidentFundId = q.Id,
                                               ProvidentFundName = q.ProvidentFundName,
                                               ProvidentFundValue = q.ProvidentFundValue,
                                               ValueTypeId = q.ValueTypeId,
                                               CategoryId = q.CategoryId
                                           }).SingleOrDefaultAsync();
                return pm;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelProvidentFund", "ProvidentFund_GetProvidentFundById");

                return null;
            }
        }

        public async Task<bool> EditProvidentFund()
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                var pm = await _db.tbl_ProvidentFundMaster.Where(x => x.Id == this.ProvidentFundId).FirstOrDefaultAsync();
                if (pm != null)
                {
                    pm.ProvidentFundName = this.ProvidentFundName;
                    pm.ValueTypeId = this.ValueTypeId;
                    pm.ProvidentFundValue = this.ProvidentFundValue;
                    pm.CategoryId = this.CategoryId;

                    pm.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                    pm.EditedOn = DateTime.Now;
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
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelProvident", "Provident_Edit");

                return false;

            }
        }
    }
}