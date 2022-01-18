using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data.Entity;
using NIDO_TimeAttandance_MVC.Utilities;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class DeductionModel
    {
        public int DeductionId { get; set; }
        public string DeductionName { get; set; }
        public int? ValueTypeId { get; set; }
        public double? DeductionValue { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string EditedBy { get; set; }
        public DateTime EditedOn { get; set; }
        public string ValueTypeName { get; set; }

        public  async Task<bool> DeleteDeduction(int id)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                DeductionMaster dm = _db.DeductionMasters.Where(x => x.Id == id).FirstOrDefault();
                if(dm != null)
                {
                    _db.DeductionMasters.Remove(dm);
                    await _db.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }

            }catch(Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelDeduction", "DeleteDeduction");
                return false;
            }
        }
        public async Task<bool> SaveDeduction()
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                DeductionMaster dm = new DeductionMaster();
                dm.DeductionName = DeductionName;
                dm.DeductionValue = DeductionValue;
                dm.ValueTypeId = ValueTypeId;
                dm.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                dm.CreatedOn = DateTime.Now;
                dm.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                dm.EditedOn = DateTime.Now;
                _db.DeductionMasters.Add(dm);
                await _db.SaveChangesAsync();
                return true;

            }
            catch(Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelDeduction", "Deduction_Save");
                return false;
            }

        }
        public async Task<DeductionModel> Get_Deduction_By_Id(int id)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                DeductionModel am = await (from q in _db.DeductionMasters
                                           where q.Id == id
                                           select new DeductionModel
                                           {
                                               DeductionId = q.Id,
                                               DeductionName = q.DeductionName,
                                               DeductionValue = q.DeductionValue,
                                               ValueTypeId = q.ValueTypeId
                                           }).SingleOrDefaultAsync();
                return am;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelDeduction", "Deduction_GetDeductionById");

                return null;
            }
        }

        public async Task<bool> EditDeduction()
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                DeductionMaster dm = _db.DeductionMasters.Where(x => x.Id == DeductionId).FirstOrDefault();
                if(dm != null)
                {
                    dm.DeductionName = DeductionName;
                    dm.ValueTypeId = ValueTypeId;
                    dm.DeductionValue = DeductionValue;
                    dm.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                    dm.EditedOn = DateTime.Now;
                    await _db.SaveChangesAsync();
                    return true;

                }
                else
                {
                    return false;
                }

            }catch(Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelDeduction", "Deduction_Edit");

                return false;
            }
        }
        public async Task<IList<DeductionModel>> GetAll()
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                IList<DeductionModel> dm = await (from q in _db.DeductionMasters
                                                  join v in _db.ValueTypes on q.ValueTypeId equals v.ValueTypeId
                                                  orderby q.Id
                                                  select new DeductionModel
                                                  {
                                                      DeductionId = q.Id,
                                                      DeductionName = q.DeductionName,
                                                      DeductionValue = q.DeductionValue,
                                                      ValueTypeId = q.ValueTypeId,
                                                      ValueTypeName = v.ValueTypeName
                                                  }).ToListAsync();
                return dm;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelDeduction", "Deduction_GetAll");
                return null;
            }
        }
    }
}