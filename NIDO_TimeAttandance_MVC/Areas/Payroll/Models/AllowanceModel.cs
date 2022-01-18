using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data.Entity;
namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class AllowanceModel
    {
        public int AllowanceId { get; set; }
        public string AllowanceName { get; set; }
        public int? ValueTypeId { get; set; }
        public double? AllowanceValue { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string EditedBy { get; set; }
        public DateTime EditedOn { get; set; }

        public async Task<bool> SaveAllowance()
        {
            
            try {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                AllownceMaster am = new AllownceMaster();
                am.AllownceName = AllowanceName;
                am.AllownceValue = AllowanceValue;
                am.ValueTypeId = ValueTypeId;
                am.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                am.CreatedOn = DateTime.Now;
                am.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                am.EditedOn = DateTime.Now;
                _db.AllownceMasters.Add(am);
                await _db.SaveChangesAsync();
                return true;

            } catch(Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAllowances", "Allowance_Save");
                return false;
            }
            
        }
        public async Task<IList<AllowanceModel>> GetAll()
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                IList<AllowanceModel> list = await(from q in _db.AllownceMasters
                                                   orderby q.Id ascending
                                                    select new AllowanceModel
                                                    {
                                                        AllowanceId = q.Id,
                                                        AllowanceName = q.AllownceName,
                                                        AllowanceValue = q.AllownceValue
                                                    }).ToListAsync();
                return list;

            }catch(Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAllowances", "Allowance_GetAll");

                return null;
            }
        }
        public async Task<bool> DeleteAllowance(int id)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                var mAllowanceModel = _db.AllownceMasters.Where(q => q.Id == id).FirstOrDefault();
                if(mAllowanceModel != null)
                {
                    _db.AllownceMasters.Remove(mAllowanceModel);
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
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAllowances", "Allowance_Delete");

                return false;
            }
        }
        public async Task<AllowanceModel> Get_Allowance_By_Id(int id)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                AllowanceModel am = await (from q in _db.AllownceMasters
                                           where q.Id == id
                                           select new AllowanceModel
                                           {
                                               AllowanceId = q.Id,
                                               AllowanceName = q.AllownceName,
                                               AllowanceValue = q.AllownceValue,
                                               ValueTypeId = q.ValueTypeId
                                           }).SingleOrDefaultAsync();
                return am;

            }catch(Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAllowances", "Allowance_GetAllowanceById");

                return null;
            }
        }

        public async Task<bool> EditAllowance()
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                var am = await _db.AllownceMasters.Where(x => x.Id == this.AllowanceId).FirstOrDefaultAsync();
                if (am != null)
                {
                    am.AllownceName = this.AllowanceName;
                    am.ValueTypeId = this.ValueTypeId;
                    am.AllownceValue = this.AllowanceValue;
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
            catch(Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelAllowances", "Allowance_Edit");

                return false;

            }
        }
    }
}