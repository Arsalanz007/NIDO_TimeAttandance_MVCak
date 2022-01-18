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
    public class ManualAttandanceController : Controller
    {
        public ManualAttandanceController()
        {
            try
            {
                ViewBag.Reason = DataHelper._getReason();
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ManualAttandanceController", "Constructor");
            }
        }
        public async Task<ActionResult> Index(string date)
        {
            try
            {
                clsEmployeeProfile model = new clsEmployeeProfile();
                if (date != null)
                {
                    ViewBag.datemodalCheck = false;
                    ViewBag.Employees = await model._GetEmployeeForManualEntries(date);
                }
                else
                {
                    ViewBag.datemodalCheck = true;
                    ViewBag.Employees = await model._GetEmployeeForManualEntries();
                }
                return View();
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ManualAttandanceController", "Index");
                return Content("something goes Wrong");
            }
        }
        public async Task<ActionResult> Index2(string date)
        {
            try
            {
                clsEmployeeProfile model = new clsEmployeeProfile();
                if (date != null)
                {
                    ViewBag.Employees = await model._GetEmployeeForManualEntries(date);
                }
                else
                {
                    ViewBag.Employees = await model._GetEmployeeForManualEntries();
                }
                return View();
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ManualAttandanceController", "Index");
                return Content("something goes Wrong");
            }
        }
        public async Task<ActionResult> Save(Array EmpId, string time, string Remarks, long ReasonID, int TypeID)
        {
            try
            {
                ModelManualPosting md = new ModelManualPosting();
                if (await md.Save(EmpId, time, Remarks, ReasonID, TypeID))
                {
                    return Json(new { Status = "Success", Message = "Data successfully updated." });
                }
                else
                {
                    return Json(new { Status = "Error", Message = "Data not successfully updated.Please try again" });
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ManualAttandanceController", "Save");
                return Content("something goes Wrong");
            }
        }

        public async Task<ActionResult> ListAll()
        {
            try
            {
                ModelManualPosting MD = new ModelManualPosting();
                var model = await MD.GetManualEntries();
                return View(model);
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ManualAttandanceController", "ListAll");
                return Content("something goes Wrong");
            }
        }

        public async Task<ActionResult> Delete(decimal id)
        {
            try
            {
                using (PakOman_NedoEntities db = new PakOman_NedoEntities())
                {
                    var data = await db.AttendanceLogMasters.Where(m => m.ID == id).FirstOrDefaultAsync();
                    if (data != null)
                    {
                        db.AttendanceLogMasters.Remove(data);
                        await db.SaveChangesAsync();
                        return Json(new { Status = "Success", Message = " Entry Successfully Deleted", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { Status = "Error", Message = " Sory this Entry can't be deleted because there are Employees maped on this Entry", JsonRequestBehavior.AllowGet });
                    }
                }
            }
            catch (Exception ex)
            {
               ErrorLogging.Add(ex.Message, ex.InnerException == null ? null : ex.InnerException.Message, DateTime.Now, HttpContext.Session["UserName"].ToString(), "ManualAttandanceController", "Delete");
                return Json(new { Status = "Error", Message = " Something Went Wrong", JsonRequestBehavior.AllowGet });
            }
        }
    }
}