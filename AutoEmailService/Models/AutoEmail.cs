using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data.Entity;
using AutoEmailService.Utilities;

namespace AutoEmailService.Models
{
    public class AutoEmail
    {
        public int ID { get; set; }
        public int MID { get; set; }
        public string Name { get; set; }
        public Nullable<System.TimeSpan> EmailTime { get; set; }
        public DateTime EmailDate { get; set; }
        public DateTime datefrom { get; set; }
        public DateTime DateTo { get; set; }
        public bool isActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Thread Background { get; set; }


        public async Task<IList<AutoEmail>> GetAutoEmailList()
        {
            try
            {
                using (var _db = new NedoDb())
                {
                    var List = await (from q in _db.tbl_AutoEmailDetail

                                      select new AutoEmail
                                      {
                                          ID = q.ID,
                                          Name = q.tbl_AutoEmailTypes.Name,
                                          isActive = q.isActive.Value,
                                          EmailTime = q.EmailTime.Value,
                                          CreatedOn = q.CreatedOn
                                      }
                                            ).ToListAsync();

                    return List;
                }

            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, "Console App", "AutoEmail", "GetAutoEmail");
                return null;
            }


        }
        public async Task<bool> Save()
        {
            var datenow = DateTime.Now;
            if(this.EmailTime<datenow.TimeOfDay)
            {
                datenow.AddDays(1);
            }
            try
            {
                using (var _db = new NedoDb())
                {
                    _db.tbl_AutoEmailDetail.Add(new tbl_AutoEmailDetail
                    {
                        MID = this.MID,

                        EmailTime = Convert.ToDateTime(this.EmailTime).TimeOfDay,
                        EmailDate = datenow,
                        isActive = this.isActive,
                        CreatedOn = DateTime.Now
                    });
                    var result = await _db.SaveChangesAsync();
                    return true;
                }

            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, "Console App", "AutoEmail", "GetAutoEmail");
                return false;
            }


        }
        public async Task<AutoEmail> GetAutoEmail(int id)
        {
            try
            {
                using (var _db = new NedoDb())
                {
                    var data = await (from q in _db.tbl_AutoEmailDetail
                                      where q.ID == id
                                      select new AutoEmail
                                      {
                                          ID = q.ID,
                                          MID = q.MID.Value,
                                          Name = q.tbl_AutoEmailTypes.Name,
                                          isActive = q.isActive.Value,
                                          EmailTime = q.EmailTime.Value,
                                          CreatedOn = q.CreatedOn
                                      }
                                            ).SingleAsync();
                    return data;
                }

            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, "Console App", "AutoEmail", "GetAutoEmail");
                return null;
            }


        }
        public async Task<bool> Delete(int id)
        {
            try
            {
                using (var _db = new NedoDb())
                {
                    var x = await (from y in _db.tbl_AutoEmailDetail where y.ID == id select y).FirstOrDefaultAsync();
                    _db.tbl_AutoEmailDetail.Remove(x);
                    await _db.SaveChangesAsync();
                    return true;
                }

            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, "Console App", "AutoEmail", "GetAutoEmail");
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

        public void AutoEmailSend()
        {
            using (var _db = new NedoDb())
            {
                var data = (from q in _db.tbl_AutoEmailDetail
                            where q.EmailTime == DateTime.Now.TimeOfDay
                            select new AutoEmail
                            {
                                ID = q.ID,
                                MID = q.MID.Value,
                                Name = q.tbl_AutoEmailTypes.Name,
                                isActive = q.isActive.Value,
                                EmailTime = q.EmailTime.Value,
                                CreatedOn = q.CreatedOn
                            }
                                        ).SingleAsync();

            }

        }
    }
}
