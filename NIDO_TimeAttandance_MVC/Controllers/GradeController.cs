using NIDO_TimeAttandance_MVC.Models;
using NIDO_TimeAttandance_MVC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;

namespace NIDO_TimeAttandance_MVC.Controllers
{
    [DashboardSession]
    public class GradeController : Controller
    {
        public async Task<ActionResult> ListAll()
        {
            try
            {
                ModelGrade MD = new ModelGrade();
                var model = await MD.GetGrade();
                return View(model);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "GradeController", "ListAll");
                return Content("Something Goes Wrong");
            }
        }

        public ActionResult Create()
        {
            return View();
        }
        public async Task<ActionResult> DeleteGrade(int id)
        {
            try
            {
                ModelGrade MD = new ModelGrade();
                bool Result = await MD.DeleteGrade(id);
                if (Result == true)
                {
                    return Json(new { Status = "Success", Message = "Grade Successfully Deleted", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "Sory this Grade can't be deleted because there are Employees maped on this Grade", JsonRequestBehavior.AllowGet });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "GradeController", "DeleteGrade");
                return Json(new { Status = "Error", Message = "Ooops Something Went Wrong", JsonRequestBehavior.AllowGet });
            }

        }
        [HttpPost]
        public async Task<ActionResult> SaveGrade(ModelGrade model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await model.Grade_Save())
                    {
                        return Json(new { Status = "Success", Message = "Grade(s) has been successfully saved",URL= "/Grade/ListAll", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Grade(s) has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "GradeController", "SaveGrade");
                return Content("Something Goes Wrong");
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                ModelGrade MD = new ModelGrade();
                var Result = await MD.Get_Grade_By_Id(id);
                return View(Result);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "GradeController", "Edit");
                return Content("Something Goes Wrong");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Update(ModelGrade model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await model.EditGrade())
                    {
                        return Json(new { Status = "Success", Message = "Grade successfully updated.", URL = "/Grade/ListAll" });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Grade not successfully updated.Please try again" });
                    }
                }

                return View(model);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "GradeController", "Update");
                return Content("Something Goes Wrong");
            }
        }
    }
}