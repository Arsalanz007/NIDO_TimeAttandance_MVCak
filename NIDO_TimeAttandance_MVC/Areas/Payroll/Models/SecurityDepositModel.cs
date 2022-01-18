using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data.Entity;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class SecurityDepositModel
    {

        public int SecurityDepositId { get; set; }
        public string SecurityDepositName { get; set; }
        public int? ValueTypeId { get; set; }
        public double? SecurityDepositValue { get; set; }
        public string ValueTypeName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string EditedBy { get; set; }
        public DateTime EditedOn { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public async Task<bool> SaveSecurityDeposit()
        {

            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                tbl_SecurityDepositMaster am = new tbl_SecurityDepositMaster();
                am.DepositName = SecurityDepositName;
                am.DepositValue = SecurityDepositValue;
                am.ValueTypeId = ValueTypeId;
                am.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                am.CreatedOn = DateTime.Now;
                am.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                am.EditedOn = DateTime.Now;
                am.StartDate = StartDate;
                am.EndDate = EndDate;
                _db.tbl_SecurityDepositMaster.Add(am);
                await _db.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelSecurityDeposits", "SecurityDeposit_Save");
                return false;
            }

        }
        public async Task<IList<SecurityDepositModel>> GetAll()
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                IList<SecurityDepositModel> list = await (from q in _db.tbl_SecurityDepositMaster
                                                    orderby q.SecurityDepositId ascending
                                                    select new SecurityDepositModel
                                                    {
                                                        SecurityDepositId = q.SecurityDepositId,
                                                        SecurityDepositName = q.DepositName,
                                                        SecurityDepositValue = q.DepositValue,
                                                        ValueTypeName = q.ValueType.ValueTypeName
                                                    }).ToListAsync();
                return list;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelSecurityDeposits", "SecurityDeposit_GetAll");

                return null;
            }
        }
        public async Task<bool> DeleteSecurityDeposit(int id)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                var mSecurityDepositModel = _db.tbl_SecurityDepositMaster.Where(q => q.SecurityDepositId == id).FirstOrDefault();
                if (mSecurityDepositModel != null)
                {
                    _db.tbl_SecurityDepositMaster.Remove(mSecurityDepositModel);
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
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelSecurityDeposits", "SecurityDeposit_Delete");

                return false;
            }
        }
        public async Task<SecurityDepositModel> Get_SecurityDeposit_By_Id(int id)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                SecurityDepositModel am = await (from q in _db.tbl_SecurityDepositMaster
                                           where q.SecurityDepositId == id
                                           select new SecurityDepositModel
                                           {
                                               SecurityDepositId = q.SecurityDepositId,
                                               SecurityDepositName = q.DepositName,
                                               SecurityDepositValue = q.DepositValue,
                                               ValueTypeId = q.ValueTypeId,
                                               StartDate = q.StartDate,
                                               EndDate = q.EndDate
                                           }).SingleOrDefaultAsync();
                return am;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelSecurityDeposits", "SecurityDeposit_GetSecurityDepositById");

                return null;
            }
        }

        public async Task<bool> EditSecurityDeposit()
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                var am = await _db.tbl_SecurityDepositMaster.Where(x => x.SecurityDepositId == this.SecurityDepositId).FirstOrDefaultAsync();
                if (am != null)
                {
                    am.DepositName = this.SecurityDepositName;
                    am.ValueTypeId = this.ValueTypeId;
                    am.DepositValue = this.SecurityDepositValue;
                    am.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                    am.EditedOn = DateTime.Now;
                    am.StartDate = StartDate;
                    am.EndDate = EndDate;
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
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelSecurityDeposits", "SecurityDeposit_Edit");

                return false;

            }
        }
    }
}