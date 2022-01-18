using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Threading.Tasks;

namespace NIDO_TimeAttandance_MVC.Models
{
    public class ModelRoaster
    {
        public int DID { get; set; }
        public DateTime AttDate { get; set; }
        public string AttDay { get; set; }
        public bool IsHoliday { get; set; }
        public string HolidayType { get; set; }
        public int MID { get; set; }
        public DateTime Startdate { get; set; }
        public DateTime EndDate { get; set; }
        public int PKId { get; set; }
        public string ScheduleCode { get; set; }
        public string ScheduleDesc { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string EditedBy { get; set; }
        public string EditedOn { get; set; }
        public int ShiftID { get; set; }
        public string color { get; set; }
        public string BarColor { get; set; }
        public string Remarks { get; set; }
        public string ShiftName { get; set; }

        public async Task<bool> Roaster_Save(string Holidays, string Shiftdays)
        {
            try
            {
                #region Master
                if (Holidays == "" || Holidays == null || Holidays == string.Empty)
                {
                    Holidays = DateTime.Now.AddYears(-5).ToString();
                }
                string[] HolidaysArry = Holidays.Split(',');
                List<DateTime> dates = HolidaysArry.Select(date => DateTime.Parse(date)).ToList();

                if (Shiftdays == "" || Shiftdays == null || Shiftdays == string.Empty)
                {
                    Shiftdays = new DateTime().ToString();
                }
                string[] shiftdaysArry = Shiftdays.Split(',');
                 List<DateTime> shiftDates = shiftdaysArry.Where(c => c != new DateTime().ToString()).Select(date => DateTime.Parse(date)).ToList();
                // List<DateTime> shiftDates = shiftdaysArry.Where(c => DateTime.ParseExact(c , "MM/dd/yyyy", null)).ToList();


               
                PakOman_NedoEntities _Db = new PakOman_NedoEntities();
                int MID = 0;
                if (PKId > 0)
                {
                    MID = PKId;
                    var DeleteData = await Task.Run(() => _Db.ShiftScheduleGeneralDetails.Where(m => m.AttDate >= Startdate && m.AttDate <= EndDate && m.MID == PKId).ToList());
                    if (DeleteData != null)
                    {
                        _Db.ShiftScheduleGeneralDetails.RemoveRange(DeleteData);
                        await _Db.SaveChangesAsync();

                    }

                    if (shiftDates.Count > 0)
                    {
                        var deleteShiftData = await Task.Run(() => _Db.ShiftScheduleGeneralDetails.Where(c => shiftDates.Contains(c.AttDate)).ToList());
                        if (deleteShiftData != null && deleteShiftData.Count > 0)
                        {
                            _Db.ShiftScheduleGeneralDetails.RemoveRange(deleteShiftData);
                            await _Db.SaveChangesAsync();

                        }
                    }

                    var data = await _Db.ShiftScheduleGeneralMasters.Where(x => x.PKId == this.PKId).FirstOrDefaultAsync();
                    data.ScheduleDesc = ScheduleDesc;
                    data.ScheduleCode = ScheduleCode;
                    await _Db.SaveChangesAsync();
                }
                else
                {
                    ShiftScheduleGeneralMaster ObjRoaster = new ShiftScheduleGeneralMaster();
                    ObjRoaster.ScheduleCode = ScheduleCode;
                    ObjRoaster.ScheduleDesc = ScheduleDesc;
                    ObjRoaster.CreatedBy = HttpContext.Current.Session["UserName"].ToString();
                    ObjRoaster.CreatedOn = DateTime.Now.ToString();
                    _Db.ShiftScheduleGeneralMasters.Add(ObjRoaster);
                    await _Db.SaveChangesAsync();
                    MID = ObjRoaster.PKId;
                }
                #endregion

                var dateRange = new List<DateTime>();
                ShiftScheduleGeneralDetail objRoasterD = new ShiftScheduleGeneralDetail();
                for (var dt = Startdate; dt <= EndDate; dt = dt.AddDays(1))
                {

                    dateRange.Add(dt.Date);
                    objRoasterD.AttDate = dt.Date;
                    objRoasterD.AttDay = dt.DayOfWeek.ToString();
                    bool exist = dates.Any(d => d.Date == dt.Date);
                    if (exist == true)
                    {
                        objRoasterD.IsHoliday = true;
                        objRoasterD.HolidayType = "H";
                        objRoasterD.BarColor = "#0088cc";
                        objRoasterD.Remarks = "Holiday";
                    }
                    else
                    {
                        IList<ShiftMaster> Objshift = await Task.Run(() => _Db.ShiftMasters.Where(m => m.ShiftId == ShiftID).ToList());
                        objRoasterD.BarColor = Objshift.Select(x => x.ShiftColor).First();
                        objRoasterD.IsHoliday = false;
                        objRoasterD.HolidayType = " ";
                        objRoasterD.Remarks = Objshift.Select(x => x.ShiftDesc).First();
                    }
                    objRoasterD.MID = MID;
                    objRoasterD.ShiftID = ShiftID;
                    _Db.ShiftScheduleGeneralDetails.Add(objRoasterD);
                    await _Db.SaveChangesAsync();
                }

                foreach (var item in shiftDates)
                {
                    if (!dateRange.Contains(item.Date))
                    {
                        var objRoasterD1 = new ShiftScheduleGeneralDetail();

                        objRoasterD1.AttDate = item;
                        objRoasterD1.AttDay = item.DayOfWeek.ToString();
                        //bool exist = dates.Any(d => d.Date == item.Date);
                        //if (exist == true)
                        //{
                        //objRoasterD1.IsHoliday = true;
                        //objRoasterD1.HolidayType = "H";
                        //objRoasterD1.BarColor = "#0088cc";
                        //objRoasterD1.Remarks = "Holiday";
                        //}
                        //else
                        //{
                        IList<ShiftMaster> Objshift = await Task.Run(() => _Db.ShiftMasters.Where(m => m.ShiftId == ShiftID).ToList());
                        objRoasterD1.BarColor = Objshift.Select(x => x.ShiftColor).First();
                        objRoasterD1.IsHoliday = false;
                        objRoasterD1.HolidayType = " ";
                        objRoasterD1.Remarks = Objshift.Select(x => x.ShiftDesc).First();
                        // }
                        objRoasterD1.MID = MID;
                        objRoasterD1.ShiftID = ShiftID;
                        _Db.ShiftScheduleGeneralDetails.Add(objRoasterD1);
                        await _Db.SaveChangesAsync();


                    }
                }
                return true;

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelRoaster", "Roaster_Save");
                return false;
            }

        }
        public async Task<bool> UpdateRoasterName(int id,string sRoasterName)
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    var md = _db.ShiftScheduleGeneralMasters.Where(x => x.PKId == id).FirstOrDefault();
                    if (md == null)
                        return false;

                    md.ScheduleDesc = sRoasterName;
                    await _db.SaveChangesAsync();
                    return true;

                }

            }

            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelRoaster", "UpdateRoasterName");
                return false; ;
            }


        }
        public async Task<IList<ModelRoaster>> GetRoaster()
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    IList<ModelRoaster> List = await (from q in _db.ShiftScheduleGeneralMasters
                                                      join d in _db.ShiftScheduleGeneralDetails
      on q.PKId equals d.MID
                                                      orderby q.ScheduleCode ascending
                                                      select new ModelRoaster
                                                      {
                                                          PKId = q.PKId,
                                                          ScheduleCode = q.ScheduleCode,
                                                          ScheduleDesc = q.ScheduleDesc,
                                                      }
                                            ).Distinct().ToListAsync();
                    return List;

                }

            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelRoaster", "GetRoaster");
                return null; ;
            }


        }

        public ModelRoaster Get_Roaster_By_Id(long Id)
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    ModelRoaster List = (from q in _db.ShiftScheduleGeneralMasters
                                         where q.PKId == Id
                                         select new ModelRoaster
                                         {
                                             PKId = q.PKId,
                                             ScheduleDesc = q.ScheduleDesc,
                                             ScheduleCode = q.ScheduleCode,
                                         }).SingleOrDefault();
                    return List;
                }
            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelRoaster", "Get_Roaster_By_Id");
                return null;
            }

        }
        // commit kue k roaster delete ni krskte kue k ye 4 tables me effect kre ga
        //public async Task<bool> DeleteRoaster(int ID)
        //{
        //    PakOman_NedoEntities _db = new PakOman_NedoEntities();
        //    var DeleteData = await Task.Run(() => _db.ShiftScheduleGeneralDetails.Where(x => x.MID == ID));
        //    try
        //    {
        //        if (DeleteData != null)
        //        {
        //            _db.ShiftScheduleGeneralDetails.RemoveRange(DeleteData);
        //            await _db.SaveChangesAsync();
        //            var Data = await _db.ShiftScheduleGeneralMasters.Where(x => x.PKId == ID).FirstOrDefaultAsync();
        //            if (Data != null)
        //            {
        //                _db.ShiftScheduleGeneralMasters.Remove(Data);
        //                await _db.SaveChangesAsync();
        //            }
        //        }
        //        return true;


        //    }
        //    catch (Exception ex)
        //    {
        //       ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "ModelRoaster", "DeleteRoaster");
        //        return false;

        //    }
        //}



    }
}