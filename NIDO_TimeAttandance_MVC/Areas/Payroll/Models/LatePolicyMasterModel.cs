using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Models
{
    public class LatePolicyMasterModel
    {
        public int LatePolicyId { get; set; }
        public string LatePolicyName { get; set; }

        public IList<LateAttendanceModel> LateAttendanceModelList { get; set; }

        public async Task<IList<LatePolicyMasterModel>> GetAll()
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                IList<LatePolicyMasterModel> aam = await (from q in _db.tbl_LatePolicyMaster
                                                        //where q.IsDeleted == false && q.LatePolicyId == LatePolicyId
                                                        select new LatePolicyMasterModel
                                                        {
                                                            LatePolicyId = q.LatePolicyId,
                                                            LatePolicyName=q.LatePolicName
                                                            

                                                        }).OrderBy(c => c.LatePolicyName).ToListAsync();
                return aam;

            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "LatePolicyMasterModel", "LatePolicyMasterModel_GetAll");
                return null;
            }
        }

        public  LatePolicyMasterModel GetLatePolicy(int LatePolicyId)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                tbl_LatePolicyMaster aam = _db.tbl_LatePolicyMaster.Where(x => x.LatePolicyId == LatePolicyId).FirstOrDefault();
                LatePolicyMasterModel llmm = new LatePolicyMasterModel();
                llmm.LatePolicyId = aam.LatePolicyId;
                llmm.LatePolicyName = aam.LatePolicName;
                LateAttendanceModel lm = new LateAttendanceModel();
                llmm.LateAttendanceModelList =  lm.GetAll(llmm.LatePolicyId);
                                                              
                return llmm;

            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "LatePolicyMasterModel", "GetLatePolicy_GetAll");
                return null;
            }
        }

        public async Task<bool> Save()
        {
            try
            {

                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                var lam = new tbl_LatePolicyMaster();
                lam.LatePolicName = LatePolicyName;
                
                lam.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                lam.CreatedOn = DateTime.Now;
                lam.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                lam.EditedOn = DateTime.Now;
                
                _db.tbl_LatePolicyMaster.Add(lam);
                await _db.SaveChangesAsync();
                LatePolicyId = lam.LatePolicyId;
                return true;
            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "LatePolicyMasterModel", "LatePolicyMaster_Save");
                return false; ;

            }
        }
        public async Task<bool> Delete_LatePolicy(int id)
        {
            try
            {
                PakOman_NedoEntities _db = new PakOman_NedoEntities();
                var late = _db.tbl_LatePolicyMaster.Where(x => x.LatePolicyId == id).FirstOrDefault();
                if (late != null)
                {
                    IList<tbl_LateAttendnaceMaster> lst = _db.tbl_LateAttendnaceMaster.Where(x => x.LatePolicyId == id).ToList();//.FirstOrDefault();
                    foreach(var lm in lst)
                    {
                        _db.tbl_LateAttendnaceMaster.Remove(lm);
                    }
                    await _db.SaveChangesAsync();
                    _db.tbl_LatePolicyMaster.Remove(late);

                    await _db.SaveChangesAsync();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "LatePolicyMasterModel", "DeleteLatePolicy");
                return false;
            }
        }

    }
}