using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NIDO_TimeAttandance_MVC.Utilities;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web;
using NIDO_TimeAttandance_MVC.Dtos;
using System.Security.Permissions;

namespace NIDO_TimeAttandance_MVC.Models
{
    public class ModelServices
    {
        public string EventsWeekMsg { get; set; }
        public string wishMsg { get; set; }


        public WishTempalte WishMessage { get; set; }
        public IList<Nstp_GetBirthdayAnd_Aniversory_Result> TodayBirthdays { get; set; }

        public IList<Nstp_GetBirthdayAnd_Aniversory_Result> TodayJobAnniversaries { get; set; }

        public IList<Nstp_GetBirthdayAnd_Aniversory_Result> UpcomingEvents { get; set; }

        public List<WishChecker> WishCheckers { get; set; }



        public async Task<IList<Nstp_GetDailyAttendanceStats_Result>> GetDailyAttendanceStats(int ManagerID)
        {
            try
            {
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                db.Database.CommandTimeout = 10000;
                return await Task.Run(() => db.Nstp_GetDailyAttendanceStats(DateTime.Now, ManagerID).ToList());
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "Home", "GetDailyAttendanceStats");
                throw;
            }
        }

        public async Task<IList<Nstp_GetDailyAttendanceDepartmentWise_Result>> GetDailyAttendanceDepartmentWise(int ManagerID)
        {
            try
            {
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                db.Database.CommandTimeout = 10000;
                return await Task.Run(() => db.Nstp_GetDailyAttendanceDepartmentWise(DateTime.Now, ManagerID).ToList());
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "Home", "GetDailyAttendanceDepartmentWise");
                throw;
            }
        }

        public async Task<RequestDto> GetRequestStatus(int ManagerID)
        {
            try
            {
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                db.Database.CommandTimeout = 10000;
                return await (from r in db.tbl_Request
                              join EP in db.EmpMasters on r.EmpID equals EP.EmpId
                              join M in db.tbl_Manager on EP.DepartmentId equals M.DepartmentID
                              join PM in db.tbl_PendingRequestMaster on r.ID equals PM.RequestID
                              join PD in db.tbl_PendingRequestDetail on PM.ID equals PD.MID
                              where r.IsCancel == false && r.IsRejected == false && M.ManagerID == ManagerID
                              && PD.tblManager_ID == M.ID
                              group PD by r.IsCancel into xg
                              select new RequestDto

                              {
                                  Total = xg.Select(x => x.DID).Count(),
                                  Pending = xg.Where(x => x.ApprovedByThisManager == false).Count(),
                              }).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "Home", "GetRequestStatus");
                throw;
            }
        }

        public async Task<IList<Nstp_GetBirthdayAnd_Aniversory_Result>> GetBirthdayAnd_Aniversory(int DaysCount)
        {
            try
            {
                PakOman_NedoEntities db = new PakOman_NedoEntities();

                return await Task.Run(() => (db.Nstp_GetBirthdayAnd_Aniversory(DaysCount)).ToList());
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "Home", "GetBirthdayAnd_Aniversory");
                throw;
            }
        }

        public IList<Nstp_GetBirthdayAnd_Aniversory_Result> GetTodayEvent(IList<Nstp_GetBirthdayAnd_Aniversory_Result> data)
        {
            var TodayEvent = (from x in data where (Convert.ToDateTime(x.DOB).Day == DateTime.Now.Day && Convert.ToDateTime(x.DOB).Month == DateTime.Now.Month) || (Convert.ToDateTime(x.AppointmentDate).Day == DateTime.Now.Day && Convert.ToDateTime(x.AppointmentDate).Month == DateTime.Now.Month) select x).ToList();
            if (TodayEvent.Count() > 0)
            {
                foreach (var item in TodayEvent)
                {
                    if (Convert.ToDateTime(item.DOB).Day == DateTime.Now.Day && Convert.ToDateTime(item.DOB).Month == DateTime.Now.Month)
                    {
                        item.IsBirthDay = 1;
                    }
                    else
                    {
                        item.IsBirthDay = 0;
                    }
                    if (Convert.ToDateTime(item.AppointmentDate).Day == DateTime.Now.Day && Convert.ToDateTime(item.AppointmentDate).Month == DateTime.Now.Month)
                    {
                        item.IsOfficeBirthDay = 1;
                    }
                    else
                    {
                        item.IsOfficeBirthDay = 0;
                    }
                }
            }
            return TodayEvent;
        }

        public IList<Nstp_GetBirthdayAnd_Aniversory_Result> GetTodayBirthdays(IList<Nstp_GetBirthdayAnd_Aniversory_Result> data)
        {
           
            return (from x in data where Convert.ToDateTime(x.DOB).Day == DateTime.Now.Day && Convert.ToDateTime(x.DOB).Month == DateTime.Now.Month select x).ToList(); 
        }

        public IList<Nstp_GetBirthdayAnd_Aniversory_Result> GetTodayJobAnniversaries(IList<Nstp_GetBirthdayAnd_Aniversory_Result> data)
        {
            return (from x in data where Convert.ToDateTime(x.AppointmentDate).Day == DateTime.Now.Day && Convert.ToDateTime(x.AppointmentDate).Month == DateTime.Now.Month select x).ToList();
        }

        public async Task<ModelServices> GetCompleteBirthdayAndAniversaryData()
        {
            try
            {
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                db.Database.CommandTimeout = 10000;
                #region Birthday and Aniversary
                var DaysCount = 120;
                var data = await GetBirthdayAnd_Aniversory(DaysCount); //await Task.Run(() => (db.Nstp_GetBirthdayAnd_Aniversory(DaysCount)).ToList());



                //var TodayEvent = (from x in data where (Convert.ToDateTime(x.DOB).Day == DateTime.Now.Day && Convert.ToDateTime(x.DOB).Month == DateTime.Now.Month) || (Convert.ToDateTime(x.AppointmentDate).Day == DateTime.Now.Day && Convert.ToDateTime(x.AppointmentDate).Month == DateTime.Now.Month) select x).ToList();
                //if (TodayEvent.Count() > 0)
                //{
                //    foreach (var item in TodayEvent)
                //    {
                //        if (Convert.ToDateTime(item.DOB).Day == DateTime.Now.Day && Convert.ToDateTime(item.DOB).Month == DateTime.Now.Month)
                //        {
                //            item.IsBirthDay = 1;
                //        }
                //        else
                //        {
                //            item.IsBirthDay = 0;
                //        }
                //        if (Convert.ToDateTime(item.AppointmentDate).Day == DateTime.Now.Day && Convert.ToDateTime(item.AppointmentDate).Month == DateTime.Now.Month)
                //        {
                //            item.IsOfficeBirthDay = 1;
                //        }
                //        else
                //        {
                //            item.IsOfficeBirthDay = 0;
                //        }
                //    }
                //}

                var TodayEvent = GetTodayEvent(data);
                var TodayBirthdays = GetTodayBirthdays(data); //(from x in data where Convert.ToDateTime(x.DOB).Day == DateTime.Now.Day && Convert.ToDateTime(x.DOB).Month == DateTime.Now.Month select x).ToList();
                var TodayJobAnniversaries = GetTodayJobAnniversaries(data);  //(from x in data where Convert.ToDateTime(x.AppointmentDate).Day == DateTime.Now.Day && Convert.ToDateTime(x.AppointmentDate).Month == DateTime.Now.Month select x).ToList();

                if (TodayEvent.Count() > 0)
                {
                    var result = await db.WishCheckers.Where(x => x.ActionDate == DateTime.Today).ToListAsync();
                    var WishStatus = result.Where(x => x.ActionDate == DateTime.Now.Date).ToList();
                    if (WishStatus.Count() == 0)
                    {

                        db.Database.ExecuteSqlCommand("TRUNCATE TABLE [WishChecker]");
                        if (TodayEvent.Count() > 0)
                        {
                            foreach (var item in TodayEvent)
                            {
                                WishChecker obj = new WishChecker();
                                obj.EmpID = (int)item.EmpId;
                                obj.EmpImg = item.EmpImg;
                                obj.EmpName = item.EmpName;
                                obj.Gender = item.Gender;

                                obj.ActionDate = DateTime.Today;
                                if (item.IsBirthDay == 1)
                                {
                                    obj.Age = item.Age;
                                    obj.isBirthday = true;
                                    obj.IsEmailBirthday = false;
                                    obj.IsNotiBirthday = false;
                                }
                                else
                                {
                                    obj.isBirthday = false;
                                }
                                if (item.IsOfficeBirthDay == 1)
                                {
                                    obj.OfficeAge = item.OfficeAge;
                                    obj.isJob = true;
                                    obj.IsEmailJob = false;
                                    obj.isNotiJob = false;
                                }
                                else
                                {
                                    obj.isJob = false;
                                }
                                db.WishCheckers.Add(obj);
                                await db.SaveChangesAsync();
                            }

                        }
                    }
                }

                var UpcomingEvents = (from x in data
                                      orderby (x.DOB, x.AppointmentDate) ascending
                                      where
                                      ((Convert.ToDateTime(x.DOB).Day != DateTime.Now.Day && Convert.ToDateTime(x.DOB).Month != DateTime.Now.Month))
                                      || (Convert.ToDateTime(x.AppointmentDate).Day != DateTime.Now.Day && Convert.ToDateTime(x.AppointmentDate).Month != DateTime.Now.Month)


                                      //&& (Convert.ToDateTime(x.DOB).Day != DateTime.Now.Day && Convert.ToDateTime(x.DOB).Month != DateTime.Now.Month) &&
                                      //(Convert.ToDateTime(x.AppointmentDate).Day != DateTime.Now.Day &&
                                      //Convert.ToDateTime(x.AppointmentDate).Month != DateTime.Now.Month)

                                      select x).ToList();
                DaysCount = 6;
                var EventsThisWeek = await GetBirthdayAnd_Aniversory(DaysCount);    //await Task.Run(() => (db.Nstp_GetBirthdayAnd_Aniversory(DaysCount)).ToList());
                int count = EventsThisWeek.Count();
                string noti = "";
                if (TodayEvent.Count() == 0)
                {
                    if (count == 0)
                    {
                        noti = "You dont have any events in Upcoming Days";
                    }
                    if (count == 1)
                    {
                        var reslut = EventsThisWeek.FirstOrDefault();
                        noti = reslut.EmpName + " Has " + ((reslut.IsBirthDay == 1 && reslut.IsOfficeBirthDay == 1) ? " Birthday and Job Anniversary " : (reslut.IsOfficeBirthDay == 1) ? " Job Anniversary " : " Birthday ") + " This Week ";
                    }
                    if (count > 1)
                    {
                        var reslut = EventsThisWeek.FirstOrDefault();
                        noti = reslut.EmpName + " and " + (EventsThisWeek.Count() - 1) + " other has Events this week ";
                    }
                }
                else
                {

                    if (TodayEvent.Count() == 1)
                    {
                        var reslut = TodayEvent.FirstOrDefault();
                        noti = reslut.EmpName + " Has " + ((Convert.ToDateTime(reslut.DOB).Day == DateTime.Now.Day && Convert.ToDateTime(reslut.DOB).Month == DateTime.Now.Month) && (Convert.ToDateTime(reslut.AppointmentDate).Day == DateTime.Now.Day && Convert.ToDateTime(reslut.AppointmentDate).Month == DateTime.Now.Month) ? " Birthday and Job Anniversary " : (Convert.ToDateTime(reslut.AppointmentDate).Day == DateTime.Now.Day && Convert.ToDateTime(reslut.AppointmentDate).Month == DateTime.Now.Month) ? " Job Anniversary " : " Birthday ") + " Today ";
                    }
                    if (TodayEvent.Count() > 1)
                    {
                        var reslut = TodayEvent.FirstOrDefault();
                        noti = reslut.EmpName + " and " + (TodayEvent.Count() - 1) + " other has Birthday Today ";
                    }
                }

                var WishMessage = await db.WishTempaltes.FirstOrDefaultAsync();
                string wishMsg = "no";


                var birthdayAndAniversary = new ModelServices
                {
                    EventsWeekMsg = noti,
                    wishMsg = wishMsg,
                    WishMessage = WishMessage,
                    TodayJobAnniversaries = TodayJobAnniversaries,
                    UpcomingEvents = UpcomingEvents,
                    TodayBirthdays = TodayBirthdays,
                    WishCheckers = await db.WishCheckers.Where(x => x.ActionDate == DateTime.Today).ToListAsync()
            };




                return birthdayAndAniversary;

           



                #endregion
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "Home", "GetBirthdayAndAniversary");
                throw;
            }
        }
    }
}