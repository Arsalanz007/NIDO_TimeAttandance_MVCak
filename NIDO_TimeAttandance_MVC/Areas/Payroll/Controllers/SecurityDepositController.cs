using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NIDO_TimeAttandance_MVC.Areas.Payroll.Models;
using NIDO_TimeAttandance_MVC.Utilities;
using System.Threading.Tasks;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Controllers
{
    [DashboardSession]
    public class SecurityDepositController : Controller
    {
        public SecurityDepositController()
        {
            ViewBag.ValueType = DataHelper._getValueTypes();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult OpeningBalance()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        public async Task<ActionResult> ListAll()
        {
            try
            {
                SecurityDepositModel am = new SecurityDepositModel();
                var lst = await am.GetAll();
                return View(lst);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "SecurityDepositController", "ListAll");
                return Content("Something Goes Wrong");
            }
        }
        [HttpPost]
        public async Task<ActionResult> SaveSecurityDeposit(SecurityDepositModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await model.SaveSecurityDeposit())
                    {
                        return Json(new { Status = "Success", Message = "SecurityDeposit has been successfully saved", URL = "/Payroll/SecurityDeposit/ListAll", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "SecurityDeposit has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "SecurityDepositController", "SaveSecurityDeposit");
                return Content("Something Goes Wrong");
            }

        }
        public async Task<ActionResult> DeleteSecurityDeposit(int id)
        {
            try
            {
                SecurityDepositModel am = new SecurityDepositModel();
                bool result = await am.DeleteSecurityDeposit(id);
                if (result)
                {
                    return Json(new { Status = "Success", Message = "SecurityDeposit Successfully Deleted", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "SecurityDeposit has not been successfully deleted.Please try again", JsonRequestBehavior.AllowGet });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "SecurityDepositController", "DeleteSecurityDeposit");
                return Content("Something Goes Wrong");
            }
        }
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                SecurityDepositModel am = new SecurityDepositModel();
                var alModel = await am.Get_SecurityDeposit_By_Id(id);
                return View(alModel);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "SecurityDepositController", "EditSecurityDeposit");
                return Content("Something Goes Wrong");
            }
        }

        public async Task<ActionResult> Update(SecurityDepositModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await model.EditSecurityDeposit();
                    if (result == true)
                    {
                        return Json(new { Status = "Success", Message = "SecurityDeposit has been successfully saved", URL = "/Payroll/SecurityDeposit/ListAll", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "SecurityDeposit has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "SecurityDepositController", "UpdateSecurityDeposit");
                return Content("Something Goes Wrong");
            }
        }
    }
}