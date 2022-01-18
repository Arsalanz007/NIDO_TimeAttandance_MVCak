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
    public class IncomeTaxController : Controller
    {
        // GET: Payroll/IncomeTax
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> ListAll()
        {
            try
            {
                IncomeTaxModel am = new IncomeTaxModel();
                var lst = await am.GetAll();
                return View(lst);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "IncomeTaxController", "ListAll");
                return Content("Something Goes Wrong");
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                IncomeTaxModel am = new IncomeTaxModel();
                var alModel = await am.Get_IncomeTax_By_Id(id);
                return View(alModel);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "IncomeTaxController", "EditIncomeTax");
                return Content("Something Goes Wrong");
            }
        }

        public async Task<ActionResult> Update(IncomeTaxModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await model.EditIncomeTax();
                    if (result == true)
                    {
                        return Json(new { Status = "Success", Message = "Income Tax has been successfully saved", URL = "/Payroll/IncomeTax/ListAll", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Income Tax has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "IncomeTaxController", "UpdateIncomeTax");
                return Content("Something Goes Wrong");
            }
        }
    }
}