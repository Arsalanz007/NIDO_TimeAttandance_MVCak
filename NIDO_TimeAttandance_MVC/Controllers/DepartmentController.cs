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
    public class DepartmentController : Controller
    {
        public async Task<ActionResult> ListAll()
        {
            try
            {
                ModelDepartment MD = new ModelDepartment();
                var model = await MD.GetDepartment();
                return View(model);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "DepartmentController", "ListAll");
                return Content("Something Goes Wrong");
            }
        }
        public ActionResult Create()
        {

            return View();
        }
        public async Task<ActionResult> DeleteDepart(int id)
        {
            try
            {

                ModelDepartment MD = new ModelDepartment();
                bool Result = await MD.DeleteDepartment(id);
                if (Result == true)
                {
                    return Json(new { Status = "Success", Message = "Department Successfully Deleted", JsonRequestBehavior.AllowGet });


                }
                else
                {
                    return Json(new { Status = "Error", Message = "Sory this Department can't be deleted because there are Employees maped on this Department", JsonRequestBehavior.AllowGet });


                }

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "DepartmentController", "DeleteDepart");
                return Json(new { Status = "Error", Message = "Something Went Wrong", JsonRequestBehavior.AllowGet });

            }
        }
        [HttpPost]
        public async Task<ActionResult> SaveDepartment(ModelDepartment model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await model.Department_Save())
                    {
                        return Json(new { Status = "Success", Message = "Department(s) has been successfully saved",URL= "/Department/ListAll", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Department(s) has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "DepartmentController", "SaveDepartment");
                return Content("Something Goes Wrong");
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                ModelDepartment MD = new ModelDepartment();
                var Result = await MD.Get_Department_By_Id(id);
                return View(Result);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "DepartmentController", "Edit");
                return Content("Something Goes Wrong");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Update(ModelDepartment model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await model.EditDepartment())
                    {
                        return Json(new { Status = "Success", Message = "Department successfully updated.", URL = "/Department/ListAll" });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Department not successfully updated.Please try again" });
                    }
                }

                return View(model);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "DepartmentController", "Update");
                return Content("Something Goes Wrong");
            }
        }
    }
}