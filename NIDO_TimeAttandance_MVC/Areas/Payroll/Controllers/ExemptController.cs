using DocumentFormat.OpenXml.Bibliography;
using NIDO_TimeAttandance_MVC.Areas.Payroll.Models;
using NIDO_TimeAttandance_MVC.Models;
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
    public class ExemptController : Controller
    {
        
        public async Task<ActionResult> Index()
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
            ViewBag.ExemptTypes = DataHelper._getExemptTypes();
            ViewBag.Months = DataHelper._getMonthNames();

            return View();
        }

        public async Task<ActionResult> GetExemptData(string data)
        {

            DataHelper.QuerystringData = data.Split('?');
            return RedirectToAction("ExemptAdjustment", "Exempt", new { area = "Payroll" });
        }

        public async Task<ActionResult> ExemptAdjustment()
        {
            try
            {
                var model = new ExemptModel();
                

                var ExemptList = await model.GetExemptData();
                return View(ExemptList);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ExemptController", "ExemptAdjustment");

                return null;
            }
        }


        public async Task<ActionResult> PostExemptData(List<ExemptModel> data)
        {
            try
            {
                var model = new ExemptModel();
                var result = await model.PostExemptData(data);

                if (result)
                {
                    return Json(new { status = "Success", Message = "Exempt Successfully Saved", URL = "/Payroll/Exempt/Index", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { status = "Error", Message = "Something Goes Wrong", JsonRequestBehavior.AllowGet });
                }

            }
            catch (Exception ex)
            {
                ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ExemptController", "PostExemptData");
                return Json(new
                {
                    status = "Error",
                    Message = "Something Goes Wrong",
                    JsonRequestBehavior.AllowGet
                });
            }

        }
    }
}