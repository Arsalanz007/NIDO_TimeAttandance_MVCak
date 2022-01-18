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
    public class RoleController : Controller
    {
        public async Task<ActionResult> ListAll()
        {
            try
            {
                ModelRole MD = new ModelRole();
                var model = await MD.GetRole();
                return View(model);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "RoleController", "ListALL");
                return View();
            }
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> DeleteRole(int id)
        {
            try
            {
                ModelRole MD = new ModelRole();
                bool Result = await MD.DeleteRole(id);
                if (Result == true)
                {
                    return Json(new { Status = "Success", Message = "Role Successfully Deleted", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "Sory this Role can't be deleted because there are Employees maped on this Role", JsonRequestBehavior.AllowGet });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "RoleController", "DeleteRole");
                return Json(new { Status = "Error", Message = "Something Went Wrong", JsonRequestBehavior.AllowGet });
            }
        }
        [HttpPost]
        public async Task<ActionResult> SaveRole(ModelRole model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await model.Role_Save())
                    {
                        return Json(new { Status = "Success", Message = "Role(s) has been successfully saved", URL = "/Role/ListAll", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Role(s) has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "RoleController", "SaveRole");
                return View();
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                ModelRole MD = new ModelRole();
                var Result = await MD.Get_Role_By_Id(id);
                return View(Result);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "RoleController", "Edit");
                return View();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Update(ModelRole model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await model.EditRole())
                    {
                        return Json(new { Status = "Success", Message = "Role successfully updated.", URL = "/Role/ListAll" });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Role not successfully updated.Please try again" });
                    }
                }

                return View(model);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "RoleController", "Update");
                return View();
            }
        }
    }
}