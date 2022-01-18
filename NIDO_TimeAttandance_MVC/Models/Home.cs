using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;

namespace NIDO_TimeAttandance_MVC.Models
{
    public class Home
    {
        public string GazettedHolidayDesc { get; set; }
        public string HolidayDate { get; set; }
        public string SenderDate { get; set; }
        public string NotificationDetail { get; set; }
        public int Month_Name { get; set; }
        public string NotificationTitle { get; set; }
        public string SeenDateTime { get; set; }
        public string EmpName { get; set; }
        public string EmpImg { get; set; }
        public string Gender { get; set; }



        public async Task<IList<Home>> GetGazetted()
        {
            try
            {
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                IList<Home> data = await db.GazettedHolidays.Where(m => m.GazettedHolidayDate.Year == DateTime.Now.Year).Select(m => new Home
                {
                    GazettedHolidayDesc = m.GazettedHolidayDesc,
                    HolidayDate = m.GazettedHolidayDate.ToString(),
                    Month_Name = m.GazettedHolidayDate.Month
                }).OrderBy(m => m.Month_Name).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "Home", "GetGazetted");
                throw;
            }
        }
        public async Task<IList<Home>> GetNotifications()
        {
            try
            {
                int NotifyFor =int.Parse(HttpContext.Current.Session["UserId"].ToString());
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                string TimeSeen = DateTime.Now.ToString();
                var some = db.tbl_Notifications.Where(x => x.NotifyFor == NotifyFor).ToList();
                some.ForEach(a =>
                { a.IsSeen = true; a.SeenDateTime = TimeSeen; });
                db.SaveChanges();
                IList<Home> data = await (from n in db.tbl_Notifications
                                          join ep in db.EmpMasters on n.CreatedByID equals ep.EmpId
                                          where n.NotifyFor == NotifyFor
                                          orderby n.CreatedDate descending

                                          select new Home
                                          {
                                              SenderDate = n.CreatedDate.ToString(),
                                              NotificationDetail = n.NotificationDetail,
                                              NotificationTitle = n.NotificationTitle,
                                              SeenDateTime = n.SeenDateTime,
                                              EmpName = ep.EmpName,
                                              EmpImg = ep.EmpImg,
                                              Gender = ep.Gender,
                                          }).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {

               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "Home", "GetNotifications");
                throw;
            }
        }
    }
}