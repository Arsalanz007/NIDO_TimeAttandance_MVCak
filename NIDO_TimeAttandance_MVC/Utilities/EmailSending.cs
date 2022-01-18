using NIDO_TimeAttandance_MVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace NIDO_TimeAttandance_MVC.Utilities
{
    public static class EmailSending
    {
        static bool emailCheck =true;
        public static bool SendEmail(string subject, string message, int TemplateID, string EmailAddress,long EmpId = 0)
        {
            try
            {
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                var setting = db.tbl_Setting.Where(x => x.IsActive == true).FirstOrDefault();
                if (setting != null)
                {
                    var Template = db.tbl_Template.Where(x => x.TemplateName == TemplateID).FirstOrDefault();
                    var senderEmail = new MailAddress(setting.SenderEmail, "Attendance System Alert");
                    string[] DisplayName = EmailAddress.Split('@');
                    var receiverEmail = new MailAddress(EmailAddress, DisplayName[0]);
                    string ReceiverName = receiverEmail.User;
                    var emp = new EmpMaster();

                    Template.Discription = Template.Discription.Replace("@User", ReceiverName);
                    Template.Discription = Template.Discription.Replace("@Message", message);

                    if (TemplateID == 7 || TemplateID == 8)
                    {
                        if (EmpId != 0)
                        {
                            emp = db.EmpMasters.Where(c => c.EmpId == EmpId).FirstOrDefault();
                 
                            Template.Discription = Template.Discription.Replace("@EmpName", emp.EmpName);
                            Template.Discription = Template.Discription.Replace("@Department", emp.DepartmentMaster.DepartmentDesc);
                            Template.Discription = Template.Discription.Replace("@Designation", emp.DesignationMaster.DesignationDesc);
                            Template.Discription = Template.Discription.Replace("@AppointmentDate", emp.AppointmentDate);
                        }
                    }


              

                    var password = EncryptDecrypt.DecryptEX(setting.Password, true);
                    var sub = subject;

                    var body = Template.Discription;
                    var smtp = new SmtpClient
                    {
                        Host = setting.Host,
                        Port = setting.Port,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    try
                    {

                        Thread T1 = new Thread(delegate ()
                        {
                            using (var mess = new MailMessage(senderEmail, receiverEmail)
                            {
                                Subject = subject,
                                Body = body,
                                IsBodyHtml = true
                            })
                            {
                                {
                                    smtp.Send(mess);
                                }

                            //return true;
                        }
                        });

                        T1.Start();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Current.Session["UserName"].ToString(), "EmailSending", "SendEmail");
                return false;
            }
        }


        //created by moiez
        
        //public static string SendBulkEmail(List<string> mailingList, string subject, string message, int TemplateID)
         public static string SendBulkEmail(EmailModelBulk model, int TemplateID)

        {
            
            PakOman_NedoEntities db = new PakOman_NedoEntities();
            var setting = db.tbl_Setting.Where(x => x.IsActive == true).FirstOrDefault();
            if (setting != null)
            {
                var Template = db.tbl_Template.Where(x => x.TemplateName == TemplateID).FirstOrDefault();
                var senderEmail = new System.Net.Mail.MailAddress(setting.SenderEmail, "Attendance System Alert");
                var password = EncryptDecrypt.DecryptEX(setting.Password, true);
                Template.Discription = Template.Discription.Replace("@Message", model.Body);
                int chunkSize = 80;
                // Split "mailingList" to multiple lists of "chunkSize" size.
                var mailingChunks = SplitMany(model.EmailList, chunkSize);
                // Process each "mailingChunks" chunk as a separate Task.
                if (emailCheck)
                {
                    IEnumerable<Task> sendMailingChunks = mailingChunks.Select(
                     mailingChunk => Task.Run(() => SendEmails(mailingChunk, model.Subject,model, Template.Discription, setting, senderEmail, password)));

                    // Create a Task that will complete when emails were sent to all the "mailingList".
                    Task sendBuilkEmails = Task.WhenAll(sendMailingChunks);
                    // Displaying the progress of bulk email sending.
                    return "Email Sending..";

                }
                else
                {
                    return "Previous Emails Sending Is in Progress Please Try Again After a While ";
                }
            }
            else
            {
                return "Email Configuration Not Available";
            }

        }
        static void SendEmails(IEnumerable<string> recipients, string subject, EmailModelBulk model, string body, tbl_Setting setting, System.Net.Mail.MailAddress senderMail, string password)
        {
            emailCheck = false;
            emailCheck = false;
            var smtp = new System.Net.Mail.SmtpClient
            {
                Host = setting.Host,
                Port = setting.Port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,

                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderMail.Address, password)
            };



            foreach (var recipient in recipients)
            {
                try
                {

                    body = body.Replace("@User", recipient.Split('@')[0]);

                    System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(senderMail.Address, recipient)
                    {
                        IsBodyHtml = true,
                        Subject = subject,
                       
                        Body = body
                        
                    };
                    if (model.CC != null)
                    {
                        message.CC.Add(model.CC);
                    }
                    if (model.UploadFiles != null)
                    {
                        foreach (var item in model.UploadFiles)
                        {
                            string fileName = Path.GetFileName(item.FileName);
                            Attachment myAttachment = new Attachment(item.InputStream, fileName);
                            message.Attachments.Add(myAttachment);
                        }
                    }
                   
                    smtp.Send(message);
                    Task.Delay(TimeSpan.FromSeconds(5));
                }
                catch (Exception)
                {
                    emailCheck = true;
                    throw;
                }



            }
            emailCheck = true;

        }
        static List<List<string>> SplitMany(List<string> source, int size)
        {
            var sourceChunks = new List<List<string>>();

            for (int i = 0; i < source.Count; i += size)
                sourceChunks.Add(source.GetRange(i, Math.Min(size, source.Count - i)));

            return sourceChunks;
        }

        public static async Task<bool> SendBulkNotification(List<long> mailingList, string subject, string message)
        {
            string SessionUserName = HttpContext.Current.Session["UserName"].ToString();
            string SessionUserId = HttpContext.Current.Session["UserId"].ToString();
            string SessionSuperAdmin = HttpContext.Current.Session["SuperAdmin"].ToString();

            //thread clears the session thats why its require to pass session as variable
            await Task.Run(() =>
               {
                   foreach (var item in mailingList)
                   {
                       DataHelper.InsertNotification(subject, message, "", item, SessionUserName, SessionUserId, SessionSuperAdmin);

                   }

               });
            return true;
        }
        public static async Task<bool> SendNotification(long mailingList, string subject, string message)
        {
            string SessionUserName = HttpContext.Current.Session["UserName"].ToString();
            string SessionUserId = HttpContext.Current.Session["UserId"].ToString();
            string SessionSuperAdmin = HttpContext.Current.Session["SuperAdmin"].ToString();

            //thread clears the session thats why its require to pass session as variable
            await Task.Run(() =>
            {
              
                    DataHelper.InsertNotification(subject, message, "", mailingList, SessionUserName, SessionUserId, SessionSuperAdmin);

              


            });
            return true;
        }


    }


}

