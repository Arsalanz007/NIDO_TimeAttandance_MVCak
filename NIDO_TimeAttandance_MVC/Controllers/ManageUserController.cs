using NIDO_TimeAttandance_MVC.Models;
using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace NIDO_TimeAttandance_MVC.Controllers
{
    [DashboardSession]
    public class ManageUserController : Controller
    {
        public ManageUserController()
        {
            try
            {
                ViewBag.Roles = DataHelper._getRoles();
                ViewBag.Employee = DataHelper._getEmployee();
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ManageUserController", "Constructor");
            }
        }
        public async Task<ActionResult> ListAll()
        {
            try
            {
                ModelManageUser MD = new ModelManageUser();
                var model = await MD.GetUsers();
                return View(model);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ManageUserController", "ListALL");
                return Content("Something Goes Wrong");
            }

        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SaveUser(ModelManageUser model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await model.User_Save())
                    {
                        return Json(new { Status = "Success", Message = "User has been successfully saved", URL = "/ManageUser/ListAll", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "User has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ManageUserController", "SaveUser");
                return Content("Something Goes Wrong");
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                ModelManageUser MD = new ModelManageUser();
                var Result = await MD.Get_User_By_Id(id);
                return View(Result);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ManageUserController", "Edit");
                return Content("Something Goes Wrong");
            }
        }
        [HttpPost]
        public async Task<ActionResult> CheckUserName(string UserName, int? UserID)
        {
            try
            {
                UserName = UserName.Trim();
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                bool alreadyExist = false;
                if (UserID != null)
                {
                    var IfExistAccept = await db.UserMasters.Where(x => x.UserName == UserName && x.UserId != UserID).ToListAsync();
                    if (IfExistAccept.Count > 0)
                    {
                        alreadyExist = true;
                    }
                }
                else
                {
                    var IfExist = await db.UserMasters.Where(x => x.UserName == UserName).ToListAsync();
                    if (IfExist.Count > 0)
                    {
                        alreadyExist = true;
                    }
                }

                if (alreadyExist == true)
                {
                    return Json(new { Status = "AlreadyExist", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Status = "NotExist", JsonRequestBehavior.AllowGet });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ManageUserController", "CheckUserName");
                return Content("Something Goes Wrong");
            }
        }
        public async Task<ActionResult> CheckEmp(long EmployeeID)
        {
            try
            {
                PakOman_NedoEntities db = new PakOman_NedoEntities();
                var data = await db.UserMasters.Where(x => x.Empid == EmployeeID).FirstOrDefaultAsync();
                if (data != null)
                {
                    return Json(new { Status = "AlreadyExist", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Status = "NotExist", JsonRequestBehavior.AllowGet });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ManageUserController", "CheckEmp");
                return Content("Something Goes Wrong");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Update(ModelManageUser model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await model.EditUser())
                    {
                        return Json(new { Status = "Success", Message = "User successfully updated.", URL = "/ManageUser/ListAll" });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "User not successfully updated.Please try again" });
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ManageUserController", "Update");
                return Content("Something Goes Wrong");
            }
        }


        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                ModelManageUser MD = new ModelManageUser();
                bool Result = await MD.DeleteUser(id);
                if (Result == true)
                {
                    return Json(new { Status = "Success", Message = "User Successfully Deleted" });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "Sory this User can't be deleted because there are record maped on this User" });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ManageUserController", "DeleteUser");
                return Json(new { Status = "Error", Message = "" });
            }
        }
    }
}