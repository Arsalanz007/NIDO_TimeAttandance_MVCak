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
    [DashboardSession]
    public class LeaveController : Controller
    {       
     
        public async Task<ActionResult> ListAll()
        {
            try
            {
                ModelLeave MD = new ModelLeave();
                var model = await MD.GetLeave();
                return View(model);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "LeaveController", "ListAll");
                return Content("Something Goes Wrong");
            }
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> SaveLeave(ModelLeave model)
        {
            try
            {
               
                if (ModelState.IsValid)
                {
                    if (await model.Leave_Save())
                    {   
                        return Json(new { Status = "Success", Message = "Leave has been successfully saved",URL= "/Leave/ListAll", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {   
                        return Json(new { Status = "Error", Message = "Leave has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "LeaveController", "SaveLeave");
                return Content("Something Goes Wrong");
            }
        }
        public async Task<ActionResult> DeleteLeave(int id)
        {
            try
            {
                ModelLeave MD = new ModelLeave();
                bool Result = await MD.DeleteLeave(id);
                if (Result == true)
                {
                    return Json(new { Status = "Success", Message = " Leave Successfully Deleted", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Status = "Error", Message = " Sory this Leave can't be deleted because they are maped on this System", JsonRequestBehavior.AllowGet });
                }
             
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "LeaveController", "DeleteLeave");
                return Json(new { Status = "Error", Message = " Something Goes Wrong", JsonRequestBehavior.AllowGet });
            }

        }
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                ModelLeave MD = new ModelLeave();
                var Result = await MD.Get_Leave_By_Id(id);
                return View(Result);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "LeaveController", "Edit");
                return Content("Something Goes Wrong");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Update(ModelLeave model)
        {
            try
            {
                
                if (ModelState.IsValid)
                {
                    if (await model.EditLeave())
                    {  
                        return Json(new { Status = "Success", Message = "Leave successfully updated.", URL = "/Leave/ListAll" });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Leave not successfully updated.Please try again" });
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "LeaveController", "Edit");
                return Content("Something Goes Wrong");
            }
        }
    }
}