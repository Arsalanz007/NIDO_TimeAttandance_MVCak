using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data.Entity;
using AutoEmailService.Models;
using AutoEmailService.Utilities;
using System.Data.Entity.Core.Objects;


using System.Speech.Synthesis;

namespace AutoEmailService
{
    class Program
    {
        public Thread Background { get; set; }

        public SpeechSynthesizer synth { get; set; }
        public Program()
        {
            synth = new SpeechSynthesizer();
            synth.SetOutputToDefaultAudioDevice();
        }
        static void Main(string[] args)
        {
            Program pp = new Program();

          
            NedoDb db = new NedoDb();
            
            DateTime datetimeNow = DateTime.Now;
            var todaydate = datetimeNow.Date;
            pp.synth.Speak("Auto Email Service is On");
            var OverDueRecords = (from x in db.tbl_AutoEmailDetail where x.EmailDate < datetimeNow select x).ToList();
            foreach (var item in OverDueRecords)
            {
                if (item.EmailTime < datetimeNow.TimeOfDay)
                {
                    item.EmailDate = datetimeNow.Date;
                    item.EmailDate = item.EmailDate.Value.AddDays(1);
                    item.EmailDate = item.EmailDate + item.EmailTime;
                }
                else
                {
                    item.EmailDate = datetimeNow.Date;
                    item.EmailDate = item.EmailDate + item.EmailTime;
                }
            }
            db.SaveChanges();
            
            pp.AutoEmailThreadStart();
            Console.ReadLine();
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
                NedoDb db = new NedoDb();
                DateTime datetimeNow = DateTime.Now;
                var timeofday = datetimeNow.TimeOfDay;
                var todaydate = datetimeNow.Date;

                var x = (from q in db.tbl_AutoEmailDetail
                         where q.EmailDate >= datetimeNow && q.isActive == true
                         orderby q.EmailDate ascending
                         select new AutoEmail
                         {
                             ID = q.ID,
                             MID = q.MID.Value,
                             Name = q.tbl_AutoEmailTypes.Name,
                             EmailTime = q.EmailTime,
                             EmailDate = q.EmailDate.Value,

                         }).FirstOrDefault();
                if (x == null)
                {
                    Console.Beep();
                    Environment.Exit(0);
                    Background.Abort();
                    Environment.Exit(0);
                    var emailSetting = db.tbl_AutoEmailSetting.Select(a => a.AutoEmailState).FirstOrDefault();
                    emailSetting = false;
                    db.SaveChangesAsync();
                }

                DateTime EmailDateTime = new DateTime(x.EmailDate.Year, x.EmailDate.Month, x.EmailDate.Day, x.EmailTime.Value.Hours, x.EmailTime.Value.Minutes, x.EmailTime.Value.Seconds);
                var diff = EmailDateTime - datetimeNow;
                Console.Beep();
                Console.WriteLine("ID:" + x.ID);
                Console.WriteLine("Email DateTime:" + EmailDateTime);
                Console.WriteLine("System Datetime:" + datetimeNow);
                Console.WriteLine("Diff:" + diff);

                Console.WriteLine("thread sleep for milisecounds:" + Convert.ToInt32(diff.TotalMilliseconds));
               
                Thread.Sleep(Convert.ToInt32(diff.TotalMilliseconds));
                Console.Beep();
                Console.WriteLine("=================================");
                try
                {
                    //synth.Speak("Email sending start");
                    AutoEmailSend(x);

                }
                catch (Exception e)
                {

                    throw;
                }
                var data = (from y in db.tbl_AutoEmailDetail where y.ID == x.ID select y).SingleOrDefault();
                data.EmailDate = EmailDateTime.AddDays(1);
                db.SaveChanges();
               // synth.Speak("Another Thread Initialize");

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
        public  void AutoEmailSend(AutoEmail model)
        {



            if (model.MID == 1)//Late
            {
                clsEmployeeProfile emmpProf = new clsEmployeeProfile();
                var data =  emmpProf._GetEmployeeLate("1", DateTime.Today.AddDays(-3), DateTime.Today.AddDays(1));
                Dictionary<long, int> dict = new Dictionary<long, int>();
                foreach (var emp in data)
                {
                    dict.Add(emp.EmpID, emp.NoOflates);
                }
                var empids = (from q in data select q.EmpID).ToArray();
                DataHelper.emailsList = DataHelper._getEmpEmailsWithID(empids);
                emailSend(dict, DateTime.Today.AddDays(-3).ToString(), DateTime.Today.AddDays(-3).AddMilliseconds(-1).ToString(), DataHelper.emailsList, "Late");
            }


        }
        public void emailSend(Dictionary<long, int> dict, string datefrom, string dateTo, IList<string> emLList, string service)
        {

            foreach (var item in emLList)
            {

                var a = item.Split(',');
                Console.WriteLine("================Email Sending to {0}) ", a[0]);
                
                Utilities.EmailSending.SendEmail("Attendace Alert", "Your Numbers Of " + service + " in " + datefrom + " to " + dateTo + " are " + dict[int.Parse(a[1])], 6, a[0]);
               // synth.Speak("Email Send To "+a[0]);
                Thread.Sleep(500);
               // Console.Beep();
            }
        }

    }
}
