using NIDO_TimeAttandance_MVC.Areas.Payroll.Models;
using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Controllers
{
    [DashboardSession]
    public class PayrollOptionController : Controller
    {
        // GET: Payroll/PayrollOption
        [DashboardSession]
        public async Task<ActionResult> Index()
        {
            ViewBag.CalulationMethod = PayrollOptionModel.GetPayCalculationMethod();
            ViewBag.PayScdeule= PayrollOptionModel.GetPaySchedule();

            
            var model = await PayrollOptionModel.GetPayrollOption();
            
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> Save(PayrollOptionModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await model.Save_PayrollOption())
                    {
                        return Json(new { Status = "Success", Message = "Payroll Option has been successfully saved", URL = "/Payroll/PayrollOption/Index", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Payroll Option has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "PayrollOptionController", "Save");
                return Content("Something Goes Wrong");
            }
        }
    }
}