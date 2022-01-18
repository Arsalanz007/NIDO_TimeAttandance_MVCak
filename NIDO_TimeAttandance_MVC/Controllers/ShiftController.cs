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
    public class ShiftController : Controller
    {

        public ShiftController()
        {
            try
            {
                ViewBag.Company = DataHelper._getCompany();

            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ShiftController", "Constructor");
            }
        }
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> ListAll()
        {
            try
            {
                ModelShift MD = new ModelShift();
                var model = await MD.GetShift();
                return View(model);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ShiftController", "ListAll");
                return Content("Something Goes Wrong");
            }
        }

        public ActionResult Create()
        {
            return View(new ModelShift());
        }
        public async Task<ActionResult> DeleteShift(int id)
        {
            try
            {
                ModelShift MD = new ModelShift();
                bool Result = await MD.DeleteShift(id);
                if (Result == true)
                {
                    return Json(new { Status = "Success", Message = "Shift Successfully Deleted", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "Sory this Shift can't be deleted because there are data maped on this Shift", JsonRequestBehavior.AllowGet });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ShiftController", "DeleteShift");
                return Json(new { Status = "Success", Message = "Something Went Wrong", JsonRequestBehavior.AllowGet });
            }
        }
        [HttpPost]
        public async Task<ActionResult> SaveShift(ModelShift model)
        {
            try
            {               
                if (ModelState.IsValid)
                {
                    if (await model.Shift_Save())
                    {
                        return Json(new { Status = "Success", Message = "Shift(s) has been successfully saved",URL= "/Shift/ListAll", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Shift(s) has not been successfully saved.Please try again", JsonRequestBehavior.AllowGet });
                    }
                }
                else
                {
                    return Json(new { Status = "Error" });
                }
            }

            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ShiftController", "SaveShift");
                return Content("Something Goes Wrong");
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                ModelShift MD = new ModelShift();
                var Result = await MD.Get_Shift_By_Id(id);
                return View(Result);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ShiftController", "Edit");
                return Content("Something Goes Wrong");
            }
        }
        [HttpPost]
        public async Task<ActionResult> Update(ModelShift model)
        {
            try
            {
                
                if (ModelState.IsValid)
                {
                    if (await model.EditShift())
                    {
                        return Json(new { Status = "Success", Message = "Shift successfully updated.", URL = "/Shift/ListAll" });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = "Shift not successfully updated.Please try again" });
                    }
                }

                return View(model);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ShiftController", "Update");
                return Content("Something Goes Wrong");
            }
        }
    }
}