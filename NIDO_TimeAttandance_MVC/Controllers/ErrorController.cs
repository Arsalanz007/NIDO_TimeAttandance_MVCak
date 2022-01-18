using NIDO_TimeAttandance_MVC.Models;
using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NIDO_TimeAttandance_MVC.Controllers
{
    //[DashboardSession]
    public class ErrorController : Controller
    {
        public ActionResult Error_404()
        {
            return View();
        }
        public async Task< ActionResult> LockScreen()
        {
            try
            {
                await Task.Run(() => { 
                if (Session["UserName"].ToString() != "")
                {
                    HttpContext.Session["EmpimgLock"] = Session["EmpImg"].ToString();
                    HttpContext.Session["UserNameLo"] = Session["UserName"].ToString() != null ? Session["UserName"].ToString() : "NEDO";
                    HttpContext.Session["EmpNameLo"] = Session["EmpName"].ToString() != null ? Session["EmpName"].ToString() : "NEDO";
                }
                });
                DataHelper._deleteCookie();
                DataHelper.delete_Session();
                return View();
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ErrorController", "LockScreen");
                return Content("Something Goes Wrong");
            }
        }
        [HttpPost]
        public async Task<ActionResult> Unlock(ModelLogin model, string UserName)
        {
            try
            {
                model.Username = UserName;
                if (await model.CheckLogin())
                {
                    string url = "/Home/Index";
                    if (bool.Parse(HttpContext.Session["SuperAdmin"].ToString()) == false)
                    {
                        url = "/User/UserHome/Index/";
                    }
                    return Json(new { Status = "Success", Message = "Logged In Successfull", URL = url });
                }

                else
                {
                    return Json(new { Status = "Error", Message = "In valid credentials" });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ErrorController", "Unlock");
                return Content("Something Goes Wrong");
            }
        }

    }
}