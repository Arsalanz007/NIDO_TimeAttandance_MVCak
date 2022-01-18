using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;

namespace NIDO_TimeAttandance_MVC.Models
{
    public class ModelGazettedHoliday
    {
        public int GazettedHolidayId { get; set; }
        public string GazettedHolidayDesc { get; set; }
        public DateTime GazettedHolidayDate { get; set; }        
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string EditedBy { get; set; }
        public DateTime EditedOn { get; set; }
        public DateTime [] GazettedHolidayDateArray { get; set; }
        public IList<ModelGazettedHoliday> GetGazettedHoliday()
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    IList<ModelGazettedHoliday> List = (from q in _db.GazettedHolidays
                                                        orderby q.GazettedHolidayDate
                                                        select new ModelGazettedHoliday
                                                        {
                                                            GazettedHolidayId = q.GazettedHolidayId,
                                                            GazettedHolidayDesc = q.GazettedHolidayDesc,
                                                            GazettedHolidayDate = q.GazettedHolidayDate
                                                        }
                                            ).ToList();
                    return List;

                }

            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelGazettedHoliday", "GetGazettedHoliday");
                return null; ;
            }


        }
        public async Task< bool> DeleteGazettedHoliday(int ID)
        {
            PakOman_NedoEntities _db = new PakOman_NedoEntities();
            var data = await _db.GazettedHolidays.Where(x => x.GazettedHolidayId == ID).FirstOrDefaultAsync();
            try
            {
                if (data != null)
                {
                     var RefrenceData=await _db.ShiftScheduleGeneralDetails.Where(m => m.AttDate == data.GazettedHolidayDate).ToListAsync();
                    if (RefrenceData != null)
                    {
                        _db.ShiftScheduleGeneralDetails.RemoveRange(RefrenceData);
                       await _db.SaveChangesAsync();
                    }
                    _db.GazettedHolidays.Remove(data);
                   await _db.SaveChangesAsync();

                }
                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelGazettedHoliday", "DeleteGazettedHoliday");
                return false;

            }
        }
        public bool EditGazettedHoliday()
        {
            PakOman_NedoEntities _db = new PakOman_NedoEntities();
            var data = _db.GazettedHolidays.Where(x => x.GazettedHolidayId == this.GazettedHolidayId).FirstOrDefault();
            try
            {
                if (data != null)
                {
                    data.GazettedHolidayDesc = this.GazettedHolidayDesc;
                    data.GazettedHolidayDate = this.GazettedHolidayDate;
                    data.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                    data.EditedOn = DateTime.Now;
                    _db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelGazettedHoliday", "EditGazettedHoliday");
                return false;

            }
        }
        public async Task< bool> GazettedHoliday_Save(string Holidays)
        {
            try
            {
                if (Holidays == "" || Holidays == null || Holidays == string.Empty)
                {
                    Holidays = DateTime.Now.AddYears(-5).ToString();
                }
                string[] HolidaysArry = Holidays.Split(',');
                List<DateTime> dates = HolidaysArry.Select(date => DateTime.Parse(date)).ToList();
                PakOman_NedoEntities _Db = new PakOman_NedoEntities();
                GazettedHoliday ObjGazettedHoliday = new GazettedHoliday();
                for (int i = 0; i < dates.Count; i++)
                {
                    DateTime dateTime = dates[i];
                    var data=await _Db.GazettedHolidays.Where(m => m.GazettedHolidayDate == dateTime).ToListAsync();
                    if (data.Count > 0)
                    {
                        _Db.GazettedHolidays.RemoveRange(data);
                       await _Db.SaveChangesAsync();
                    }
                ObjGazettedHoliday.GazettedHolidayDesc = GazettedHolidayDesc;
                ObjGazettedHoliday.GazettedHolidayDate = dates[i];
                ObjGazettedHoliday.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                ObjGazettedHoliday.CreatedOn = DateTime.Now;
                ObjGazettedHoliday.EditedBy = HttpContext.Current.Session["UserName"].ToString();
                ObjGazettedHoliday.EditedOn = DateTime.Now;
                _Db.GazettedHolidays.Add(ObjGazettedHoliday);           
              await  _Db.SaveChangesAsync();
            }

                return true;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelGazettedHoliday", "GazettedHoliday_Save");
                return false;
            }

        }
        public ModelGazettedHoliday Get_GazettedHoliday_By_Id(long Id)
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    ModelGazettedHoliday List = (from q in _db.GazettedHolidays
                                                 where q.GazettedHolidayId == Id
                                                 select new ModelGazettedHoliday
                                                 {
                                                     GazettedHolidayId = q.GazettedHolidayId,
                                                     GazettedHolidayDesc = q.GazettedHolidayDesc,
                                                     GazettedHolidayDate =  q.GazettedHolidayDate,
                                                 }
                                            ).SingleOrDefault();
                    return List;
                }
            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelGazettedHoliday", "Get_GazettedHoliday_By_Id");
                return null;
            }

        }
    }
}