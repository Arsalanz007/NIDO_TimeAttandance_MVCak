using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data.Entity;
namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class BonusModel
    {
        public int BonusId { get; set; }
        public string BonusName { get; set; }
        public int? ValueTypeId { get; set; }
        public double? BonusValue { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string EditedBy { get; set; }
        public DateTime EditedOn { get; set; }

        public async Task<bool> SaveBonus()
        {

            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                tbl_BonusMaster am = new tbl_BonusMaster();
                am.BonusName = BonusName;
                am.BonusValue = BonusValue;
                am.ValueTypeId = ValueTypeId;
                am.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                am.CreatedOn = DateTime.Now;
                am.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                am.EditedOn = DateTime.Now;
                _db.tbl_BonusMaster.Add(am);
                await _db.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelBonuss", "Bonus_Save");
                return false;
            }

        }
        public async Task<IList<BonusModel>> GetAll()
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                IList<BonusModel> list = await (from q in _db.tbl_BonusMaster
                                                    orderby q.Id ascending
                                                    select new BonusModel
                                                    {
                                                        BonusId = q.Id,
                                                        BonusName = q.BonusName,
                                                        BonusValue = q.BonusValue
                                                    }).ToListAsync();
                return list;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelBonuss", "Bonus_GetAll");

                return null;
            }
        }
        public async Task<bool> DeleteBonus(int id)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
               
                var mBonusModel = _db.tbl_BonusMaster.Where(q => q.Id == id).FirstOrDefault();
                if (mBonusModel != null)
                {
                    _db.tbl_BonusMaster.Remove(mBonusModel);
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
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelBonuss", "Bonus_Delete");

                return false;
            }
        }
        public async Task<BonusModel> Get_Bonus_By_Id(int id)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                BonusModel am = await (from q in _db.tbl_BonusMaster
                                           where q.Id == id
                                           select new BonusModel
                                           {
                                               BonusId = q.Id,
                                               BonusName = q.BonusName,
                                               BonusValue = q.BonusValue,
                                               ValueTypeId = q.ValueTypeId
                                           }).SingleOrDefaultAsync();
                return am;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelBonuss", "Bonus_GetBonusById");

                return null;
            }
        }

        public async Task<bool> EditBonus()
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                var am = await _db.tbl_BonusMaster.Where(x => x.Id == this.BonusId).FirstOrDefaultAsync();
                if (am != null)
                {
                    am.BonusName = this.BonusName;
                    am.ValueTypeId = this.ValueTypeId;
                    am.BonusValue = this.BonusValue;
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
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelBonuss", "Bonus_Edit");

                return false;

            }
        }
    }
}