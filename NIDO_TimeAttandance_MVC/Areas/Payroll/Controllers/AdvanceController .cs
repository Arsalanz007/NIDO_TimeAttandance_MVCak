using System;

using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NIDO_TimeAttandance_MVC.Areas.Payroll.Models;
using System.Threading.Tasks;
using System.Data.Entity;
using NIDO_TimeAttandance_MVC.Utilities;
using NIDO_TimeAttandance_MVC.Models;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Controllers
{
    [DashboardSession]
    public class AdvanceController : Controller
    {
        public AdvanceController()
        {
            ViewBag.Employees = DataHelper._getEmployee();
            ViewBag.PaymentStatus = DataHelper._getPaymentStatus();
        }
        // GET: Payroll/Advance
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> DeleteAdvance(int id)
        {
            try
            {
                AdvanceModel lm = new AdvanceModel();
                bool result = await lm.DeleteAdvance(id);
                if (result)
                {
                    return Json(new { Status = "Success", Message = "Advance Successfully Deleted", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "Advance has not been successfully deleted.Please try again", JsonRequestBehavior.AllowGet });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "AdvanceController", "DeleteAdvance");
                return Content("Something Goes Wrong");
            }
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                AdvanceModel lm = new AdvanceModel();
                
                var lmModel = await lm.Get_Advance_By_Id(id);
                return View(lmModel);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "AdvanceController", "EditAdvance");
                return Content("Something Goes Wrong");
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateAdvanceMaster(int? id ,string description) {

            AdvanceDetailModel obj = new AdvanceDetailModel();

            if (id != null && description != null)
            {
                var updateAdvance = await obj.UpdateAdvanceMaster(description, id);
                if (updateAdvance)
                {
                    return Json(new { Status = "Success", Message = "Advance Successfully Updated", Url = "/Payroll/Advance/ListAll", JsonRequestBehavior.AllowGet });

                }
                else
                {

                    return Json(new { Status = "Error", Message = "Advance has not been successfully Update.Please try again", JsonRequestBehavior.AllowGet });
                }
            }
            else {

                return Json(new { Status = "Error", Message = "Advance has not been successfully Update.Please try again", JsonRequestBehavior.AllowGet });
            }

          
        
        
        }
      
        public async Task<ActionResult> UpdateAdvanceDetail(AdvanceDetailModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await model.EditAdvanceDetail();
                    if (result == true)
                    {
                        return Json(new { Status = "Success", Message = "Advance Installment has been successfully saved", URL = ( "/Payroll/Advance/Edit/" + model.AdvanceId.ToString()), JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Advance Installment has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "AdvanceController", "UpdateAllowance");
                return Content("Something Goes Wrong");
            }
        }
        public async Task<ActionResult> CreateAdvanceDetail(int advanceId)
        {
            try
            {
                var ad = new AdvanceDetailModel();
                ad.AdvanceId = advanceId;
                return View(ad);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "AdvanceController", "CreateAdvanceDetail");
                return Content("Something Goes Wrong");
            }
        }
        public async Task<ActionResult> InsertLoanDetail(AdvanceDetailModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await model.InsertAdvanceDetail();
                    if (result == true)
                    {
                        return Json(new { Status = "Success", Message = "Advance Installment has been successfully saved", URL = ("/Payroll/Advance/Edit/" + model.AdvanceId.ToString()), JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Advance Installment has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "AllowanceController", "UpdateAllowance");
                return Content("Something Goes Wrong");
            }
        }
        public async Task<ActionResult> EditAdvanceDetail(int detailId)
        {
            try
            {
                AdvanceModel lm = new AdvanceModel();

                var lmModel = await lm.Get_Advance_Detail_By_Id(detailId);
                return View(lmModel);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "AdvanceController", "EditAdvanceDetail");
                return Content("Something Goes Wrong");
            }
        }


        [HttpPost]
        public async Task<ActionResult> SaveAdvance(AdvanceModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await model.SaveAdvance())
                    {
                        //return RedirectToAction("Edit", "Advance", new { area = "Payroll" ,id=model.AdvanceId});
                        return Json(new { Status = "Success", Message = "Advance has been successfully saved", URL = "/Payroll/Advance/ListAll", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Laon has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "AdvanceController", "SaveAdvance");
                return Content("Something Goes Wrong");
            }

        }
        public async Task<ActionResult> ListAll()
        {
            AdvanceModel aam = new AdvanceModel();
            IList<AdvanceModel> lst = await aam.GetAll();

            return View(lst);
        }
        public ActionResult Create()
        {


            return View();
        }

    }
}