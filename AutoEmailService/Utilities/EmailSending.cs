using AutoEmailService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace AutoEmailService.Utilities
{
    public static class EmailSending
    {
       
        static bool emailCheck =true;
        public static bool SendEmail(string subject, string message, int TemplateID, string EmailAddress)
        {
            try
            {
                NedoDb db = new NedoDb();
                var setting = db.tbl_Setting.Where(x => x.IsActive == true).FirstOrDefault();
                if (setting != null)
                {
                    var Template = db.tbl_Template.Where(x => x.TemplateName == TemplateID).FirstOrDefault();
                    var senderEmail = new MailAddress(setting.SenderEmail, "Attendance System Alert");
                    string[] DisplayName = EmailAddress.Split('@');
                    var receiverEmail = new MailAddress(EmailAddress, DisplayName[0]);
                    string ReceiverName = receiverEmail.User;
                    var password = EncryptDecrypt.DecryptEX(setting.Password, true);
                    var sub = subject;
                    Template.Discription = Template.Discription.Replace("@User", ReceiverName);
                    Template.Discription = Template.Discription.Replace("@Message", message);

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
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, "Console App", "EmailSending", "SendEmail");
                return false;
            }
        }


        
    }


}

