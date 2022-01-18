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
    public class AllowancesController : Controller
    {
        // GET: Payroll/Allowances
        public AllowancesController()
        {
            ViewBag.ValueType = DataHelper._getValueTypes();
        }
        public ActionResult Index()
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
                AllowanceModel am = new AllowanceModel();
                var lst = await am.GetAll();
                return View(lst);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "AllowanceController", "ListAll");
                return Content("Something Goes Wrong");
            }
        }
        [HttpPost]
        public async Task<ActionResult> SaveAllowance(AllowanceModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(await model.SaveAllowance())
                    {
                        return Json(new { Status = "Success", Message = "Allowance has been successfully saved", URL = "/Payroll/Allowances/ListAll", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Allowance has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }
            }
            catch(Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "AllowanceController", "SaveAllowance");
                return Content("Something Goes Wrong");
            }

        }
        public async Task<ActionResult> DeleteAllowance(int id)
        {
            try
            {
                AllowanceModel am = new AllowanceModel();
                bool result = await am.DeleteAllowance(id);
                if (result)
                {
                    return Json(new { Status = "Success", Message = "Allowance Successfully Deleted", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "Allowance has not been successfully deleted.Please try again", JsonRequestBehavior.AllowGet });
                }
            }catch(Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "AllowanceController", "DeleteAllowance");
                return Content("Something Goes Wrong");
            }
        }
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                AllowanceModel am = new AllowanceModel();
                var alModel = await am.Get_Allowance_By_Id(id);
                return View(alModel);
            }
            catch(Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "AllowanceController", "EditAllowance");
                return Content("Something Goes Wrong");
            }
        }
       
        public async Task<ActionResult> Update(AllowanceModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await model.EditAllowance();
                    if(result == true)
                    {
                        return Json(new { Status = "Success", Message = "Allowance has been successfully saved", URL = "/Payroll/Allowances/ListAll", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Allowance has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }

            }
            catch(Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "AllowanceController", "UpdateAllowance");
                return Content("Something Goes Wrong");
            }
        }
    }
}