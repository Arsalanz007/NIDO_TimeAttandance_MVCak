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
    public class EmployeeTypeController : Controller
    {
        public async Task<ActionResult> ListAll()
        {
            try
            {
                ModelEmployeeType MD = new ModelEmployeeType();
                var model = await MD.GetEmployeeType();
                return View(model);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "EmployeeTypeController", "ListAll");
                return Content("Something Goes Wrong");
            }
        }
        public ActionResult Create()
        {
            return View();
        }
        public async Task<ActionResult> DeleteEmpType(int id)
        {
            try
            {
                ModelEmployeeType MD = new ModelEmployeeType();
                bool Result = await MD.DeleteEmployeeType(id);
                if (Result == true)
                {
                    return Json(new { Status = "Success", Message = "EmployeeType Successfully Deleted", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Status = "Error", Message = " Sory this EmployeeType can't be deleted because there are Employees maped on this EmployeeType", JsonRequestBehavior.AllowGet });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "EmployeeTypeController", "DeleteEmpType");
                return Json(new { Status = "Success", Message = " Something Went Wrong", JsonRequestBehavior.AllowGet });
            }

        }
        [HttpPost]
        public async Task<ActionResult> SaveEmployeeType(ModelEmployeeType model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await model.EmployeeType_Save())
                    {
                        return Json(new { Status = "Success", Message = "EmployeeType(s) has been successfully saved",URL= "/EmployeeType/ListAll", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "EmployeeType(s) has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "EmployeeTypeController", "SaveEmployeeType");
                return Content("Something Goes Wrong");

            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                ModelEmployeeType MD = new ModelEmployeeType();
                var Result = await MD.Get_EmployeeType_By_Id(id);
                return View(Result);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "EmployeeTypeController", "Edit");
                return Content("Something Goes Wrong");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Update(ModelEmployeeType model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await model.EditEmployeeType())
                    {
                        return Json(new { Status = "Success", Message = "EmployeeType successfully updated.",URL = "/EmployeeType/ListAll" });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "EmployeeType not successfully updated.Please try again" });
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "EmployeeTypeController", "Update");
                return Content("Something Goes Wrong");
            }
        }
    }
}