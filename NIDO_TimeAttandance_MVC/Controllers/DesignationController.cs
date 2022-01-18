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
    public class DesignationController : Controller
    {
        public async Task<ActionResult> ListAll()
        {
            try
            {
                ModelDesignation MD = new ModelDesignation();
                var model = await MD.GetDesignation();
                return View(model);
            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "DesignationController", "ListAll");
                return Content("Something Goes Wrong");
            }
        }
        public ActionResult Create()
        {
            return View();
        }

        public async Task<ActionResult> DeleteDesig(int id)
        {
            try
            {
                ModelDesignation MD = new ModelDesignation();
                bool Result = await MD.DeleteDesignation(id);
                if (Result == true)
                {
                    return Json(new { Status = "Success", Message = "Designation Successfully Deleted", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "Sory this Designation can't be deleted because there are Employees maped on this designation", JsonRequestBehavior.AllowGet });
                }
                
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "DesignationController", "DeleteDesig");
                return Json(new { Status = "Error", Message = "Something Went Wrong", JsonRequestBehavior.AllowGet });

            }
        }
        [HttpPost]
        public async Task<ActionResult> SaveDesignation(ModelDesignation model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await model.Designation_Save())
                    {                      
                        return Json(new { Status = "Success", Message = "Designation(s) has been successfully saved",URL= "/Designation/ListAll", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {                       
                        return Json(new { Status = "Error", Message = "Designation(s) has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                { 
                    return Json(new { Status = "Error" });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "DesignationController", "SaveDesignation");
                return Content("Something Goes Wrong");
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                ModelDesignation MD = new ModelDesignation();
                var Result = await MD.Get_Designation_By_Id(id);
                return View(Result);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "DesignationController", "Edit");
                return Content("Something Goes Wrong");
            }
        }
        [HttpPost]
        public async Task<ActionResult> Update(ModelDesignation model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await model.EditDesignation())
                    {                     
                        return Json(new { Status = "Success", Message = "Designation successfully updated.", URL = "/Designation/ListAll" });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Designation not successfully updated.Please try again" });
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "DesignationController", "Update");
                return Content("Something Goes Wrong");
            }
        }

    }
}