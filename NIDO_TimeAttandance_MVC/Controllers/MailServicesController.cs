using NIDO_TimeAttandance_MVC.Models;
using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;


namespace NIDO_TimeAttandance_MVC.Controllers
{
    [DashboardSession]
    public class MailServicesController : Controller
    {

        public MailServicesController()
        {
            try
            {
                DataHelper.isWish = true;
          
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "SystemPolicyController", "Constructor");
            }
        }

        public async Task<ActionResult> Index()
        {
            try
            {
                clsEmployeeProfile model = new clsEmployeeProfile();
                if (bool.Parse(HttpContext.Session["SuperAdmin"].ToString()))
                {
                    ViewBag.Employees = await model._GetEmployeePosting();
                }
                else
                {
                    int id = int.Parse(HttpContext.Session["UserId"].ToString());
                    ViewBag.Employees = await model._GetEmployeePosting(id);
                }
                return View();
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "MailServicesController", "Index");
                return Content("Something Goes Wrong ..");
            }
        }
        public ActionResult Generate(string data)
        {
            try
            {
                var empIds = data.Split(',');
                var empIdList = Array.ConvertAll(empIds, s => long.Parse(s));
                DataHelper.emailsList = DataHelper._getEmpEmails(empIdList);
                IList<string> splitIterator = data.Split(',').ToList();
                DataHelper.EmpList = splitIterator.Select(long.Parse).ToArray();
                return RedirectToAction("SendService", "MailServices");
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "MailServicesController", "Generate");
                return Content("Something Goes Wrong ..");
            }


        }
        public ActionResult SendService()
        {
            try
            {
                var emails = DataHelper.emailsList;
                var EmpIDs = DataHelper.EmpList;
                ViewBag.EmpEmails = emails;
                ViewBag.EmpIDs = EmpIDs;
                EmailModelBulk model = new EmailModelBulk();

                return View();
            }
            catch (Exception ex)
            {

               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "MailServicesController", "SendService");
                return Content("Something Goes Wrong ..");
            }
        }
        [ValidateAntiForgeryToken]
        public ActionResult sendMail(EmailModelBulk model)
        {
            try
            {

                if (model.Body == null)
                {
                    model.Body = "";
                }
                model.Body = DataHelper.StripHTML(model.Body);
                model.EmailList = DataHelper.emailsList as List<string>;
                if (model.UploadFiles != null)
                {
                    foreach (var item in model.UploadFiles)
                    {
                        string ext = Path.GetExtension(item.FileName);
                        if (ext != ".pdf" || ext != ".doc" || ext != ".docx" || ext != ".xls" || ext != ".xlsx" || ext != ".jpeg" || ext != ".jpg" || ext != ".png" || ext != ".avi")
                        {
                            return Json(new { Status = "error", Message = "Incorrect File Type" });
                        }
                    }
                }

                // model.Body = model.Body.Replace("<p>", "");
                // model.Body = model.Body.Replace("</p>", "");
                //string response = EmailSending.SendBulkEmail(model.EmailList,model.Subject, model.Body, 5);
                string response = EmailSending.SendBulkEmail(model, 7);
                return Json(new { Status = "success", Message = response });
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "MailServicesController", "sendMail");
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public async Task<ActionResult> SendNotification(List<long> data, string subject, string body)
        {
            try
            {
                body = DataHelper.StripHTML(body);
                await EmailSending.SendBulkNotification(data, subject, body);

                return Json(new { Status = "success", Message = "Success" });
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "MailServicesController", "SendNotification");
                return Json("error", JsonRequestBehavior.AllowGet);
            }
            
        }
        [HttpPost]
        public async Task<ActionResult> SendBoth(List<long> EmpsIDs, List<string> EmailList, string subject, string body)
        {
            try
            {
                body = DataHelper.StripHTML(body);
                await EmailSending.SendBulkNotification(EmpsIDs, subject, body);
                EmailModelBulk model = new EmailModelBulk();
                model.EmailList = EmailList;
                model.Subject = subject;
                model.Body = body;
                string response = EmailSending.SendBulkEmail(model, 6);
                return Json(new { Status = "success", Message = response });
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "MailServicesController", "SendBoth");
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Late()
        {
            try
            {
                var db = new PakOman_NedoEntities();
                ViewBag.Services = (from x in db.EmailServices select x).ToList();
                return View();
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "MailServicesController", "Late");
                return Content("Something Goes Wrong ..");
            }
        }
        [HttpPost]
        public async Task<ActionResult> LateView(string ID, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                clsEmployeeProfile model = new clsEmployeeProfile();
                if (bool.Parse(HttpContext.Session["SuperAdmin"].ToString()))
                {

                    ViewBag.Employees = await model._GetEmployeeLate(ID, FromDate, ToDate);

                }
                else
                {
                    int id = int.Parse(HttpContext.Session["UserId"].ToString());
                    ViewBag.Employees = await model._GetEmployeeLate(id, FromDate, ToDate);

                }
                ViewBag.flag = int.Parse(ID);
                ViewBag.from = FromDate;
                DataHelper.DateFrom = FromDate.ToString("MM/dd/yyyy");
                DataHelper.Dateto = ToDate.ToString("MM/dd/yyyy");
                ViewBag.to = ToDate;

                return PartialView("ReportTable");
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "MailServicesController", "Late");
                return Content("Something Goes Wrong ..");
            }
        }
        [HttpPost]
        public ActionResult LateGenrate(string[] data, string[] empID, string ID)
        {
            string serviceName = "";
            if (ID == "1")
            {
                serviceName = "Late";
            }
            if (ID == "2")
            {
                serviceName = "Absent";
            }
            if (ID == "3")
            {
                serviceName = "Half Day";
            }
            try
            {
                Dictionary<int, string> dict = new Dictionary<int, string>();
                foreach (var item in data)
                {
                    var a = item.Split(',');
                    dict.Add(int.Parse(a[0]), a[1]);

                }
                var empIds = empID;
                var names = Array.ConvertAll(empIds, s => long.Parse(s));
                DataHelper.emailsList = DataHelper._getEmpEmailsWithID(names);
                foreach (var item in DataHelper.emailsList)
                {
                    var a = item.Split(',');
                    EmailSending.SendEmail("Attendace Alert", "Your Numbers Of " + serviceName + " in " + DataHelper.DateFrom + " to " + DataHelper.Dateto + " are " + dict[int.Parse(a[1])], 6, a[0]);
                }
                return Json(new { Status = "success", Message = "Email Sending .." });
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "MailServicesController", "Generate");
                return Content("Something Goes Wrong ..");
            }
        }
        public async Task<ActionResult> ListAll()
        {
            AutoEmail MD = new AutoEmail();
            var model = await MD.GetAutoEmailList();
            return View(model);

        }
        public ActionResult Create()
        {
            try
            {
                ViewBag.EmailName = DataHelper.GetAutoEmailName();
                AutoEmail model = new AutoEmail();
                return View(model);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "MailServicesController", "Save");
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public async Task<ActionResult> Save(AutoEmail model)
        {
            try
            {
                model.EmailTime = Convert.ToDateTime(Request["txttime"]).TimeOfDay.ToString();
                if (await model.Save())
                {
                    return Json(new { Status = "Success", Message = "Saved" });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "Not Saved" });
                }

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "MailServicesController", "Save");
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                ViewBag.EmailName = DataHelper.GetAutoEmailName();
                AutoEmail MD = new AutoEmail();
                var model = await MD.GetAutoEmail(id);
                //string time = model.EmailTime.ToString().Split(' ')[1];
                // model.EmailTime = time;
                return View(model);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "MailServicesController", "Save");
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> Update(AutoEmail model)
        {
            try
            {
                model.EmailTime = Convert.ToDateTime(Request["txttime"]).TimeOfDay.ToString();
                if (await model.Update())
                {
                    return Json(new { Status = "Success", Message = "Saved" });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "Not Saved" });
                }

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "MailServicesController", "Save");
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<ActionResult> Delete(int ID)
        {
            try
            {
                AutoEmail MD = new AutoEmail();
                if (await MD.Delete(ID))
                {
                    return Json(new { Status = "Success", Message = "Delete Successfully" });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "Operation Failed" });
                }


            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "MailServicesController", "Save");
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<ActionResult> UpdateAutoEmail()
        {


            PakOman_NedoEntities db = new PakOman_NedoEntities();
            try
            {

                var setting = await (from x in db.tbl_AutoEmailSetting select x).FirstOrDefaultAsync();
                if (setting.AutoEmailState == false)
                {
                    setting.AutoEmailState = true;
                    await db.SaveChangesAsync();
                    AutoEmail model = new AutoEmail();
                    model.AutoEmailServiceON();
                    Session["AutoEmail"] = setting;
                    return Json(new { status = "success", msg = "Auto Emails Updated", JsonRequestBehavior.AllowGet });


                }
                else
                {
                    setting.AutoEmailState = false;
                    await db.SaveChangesAsync();
                    AutoEmail model = new AutoEmail();
                    model.AutoEmailServiceOFF();
                    Session["AutoEmail"] = setting;
                    return Json(new { status = "success", msg = "Auto Emails Updated", JsonRequestBehavior.AllowGet });
                }
            }
            catch (Exception ex)
            {

                return Json(new { status = "error", msg = "Something Goes Wrong", JsonRequestBehavior.AllowGet });
            }



        }
        public ActionResult SendWishEmail(int WishType, int Empid)
        {
            try
            {
             
                // Wishtype ==1  // birthday email template
                // wishtype==2   // job wish email template
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                var email = DataHelper._getEmpEmails(Empid);
                
                if (email != null)
                {
                    var data = db.WishCheckers.Where(x => x.EmpID == Empid).FirstOrDefault();
                    bool status=false;
                    if(WishType==1)
                    {
                         status = EmailSending.SendEmail("Birthday Boy", "Happy Birthday", 7, email);

                        
                        data.IsEmailBirthday = true;
                    }

                    else
                    {
                         status = EmailSending.SendEmail("Job Anniversary", "Happy J", 8, email);
                        data.IsEmailJob = true;
                    }
                   
                    if(status)
                    {
                        db.SaveChangesAsync();
                        return Json(new { Status = "success", Message = "Successfully Send !" });
                    }
                    return Json(new { Status = 500, Message = "Something went wrong" });
                }
                else
                {
                    return Json(new { Status = 400, Message = "No Email Found " });
                }
                    
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "MailServicesController", "sendMail");
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }
            
        public async Task<ActionResult> SendWishNotification(int WishType, int Empid,string body)
        {// Wishtype ==1  // birthday email template
         // wishtype==2   // job wish email template

            try
            {
                DataHelper.isWish = true;
                DataHelper.isWishDetail = body;

                string subject="";
                bool check = false;
                if(WishType==1)
                {
                   check= await EmailSending.SendNotification(Empid, subject="Happy Birthday", body);
                    DataHelper.isWishTitle = subject;
                   
                   return Json(new { Status = "success", Message = "Success" });
                }
                if(WishType==2)
                {
                    check = await EmailSending.SendNotification(Empid, subject="Happy Job Anniversary", body);
                    DataHelper.isWishTitle = subject;
                    return Json(new { Status = "success", Message = "Success" });
                }
                else
                {
                    return Json(new { Status = "error", Message = "error" });
                }
                
               
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "MailServicesController", "SendNotification");
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }
    }
}
