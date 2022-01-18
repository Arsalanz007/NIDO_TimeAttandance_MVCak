using NIDO_TimeAttandance_MVC.Models;
using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using NIDO_TimeAttandance_MVC.Areas.Payroll.Models;

namespace NIDO_TimeAttandance_MVC.Controllers
{
   //[MACRestricted]
    public class LoginController : Controller
    {
       
        public LoginController()
        {
            try
            {
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                DataHelper.IconSession = (tbl_IconControl)(from x in db.tbl_IconControl where x.isActive == true select x).FirstOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult Index()
        {
            PakOman_NedoEntities _db = new PakOman_NedoEntities();


           

            Session["Icon"] = DataHelper.IconSession;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Validate(ModelLogin model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    PakOman_NedoEntities db = new PakOman_NedoEntities();
                    if (await model.CheckLogin())
                    {
                       
                        NotificationComponents NC = new NotificationComponents();
                        NC.RegisterNotification(long.Parse(Session["UserId"].ToString()));

                       //Task.Run(()=> CheckEmpYear());

                        string url = "/Home/Index";
                        if (bool.Parse(HttpContext.Session["SuperAdmin"].ToString()) == false)
                        {
                            url = "/User/UserHome/Index/";
                            DataHelper.SuperAdmin = false;
                        }
                        else
                        {
                            DataHelper.SuperAdmin = true;

                            Session["Policies"] =  db.SysPolicies.FirstOrDefault();
                            Session["AutoEmail"] = db.tbl_AutoEmailSetting.FirstOrDefault();


                          



                        }
                        return Json(new { Status = "Success", Message = "Logged In Successfull", URL = url });

                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Plese Enter valid credentials." });
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "LoginController", "Validate");
                return Json(new { Status = "Error", Message = "Exception Occur Something Goes Wrong" });
            }
        }


        public async Task<ActionResult> Logout()
        {
            try
            {
                ModelLogin model = new ModelLogin();
                if (await model.Logout())
                {
                    return Redirect("/Login/Index");
                }
                return View();
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "LoginController", "Logout");
                return Json(new { Status = "Error", Message = "Exception Occur Something Goes Wrong" });
            }
        }
        public async Task<ActionResult> ChangePassword(string txtPassword)
        {
            try
            {
                txtPassword = EncryptDecrypt.EncryptEX(txtPassword, true);
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                int empid = int.Parse(HttpContext.Session["UserId"].ToString());
                var data = await db.UserMasters.Where(m => m.Empid == empid).FirstOrDefaultAsync();
                if (data != null)
                {
                    data.UserPwd = txtPassword;
                    await db.SaveChangesAsync();
                }

                return RedirectToAction("Logout");
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "LoginController", "ChangePassword");
                return Json(new { Status = "Error", Message = "Exception Occur Something Goes Wrong" });
            }
        }

        public async void EmployeeLeave()
        {
            try
            {
                
            }
            catch (Exception ex)
            {

            }
        }
    }
}