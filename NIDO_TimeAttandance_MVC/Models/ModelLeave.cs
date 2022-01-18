using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;

namespace NIDO_TimeAttandance_MVC.Models
{
    public class ModelLeave
    {
        public int ID { get; set; }
        public string LeaveCode { get; set; }
        public string LeaveDsc { get; set; }
        public int NoOfDays { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string EditedBy { get; set; }
        public string EditedOn { get; set; }
        public string LeaveColor { get; set; }
        public bool IscarriedForward { get; set; }
        public int? CarryForwardyearsCount { get; set; }
        public int? HowManyCr_Frwd { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }



        public async Task<IList<ModelLeave>> GetLeave()
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    IList<ModelLeave> List = await (from q in _db.LeaveSetups
                                                    orderby q.LeaveDsc
                                                    select new ModelLeave
                                                    {
                                                        LeaveDsc = q.LeaveDsc,
                                                        CarryForwardyearsCount = q.CarryForwardyearsCount,
                                                        HowManyCr_Frwd = q.HowManyCr_Frwd,
                                                        ID = q.ID,
                                                        IscarriedForward = q.IscarriedForward,
                                                        LeaveCode = q.LeaveCode,
                                                        NoOfDays = q.NoOfDays,
                                                    }
                                            ).ToListAsync();
                    return List;

                }

            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelLeave", "GetLeave");
                return null; ;
            }


        }

        public async Task<bool> Leave_Save()
        {
            try
            {
                PakOman_NedoEntities _Db = new PakOman_NedoEntities();
                LeaveSetup ObjLeave = new LeaveSetup();
                ObjLeave.CarryForwardyearsCount = CarryForwardyearsCount;
                ObjLeave.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                ObjLeave.CreatedOn = DateTime.Now.ToString();
                ObjLeave.HowManyCr_Frwd = HowManyCr_Frwd;
                ObjLeave.IscarriedForward = IscarriedForward;
                ObjLeave.LeaveCode = LeaveCode;
                ObjLeave.LeaveDsc = LeaveDsc;
                ObjLeave.NoOfDays = NoOfDays;
                ObjLeave.Color = LeaveColor;
                _Db.LeaveSetups.Add(ObjLeave);
                await _Db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelLeave", "Leave_Save");
                return false;
            }

        }
        public async Task<bool> DeleteLeave(int ID)
        {
            PakOman_NedoEntities _db = new PakOman_NedoEntities();
            var data = await _db.LeaveSetups.Where(x => x.ID == ID).FirstOrDefaultAsync();
            try
            {
                if (data != null)
                {
                    _db.LeaveSetups.Remove(data);
                    await _db.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelLeave", "DeleteLeave");
                return false;

            }
        }
        public async Task<ModelLeave> Get_Leave_By_Id(long Id)
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    ModelLeave List = await (from q in _db.LeaveSetups
                                             where q.ID == Id
                                             select new ModelLeave
                                             {
                                                 CarryForwardyearsCount = q.CarryForwardyearsCount,
                                                 HowManyCr_Frwd = q.HowManyCr_Frwd,
                                                 IscarriedForward = q.IscarriedForward,
                                                 LeaveCode = q.LeaveCode,
                                                 ID = q.ID,
                                                 LeaveDsc = q.LeaveDsc,
                                                 NoOfDays = q.NoOfDays,
                                                 LeaveColor = q.Color,
                                             }
                                            ).SingleOrDefaultAsync();
                    return List;
                }
            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelLeave", "Get_Leave_By_Id");
                return null;
            }

        }
        public async Task<bool> EditLeave()
        {
            PakOman_NedoEntities _db = new PakOman_NedoEntities();
            var data = await _db.LeaveSetups.Where(x => x.ID == this.ID).FirstOrDefaultAsync();
            try
            {
                if (data != null)
                {
                    data.CarryForwardyearsCount = this.CarryForwardyearsCount;
                    data.HowManyCr_Frwd = this.HowManyCr_Frwd;
                    data.IscarriedForward = IscarriedForward;
                    data.LeaveCode = this.LeaveCode;
                    data.LeaveDsc = this.LeaveDsc;
                    data.NoOfDays = this.NoOfDays;
                    data.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                    data.Color = LeaveColor;
                    data.EditedOn = DateTime.Now.ToString();
                    await _db.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelLeave", "EditLeave");
                return false;

            }
        }
    }
}