using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace NIDO_TimeAttandance_MVC.Models
{
    public class AutoEmail
    {
        public int ID { get; set; }
        public int MID { get; set; }
        public string Name { get; set; }
        public string EmailTime { get; set; }
        public DateTime datefrom { get; set; }
        public DateTime DateTo { get; set; }
        public bool isActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Thread Background { get; set; }
        public static Process process { get; set; }

        public async Task<IList<AutoEmail>> GetAutoEmailList()
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    var List = await (from q in _db.tbl_AutoEmailDetail

                                      select new AutoEmail
                                      {
                                          ID = q.ID,
                                          Name = q.tbl_AutoEmailTypes.Name,
                                          isActive = q.isActive.Value,
                                          EmailTime = q.EmailTime.ToString(),
                                          CreatedOn = q.CreatedOn
                                      }
                                            ).ToListAsync();

                    return List;
                }

            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "AutoEmail", "GetAutoEmail");
                return null;
            }


        }
        public async Task<bool> Save()
        {
            try
            {
                var datenow = DateTime.Now;
                if (Convert.ToDateTime(this.EmailTime) < datenow)
                {
                    datenow = datenow.AddDays(1);
                }
                TimeSpan EmlTime = Convert.ToDateTime(this.EmailTime).TimeOfDay;
                DateTime EmailDate = new DateTime(datenow.Year, datenow.Month, datenow.Day, EmlTime.Hours, EmlTime.Minutes, EmlTime.Seconds);
                using (var _db = new PakOman_NedoEntities())
                {
                    _db.tbl_AutoEmailDetail.Add(new tbl_AutoEmailDetail
                    {
                        MID = this.MID,
                        EmailTime = EmlTime,
                        EmailDate = EmailDate,
                        isActive = this.isActive,
                        CreatedOn = DateTime.Now
                    });
                    var result = await _db.SaveChangesAsync();
                    return true;
                }

            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "AutoEmail", "GetAutoEmail");
                return false;
            }


        }
        public async Task<bool> Update()
        {
            try
            {
                var datenow = DateTime.Now;
                if (Convert.ToDateTime(this.EmailTime) < datenow)
                {
                    datenow = datenow.AddDays(1);
                }
                using (var _db = new PakOman_NedoEntities())
                {
                    var data = _db.tbl_AutoEmailDetail.Where(x => x.ID == this.ID).FirstOrDefault();
                    data.MID = this.MID;
                    data.EmailTime = Convert.ToDateTime(this.EmailTime).TimeOfDay;
                    data.EmailDate = datenow;
                    data.isActive = this.isActive;
                    await _db.SaveChangesAsync();
                    return true;
                }

            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "AutoEmail", "GetAutoEmail");
                return false;
            }


        }
        public async Task<AutoEmail> GetAutoEmail(int id)
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    var data = await (from q in _db.tbl_AutoEmailDetail
                                      where q.ID == id
                                      select new AutoEmail
                                      {
                                          ID = q.ID,
                                          MID = q.MID.Value,
                                          Name = q.tbl_AutoEmailTypes.Name,
                                          isActive = q.isActive.Value,
                                          EmailTime = q.EmailTime.Value.ToString(),
                                          CreatedOn = q.CreatedOn
                                      }
                                            ).SingleAsync();
                    return data;
                }

            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "AutoEmail", "GetAutoEmail");
                return null;
            }


        }
        public async Task<bool> Delete(int id)
        {
            try
            {
                using (var _db = new PakOman_NedoEntities())
                {
                    var x = await (from y in _db.tbl_AutoEmailDetail where y.ID == id select y).FirstOrDefaultAsync();
                    _db.tbl_AutoEmailDetail.Remove(x);
                    await _db.SaveChangesAsync();
                    return true;
                }

            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "AutoEmail", "GetAutoEmail");
                return false;
            }


        }
        public void AutoEmailThreadStart()
        {
            if (Background == null)
            {
                Background = new Thread(EmailSending);
                Background.IsBackground = true;
                Background.Start();
            }
        }
        public void EmailSending()
        {
            while (true)
            {
                Thread.Sleep(60000);
            }
        }
        public void AutoEmailThreadStop()
        {
            if (Background != null)
            {
                Background.Abort();
                Background = null;
            }
        }

        public void AutoEmailServiceON()
        {
           

            process = new Process();
            // Redirect the output stream of the child process.
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = false;
            process.StartInfo.RedirectStandardOutput = true;

           // string path = MapPath("/");
            string imagePath = HttpContext.Current.Server.MapPath("~/Email/Debug/AutoEmailService.exe");
            process.StartInfo.FileName = imagePath;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            process.Start();
        }
        public void AutoEmailServiceOFF()
        {
            var service = Process.GetProcessesByName("AutoEmailService");
            if(service.Length!=0)
            {
                foreach (var item in service)
                {
                    item.Kill();
                    item.Dispose();
                }
            }
            //if (process != null)
            //{
            //    process.Kill();
            //    process.Dispose();

            //}
        }

    }
}