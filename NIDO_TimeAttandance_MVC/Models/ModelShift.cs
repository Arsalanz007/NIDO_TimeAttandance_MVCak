using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;

namespace NIDO_TimeAttandance_MVC.Models
{
    public class ModelShift
    {
        #region Properties
        public int ShiftId { get; set; }
        public string ShiftDesc { get; set; }
        public TimeSpan? TimeIn { get; set; }
        public TimeSpan? TimeOut { get; set; }
        public TimeSpan? ActualIn { get; set; }
        public TimeSpan? ActualOut { get; set; }
        public bool IsHalfDayApplicable { get; set; }
        public TimeSpan? HalfDay { get; set; }
        public bool IsNightShift { get; set; }
        public string CreatedBy { get; set; }
        public string ShiftColor { get; set; }
        public DateTime CreatedOn { get; set; }
        public string EditedBy { get; set; }
        public DateTime EditedOn { get; set; }
        public int? CompanyID { get; set; }
        public string CompanyName { get; set; }
        public TimeSpan? MaxTimeout { get; set; }
        public TimeSpan? AbsentTime { get; set; }
        public TimeSpan? ShiftOverTimeStart { get; set; }
        public TimeSpan? ShiftOverTimeEnd { get; set; }
        public bool IsOverTimeApplicable { get; set; }
        #endregion

        public async Task <IList<ModelShift>> GetShift()
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    IList<ModelShift> List = await (from q in _db.ShiftMasters
                                              orderby q.ShiftDesc ascending
                                              select new ModelShift
                                              {
                                                  ShiftDesc = q.ShiftDesc,
                                                  TimeIn = q.TimeIn,
                                                  TimeOut = q.TimeOut,
                                                  ActualOut = q.ActualOut,
                                                  ActualIn = q.ActualIn,
                                                  CompanyName = q.CompanyMaster.CompanyDesc,
                                                  HalfDay = q.HalfDay,
                                                  IsNightShift = q.IsNightShift,
                                                  IsHalfDayApplicable = q.IsHalfDayApplicable,                                                  
                                                  IsOverTimeApplicable = q.IsOverTimeApplicable,
                                                  ShiftId = q.ShiftId,
                                              }
                                            ).ToListAsync();
                    return List;

                }

            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelShift", "GetShift");
                return null; ;
            }


        }
        public async Task< bool> DeleteShift(int ID)
        {
            PakOman_NedoEntities _db = new PakOman_NedoEntities();
            var data =await _db.ShiftMasters.Where(x => x.ShiftId == ID).FirstOrDefaultAsync();
            try
            {
                if (data != null)
                {
                    _db.ShiftMasters.Remove(data);
                  await  _db.SaveChangesAsync();

                }
                return true;


            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelShift", "DeleteShift");
                return false;

            }
        }
        public async Task < bool> EditShift()
        {
            PakOman_NedoEntities _db = new PakOman_NedoEntities();
            var data =await _db.ShiftMasters.Where(x => x.ShiftId == this.ShiftId).FirstOrDefaultAsync();
            try
            {
                if (data != null)
                {

                    if (IsOverTimeApplicable == true)
                    {
                        data.ShiftOverTimeEnd = ShiftOverTimeEnd;
                        data.ShiftOverTimeStart = ShiftOverTimeStart;
                    }
                    else
                    {
                        data.ShiftOverTimeEnd = null;
                        data.ShiftOverTimeStart = null;
                    }
                    if (IsHalfDayApplicable == true)
                    {
                        data.HalfDay = HalfDay;
                    }
                    else
                    {
                        data.HalfDay = null;
                    }
                    data.ShiftColor = ShiftColor;
                    data.IsOverTimeApplicable = IsOverTimeApplicable;
                    data.IsHalfDayApplicable = IsHalfDayApplicable;
                    data.AbsentTime = AbsentTime;
                    data.ActualOut = ActualOut;
                    data.ActualIn = ActualIn;
                    data.CompanyID = CompanyID;                                      
                    data.IsNightShift = IsNightShift;                    
                    data.MaxTimeout = MaxTimeout;
                    data.ShiftDesc = ShiftDesc;                    
                    data.TimeIn = TimeIn;
                    data.TimeOut = TimeOut;
                    data.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                    data.EditedOn = DateTime.Now;
                   await _db.SaveChangesAsync();

                }
                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelShift", "EditShift");
                return false;

            }
        }
        public async Task < bool> Shift_Save()
        {
            try
            {
                PakOman_NedoEntities _Db = new PakOman_NedoEntities();
                ShiftMaster ObjShift = new ShiftMaster();
                if (IsOverTimeApplicable == true)
                {
                    ObjShift.ShiftOverTimeEnd = ShiftOverTimeEnd;
                    ObjShift.ShiftOverTimeStart = ShiftOverTimeStart;
                }
                else
                {
                    ObjShift.ShiftOverTimeEnd = null;
                    ObjShift.ShiftOverTimeStart = null;
                }
                if (IsHalfDayApplicable == true)
                {
                    ObjShift.HalfDay = HalfDay;
                }
                else
                {
                    ObjShift.HalfDay = null;
                }
                ObjShift.ShiftColor = ShiftColor;
                ObjShift.AbsentTime = AbsentTime;
                ObjShift.ActualOut = ActualOut;
                ObjShift.ActualIn = ActualIn;
                ObjShift.CompanyID = CompanyID;                                
                ObjShift.IsNightShift =IsNightShift;                
                ObjShift.MaxTimeout = MaxTimeout;
                ObjShift.ShiftDesc = ShiftDesc;                
                ObjShift.IsHalfDayApplicable = IsHalfDayApplicable;
                ObjShift.IsOverTimeApplicable = IsOverTimeApplicable;              
                ObjShift.TimeIn = TimeIn;
                ObjShift.TimeOut = TimeOut;
                ObjShift.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                ObjShift.CreatedOn = DateTime.Now;
                _Db.ShiftMasters.Add(ObjShift);
                await _Db.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelShift", "Shift_Save");
                return false;
            }

        }
        public async Task<ModelShift> Get_Shift_By_Id(long Id)
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    ModelShift List =await (from q in _db.ShiftMasters
                                       where q.ShiftId == Id
                                       select new ModelShift
                                       {
                                           AbsentTime = q.AbsentTime,
                                           ActualOut = q.ActualOut,
                                           ActualIn = q.ActualIn,
                                           CompanyID = q.CompanyID,
                                           HalfDay = q.HalfDay,
                                           IsHalfDayApplicable = q.IsHalfDayApplicable,
                                           IsNightShift = q.IsNightShift,
                                           IsOverTimeApplicable = q.IsOverTimeApplicable,
                                           MaxTimeout = q.MaxTimeout,
                                           ShiftDesc = q.ShiftDesc,
                                           ShiftId = q.ShiftId,
                                           ShiftOverTimeEnd = q.ShiftOverTimeEnd,
                                           ShiftOverTimeStart = q.ShiftOverTimeStart,
                                           TimeIn = q.TimeIn,
                                           TimeOut = q.TimeOut,
                                           ShiftColor=q.ShiftColor,
                                       }
                                            ).SingleOrDefaultAsync();
                    return List;
                }
            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelShift", "Get_Shift_By_Id");
                return null;
            }

        }
    }
}