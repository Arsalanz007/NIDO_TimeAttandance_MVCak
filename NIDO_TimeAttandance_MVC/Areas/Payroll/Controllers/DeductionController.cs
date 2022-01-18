using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NIDO_TimeAttandance_MVC.Areas.Payroll.Models;
using System.Threading.Tasks;
using System.Data.Entity;
using NIDO_TimeAttandance_MVC.Utilities;

namespace NIDO_TimeAttandance_MVC.Areas.Payroll.Controllers
{
    [DashboardSession]
    public class DeductionController : Controller
    {
        public DeductionController()
        {
            ViewBag.ValueType = DataHelper._getValueTypes();
        }
        // GET: Payroll/Deduction
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                DeductionModel dm = new DeductionModel();
                var dModel = await dm.Get_Deduction_By_Id(id);
                return View(dModel);

            }catch(Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "DeductionController", "EditDeduction");
                return Content("Something Goes Wrong");
            }
        }
        public async Task<ActionResult> DeleteDeduction(int id)
        {
            try
            {
                DeductionModel dm = new DeductionModel();
                bool result = await dm.DeleteDeduction(id);
                if (result == true)
                {
                    return Json(new { Status = "Success", Message = "Deduction Successfully Deleted", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "Deduction has not been successfully deleted.Please try again", JsonRequestBehavior.AllowGet });
                }


            }catch(Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "DeductionController", "DeleteDeduction");
                return Content("Something Goes Wrong");
            }
        } 
        public async Task<ActionResult> SaveDeduction(DeductionModel model)            
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(await model.SaveDeduction())
                    {
                        return Json(new { Status = "Success", Message = "Deduction has been successfully saved", URL = "/Payroll/Deduction/ListAll", JsonRequestBehavior.AllowGet });

                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Deduction has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }
            }catch(Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "DeductionController", "SaveDeduction");
                return Content("Something Goes Wrong");
            }

        }

        public async Task<ActionResult> Update(DeductionModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await model.EditDeduction();
                    if (result == true)
                    {
                        return Json(new { Status = "Success", Message = "deduction has been successfully saved", URL = "/Payroll/Deduction/ListAll", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Deduction has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "DeductionController", "UpdateDeduction");
                return Content("Something Goes Wrong");
            }
        }
        public async Task<ActionResult> ListAll()
        {
            try
            {
                DeductionModel dm = new DeductionModel();
                IList<DeductionModel> lst = await dm.GetAll();
                return View(lst);

            }catch(Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "DeductionController", "ListAllDeduction");
                return Content("Something Goes Wrong");
            }
        }
        
    }
}