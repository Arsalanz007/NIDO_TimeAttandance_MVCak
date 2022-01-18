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
    public class ProvidentFundController : Controller
    {
        // GET: Payroll/ProvidentFund
        
        public ProvidentFundController()
        {
            ViewBag.ValueType = DataHelper._getValueTypes();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            ViewBag.Categories = EOBIModel.GetCategories();

            return View();
        }
        public async Task<ActionResult> ListAll()
        {
            try
            {
                ProvidentFundModel pm = new ProvidentFundModel();
                var lst = await pm.GetAll();
                return View(lst);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ProvidentFundController", "ListAll");
                return Content("Something Goes Wrong");
            }
        }
        [HttpPost]
        public async Task<ActionResult> SaveProvidentFund(ProvidentFundModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await model.SaveProvidentFund())
                    {
                        return Json(new { Status = "Success", Message = "Provident Fund has been successfully saved", URL = "/Payroll/ProvidentFund/ListAll", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Provident fund has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ProvidentFundController", "SaveProvidentFund");
                return Content("Something Goes Wrong");
            }

        }
        public async Task<ActionResult> DeleteProvidentFund(int id)
        {
            try
            {
                ProvidentFundModel am = new ProvidentFundModel();
                bool result = await am.DeleteProvidentFund(id);
                if (result)
                {
                    return Json(new { Status = "Success", Message = "ProvidentFund Successfully Deleted", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "ProvidentFund has not been successfully deleted.Please try again", JsonRequestBehavior.AllowGet });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ProvidentFundController", "DeleteProvidentFund");
                return Content("Something Goes Wrong");
            }
        }
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                ViewBag.Categories = EOBIModel.GetCategories();
                ProvidentFundModel am = new ProvidentFundModel();
                var alModel = await am.Get_ProvidentFund_By_Id(id);
                return View(alModel);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ProvidentFundController", "EditProvidentFund");
                return Content("Something Goes Wrong");
            }
        }

        public async Task<ActionResult> Update(ProvidentFundModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await model.EditProvidentFund();
                    if (result == true)
                    {
                        return Json(new { Status = "Success", Message = "ProvidentFund has been successfully saved", URL = "/Payroll/ProvidentFund/ListAll", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "ProvidentFund has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ProvidentFundController", "UpdateProvidentFund");
                return Content("Something Goes Wrong");
            }
        }
    }
}